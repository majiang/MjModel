using Microsoft.VisualStudio.TestTools.UnitTesting;
using MjServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MjNetworkProtocolLibrary;
using System.IO;
using MjModelLibrary;
using System.Diagnostics;

namespace MjServer.Tests
{
    [TestClass()]
    public class GameRoomTests
    {
        GameRoom room;
        Dictionary<ClientHolderInterface, string> clientDict;
        List<ClientHolderForTest> clients;



        void SetUp()
        {

        }
        void SetUp(string filepath)
        {
            clients = new List<ClientHolderForTest>()
            {
                new ClientHolderForTest(),
                new ClientHolderForTest(),
                new ClientHolderForTest(),
                new ClientHolderForTest()
            };
            clientDict = new Dictionary<ClientHolderInterface, string>()
            {
                { clients[0], "player0"},
                { clients[1], "player1"},
                { clients[2], "player2"},
                { clients[3], "player3"},
            };

            room = new GameRoom(clientDict);


            // read file
            var msgList = ReadTestFile(filepath);

            room.StartGame();
            var tempMsg = room.gameModel.StartKyoku();
            room.gameContext.ChangeState(tempMsg);
            // Replase Yama and Tehai
            ReplaceYamaAndTehaiForTEst(msgList);

            // execution client to server mesages
            ExecLines(msgList);
        }


        void ReplaceYamaAndTehaiForTEst(List<string> msgList)
        {
            var serverToClientMessages = msgList.Where((e, index) => index % 2 == 0).ToList().Select( e => JsonConvert.DeserializeObject<MJsonMessageAll>(e) ).ToList();
            var startKyokuMessage = serverToClientMessages.First(e => e.IsSTART_KYOKU());
            room.ReplaceStartKyokuForTest(startKyokuMessage);
            var tsumopais = serverToClientMessages.Where(e => e.IsTSUMO()).Select(e => e.pai).ToList();
            room.ReplaceYamaForTest(tsumopais);
        }

        List<string> ReadTestFile( string filepath)
        {
            var lines = new List<string>();
            using (StreamReader sr = new StreamReader(filepath, System.Text.Encoding.GetEncoding("shift_jis")))
            {
                while (true)
                {
                    string line = sr.ReadLine();
                    if (line == null) break;
                    lines.Add(line);
                }
            }

            return lines;
        }

        void ExecLines(List<string> msgList)
        {
            var clientToServerMessages = msgList.Where((e, index) => index % 2 == 1).ToList();
            clientToServerMessages.ForEach(e => SendMessageToServerFromAllTestClients(e));

        }

        void SendMessageToServerFromAllTestClients(string message)
        {
            var mjsonObj = JsonConvert.DeserializeObject<MJsonMessageAll>(message);
            var actor = mjsonObj.actor;
            var noneMessage = JsonConvert.SerializeObject(new MJsonMessageNone());
            
            for(int i=0; i<room.clients.Count; i++)
            {
                var sendMessage = i == actor ? message : noneMessage;
                clients[i].SendMessageToServer(sendMessage);
            }
        }

        // action test
        [TestMethod()]
        public void ChiTest()
        {
            SetUp(@"C:\Users\rick_\Source\Repos\MjModel\MjServerTests\ChiTestData.txt");
            Assert.IsTrue(clients[0].ReceivedMessageList.Any(e => e.IsCHI()));
        }
        [TestMethod()]
        public void PonTest()
        {
        }
        [TestMethod()]
        public void AnkanTest()
        {
        }
        [TestMethod()]
        public void KakanTest()
        {
        }
        [TestMethod()]
        public void DaiminkanTest()
        {
        }
        [TestMethod()]
        public void RyukyokuTest()
        {
        }
        [TestMethod()]
        public void SukaikanRyukyokuTest()
        {
        }
        [TestMethod()]
        public void SanchahoRyukyokuTest()
        {

        }
        [TestMethod()]
        public void EndGameTest()
        {

        }
    


        // Accidental Hands Test
        [TestMethod()]
        public void TenhouTest()
        {
            SetUp(@"C:\Users\rick_\Source\Repos\MjModel\MjServerTests\TenhoTestData.txt");
            var horaMessage = clients[0].ReceivedMessageList.FirstOrDefault(e => e.IsHORA());

            Assert.IsTrue(horaMessage.yakus.Any(e => (string)e[0] == MJUtil.YAKU_STRING[(int)MJUtil.Yaku.CHURENPOTO]));
            Assert.IsTrue(horaMessage.yakus.Any(e => (string)e[0] == MJUtil.YAKU_STRING[(int)MJUtil.Yaku.TENHO]));

            SetUp(@"C:\Users\rick_\Source\Repos\MjModel\MjServerTests\TenhoTestData2.txt");
            var horaMessage2 = clients[0].ReceivedMessageList.FirstOrDefault(e => e.IsHORA());

            Assert.IsTrue(horaMessage2.yakus.Any(e => (string)e[0] == MJUtil.YAKU_STRING[(int)MJUtil.Yaku.CHURENPOTO]));
            Assert.IsFalse(horaMessage2.yakus.Any(e => (string)e[0] == MJUtil.YAKU_STRING[(int)MJUtil.Yaku.TENHO]));
        }



        [TestMethod()]
        public void ChihouTest()
        {
            SetUp(@"C:\Users\rick_\Source\Repos\MjModel\MjServerTests\ChihoTestData.txt");
            var horaMessage = clients[1].ReceivedMessageList.FirstOrDefault(e => e.IsHORA());

            Assert.IsTrue(horaMessage.yakus.Any(e => (string)e[0] == MJUtil.YAKU_STRING[(int)MJUtil.Yaku.CHIHO]));
        }

        [TestMethod()]
        public void ChankanTest()
        {
        }



    }
} 