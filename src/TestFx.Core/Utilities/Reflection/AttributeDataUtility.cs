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
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using TestFx.Utilities.Collections;

namespace TestFx.Utilities.Reflection
{
  public interface IAttributeDataUtility
  {
    IEnumerable<CustomAttributeData> GetAttributeDatas<T> (Assembly assembly) where T : Attribute;
    IEnumerable<CustomAttributeData> GetAttributeDatas<T> (Type type) where T : Attribute;
    IEnumerable<CustomAttributeData> GetAttributeDatas<T> (MemberInfo memberInfo) where T : Attribute;

    [CanBeNull]
    CustomAttributeData GetAttributeData<T> (Assembly assembly) where T : Attribute;

    [CanBeNull]
    CustomAttributeData GetAttributeData<T> (Type type) where T : Attribute;

    [CanBeNull]
    CustomAttributeData GetAttributeData<T> (MemberInfo memberInfo) where T : Attribute;
  }

  [ExcludeFromCodeCoverage]
  internal class AttributeDataUtility : IAttributeDataUtility
  {
    public static IAttributeDataUtility Instance = new AttributeDataUtility();

    public IEnumerable<CustomAttributeData> GetAttributeDatas<T> (Assembly assembly) where T : Attribute
    {
      return GetAttributeDatas<T>(CustomAttributeData.GetCustomAttributes(assembly));
    }

    public IEnumerable<CustomAttributeData> GetAttributeDatas<T> (Type type) where T : Attribute
    {
      return GetAttributeDatas<T>(CustomAttributeData.GetCustomAttributes(type));
    }

    public IEnumerable<CustomAttributeData> GetAttributeDatas<T> (MemberInfo memberInfo) where T : Attribute
    {
      return GetAttributeDatas<T>(CustomAttributeData.GetCustomAttributes(memberInfo));
    }

    [CanBeNull]
    public CustomAttributeData GetAttributeData<T> (Assembly assembly) where T : Attribute
    {
      return GetAttributeDatas<T>(assembly).SingleOrDefault();
    }

    [CanBeNull]
    public CustomAttributeData GetAttributeData<T> (Type type) where T : Attribute
    {
      return GetAttributeDatas<T>(type).SingleOrDefault();
    }

    [CanBeNull]
    public CustomAttributeData GetAttributeData<T> (MemberInfo memberInfo) where T : Attribute
    {
      return GetAttributeDatas<T>(memberInfo).SingleOrDefault();
    }

    private IEnumerable<CustomAttributeData> GetAttributeDatas<T> (IEnumerable<CustomAttributeData> customAttributeDatas) where T : Attribute
    {
      return
          from customAttributeData in customAttributeDatas
          let originalAttributeType = customAttributeData.Constructor.DeclaringType
          where originalAttributeType.DescendantsAndSelf(x => x.BaseType).Any(x => x.FullName == typeof (T).FullName)
          select customAttributeData;
    }
  }
}