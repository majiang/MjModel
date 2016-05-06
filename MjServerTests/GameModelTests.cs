using Microsoft.VisualStudio.TestTools.UnitTesting;
using MjModelLibrary;
using System.Diagnostics;


namespace MjServer.Tests
{
    [TestClass()]
    public class GameModelTests
    {
        GameModel model;

        private void SetUp()
        {
            model = new GameModel();
            model.StartGame();
            model.StartKyoku();
        }

        [TestMethod()]
        public void Unit_StartKyokuTest()
        {
            SetUp();
            Assert.AreEqual(MJUtil.LENGTH_ID - MJUtil.LENGTH_HAIPAI*4  - MJUtil.LENGTH_WANPAI, model.yama.GetRestYamaNum());

        }

        [TestMethod()]
        public void Unit_TsumoTest()
        {
            SetUp();
            var restpainum = MJUtil.LENGTH_ID - MJUtil.LENGTH_HAIPAI * 4 - MJUtil.LENGTH_WANPAI;
            Assert.AreEqual(restpainum, model.yama.GetRestYamaNum());
            Assert.AreEqual(MJUtil.LENGTH_HAIPAI, model.tehais[model.currentActor].tehai.Count);

            model.Tsumo();

            Assert.AreEqual(restpainum - 1 , model.yama.GetRestYamaNum());
            Assert.AreEqual(MJUtil.LENGTH_HAIPAI + 1, model.tehais[model.currentActor].tehai.Count);
        }

        [TestMethod()]
        public void Unit_RinshanTest()
        {
            SetUp();
            var restpainum = MJUtil.LENGTH_ID - MJUtil.LENGTH_HAIPAI * 4 - MJUtil.LENGTH_WANPAI;
            Assert.AreEqual(restpainum, model.yama.GetRestYamaNum());
            Assert.AreEqual(MJUtil.LENGTH_HAIPAI, model.tehais[model.currentActor].tehai.Count);
            model.Rinshan();
            Assert.AreEqual(restpainum - 1, model.yama.GetRestYamaNum());
            Assert.AreEqual(MJUtil.LENGTH_HAIPAI + 1, model.tehais[model.currentActor].tehai.Count);
        }

        [TestMethod()]
        public void Unit_DahaiTest()
        {
            SetUp();
            var firstActor = model.currentActor;
            var tsumoMsg = model.Tsumo();
            model.Dahai(tsumoMsg.actor, tsumoMsg.pai, false);
            
            Assert.AreEqual(model.CalcNextActor(firstActor), model.currentActor);

        }

        [TestMethod()]
        public void Unit_PonTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void Unit_ChiTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void Unit_KakanTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void Unit_AnkanTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void Unit_DaiminkanTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void Unit_OpenDoraTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void Unit_ReachTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void Unit_HoraTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void Unit_NoneTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void Unit_RyukyokuTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void Unit_ReachAcceptTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void Unit_CanTsumoTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void Unit_CanDahaiTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void Unit_CanChiTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void Unit_CanPonTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void Unit_CanFinishKyokuTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void Unit_CanEndGameTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void Unit_SetReachTest()
        {
            Assert.Fail();
        }
    }
}