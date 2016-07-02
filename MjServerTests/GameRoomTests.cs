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
        Dictionary<IClientHolder, string> clientDict;
        List<ClientHolderForTest> clients;




        void TestInputLines(string filepath)
        {
            clients = new List<ClientHolderForTest>()
            {
                new ClientHolderForTest(),
                new ClientHolderForTest(),
                new ClientHolderForTest(),
                new ClientHolderForTest()
            };
            clientDict = new Dictionary<IClientHolder, string>()
            {
                { clients[0], "player0"},
                { clients[1], "player1"},
                { clients[2], "player2"},
                { clients[3], "player3"},
            };

            room = new GameRoom(clientDict);


            // read file
            var msgList = ReadTestFile(filepath);


            // start game
            room.StartGame();

            // test each kyoku
            var splitedEachKyokuMsgList = SplitEachKyoku(msgList);

            foreach (var kyokuMsg in splitedEachKyokuMsgList)
            {
                // start kyoku
                room.gameModel.StartKyoku();

                // Replase Yama and Tehai
                ReplaceYamaAndTehaiForTest(kyokuMsg);

                // execution client to server mesages
                ExecLines(kyokuMsg);
            }

        }
        List<List<string>> SplitEachKyoku(List<string> msgList)
        {
            var endKyokuResponseMessageIndexs = msgList.Select(e => JsonConvert.DeserializeObject<MJsonMessageAll>(e))
                                                .Select((item, index) => new { Index = index, Value = item })
                                                .Where(e => e.Value.IsEND_KYOKU())
                                                .Select(e => e.Index + 1);
                                                

            var startIndex = 0;
            var splitedMsgList = new List<List<string>>();

            if( endKyokuResponseMessageIndexs.Count() == 0 )
            {
                splitedMsgList.Add(msgList);
                return splitedMsgList;
            }


            foreach(var endKyokuIndex in endKyokuResponseMessageIndexs)
            {
                splitedMsgList.Add( msgList.GetRange( startIndex, endKyokuIndex - startIndex + 1) );// + 1 is need for include endKyokuRespounceMessage.
                startIndex = GetNextStartKyokuIndex(endKyokuIndex);

                if( startIndex >= msgList.Count)
                {
                    break;
                }
            }

            return splitedMsgList;
        }

        int GetNextStartKyokuIndex(int endKyokuIndex)
        {
            return endKyokuIndex + 1;
        }

        void ReplaceYamaAndTehaiForTest(List<string> msgList)
        {
            var serverToClientMessages = msgList.Where((e, index) => index % 2 == 0).ToList().Select( e => JsonConvert.DeserializeObject<MJsonMessageAll>(e) ).ToList();
            var startKyokuMessage =   serverToClientMessages.First(e => e.IsSTART_KYOKU());
            var typeModifiedStartKyokuMessage = JsonConvert.DeserializeObject<MJsonMessageStartKyoku>(JsonConvert.SerializeObject(startKyokuMessage));

            room.ReplaceKyokuInfoForTest(typeModifiedStartKyokuMessage);

            var tsumopais = FilterTsumo(serverToClientMessages);
            var rinshanpais = FilterRinshan(serverToClientMessages);

            room.ReplaceYamaForTest(tsumopais,rinshanpais);

        }

        List<string> FilterRinshan(List<MJsonMessageAll> msgobjList)
        {
            var tsumoAndKanList = msgobjList.Where(e => e.IsTSUMO() || e.IsKAKAN() || e.IsDAIMINKAN() || e.IsANKAN()).ToList();
            return RecursiveFilterRinshan(tsumoAndKanList).Select(e => e.pai).ToList();
        }

        List<MJsonMessageAll> RecursiveFilterRinshan(List<MJsonMessageAll> msgobjList)
        {
            var lastKanIndex = msgobjList.FindLastIndex(e => e.IsKAKAN() || e.IsDAIMINKAN() || e.IsANKAN());
            
            if ( lastKanIndex == -1)
            {
                return new List<MJsonMessageAll>();
            }

            var rinshanIndex = lastKanIndex + 1;

            if ( rinshanIndex >= msgobjList.Count)
            {
                return new List<MJsonMessageAll>();
            }
            var rinshanObj = msgobjList[rinshanIndex];

            var rinshanList = RecursiveFilterRinshan(msgobjList.GetRange(0, lastKanIndex));
            rinshanList.Add(rinshanObj);
            return rinshanList;
        }

        List<string> FilterTsumo(List<MJsonMessageAll> msgobjList)
        {
            var tsumoAndKanList = msgobjList.Where(e => e.IsTSUMO() || e.IsKAKAN() || e.IsDAIMINKAN() || e.IsANKAN()).ToList();
            return RecursiveFilterTsumo(tsumoAndKanList).Select(e => e.pai).ToList();
        }

        // this function removes rinshan tsumo
        List<MJsonMessageAll> RecursiveFilterTsumo(List<MJsonMessageAll> msgobjList)
        {
            
            var lastKanIndex = msgobjList.FindIndex(e => e.IsKAKAN() || e.IsDAIMINKAN() || e.IsANKAN());

            if (lastKanIndex == -1)
            {
                return msgobjList;
            }

            var rinshanIndex = lastKanIndex + 1;
            if (rinshanIndex >= msgobjList.Count)
            {
                return msgobjList.GetRange(0, lastKanIndex);
            }
            else
            {
                msgobjList.RemoveRange(lastKanIndex, 2);
                return RecursiveFilterTsumo(msgobjList);
            }
        }

        List<string> ReadTestFile( string filepath)
        {
            var lines = new List<string>();
            using (StreamReader sr = new StreamReader(filepath, System.Text.Encoding.GetEncoding("shift_jis")))
            {
                while (true)
                {
                    string line = sr.ReadLine();
                    if (line == null || line == string.Empty) break;
                    lines.Add(line);
                }
            }

            return lines;
        }

        void ExecLines(List<string> msgList)
        {
            var clientToServerMessages = msgList.Where((e, index) => index % 2 == 1).ToList();
            foreach(var msg in clientToServerMessages)
            {
                SendMessageToServerFromAllTestClients(msg);
            }


        }

        void SendMessageToServerFromAllTestClients(string message)
        {
            var mjsonObj = JsonConvert.DeserializeObject<MJsonMessageAll>(message);
            var actor = mjsonObj.actor;
            var noneMessage = JsonConvert.SerializeObject(new MJsonMessageNone());
            
            for(int i=0; i<room.clients.Count; i++)
            {
                var sendMessage = i == actor ? message : noneMessage;
                clients[i].GetMessageFromClient(sendMessage);
            }
        }

        // action test
        [TestMethod()]
        public void E2E_ChiTest()
        {
            TestInputLines(@"../../E2E_TestData/ChiTestData.txt");
            Assert.IsTrue(clients[0].ReceivedMessageList.Any(e => e.IsCHI()));
        }
        [TestMethod()]
        public void E2E_PonTest()
        {
            TestInputLines(@"../../E2E_TestData/PonTestData.txt");
            Assert.IsTrue(clients[0].ReceivedMessageList.Any(e => e.IsPON()));
        }
        [TestMethod()]
        public void E2E_AnkanTest()
        {
            TestInputLines(@"../../E2E_TestData/AnkanTestData.txt");
            Assert.IsTrue(clients[0].ReceivedMessageList.Any(e => e.IsANKAN()));
            Assert.IsTrue(clients[0].ReceivedMessageList.Any(e => e.IsDORA()));
        }
        [TestMethod()]
        public void E2E_KakanTest()
        {
            TestInputLines(@"../../E2E_TestData/KakanTestData.txt");
            Assert.IsTrue(clients[0].ReceivedMessageList.Any(e => e.IsKAKAN()));
            Assert.IsTrue(clients[0].ReceivedMessageList.Any(e => e.IsDORA()));
        }
        [TestMethod()]
        public void E2E_DaiminkanTest()
        {
            TestInputLines(@"../../E2E_TestData/DaiminkanTestData.txt");
            Assert.IsTrue(clients[0].ReceivedMessageList.Any(e => e.IsDAIMINKAN()));
            Assert.IsTrue(clients[0].ReceivedMessageList.Any(e => e.IsDORA()));

        }
        [TestMethod()]
        public void E2E_RyukyokuTest()
        {
            TestInputLines(@"../../E2E_TestData/RyukyokuTestData.txt");
            Assert.IsTrue(clients[0].ReceivedMessageList.Any(e => e.IsRYUKYOKU()));
            Assert.IsTrue(clients[0].ReceivedMessageList.Any(e => e.IsEND_KYOKU()));
        }
        [TestMethod()]
        public void E2E_SukaikanRyukyokuTest()
        {
            TestInputLines(@"../../E2E_TestData/SukaikanRyukyokuTestData.txt");
            Assert.IsTrue(clients[0].ReceivedMessageList.Any(e => e.IsRYUKYOKU()));
        }

        [TestMethod()]
        public void E2E_EndGameTest()
        {
            TestInputLines(@"../../E2E_TestData/EndGameTestData.txt");
            Assert.IsTrue(clients[0].ReceivedMessageList.Any(e => e.IsRYUKYOKU()));
            Assert.IsTrue(clients[0].ReceivedMessageList.Any(e => e.IsEND_KYOKU()));
            Assert.IsTrue(clients[0].ReceivedMessageList.Any(e => e.IsEND_GAME()));

        }

        [TestMethod()]
        public void E2E_GoNextKyokuTest()
        {
            TestInputLines(@"../../E2E_TestData/GoNextKyokuTestData.txt");
            Assert.IsTrue(clients[0].ReceivedMessageList.Count(e => e.IsRYUKYOKU()) == 2);
            Assert.IsTrue(clients[0].ReceivedMessageList.Count(e => e.IsEND_KYOKU()) == 2);
        }

        
        int yakuNamepos = 0;
        [TestMethod()]
        public void E2E_TenhouTest()
        {
            TestInputLines(@"../../E2E_TestData/TenhoTestData.txt");
            var horaMessage = clients[0].ReceivedMessageList.FirstOrDefault(e => e.IsHORA());


            Assert.IsTrue(horaMessage.yakus.Any(e => (string)e[yakuNamepos] == MJUtil.YAKU_STRING[(int)MJUtil.Yaku.CHURENPOTO]));
            Assert.IsTrue(horaMessage.yakus.Any(e => (string)e[yakuNamepos] == MJUtil.YAKU_STRING[(int)MJUtil.Yaku.TENHO]));

        }
        [TestMethod()]
        public void E2E_TenhouTest2()
        {
            TestInputLines(@"../../E2E_TestData/TenhoTestData2.txt");
            var horaMessage2 = clients[0].ReceivedMessageList.FirstOrDefault(e => e.IsHORA());

            Assert.IsTrue(horaMessage2.yakus.Any(e => (string)e[yakuNamepos] == MJUtil.YAKU_STRING[(int)MJUtil.Yaku.CHURENPOTO]));
            Assert.IsFalse(horaMessage2.yakus.Any(e => (string)e[yakuNamepos] == MJUtil.YAKU_STRING[(int)MJUtil.Yaku.TENHO]));
        }



        [TestMethod()]
        public void E2E_ChihouTest()
        {
            TestInputLines(@"../../E2E_TestData/ChihoTestData.txt");
            var horaMessage = clients[1].ReceivedMessageList.FirstOrDefault(e => e.IsHORA());

            Assert.IsTrue(horaMessage.yakus.Any(e => (string)e[yakuNamepos] == MJUtil.YAKU_STRING[(int)MJUtil.Yaku.CHIHO]));
        }

        [TestMethod()]
        public void E2E_ChankanTest()
        {
            TestInputLines(@"../../E2E_TestData/ChankanTestData.txt");
            var horaMessage = clients[1].ReceivedMessageList.FirstOrDefault(e => e.IsHORA());

            Assert.IsTrue(horaMessage.yakus.Any(e => (string)e[yakuNamepos] == MJUtil.YAKU_STRING[(int)MJUtil.Yaku.CHANKAN]));
        }

        [TestMethod()]
        public void E2E_ReachIppatsuTest()
        {
            TestInputLines(@"../../E2E_TestData/ReachIppatsuTestData.txt");
            var horaMessage = clients[1].ReceivedMessageList.FirstOrDefault(e => e.IsHORA());

            Assert.IsTrue(horaMessage.yakus.Any(e => (string)e[yakuNamepos] == MJUtil.YAKU_STRING[(int)MJUtil.Yaku.IPPATSU]));

        }
        [TestMethod()]
        public void E2E_ReachIppatsuTest2()
        {
            TestInputLines(@"../../E2E_TestData/ReachIppatsuTestData2.txt");
            var horaMessage = clients[1].ReceivedMessageList.FirstOrDefault(e => e.IsHORA());

            Assert.IsFalse(horaMessage.yakus.Any(e => (string)e[yakuNamepos] == MJUtil.YAKU_STRING[(int)MJUtil.Yaku.IPPATSU]));

        }

        [TestMethod()]
        public void E2E_DoubleReachTest()
        {
            TestInputLines(@"../../E2E_TestData/DoubleReachTestData.txt");
            var horaMessage = clients[1].ReceivedMessageList.FirstOrDefault(e => e.IsHORA());
            Assert.IsTrue(horaMessage.yakus.Any(e => (string)e[yakuNamepos] == MJUtil.YAKU_STRING[(int)MJUtil.Yaku.DOUBLEREACH]));
            Assert.IsFalse(horaMessage.yakus.Any(e => (string)e[yakuNamepos] == MJUtil.YAKU_STRING[(int)MJUtil.Yaku.REACH]));
        }
        [TestMethod()]
        public void E2E_DoubleReachTest2()
        {
            TestInputLines(@"../../E2E_TestData/DoubleReachTestData2.txt");
            var horaMessage = clients[1].ReceivedMessageList.FirstOrDefault(e => e.IsHORA());
            Assert.IsFalse(horaMessage.yakus.Any(e => (string)e[yakuNamepos] == MJUtil.YAKU_STRING[(int)MJUtil.Yaku.DOUBLEREACH]));
            Assert.IsTrue(horaMessage.yakus.Any(e => (string)e[yakuNamepos] == MJUtil.YAKU_STRING[(int)MJUtil.Yaku.REACH]));
        }

        [TestMethod()]
        public void E2E_RinshanTest()
        {
            TestInputLines(@"../../E2E_TestData/RinshanTestData.txt");
            var horaMessage = clients[0].ReceivedMessageList.FirstOrDefault(e => e.IsHORA());
            Assert.IsTrue(horaMessage.yakus.Any(e => (string)e[yakuNamepos] == MJUtil.YAKU_STRING[(int)MJUtil.Yaku.RINSHAN]));

        }

        [TestMethod()]
        public void E2E_UradoraTest()
        {
            TestInputLines(@"../../E2E_TestData/UradoraTestData.txt");
            var horaMessage = clients[1].ReceivedMessageList.FirstOrDefault(e => e.IsHORA());

            Assert.IsTrue(horaMessage.uradora_markers.Count > 0);
        }

        [TestMethod()]
        public void E2E_HaiteiTest()
        {
            TestInputLines(@"../../E2E_TestData/HaiteiTestData.txt");
            var horaMessage = clients[1].ReceivedMessageList.FirstOrDefault(e => e.IsHORA());
            Assert.IsTrue(horaMessage.yakus.Any(e => (string)e[yakuNamepos] == MJUtil.YAKU_STRING[(int)MJUtil.Yaku.HAITEI]));
        }

        [TestMethod()]
        public void E2E_HouteiTest()
        {
            TestInputLines(@"../../E2E_TestData/HouteiTestData.txt");
            var horaMessage = clients[0].ReceivedMessageList.FirstOrDefault(e => e.IsHORA());
            Assert.IsTrue(horaMessage.yakus.Any(e => (string)e[yakuNamepos] == MJUtil.YAKU_STRING[(int)MJUtil.Yaku.HOUTEI]));
        }
    }
} 