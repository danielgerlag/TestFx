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
using System.Text;
using JetBrains.Annotations;
using TestFx.Evaluation.Intents;
using TestFx.Evaluation.Results;
using TestFx.Utilities.Collections;

namespace TestFx.Evaluation.Reporting
{
  [PublicAPI]
  [UsedImplicitly (ImplicitUseTargetFlags.WithMembers)]
  public interface IRunListener
  {
    void OnRunStarted (IRunIntent intent);
    void OnRunFinished (IRunResult result);

    void OnSuiteStarted (IIntent intent, string text);
    void OnSuiteFinished (ISuiteResult result);

    void OnTestStarted (IIntent intent, string text);
    void OnTestFinished (ITestResult result);

    void OnError (IExceptionDescriptor exception);
  }

  public class RunListener : MarshalByRefObject, IRunListener
  {
    private readonly ISymbolProvider _symbolProvider;

    public RunListener (ISymbolProvider symbolProvider = null)
    {
      _symbolProvider = symbolProvider ?? new DefaultSymbolProvider();
    }

    public virtual void OnRunStarted (IRunIntent intent)
    {
    }

    public virtual void OnRunFinished (IRunResult result)
    {
    }

    public virtual void OnSuiteStarted (IIntent intent, string text)
    {
    }

    public virtual void OnSuiteFinished (ISuiteResult result)
    {
    }

    public virtual void OnTestStarted (IIntent intent, string text)
    {
    }

    public virtual void OnTestFinished (ITestResult result)
    {
    }

    public virtual void OnError (IExceptionDescriptor exception)
    {
    }

    protected IEnumerable<IOperationResult> MergeSetupsAndCleanups (ISuiteResult result)
    {
      return result.SetupResults.Concat(new FillingOperationResult()).Concat(result.CleanupResults);
    }

    protected IEnumerable<IExceptionDescriptor> GetExceptions (IEnumerable<IOperationResult> operations)
    {
      return operations.Select(x => x.Exception).WhereNotNull();
    }

    protected string GetGeneralMessage (IList<IExceptionDescriptor> exceptions, IEnumerable<IOperationResult> operations)
    {
      return exceptions.Count == 0
          ? operations.Count(x => !(x is FillingOperationResult)) + " Operations"
          : exceptions.Count == 1
              ? exceptions.Single().Name
              : exceptions.Count + " Exceptions";
    }

    protected string GetDetails (
        IEnumerable<IOperationResult> results,
        IEnumerable<OutputEntry> entries,
        IEnumerable<IExceptionDescriptor> exceptions = null)
    {
      var builder = new StringBuilder();

      AppendOperations(results, builder);
      AppendOutput(entries, builder);
      if (exceptions != null)
        AppendExceptions(exceptions, builder);

      return builder.ToString();
    }

    private void AppendOperations (IEnumerable<IOperationResult> results, StringBuilder builder)
    {
      builder.AppendLine("Operations:");
      foreach (var result in results)
      {
        if (result is FillingOperationResult)
        {
          builder.AppendLine(".. InnerOperations ..");
          continue;
        }

        builder.AppendFormat("{0} {1}", _symbolProvider.GetSymbol(result.State), result.Text);

        if (result.Exception != null)
          builder.AppendFormat(" ({0})", result.Exception.Name);

        builder.Append("\r\n");
      }
    }

    private void AppendOutput (IEnumerable<OutputEntry> entries, StringBuilder builder)
    {
      var entriesList = entries.ToList();
      if (entriesList.Count != 0)
      {
        builder.AppendLine().AppendLine("Output:");
        entriesList.ForEach(x => builder.AppendFormat("{0} {1}\r\n", _symbolProvider.GetSymbol(x.Type), x.Message));
      }
    }

    private static void AppendExceptions (IEnumerable<IExceptionDescriptor> exceptions, StringBuilder builder)
    {
      foreach (var exception in exceptions)
      {
        builder.AppendLine().AppendLine();

        builder.AppendFormat("{0}:", exception.FullName).AppendLine();
        builder.Append(exception.Message).AppendLine();
        builder.Append(exception.StackTrace);
      }
    }
  }
}