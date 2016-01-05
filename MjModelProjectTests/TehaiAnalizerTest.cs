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
        public void 点数算出テスト()
        {
            //親40符3翻 ダブ東 ツモ 7800点
            {
                var expected = 7800;
                var tehai = new Tehai(new List<string>() { "1m", "2m", "3m", "4p", "5p", "6p", "7p", "7p", "7p", "9p", "9p", "E", "E", "E" });

                var gameId = 0;
                var playerPosition = 0;
                var lastAddedPai = "E";

                var ifr = new InfoForResult(gameId, playerPosition);
                ifr.IsTsumo = true;
                ifr.IsFured = false;
                ifr.IsMenzen = true;
                ifr.PassedTurn = 4;
                ifr.SetLastAddedPai(lastAddedPai);
                var fd = new Field();

                var result = ResultCalclator.CalcHoraResult(tehai, ifr, fd, lastAddedPai);
                Assert.AreEqual(expected, result.pointResult.HoraPlayerIncome);
            }

            //親50符2翻 ツモ 三暗刻
            {
                var expected = 9600;
                var tehai = new Tehai(new List<string>() { "1m", "1m", "1m", "2p", "2p", "2p", "4p", "5p", "5p", "5p", "6p", "E", "E","5p" });

                var gameId = 0;
                var playerPosition = 0;
                var lastAddedPai = "5p";

                var ifr = new InfoForResult(gameId, playerPosition);
                ifr.IsTsumo = true;
                ifr.IsFured = false;
                ifr.IsMenzen = true;
                ifr.PassedTurn = 4;
                ifr.SetLastAddedPai(lastAddedPai);
                var fd = new Field();

                var result = ResultCalclator.CalcHoraResult(tehai, ifr, fd, lastAddedPai);
                Assert.AreEqual(expected, result.pointResult.HoraPlayerIncome);
            }

            //子20符2翻 タンピン
            {
                var expected = 2000;
                var tehai = new Tehai(new List<string>() { "2m", "3m", "4m", "2p", "3p", "4p", "4p", "5p", "6p", "4s", "5s", "8s", "8s" });

                var gameId = 0;
                var playerPosition = 1;
                var lastAddedPai = "6s";

                var ifr = new InfoForResult(gameId, playerPosition);
                ifr.IsTsumo = false;
                ifr.IsFured = false;
                ifr.IsMenzen = true;
                ifr.PassedTurn = 4;
                var fd = new Field();
                ifr.SetLastAddedPai(lastAddedPai);
                var result = ResultCalclator.CalcHoraResult(tehai, ifr, fd, lastAddedPai);
                Assert.AreEqual(expected, result.pointResult.HoraPlayerIncome);
            }


            //子20符3翻 タンヤオ三食一盃口
            {
                var expected = 8000;
                var tehai = new Tehai(new List<string>() { "2m", "3m", "4m", "2p", "3p", "4p", "2p", "4p", "5s", "5s", "2s", "3s", "4s" });

                var gameId = 0;
                var playerPosition = 1;
                var lastAddedPai = "3p";

                var ifr = new InfoForResult(gameId, playerPosition);
                ifr.IsTsumo = false;
                ifr.IsFured = false;
                ifr.IsMenzen = true;
                ifr.PassedTurn = 4;
                var fd = new Field();
                ifr.SetLastAddedPai(lastAddedPai);
                var result = ResultCalclator.CalcHoraResult(tehai, ifr, fd, lastAddedPai);
                Assert.AreEqual(expected, result.pointResult.HoraPlayerIncome);
            }

            //子40符1翻 自摸
            {
                var expected = 1500;
                var tehai = new Tehai(new List<string>() { "1m", "2m", "3m", "3m", "4m", "5m", "1p", "1p", "1p", "2s", "3s", "4s", "5s", "5s" });

                var gameId = 0;
                var playerPosition = 1;
                var lastAddedPai = "3m";

                var ifr = new InfoForResult(gameId, playerPosition);
                ifr.IsTsumo = true;
                ifr.IsFured = false;
                ifr.IsMenzen = true;
                ifr.PassedTurn = 4;
                var fd = new Field();
                ifr.SetLastAddedPai(lastAddedPai);
                var result = ResultCalclator.CalcHoraResult(tehai, ifr, fd, lastAddedPai);
                Assert.AreEqual(expected, result.pointResult.HoraPlayerIncome);
            }

            //子50符2翻 ロン
            {
                var expected = 3200;
                var tehai = new Tehai(new List<string>() { "1m", "1m", "1m", "2m", "2m", "2m", "1p", "1p", "1p", "2p", "3p", "4s", "4s" });

                var gameId = 0;
                var playerPosition = 1;
                var lastAddedPai = "1p";

                var ifr = new InfoForResult(gameId, playerPosition);
                ifr.IsTsumo = false;
                ifr.IsFured = false;
                ifr.IsMenzen = true;
                ifr.PassedTurn = 4;
                var fd = new Field();
                ifr.SetLastAddedPai(lastAddedPai);
                var result = ResultCalclator.CalcHoraResult(tehai, ifr, fd, lastAddedPai);
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
            Tehai testTehai = new Tehai(new List<string> { "1m", "2m", "3m", "4m", "5m", "6m", "7m", "7m", "1m", "1m", "2m", "2m", "3m" });

            var isRon = true;
            var result = SplitedTehaiCalclator.CalcSplitedTehai(testTehai,"3m", isRon);
            Debug.WriteLine(result.AllHoraPatternList.Count);
            foreach (var r in result.AllHoraPatternList)
            {
                foreach (var t in r.TartsuList)
                {
                    Debug.WriteLine(t.TartsuType + "," + t.TartsuStartPaiSyu);
                }
                Debug.WriteLine("");
            }

            Assert.AreEqual(result.AllHoraPatternList.Count, 4);//testTehaiは111222333の部分が順子3つのパターンと暗刻3つのパターンがある
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
                Tehai testTehai = new Tehai(new List<string> { "1m", "2m", "3m", "4m", "5m", "6m", "7m", "7m", "1m", "2m", "2m", "3m", "3m", "4m" });

                var lastAdded = "4m";
                var testIfr = new InfoForResult();
                testIfr.IsMenzen = true;
                testIfr.IsOya = true;
                testIfr.IsTsumo = true;
                testIfr.PassedTurn = 10;
                testIfr.SetLastAddedPai(lastAdded);
                var result = new HoraResult();
                result = ResultCalclator.CalcHoraResult(testTehai, testIfr, new Field(), lastAdded);
                var yakuMap = result.yakuResult.yakus;
                Assert.IsTrue(yakuMap.ContainsKey(MJUtil.YAKU_STRING[(int)MJUtil.Yaku.CHINNITSU]));
                Assert.IsTrue(yakuMap.ContainsKey(MJUtil.YAKU_STRING[(int)MJUtil.Yaku.PINFU]));
                Assert.IsTrue(yakuMap.ContainsKey(MJUtil.YAKU_STRING[(int)MJUtil.Yaku.IIPEIKOU]));
            }
        }
    }
    
    

}
