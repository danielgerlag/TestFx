// Copyright 2015, 2014 Matthias Koch
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
using FakeItEasy;
using NUnit.Framework;

namespace TestFx.MSpec.IntegrationTests.NestedContext
{
  [TestFixture]
  public abstract class ThrowingSetupTestBase : NestedContextTestBase
  {
    public override void SetUp ()
    {
      Setup = ThrowingAction;

      base.SetUp ();
    }

    [Test]
    public override void Test ()
    {
      A.CallTo (() => NestedSetup ()).MustHaveHappened ();
      A.CallTo (() => Action ()).MustNotHaveHappened ();
      A.CallTo (() => Assertion ()).MustNotHaveHappened ();
      A.CallTo (() => Cleanup ()).MustNotHaveHappened ();
      A.CallTo (() => NestedCleanup ()).MustHaveHappened ();
    }
  }
}