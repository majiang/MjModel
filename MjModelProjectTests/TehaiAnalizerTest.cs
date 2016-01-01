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
        public void ツモ点数算出テスト0()
        {
            //40符3翻 ダブ東 ツモ 7800点
            {
                var expected = 7800;
                var tehai = new Tehai(new List<string>() { "1m", "2m", "3m", "4p", "5p", "6p", "7p", "7p", "7p", "9p", "9p", "E", "E" });

                var gameId = 0;
                var playerPosition = 0;

                var ifr = new InfoForResult(gameId, playerPosition);
                ifr.IsTsumo = true;
                ifr.IsMenzen = true;
                ifr.PassedTurn = 4;
                var fd = new Field();

                var result = HoraResultCalclator.CalcHoraResult(tehai, ifr, fd, "E");
                Assert.AreEqual(expected, result.pointResult.HoraPlayerIncome);
            }

            //50符2翻 ツモ 三暗刻 9600点
            {
                var expected = 9600;
                var tehai = new Tehai(new List<string>() { "1m", "1m", "1m", "2p", "2p", "2p", "4p", "5p", "5p", "5p", "6p", "E", "E" });

                var gameId = 0;
                var playerPosition = 0;

                var ifr = new InfoForResult(gameId, playerPosition);
                ifr.IsTsumo = true;
                ifr.IsMenzen = true;
                ifr.PassedTurn = 4;
                var fd = new Field();

                var result = HoraResultCalclator.CalcHoraResult(tehai, ifr, fd, "5p");
                Assert.AreEqual(expected, result.pointResult.HoraPlayerIncome);
            }

        }

    }

    




    [TestClass]
    public class TehaiAnalizerTest
    {
        [TestMethod]
        public void 和了時構成分割テスト()
        {
            Tehai testTehai = new Tehai(new List<string> { "1m", "2m", "3m", "4m", "5m", "6m", "7m", "7m", "1m", "1m", "2m", "2m", "3m"});

            var result = TehaiAnalizer.AnalizePattern(testTehai,"3m",false);
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

            //清一色ピンフ一盃口
            {
                Tehai testTehai = new Tehai(new List<string> { "1m", "2m", "3m", "4m", "5m", "6m", "7m", "7m", "1m", "2m", "2m", "3m", "3m" });

                var testIfr = new InfoForResult();
                testIfr.IsMenzen = true;
                testIfr.IsOya = true;
                testIfr.IsTsumo = true;
                testIfr.PassedTurn = 10;
                var result = new HoraResult();
                result = HoraResultCalclator.CalcHoraResult(testTehai, testIfr, new Field(), "4m");
                var yakuMap = result.yakuResult.yakus;
                Assert.IsTrue(yakuMap.ContainsKey(MJUtil.YAKU_STRING[(int)MJUtil.Yaku.CHINNITSU]));
                Assert.IsTrue(yakuMap.ContainsKey(MJUtil.YAKU_STRING[(int)MJUtil.Yaku.PINFU]));
                Assert.IsTrue(yakuMap.ContainsKey(MJUtil.YAKU_STRING[(int)MJUtil.Yaku.IIPEIKOU]));
            }
        }
    }
    
    

}
