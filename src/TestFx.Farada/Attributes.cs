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
using Farada.TestDataGeneration.Fluent;
using JetBrains.Annotations;

namespace TestFx.Farada
{
  public interface ITestDataConfigurationProvider
  {
    Func<ITestDataConfigurator, ITestDataConfigurator> Configuration { get; }
  }

  [PublicAPI]
  [UsedImplicitly (ImplicitUseTargetFlags.WithMembers)]
  [AttributeUsage (AttributeTargets.Field)]
  [MeansImplicitUse (ImplicitUseKindFlags.Assign)]
  // ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
  public class AutoDataAttribute : Attribute
  {
    public AutoDataAttribute ()
    {
      MaxRecursionDepth = 3;
    }

    public int MaxRecursionDepth { get; set; }

    [UsedImplicitly]
    public virtual void Mutate (object autoData, object suite)
    {
    }
  }

  [AttributeUsage (AttributeTargets.Class)]
  [UsedImplicitly]
  public sealed class AutoDataConfigurationAttribute : Attribute
  {
    public AutoDataConfigurationAttribute (Type configurationType)
    {
      ConfigurationType = configurationType;
    }

    public Type ConfigurationType { get; }
  }

  [AttributeUsage (AttributeTargets.Class)]
  [UsedImplicitly]
  public sealed class AutoDataSeedAttribute : Attribute
  {
    public AutoDataSeedAttribute (int seed)
    {
      Seed = seed;
    }

    public int Seed { get; }
  }
}