using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Diagnostics;



namespace MjModelProject
{
    public class ServerController
    {
        
        private ServerRouter serverRouter;
        public List<string> playerNames = new List<string>();//入室した順番でプレイヤー名が入っている
        public ServerMjModel serverMjModel;
       // public List<>//ipaddresとポートが入る？
       


        public ServerController(ServerRouter sr,  ServerMjModel mm) {
            serverRouter = sr;
            serverMjModel = mm;
        }


        public bool CanJoin()
        {
            return playerNames.Count < Constants.PLAYER_NUM;//4人まで参加可能
        }

        public void Join(string name)
        {
            
            playerNames.Add(name);
        }

        public bool CanStartGame()
        {
            return playerNames.Count == Constants.PLAYER_NUM;
        }


        //ここからモデルを操作後に、クライアントへ送信するメッセージを送信する関数群
        public void StartGame()
        {
            //席順をシャッフルする
            playerNames = new List<String>(playerNames.OrderBy(i => Guid.NewGuid()));

            serverMjModel.StartGame();
            SendStartGame();
        }

        public void StartKyoku()
        {
            var msgobj = serverMjModel.StartKyoku();
            SendStartKyoku(msgobj);
        }



        public void Tsumo()
        {
            var msgobj = serverMjModel.Tsumo();
            SendTsumo(msgobj);
        }



        public void Dahai(int actor, string pai, bool tsumogiri)
        {
            var msgobj = serverMjModel.Dahai(actor, pai, tsumogiri);
            SendDahai(msgobj);
            
        }

        public void Pon(int actor, int target, string pai, List<string> consumed)
        {
            var msg = serverMjModel.Pon(actor, target, pai, consumed);
            SendPon(msg);
          
        }

        public void Chi(int actor, int target, string pai, List<string> consumed)
        {
            var msg = serverMjModel.Chi(actor, target, pai, consumed);
            SendChi(msg);

        }

        public void Kakan(int actor, int target, string pai, List<string> consumed)
        {
            var msg = serverMjModel.Kakan(actor, target, pai, consumed);
            SendKakan(msg);

        }

        public void Ankan(int actor, int target, string pai, List<string> consumed)
        {
            var msg = serverMjModel.Ankan(actor, target, pai, consumed);
            SendAnkan(msg);
        }

        public void Daiminkan(int actor, int target, string pai, List<string> consumed)
        {
            var msg = serverMjModel.Daiminkan(actor, target, pai, consumed);
            SendDaiminkan(msg);

        }

        public void Rinshan()
        {
            var msg = serverMjModel.Rinshan();
            SendRinshan(msg);
        }

        public void OpenDora()
        {
            var msg = serverMjModel.OpenDora();
            SendDora(msg);
        }

        public void Reach(int actor)
        {
            var msg = serverMjModel.Reach(actor);
            SendReach(msg);
        }

        public void Hora(int actor, int target, string pai)
        {
            var msg = serverMjModel.Hora(actor,target,pai);

            SendHora(msg);
        }

        public void Ryukyoku()
        {
            var ryukyokuMsgobj = serverMjModel.Ryukyoku();

            SendRyukyoku(ryukyokuMsgobj);
        }

        public void ReachAccept()
        {
            var reachAcceptMsgobj = serverMjModel.ReachAccept();
            SendReachAccept(reachAcceptMsgobj);
        }


        //ここからメッセージを送信するための関数
        //モデル操作完了後に呼び出される。
        public void SendErrorToRoomMember(MjsonMessageAll msgobj)
        {
            var errorMsg = JsonConvert.SerializeObject(msgobj);
            foreach (var clientName in playerNames) {
                serverRouter.SendErrorToClient(clientName, errorMsg);
            }
        }



        public void SendStartGame()
        {
            for (int i = 0; i < playerNames.Count; i++)
            {
                serverRouter.SendStartGame(playerNames[i], new MJsonMessageStartGame(i, playerNames));
                DebugUtil.ServerDebug(JsonConvert.SerializeObject(new MJsonMessageStartGame(i, playerNames)));
            }
        }

        public void SendStartKyoku(MJsonMessageStartKyoku msgobj)
        {
            //自身の手配しか見えない状態に加工してクライアントへメッセージ送信
            var knownTehais = msgobj.tehais;

            for (int i = 0; i < playerNames.Count; i++)
            {
                var unknownTehais = new List<List<string>> { Tehai.UNKNOWN_TEHAI_STRING, Tehai.UNKNOWN_TEHAI_STRING, Tehai.UNKNOWN_TEHAI_STRING, Tehai.UNKNOWN_TEHAI_STRING };
                unknownTehais[i] = knownTehais[i];

                 serverRouter.SendStartKyoku(playerNames[i], new MJsonMessageStartKyoku(
                    msgobj.bakaze,
                    msgobj.kyoku,
                    msgobj.honba,
                    msgobj.kyotaku,
                    msgobj.oya,
                    msgobj.dora_marker,
                    unknownTehais)); 
            }

            DebugUtil.ServerDebug(JsonConvert.SerializeObject(msgobj));
        }

