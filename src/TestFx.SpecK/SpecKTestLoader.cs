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
using System.Linq;
using TestFx.Extensibility;
using TestFx.Extensibility.Providers;
using TestFx.SpecK.Implementation;
using TestFx.SpecK.Implementation.Controllers;
using TestFx.SpecK.Implementation.Utilities;
using TestFx.Utilities;
using TestFx.Utilities.Reflection;

namespace TestFx.SpecK
{
  [OperationOrdering (
      typeof (SetupExtension),
      typeof (SetupCommon),
      typeof (SetupSubject),
      typeof (Arrange),
      typeof (BeforeAct),
      typeof (Act),
      typeof (AfterAct),
      typeof (Assert),
      typeof (CleanupCommon),
      typeof (CleanupExtension))]
  public class SpecKTestLoader : TestLoaderBase
  {
    private readonly IControllerFactory _controllerFactory;
    private readonly ISubjectFactory _subjectFactory;

    public SpecKTestLoader (
        IControllerFactory controllerFactory,
        ISubjectFactory subjectFactory,
        IIntrospectionPresenter introspectionPresenter)
        : base(introspectionPresenter)
    {
      _controllerFactory = controllerFactory;
      _subjectFactory = subjectFactory;
    }

    protected override void Initialize (Type suiteType, object suite, SuiteProvider provider)
    {
      var closedSpeckType = suiteType.GetClosedTypeOf(typeof (ISuite<>)).NotNull();
      var subjectType = closedSpeckType.GetGenericArguments().Single();

      var suiteController = _controllerFactory.CreateClassSuiteController(subjectType, provider);

      suite.SetMemberValue("_classSuiteController", suiteController);
      suite.SetMemberValue("_subjectFactory", _subjectFactory);

      InvokeConstructor(suite);
    }
  }
}