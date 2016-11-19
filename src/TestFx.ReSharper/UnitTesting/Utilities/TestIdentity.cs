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
using JetBrains.ReSharper.UnitTestFramework;
using TestFx.Utilities;

namespace TestFx.ReSharper.UnitTesting.Utilities
{
  public interface ITestIdentity : IIdentity
  {
    UnitTestElementId ElementId { get; }
  }

  public class TestIdentity : ITestIdentity
  {
    private readonly IIdentity _wrappedIdentity;

    public TestIdentity (UnitTestElementId elementId, IIdentity wrappedIdentity)
    {
      _wrappedIdentity = wrappedIdentity;

      ElementId = elementId;
    }

    public UnitTestElementId ElementId { get; }

    public IIdentity Parent => _wrappedIdentity.Parent;

    public string Relative => _wrappedIdentity.Relative;

    public string Absolute => _wrappedIdentity.Absolute;
  }
}