﻿// Copyright 2016, 2015, 2014 Matthias Koch
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.TaskRunnerFramework;
using JetBrains.ReSharper.UnitTestFramework;
using TestFx.Extensibility;
using TestFx.ReSharper.Model;
using TestFx.ReSharper.Runner.Tasks;
using TestFx.ReSharper.UnitTesting.Elements;
using TestFx.ReSharper.Utilities.Psi;
using TestFx.Utilities;
using RecursiveRemoteTaskRunner = TestFx.ReSharper.Runner.RecursiveRemoteTaskRunner;

namespace TestFx.ReSharper.UnitTesting
{
  [UnitTestProvider]
  public partial class TestProvider : IUnitTestProvider, IDynamicUnitTestProvider
  {
    public string ID => RecursiveRemoteTaskRunner.ID;

    public string Name => ID;

    public bool IsElementOfKind ([NotNull] IDeclaredElement declaredElement, UnitTestElementKind elementKind)
    {
      var clazz = declaredElement as ITypeElement;
      if (clazz == null)
      {
        var member = declaredElement as ITypeMember;
        if (member == null)
          return false;

        clazz = member.GetContainingType().NotNull();
      }

      return clazz.GetAttributeData<SuiteAttributeBase>() != null;
    }

    public bool IsElementOfKind ([NotNull] IUnitTestElement element, UnitTestElementKind elementKind)
    {
      var testElement = element as ITestElement;
      return testElement != null && testElement.ElementKind == elementKind;
    }

    public bool IsSupported ([NotNull] IHostProvider hostProvider)
    {
      return true;
    }

    public bool IsSupported ([NotNull] IProject project)
    {
      return true;
    }

    public int CompareUnitTestElements ([NotNull] IUnitTestElement firstElement, [NotNull] IUnitTestElement secondElement)
    {
      if (firstElement.State == UnitTestElementState.Dynamic || secondElement.State == UnitTestElementState.Dynamic)
        return 0;

      if (firstElement is ClassTestElement && secondElement is ClassTestElement)
        return string.Compare(firstElement.ShortName, secondElement.ShortName, StringComparison.Ordinal);

      // TODO: Performance critical. should cache test file
      var firstLocation = firstElement.GetDisposition().Locations.SingleOrDefault();
      var secondLocation = secondElement.GetDisposition().Locations.SingleOrDefault();
      if (firstLocation == null || secondLocation == null)
        return 0;

      return firstLocation.NavigationRange.StartOffset.CompareTo(secondLocation.NavigationRange.StartOffset);
    }

    public IUnitTestElement GetDynamicElement (RemoteTask remoteTask, Func<string, ITestElement> elementProvider)
    {
      var dynamicTask = (DynamicTask) remoteTask;
      var parentElement = elementProvider(dynamicTask.ParentGuid).NotNull("parentElement != null");

      var elementTypeFullName = typeof(ChildTestElement).FullName;
      var project = parentElement.GetProject().NotNull();
      var entity = new TestEntitySurrogate(dynamicTask.Identity, project, new string[0], dynamicTask.Text);

      var elementFactory = project.GetComponent<ITestElementFactory>();
      var element = elementFactory.GetOrCreateTestElement(elementTypeFullName, entity, parentElement);

      // TODO: parameter for elementFactory instead?
      element.State = UnitTestElementState.Dynamic;

      return element;
    }

    public IUnitTestElement GetDynamicElement ([NotNull] RemoteTask remoteTask, [NotNull] Dictionary<string, IUnitTestElement> elements)
    {
      return GetDynamicElement(
        remoteTask,
          absoluteId =>
          {
            IUnitTestElement element;
            return elements.TryGetValue(absoluteId, out element) ? (ITestElement) element : null;
          });
    }
  }
}