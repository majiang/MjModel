using Microsoft.VisualStudio.TestTools.UnitTesting;
using MjModelLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MjModelLibrary.Tests
{
    [TestClass()]
    public class TehaiTests
    {
        [TestMethod()]
        public void Unit_TehaiTsumoTest()
        {
            Tehai testTehai = new Tehai(new List<string> { "1m", "2m", "3m", "4m", "5m", "6m", "7m", "8m", "9m", "1s", "2s", "3s", "4s" });
            Assert.IsFalse(testTehai.tehai.Contains(new Pai("5s")));
            testTehai.Tsumo(new Pai("5s"));
            Assert.IsTrue(testTehai.tehai.Contains(new Pai("5s")));
        }

        [TestMethod()]
        public void Unit_TehaiChiTest()
        {
            Tehai testTehai = new Tehai(new List<string> { "1m", "2m", "3m", "4m", "5m", "6m", "7m", "8m", "9m", "1s", "2s", "3s", "4s" });
            testTehai.Tsumo(new Pai("5s"));


            //chi
            var actor = 0;
            var target = 3;
            var furopai = new Pai("6s");
            var consumed = new List<Pai> { new Pai("4s"), new Pai("5s") };
            consumed.Sort();

            Assert.IsTrue(testTehai.tehai.Contains(new Pai("4s")));
            Assert.IsTrue(testTehai.tehai.Contains(new Pai("5s")));

            //実施
            testTehai.Chi(actor, target, furopai, consumed);

            //フーロオブジェクトの構成が正しいか
            Assert.AreEqual(testTehai.furos[0].furoType, MJUtil.TartsuType.MINSYUN);
            Assert.AreEqual(testTehai.furos[0].furopai, furopai);
            CollectionAssert.AreEqual(testTehai.furos[0].consumed, consumed);

            //晒した牌が手配に残っていないか
            Assert.IsFalse(testTehai.tehai.Contains(new Pai("4s")));
            Assert.IsFalse(testTehai.tehai.Contains(new Pai("5s")));

        }

        [TestMethod()]
        public void Unit_TehaiPonTest()
        {
            Tehai testTehai = new Tehai(new List<string> { "1m", "2m", "3m", "4m", "5m", "6m", "7m", "8m", "9m", "1s", "2s", "3s", "4s" });
            testTehai.Tsumo(new Pai("1m"));


            //chi
            var actor = 0;
            var target = 3;
            var furopai = new Pai("1m");
            var consumed = new List<Pai> { new Pai("1m"), new Pai("1m") };
            consumed.Sort();

            Assert.IsTrue(testTehai.tehai.Contains(new Pai("1m")));


            //実施
            testTehai.Pon(actor, target, furopai, consumed);

            //フーロオブジェクトの構成が正しいか
            Assert.AreEqual(testTehai.furos[0].furoType, MJUtil.TartsuType.MINKO);
            Assert.AreEqual(testTehai.furos[0].furopai, furopai);
            CollectionAssert.AreEqual(testTehai.furos[0].consumed, consumed);

            //晒した牌が手配に残っていないか
            Assert.IsFalse(testTehai.tehai.Contains(new Pai("1m")));

        }

        [TestMethod()]
        public void Unit_TehaiDaiminkanTest()
        {
            Tehai testTehai = new Tehai(new List<string> { "1m", "2m", "3m", "4m", "5m", "6m", "7m", "8m", "9m", "1s", "2s", "3s", "1m" });
            testTehai.Tsumo(new Pai("1m"));


            //chi
            var actor = 0;
            var target = 3;
            var furopai = new Pai("1m");
            var consumed = new List<Pai> { new Pai("1m"), new Pai("1m"), new Pai("1m") };
            consumed.Sort();

            Assert.IsTrue(testTehai.tehai.Contains(new Pai("1m")));

            //実施
            testTehai.Daiminkan(actor, target, furopai, consumed);

            //フーロオブジェクトの構成が正しいか
            Assert.AreEqual(testTehai.furos[0].furoType, MJUtil.TartsuType.MINKANTSU);
            Assert.AreEqual(testTehai.furos[0].furopai, furopai);
            CollectionAssert.AreEqual(testTehai.furos[0].consumed, consumed);

            //晒した牌が手配に残っていないか
            Assert.IsFalse(testTehai.tehai.Contains(new Pai("1m")));

        }

        [TestMethod()]
        public void Unit_TehaiAnkanTest()
        {
            Tehai testTehai = new Tehai(new List<Pai> { new Pai("1m"), new Pai("1m"), new Pai("1m"), new Pai("1m") });

            //ankan
            var actor = 0;
            var consumed = new List<Pai> { new Pai("1m"), new Pai("1m"), new Pai("1m"), new Pai("1m") };
            consumed.Sort();

            Assert.IsTrue(testTehai.tehai.Contains(new Pai("1m")));

            //実施
            testTehai.Ankan(actor, consumed);

            //フーロオブジェクトの構成が正しいか
            Assert.AreEqual(testTehai.furos[0].furoType, MJUtil.TartsuType.ANKANTSU);
            CollectionAssert.AreEqual(testTehai.furos[0].consumed, consumed);

            //晒した牌が手配に残っていないか
            Assert.IsFalse(testTehai.tehai.Contains(new Pai("1m")));

        }

        [TestMethod()]
        public void Unit_TehaiKakanTest()
        {
            Tehai testTehai = new Tehai(new List<string> { "1m", "2m", "3m", "4m", "5m", "6m", "7m", "8m", "9m", "1s", "2s", "3s", "4s" });
            testTehai.Tsumo(new Pai("1m"));


            //pon infomation
            var actor = 0;
            var target = 3;
            var furopai = new Pai("1m");
            var consumed = new List<Pai> { new Pai("1m"), new Pai("1m") };
            consumed.Sort();

            Assert.IsTrue(testTehai.tehai.Contains(new Pai("1m")));

            //execute pon
            testTehai.Pon(actor, target, furopai, consumed);

            //tsumo for kakan
            testTehai.Tsumo(new Pai("1m"));

            //kakan infomation
            var kakanFuropai = new Pai("1m");
            var kakanConsumed = new List<Pai> { new Pai("1m"), new Pai("1m"), new Pai("1m") };
            consumed.Sort();

            //execute kakan
            testTehai.Kakan(actor, kakanFuropai, kakanConsumed);


            //フーロオブジェクトの構成が正しいか
            Assert.AreEqual(MJUtil.TartsuType.MINKANTSU, testTehai.furos.LastOrDefault().furoType);
            Assert.AreEqual(kakanFuropai, testTehai.furos.LastOrDefault().furopai);
            CollectionAssert.AreEqual(kakanConsumed, testTehai.furos.LastOrDefault().consumed);

            //晒した牌が手配に残っていないか
            Assert.IsFalse(testTehai.tehai.Contains(new Pai("1m")));

        }

    }
}