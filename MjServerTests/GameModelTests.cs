using Microsoft.VisualStudio.TestTools.UnitTesting;
using MjModelLibrary;
using System.Collections.Generic;
using System.Diagnostics;


namespace MjServer.Tests
{
    [TestClass()]
    public class GameModelTests
    {
        GameModel model;

        private void SetUp()
        {
            MjLogger testLogger = new MjLogger();
            model = new GameModel(testLogger);
            model.StartGame();
            model.StartKyoku();
        }

        [TestMethod()]
        public void Unit_StartKyokuTest()
        {
            SetUp();
            Assert.AreEqual(MJUtil.LENGTH_ALLPAI - MJUtil.LENGTH_HAIPAI* Constants.PLAYER_NUM - MJUtil.LENGTH_WANPAI, model.yama.GetRestYamaNum());

        }

        [TestMethod()]
        public void Unit_TsumoTest()
        {
            SetUp();
            var restpainum = MJUtil.LENGTH_ALLPAI - MJUtil.LENGTH_HAIPAI * Constants.PLAYER_NUM - MJUtil.LENGTH_WANPAI;
            Assert.AreEqual(restpainum, model.yama.GetRestYamaNum());
            Assert.AreEqual(MJUtil.LENGTH_HAIPAI, model.tehais[model.CurrentActor].tehai.Count);

            model.Tsumo();

            Assert.AreEqual(restpainum - 1 , model.yama.GetRestYamaNum());
            Assert.AreEqual(MJUtil.LENGTH_HAIPAI + 1, model.tehais[model.CurrentActor].tehai.Count);
        }

        [TestMethod()]
        public void Unit_RinshanTest()
        {
            SetUp();
            var restpainum = MJUtil.LENGTH_ALLPAI - MJUtil.LENGTH_HAIPAI * Constants.PLAYER_NUM - MJUtil.LENGTH_WANPAI;
            Assert.AreEqual(restpainum, model.yama.GetRestYamaNum());
            Assert.AreEqual(MJUtil.LENGTH_HAIPAI, model.tehais[model.CurrentActor].tehai.Count);
            model.Rinshan();
            Assert.AreEqual(restpainum - 1, model.yama.GetRestYamaNum());
            Assert.AreEqual(MJUtil.LENGTH_HAIPAI + 1, model.tehais[model.CurrentActor].tehai.Count);
        }

        [TestMethod()]
        public void Unit_DahaiTest()
        {
            SetUp();
            var tsumoMsg = model.Tsumo();
            var firstActor = model.CurrentActor;

            model.Dahai(tsumoMsg.actor, tsumoMsg.pai, true);
            
            Assert.AreEqual(model.CalcNextActor(firstActor), model.CurrentActor);

        }

        [TestMethod()]
        public void Unit_PonTest()
        {
             
            SetUp();

            var furopai = "1m";
            var furoConsumed = new List<string>() { furopai, furopai };

            var actor = 2;
            var target = 0;

            model.tehais[target] = new Tehai(new List<string>() { furopai });
            model.tehais[actor] = new Tehai(new List<string>() { furopai, furopai, "2m", "E" });

            model.Tsumo();
            model.Dahai(target, furopai, false);
            Assert.IsTrue(model.CanPon(actor, 0, furopai, furoConsumed));
            model.Pon(actor, target, furopai, furoConsumed);

            Assert.AreEqual(1, model.tehais[actor].furos.Count);

            Assert.AreEqual(MJUtil.TartsuType.MINKO, model.tehais[actor].furos[0].ftype);
            Assert.AreEqual(furopai, model.tehais[actor].furos[0].furopai.PaiString);
        
        }

        [TestMethod()]
        public void Unit_ChiTest()
        {
            SetUp();

            var furopai = "1m";
            var furoConsumed = new List<string>() { "2m", "3m" };
            var beforeChiTehai = new List<string>() { "2m", "3m", "4m", "5m" };

            var actor = 1;
            var target = 0;


            model.tehais[target] = new Tehai(new List<string>() { furopai });
            model.tehais[actor] = new Tehai(new List<string>(beforeChiTehai) );


            model.Tsumo();
            model.Dahai(target, furopai, false);
            model.Chi(actor, target, furopai, furoConsumed);

            Assert.AreEqual(1, model.tehais[actor].furos.Count);
            Assert.AreEqual(MJUtil.TartsuType.MINSYUN, model.tehais[actor].furos[0].ftype);
            Assert.AreEqual(furopai, model.tehais[actor].furos[0].furopai.PaiString);
        }

