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
using JetBrains.Annotations;
using TestFx.Utilities;

namespace TestFx.Extensibility.Providers
{
  public interface ITestProvider : IOperationCollectionProvider
  {
    [CanBeNull]
    string FilePath { get; }
    int LineNumber { get; }
  }

  public class TestProvider : OperationCollectionProvider, ITestProvider
  { 
    public static TestProvider Create (IIdentity identity, string text, [CanBeNull] string ignoreReason, string filePath = null, int lineNumber = -1)
    {
      return new TestProvider(identity, text, ignoreReason, filePath, lineNumber);
    }

    private TestProvider (IIdentity identity, string text, [CanBeNull] string ignoreReason, [CanBeNull] string filePath, int lineNumber)
        : base(identity, text, ignoreReason)
    {
      FilePath = filePath;
      LineNumber = lineNumber;
    }

    [CanBeNull]
    public string FilePath { get; }

    public int LineNumber { get; }
  }
}