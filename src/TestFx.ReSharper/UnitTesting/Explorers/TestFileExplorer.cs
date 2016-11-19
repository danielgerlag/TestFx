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
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.UnitTestFramework;
using TestFx.ReSharper.UnitTesting.Elements;
using TestFx.ReSharper.UnitTesting.Explorers.Tree;
using TestFx.Utilities.Collections;

namespace TestFx.ReSharper.UnitTesting.Explorers
{
  public interface ITestFileExplorer
  {
    void Explore (IFile psiFile, Action<UnitTestElementDisposition> consumer, Func<bool> notInterrupted);
  }
  
  [SolutionComponent]
  public class TestFileExplorer : ITestFileExplorer
  {
    private readonly ITestElementFactory _testElementFactory;
    
    public TestFileExplorer (ITestElementFactory testElementFactory)
    {
      _testElementFactory = testElementFactory;
    }

    public void Explore (IFile psiFile, Action<UnitTestElementDisposition> consumer, Func<bool> notInterrupted)
    {
      var file = psiFile.ToTestFile(notInterrupted);
      if (file == null)
        return;

      var testElements = file.TestDeclarations.Select(_testElementFactory.GetOrCreateClassTestElementRecursively);
      var allElements = testElements.SelectMany(x => x.DescendantsAndSelf(y => y.Children)).Cast<ITestElement>();
      var dispositions = allElements.Select(x => x.GetDispositionFromFiles(file))
          .Where(x => x != UnitTestElementDisposition.InvalidDisposition).ToList();

      dispositions.ForEach(consumer);
    }
  }
}