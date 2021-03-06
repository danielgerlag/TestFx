using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.ReSharper.Feature.Services.Daemon;
using TestFx.ReSharper.Model.Tree;

namespace TestFx.ReSharper.Daemon
{
  internal class TestFileDaemonStageProcess : IDaemonStageProcess
  {
    private readonly ITestFile _file;
    private readonly IEnumerable<ITestFileAnalyzer> _testFileAnalyzers;

    public TestFileDaemonStageProcess (IDaemonProcess process, ITestFile file, IEnumerable<ITestFileAnalyzer> testFileAnalyzers)
    {
      DaemonProcess = process;
      _file = file;
      _testFileAnalyzers = testFileAnalyzers;
    }

    public void Execute (Action<DaemonStageResult> committer)
    {
      var highlightings = _testFileAnalyzers.SelectMany(x => x.GetHighlightings(_file));
      var result = highlightings.Select(x => new HighlightingInfo(x.CalculateRange(), x)).ToList();
      committer(new DaemonStageResult(result));
    }

    public IDaemonProcess DaemonProcess { get; }
  }
}