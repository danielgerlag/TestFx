﻿// Copyright 2015, 2014 Matthias Koch
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
using TestFx.Extensibility;
// ReSharper disable InconsistentNaming

namespace TestFx.SpecK.Implementation
{
  public interface Assert : IAssertionDescriptor
  {
  }

  public interface Arrange : IActionDescriptor
  {
  }

  public interface BeforeAct : IActionDescriptor
  {
  }

  public interface Act : IActionDescriptor
  {
  }

  public interface AfterAct : IActionDescriptor
  {
  }

  public interface SubjectFactory : IActionDescriptor
  {
  }

  public interface SetupSubject : SetupCommon, SubjectFactory
  {
  }

  public interface ArrangeSubject : Arrange, SubjectFactory
  {
  }
}