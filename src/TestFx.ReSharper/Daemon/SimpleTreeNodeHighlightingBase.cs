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
using JetBrains.Annotations;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.Tree;

namespace TestFx.ReSharper.Daemon
{
  public interface INavigatableHighlighting : IHighlighting
  {
    DocumentRange NavigationRange { get; } 
  }

  public abstract class SimpleTreeNodeHighlightingBase : INavigatableHighlighting
  {
    private readonly ITreeNode _treeNode;
    private readonly string _toolTipText;

    protected SimpleTreeNodeHighlightingBase (ITreeNode treeNode, [NotNull] string toolTipText)
    {
      _treeNode = treeNode;
      _toolTipText = toolTipText;
    }

    public string ToolTip
    {
      get { return _toolTipText; }
    }

    public string ErrorStripeToolTip
    {
      get { return _toolTipText; }
    }

    public int NavigationOffsetPatch
    {
      get { return 0; }
    }

    public bool IsValid ()
    {
      return _treeNode.IsValid();
    }

    public virtual DocumentRange CalculateRange ()
    {
      return _treeNode.GetDocumentRange();
    }

    public DocumentRange NavigationRange
    {
      get { return _treeNode.GetNavigationRange(); }
    }
  }
}