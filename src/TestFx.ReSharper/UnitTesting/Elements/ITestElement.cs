// Copyright 2016, 2015, 2014 Matthias Koch
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
using JetBrains.Annotations;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.UnitTestFramework;
using TestFx.ReSharper.Model.Tree;
using TestFx.Utilities;

namespace TestFx.ReSharper.UnitTesting.Elements
{
  public interface ITestElement : IUnitTestElement, IIdentifiable
  {
    [CanBeNull]
    IProject GetProject ();

    UnitTestElementKind ElementKind { get; }

    void Update (string text, [CanBeNull] string explicitReason, IEnumerable<UnitTestElementCategory> categories);

    UnitTestElementDisposition GetDispositionFromFiles (params ITestFile[] testFiles);
  }
}