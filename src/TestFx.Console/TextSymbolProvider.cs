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
using TestFx.Evaluation.Reporting;
using TestFx.Evaluation.Results;

namespace TestFx.Console
{
  internal class TextSymbolProvider : ISymbolProvider
  {
    public static ISymbolProvider Instance = new TextSymbolProvider();

    private readonly Dictionary<State, string> _stateSymbols;
    private readonly Dictionary<OutputType, string> _outputTypeSymbols;

    public TextSymbolProvider ()
    {
      _stateSymbols =
          new Dictionary<State, string>
          {
              { State.Passed, "       " },
              { State.Failed, "FAILED " },
              { State.Inconclusive, "SKIPPED" }
          };

      _outputTypeSymbols =
          new Dictionary<OutputType, string>
          {
              { OutputType.Standard, "STD:" },
              { OutputType.Debug, "DBG:" },
              { OutputType.Error, "ERR:" }
          };
    }

    public string GetSymbol (State state)
    {
      return _stateSymbols[state];
    }

    public string GetSymbol (OutputType outputType)
    {
      return _outputTypeSymbols[outputType];
    }
  }
}