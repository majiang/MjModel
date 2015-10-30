using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MjModelProject;
using System.Collections.Generic;
using MjModelProject.Model;
using System.Diagnostics;

namespace MjModelProjectTests
{
    [TestClass]
    public class TehaiAnalizerTest
    {
        [TestMethod]
        public void 手配分割テスト()
        {
            Tehai testTehai = new Tehai(new List<string> { "1m", "2m", "3m", "4m", "5m", "6m", "7m", "7m", "1m", "1m", "2m", "2m", "3m", "3m" });
            TehaiAnalizer ta = new TehaiAnalizer(testTehai);
            var result = ta.AnalizePattern();
            Debug.WriteLine(result.AllPatternTartsuList.Count);
            foreach (var r in result.AllPatternTartsuList)
            {
                foreach (var t in r) { 
                    Debug.WriteLine(t.TartsuType +","+ t.TartsuStartPaiSyu);
                }
                Debug.WriteLine("");
            }
        }
    }
}
