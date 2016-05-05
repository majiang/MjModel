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
        public void StartKyokuTest()
        {
            SetUp();
            Assert.AreEqual(MJUtil.LENGTH_ID - MJUtil.LENGTH_HAIPAI*4  - MJUtil.LENGTH_WANPAI, model.yama.GetRestYamaNum());

        }

        [TestMethod()]
        public void TsumoTest()
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
        public void RinshanTest()
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
        public void DahaiTest()
        {
            SetUp();
            var firstActor = model.currentActor;
            var tsumoMsg = model.Tsumo();
            model.Dahai(tsumoMsg.actor, tsumoMsg.pai, false);
            
            Assert.AreEqual(model.CalcNextActor(firstActor), model.currentActor);

        }

        [TestMethod()]
        public void PonTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ChiTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void KakanTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void AnkanTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void DaiminkanTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void OpenDoraTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ReachTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void HoraTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void NoneTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void RyukyokuTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ReachAcceptTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void CanTsumoTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void CanDahaiTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void CanChiTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void CanPonTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void CanFinishKyokuTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void CanEndGameTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SetReachTest()
        {
            Assert.Fail();
        }
    }
}