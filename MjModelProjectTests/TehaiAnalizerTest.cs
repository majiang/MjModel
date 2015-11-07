using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MjModelProject;
using System.Collections.Generic;
using MjModelProject.Model;
using MjModelProject.Result;
using MjModelProject.Util;
using System.Diagnostics;

namespace MjModelProjectTests
{


    [TestClass]
    public class PointTest
    {
        [TestMethod]
        public void 結果算出テスト()
        {
            Tehai testTehai = new Tehai(new List<string> { "1m", "2m", "3m", "4m", "5m", "6m", "7m", "8m", "9m", "1m", "1m", "1m", "2m", "3m" });//チンイツピンフイッツーイーペーコー
            InfoForResult ifpc = new InfoForResult();            
            HoraResult horaResult = HoraResultCalclator.CalcHoraResult(testTehai, ifpc);
            //Notyet

        }
    }

    




    [TestClass]
    public class TehaiAnalizerTest
    {
        [TestMethod]
        public void 手配分割テスト()
        {
            Tehai testTehai = new Tehai(new List<string> { "1m", "2m", "3m", "4m", "5m", "6m", "7m", "7m", "1m", "1m", "2m", "2m", "3m", "3m" });

            var result = TehaiAnalizer.AnalizePattern(testTehai);
            Debug.WriteLine(result.AllHoraPatternList.Count);
            foreach (var r in result.AllHoraPatternList)
            {
                foreach (var t in r.TartsuList)
                {
                    Debug.WriteLine(t.TartsuType + "," + t.TartsuStartPaiSyu);
                }
                Debug.WriteLine("");
            }

            Assert.AreEqual(result.AllHoraPatternList.Count, 2);//testTehaiは111222333の部分が順子3つのパターンと暗刻3つのパターンがある
        }
    }


    //役判定
    [TestClass]
    public class YakuiAnalizerTest
    {
        [TestMethod]
        public void 役算出テスト()
        {
            Tehai testTehai = new Tehai(new List<string> { "1m", "2m", "3m", "4m", "5m", "6m", "7m", "7m", "1m", "1m", "2m", "2m", "3m", "3m" });

            var testIfr = new InfoForResult();
            testIfr.IsMenzen = true;
            testIfr.IsOya = true;
            testIfr.IsTsumo = true;


            var result = HoraResultCalclator.CalcHoraResult(testTehai, testIfr);
            
            var yakuMap = result.yakuResult.yakus;
            //Assert.IsTrue( yakuMap.ContainsKey( MJUtil.YAKU_STRING[(int)MJUtil.Yaku.CHINNITSU]) );
            //Assert.IsTrue( yakuMap.ContainsKey( MJUtil.YAKU_STRING[(int)MJUtil.Yaku.PINFU]) );
            Assert.IsTrue( yakuMap.ContainsKey( MJUtil.YAKU_STRING[(int)MJUtil.Yaku.IIPEIKOU]) );
            //Assert.IsTrue( yakuMap.ContainsKey( MJUtil.YAKU_STRING[(int)MJUtil.Yaku.ITTSUU]) );
            
        }
    }
    
    
    //点数計算テスト
    [TestClass]
    public class PointAnalizerTest
    {
        [TestMethod]
        public void 点数計算テスト()
        {
            Tehai testTehai = new Tehai(new List<string> { "1m", "2m", "3m", "4m", "5m", "6m", "7m", "7m", "1m", "1m", "2m", "2m", "3m", "3m" });

            var result = TehaiAnalizer.AnalizePattern(testTehai);

        }
    }
}
