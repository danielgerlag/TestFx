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
using System.Linq;

namespace TestFx.Utilities.Introspection
{
  public class CommonType
  {
    private readonly IEnumerable<string> _implementedTypes;

    public CommonType (string name, string fullname, IEnumerable<string> implementedTypes)
    {
      Name = name;
      Fullname = fullname;
      _implementedTypes = implementedTypes.ToList();
    }

    public string Name { get; }

    public string Fullname { get; }

    public bool IsAssignableTo (string fullName)
    {
      return _implementedTypes.Any(x => x == fullName);
    }

    public override string ToString ()
    {
      return Name;
    }
  }
}