        public void SendTsumo(MJsonMessageTsumo msgobj)
        {
            foreach (var name in playerNames)
            {
                if (name == playerNames[msgobj.actor])
                {
                    serverRouter.SendTsumo(name, msgobj);
                }
                else
                {
                    serverRouter.SendTsumo(name, new MJsonMessageTsumo(msgobj.actor, "?"));
                }
            }

            DebugUtil.ServerDebug(JsonConvert.SerializeObject(msgobj));
        }



        public void SendDahai(MJsonMessageDahai msgobj)
        {
            foreach (var name in playerNames)
            {
               serverRouter.SendDahai(name, msgobj);  
            }

            DebugUtil.ServerDebug(JsonConvert.SerializeObject(msgobj));
        }

        public void SendDaiminkan(MJsonMessageDaiminkan msgobj)
        {
            foreach (var name in playerNames)
            {
                serverRouter.SendDaiminkan(name, msgobj);
            }

            DebugUtil.ServerDebug(JsonConvert.SerializeObject(msgobj));
        }

        public void SendRinshan(MJsonMessageTsumo msgobj)
        {           
            serverRouter.SendTsumo(playerNames[msgobj.actor], msgobj);
            DebugUtil.ServerDebug(JsonConvert.SerializeObject(msgobj));
        }

        public void SendPon(MJsonMessagePon msgobj)
        {
            foreach (var name in playerNames)
            {
                serverRouter.SendPon(name, msgobj);
            }

            DebugUtil.ServerDebug(JsonConvert.SerializeObject(msgobj));
        }

        public void SendChi(MJsonMessageChi msgobj)
        {
            foreach (var name in playerNames)
            {
                serverRouter.SendChi(name, msgobj);
            }

            DebugUtil.ServerDebug(JsonConvert.SerializeObject(msgobj));
        }
        public void SendKakan(MJsonMessageKakan msgobj)
        {
            foreach (var name in playerNames)
            {
                serverRouter.SendKakan(name, msgobj);
            }

            DebugUtil.ServerDebug(JsonConvert.SerializeObject(msgobj));
        }

        public void SendAnkan(MJsonMessageAnkan msgobj)
        {
            foreach (var name in playerNames)
            {
                serverRouter.SendAnkan(name, msgobj);
            }

            DebugUtil.ServerDebug(JsonConvert.SerializeObject(msgobj));
        }

        public void SendDora(MJsonMessageDora msgobj)
        {
            foreach (var name in playerNames)
            {
                serverRouter.SendDora(name, msgobj);
            }

            DebugUtil.ServerDebug(JsonConvert.SerializeObject(msgobj));
        }
        public void SendReach(MJsonMessageReach msgobj)
        {
            foreach (var name in playerNames)
            {
                serverRouter.SendReach(name, msgobj);
            }

            DebugUtil.ServerDebug(JsonConvert.SerializeObject(msgobj));
        }

        public void SendReachAccept(MJsonMessageReachAccept msgobj)
        {
            foreach (var name in playerNames)
            {
                serverRouter.SendReachAccept(name, msgobj);
            }

            DebugUtil.ServerDebug(JsonConvert.SerializeObject(msgobj));
        }

        public void SendHora(MJsonMessageHora msgobj)
        {
            foreach (var name in playerNames)
            {
                serverRouter.SendHora(name, msgobj);
            }

            DebugUtil.ServerDebug(JsonConvert.SerializeObject(msgobj));
        }


        public bool CanFinishKyoku()
        {
            return serverMjModel.CanFinishKyoku();  
        }

        public void SendRyukyoku(MJsonMessageRyukyoku msgobj)
        {
            foreach (var name in playerNames)
            {
                serverRouter.SendRyukyoku(name, msgobj);
            }

            DebugUtil.ServerDebug(JsonConvert.SerializeObject(msgobj));
        }

        public void SendEndkyoku()
        {
            var msgobj = new MJsonMessageEndkyoku();
            foreach (var name in playerNames)
            {
                serverRouter.SendEndkyoku(name,msgobj);
            }

            DebugUtil.ServerDebug(JsonConvert.SerializeObject(msgobj));
        }

        public bool CanEndGame()
        {
            return serverMjModel.CanEndGame();
                
        }

        public void SendEndgame()
        {
            var msgobj = new MJsonMessageEndgame();
            foreach (var name in playerNames)
            {
                serverRouter.SendEndgame(name, msgobj);
            }

            DebugUtil.ServerDebug(JsonConvert.SerializeObject(msgobj));
        }



        //ここからInfoForResult操作用関数


    }
}
