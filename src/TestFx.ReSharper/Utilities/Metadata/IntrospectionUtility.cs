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
using System.Diagnostics;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.Metadata.Reader.API;
using JetBrains.Util;
using TestFx.Utilities.Introspection;

namespace TestFx.ReSharper.Utilities.Metadata
{
  public interface IIntrospectionUtility
  {
    CommonType GetCommonType (IMetadataTypeInfo type);
    CommonType GetCommonType (IMetadataType type);

    CommonMember GetCommonMember (IMetadataField member);
    CommonMember GetCommonMember (IMetadataMethod member);

    CommonAttribute GetCommonAttribute (IMetadataCustomAttribute metadataCustomAttribute);
  }

  internal class IntrospectionUtility : IIntrospectionUtility
  {
    public static IIntrospectionUtility Instance = new IntrospectionUtility();

    public CommonType GetCommonType (IMetadataTypeInfo type)
    {
      return new CommonType(type.Name, type.FullyQualifiedName, type.GetImplementedTypes().Select(x => x.FullyQualifiedName));
    }

    public CommonType GetCommonType (IMetadataType type)
    {
      if (type is IMetadataArrayType)
        return GetCommonType(((IMetadataArrayType) type).TypeInfo);
      if (type is IMetadataClassType)
        return GetCommonType(((IMetadataClassType) type).Type);

      Trace.Fail($"Instance of type {type} cannot be converted to CommonType");
      throw new Exception();
    }

    public CommonMember GetCommonMember (IMetadataField member)
    {
      return GetCommonMember(member, member.Type.NotNull());
    }

    public CommonMember GetCommonMember (IMetadataMethod member)
    {
      return GetCommonMember(member, member.ReturnValue.Type);
    }

    private CommonMember GetCommonMember(IMetadataTypeMember member, IMetadataType type)
    {
      return new CommonMember(member.Name, type.ToCommon());
    }

    public CommonAttribute GetCommonAttribute (IMetadataCustomAttribute metadataCustomAttribute)
    {
      var type = GetCommonType(metadataCustomAttribute.UsedConstructor.NotNull().DeclaringType);
      var positionalArguments = metadataCustomAttribute.ConstructorArguments.Select(GetPositionalArgument).WhereNotNull();
      var namedArguments = metadataCustomAttribute.InitializedFields.Select(GetNamedArgument)
          .Concat(metadataCustomAttribute.InitializedProperties.Select(GetNamedArgument)).WhereNotNull();

      return new CommonAttribute(type, positionalArguments, namedArguments);
    }

    [CanBeNull]
    private CommonPositionalArgument GetPositionalArgument (MetadataAttributeValue argument, int position)
    {
      if (argument.IsBadValue())
        return null;

      return new CommonPositionalArgument(position, GetCommonType(argument.Type), GetValue(argument));
    }

    private CommonNamedArgument GetNamedArgument (IMetadataCustomAttributeFieldInitialization argument)
    {
      return new CommonNamedArgument(argument.Field.Name, GetCommonType(argument.Field.Type.NotNull()), GetValue(argument.Value));
    }

    private CommonNamedArgument GetNamedArgument (IMetadataCustomAttributePropertyInitialization argument)
    {
      return new CommonNamedArgument(argument.Property.Name, GetCommonType(argument.Property.Type), GetValue(argument.Value));
    }

    private object GetValue (MetadataAttributeValue argument)
    {
      Trace.Assert(!argument.IsBadValue(), "Values in MetadataCustomAttributes can be bad.");

      if (argument.ValuesArray != null)
        return argument.ValuesArray.Select(GetValue).ToArray();

      var type = argument.Value as IMetadataType;
      if (type != null)
        return GetCommonType(type);

      return argument.Value;
    }
  }
}