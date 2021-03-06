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
using FakeItEasy.Core;
using JetBrains.Annotations;
using TestFx.Evaluation.Results;
using TestFx.TestInfrastructure;

namespace TestFx.SpecK.Tests.Subject
{
  internal class MissingArgumentsTest : TestBase<MissingArgumentsTest.DomainSpec>
  {
    [Subject (typeof (MissingArgumentsTest))]
    internal class DomainSpec : Spec<DomainType>
    {
      [Injected] static string MyString = "MyString";

      DomainSpec ()
      {
        Specify (x => 0)
            .DefaultCase (_ => _);
      }
    }

    protected override void AssertResults (IRunResult runResult, IFakeScope scope)
    {
      runResult.GetTestResult ()
          .HasFailed ()
          .HasFailingOperation (Constants.Create_Subject,
              "Missing constructor arguments for subject type 'DomainType': firstMissingString, secondMissingString");
    }

    [UsedImplicitly]
    internal class DomainType
    {
      public DomainType (string firstMissingString, string myString, string secondMissingString)
      {
      }
    }
  }
}