        [TestMethod()]
        public void Unit_KakanTest()
        {

            SetUp();

            var furopai = "1m";
            var ponConsumed = new List<string>() { "1m", "1m" };
            var kakanConsumed = new List<string>() { "1m", "1m","1m" };
            var actorTehai = new List<string>() { "1m", "1m", "1m", "2m", "3m", "4m", "5m" };

            var actor = 1;
            var target = 0;


            model.tehais[target] = new Tehai(new List<string>() { furopai });
            model.tehais[actor] = new Tehai(new List<string>(actorTehai));

            

            //player0 discard pai is poned by player1
            model.Tsumo();
            model.Dahai(target, furopai, false);

            //player1 pon
            model.Pon(actor, target, furopai, ponConsumed);
            model.Dahai(actor, "5m", false);

            //player2 tsumogiri
            var tsumoObj2 = model.Tsumo();
            model.Dahai(tsumoObj2.actor, tsumoObj2.pai, true);

            //player3 tsumogiri
            var tsumoObj3 = model.Tsumo();
            model.Dahai(tsumoObj3.actor, tsumoObj3.pai, true);

            //player0 tsumogiri
            var tsumoObj0 = model.Tsumo();
            model.Dahai(tsumoObj0.actor, tsumoObj0.pai, true);

            //player1 kakan
            model.Tsumo();
            Assert.IsTrue(model.CanKakan(actor, furopai, kakanConsumed));
            model.Kakan(actor, furopai, kakanConsumed);
            model.OpenDora();


            Assert.AreEqual(1, model.tehais[actor].furos.Count);
            Assert.AreEqual(MJUtil.TartsuType.MINKANTSU, model.tehais[actor].furos[0].ftype);
            Assert.AreEqual(furopai, model.tehais[actor].furos[0].furopai.PaiString);
            
        }

        [TestMethod()]
        public void Unit_AnkanTest()
        {
            SetUp();

            var furopai = "2m";
            var beforeAnjkanTehai = new List<string>() { furopai , furopai , furopai , furopai };

            var actor = 0; 


            model.tehais[actor] = new Tehai(new List<string>(beforeAnjkanTehai));


            model.Tsumo();
            Assert.IsTrue(model.CanAnkan(actor, beforeAnjkanTehai));
            model.Ankan(actor, beforeAnjkanTehai);

            Assert.AreEqual(1, model.tehais[actor].furos.Count);
            Assert.AreEqual(MJUtil.TartsuType.ANKANTSU, model.tehais[actor].furos[0].ftype);
            Assert.AreEqual(furopai, model.tehais[actor].furos[0].consumed[0].PaiString);
            

        }

        [TestMethod()]
        public void Unit_DaiminkanTest()
        {

            SetUp();

            var furopai = "1m";
            var furoConsumed = new List<string>() { furopai, furopai, furopai };

            var actor = 2;
            var target = 0;

            model.tehais[target] = new Tehai(new List<string>() { furopai });
            model.tehais[actor] = new Tehai(new List<string>() { furopai, furopai, furopai, "2m", "E" });

            model.Tsumo();
            model.Dahai(target, furopai, false);
            Assert.IsTrue(model.CanDaiminkan(actor, 0, furopai, furoConsumed));
            model.Daiminkan(actor, target, furopai, furoConsumed);

            Assert.AreEqual(1, model.tehais[actor].furos.Count);

            Assert.AreEqual(MJUtil.TartsuType.MINKANTSU, model.tehais[actor].furos[0].ftype);
            Assert.AreEqual(furopai, model.tehais[actor].furos[0].furopai.PaiString);

        }

        [TestMethod()]
        public void Unit_OpenDoraTest()
        {
            SetUp();
            var doraNum = 1;
            Assert.AreEqual(doraNum, model.yama.GetDoraMarkerStrings().Count);
            Assert.AreEqual(doraNum, model.yama.GetUradoraMarkerStrings().Count);
            model.OpenDora();
            Assert.AreEqual(doraNum + 1, model.yama.GetDoraMarkerStrings().Count);
            Assert.AreEqual(doraNum + 1, model.yama.GetUradoraMarkerStrings().Count);
            
        }

        [TestMethod()]
        public void Unit_Reach_ReachAcceptTest()
        {
            SetUp();
            var startPoint = 25000;
            var reachPoint = 1000;
            
            Assert.AreEqual(startPoint, model.points[0]);

            var tsumoObj = model.Tsumo();
            model.Reach(0);
            model.Dahai(tsumoObj.actor,tsumoObj.pai, true);
            model.ReachAccept();

            Assert.AreEqual(startPoint - reachPoint, model.points[0]);
            Assert.IsFalse(model.CanReach(0));
        }

        [TestMethod()]
        public void Unit_RyukyokuTest()
        {
            SetUp();
            var motaNum = 70;

            Assert.IsFalse(model.CanRyukyoku());

            for (int i = 0; i < motaNum; i++)
            {
                var tsumoObj = model.Tsumo();
                model.Dahai(tsumoObj.actor, tsumoObj.pai, true);
            }

            Assert.IsTrue(model.CanRyukyoku());
            model.EndKyoku();
        }



    }
}