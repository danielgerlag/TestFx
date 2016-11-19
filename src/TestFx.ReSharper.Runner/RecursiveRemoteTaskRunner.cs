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

extern alias mscorlib;
using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.ReSharper.TaskRunnerFramework;
using TestFx.Evaluation;
using TestFx.Evaluation.Intents;
using TestFx.Evaluation.Utilities;
using TestFx.ReSharper.Runner.Tasks;
using TestFx.Utilities;
using TestFx.Utilities.Collections;

namespace TestFx.ReSharper.Runner
{
  public class RecursiveRemoteTaskRunner : JetBrains.ReSharper.TaskRunnerFramework.RecursiveRemoteTaskRunner
  {
    public static readonly string ID = typeof (RecursiveRemoteTaskRunner).FullName;

    private ICancellation _cancellation;

    public RecursiveRemoteTaskRunner (IRemoteTaskServer server)
        : base(server)
    {
    }

    public override void ExecuteRecursive ([NotNull] TaskExecutionNode node)
    {
      var taskDictionary = CreateTaskDictionary(node);
      var listener = new ReSharperRunListener(Server, taskDictionary);
      var runIntent = CreateRunIntent(node);
      _cancellation = runIntent.CancellationTokenSource;

      try
      {
        Evaluator.Run(runIntent, listener);
      }
      catch (Exception)
      {
        Server.SetTempFolderPath(runIntent.ShadowCopyPath);
        throw;
      }
    }

    public override void Abort ()
    {
      _cancellation.Cancel();
    }

    private static Dictionary<IIdentity, Task> CreateTaskDictionary (TaskExecutionNode node)
    {
      return node.DescendantsAndSelf(x => x.Children)
          .Select(x => x.RemoteTask).Cast<Task>()
          .ToDictionary(x => x.Identity);
    }

    private IRunIntent CreateRunIntent (TaskExecutionNode node)
    {
      var configuration = TaskExecutor.Configuration;

      var runTask = (RunTask) node.RemoteTask;
      var runIntent = RunIntent.Create(configuration.SeparateAppDomain, configuration.ShadowCopy, runTask.VisualStudioProcessId);

      node.Children.Select(CreateIntent).ForEach(runIntent.AddIntent);

      return runIntent;
    }

    private IIntent CreateIntent (TaskExecutionNode node)
    {
      var task = (Task) node.RemoteTask;
      var intent = Intent.Create(task.Identity);

      node.Children.Select(CreateIntent).ForEach(intent.AddIntent);

      return intent;
    }
  }
}