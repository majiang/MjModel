using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Diagnostics;
using MjModelProject.DebugUtil;


namespace MjModelProject
{
    public class ServerController
    {
        
        private ServerRouter serverRouter;
        
       
        public List<string> playerNames;//入室した順番でプレイヤー名が入っている
        public List<int> startPositionID;//入室した順番で初期配置が入っている
        public ServerMjModel serverMjModel;
       // public List<>//ipaddresとポートが入る？
       


        public ServerController(ServerRouter sr,  ServerMjModel mm) {
            serverRouter = sr;
            serverMjModel = mm;
            playerNames = new List<string>();
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


        //ここからモデルをいじって、クライアントへ送信するメッセージを作成する関数群
        public void StartGame()
        {
            
            serverMjModel.StartGame();
            SendStartGame();

        }

        public void StartKyoku()
        {
            //modelへ指示
            var msgobj = serverMjModel.StartKyoku();
            SendStartKyoku(msgobj);
        }



        public void Tsumo()
        {
            var msgobj = serverMjModel.Tsumo();
            SendTsumo(msgobj);
        }

        //ここからメッセージを受け取った際の関数
        //モデルの操作後にビューを変更する。

        public void Dahai(int actor, string pai, bool tsumogiri)
        {
            var msgobj = serverMjModel.Dahai(actor, pai, tsumogiri);
            SendDahai(msgobj);
            serverMjModel.GoNextActor();
        }

        public void Pon(int actor, int target, string pai, List<string> consumed)
        {
            var msg = serverMjModel.Pon(actor, target, pai, consumed);
            SendPon(msg);
            serverMjModel.SetCurrentActor(actor);
        }

        public void Chi(int actor, int target, string pai, List<string> consumed)
        {
            var msg = serverMjModel.Chi(actor, target, pai, consumed);
            SendChi(msg);
            serverMjModel.SetCurrentActor(actor);
        }

        public void Kakan(int actor, int target, string pai, List<string> consumed)
        {
            throw new NotImplementedException();
        }

        public void Ankan(int actor, int target, string pai, List<string> consumed)
        {
            throw new NotImplementedException();
        }

        public void Daiminkan(int actor, int target, string pai, List<string> consumed)
        {
            var msg = serverMjModel.Daiminkan(actor, target, pai, consumed);
            SendDaiminkan(msg);
            serverMjModel.SetCurrentActor(actor);
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
            throw new NotImplementedException();
        }

        public void Hora(int actor, int target, string pai)
        {
          //  var msg = serverMjModel.Hora(actor,target,pai);

        }

        public void Ryukyoku()
        {
            var ryukyokuMsgobj = new MJsonMessageRyukyoku("fanpai",
                new List<List<string>>() { 
                    serverMjModel.tehais[0].GetTehaiString(),
                    serverMjModel.tehais[1].GetTehaiString(),
                    serverMjModel.tehais[2].GetTehaiString(),
                    serverMjModel.tehais[3].GetTehaiString()
                },
                new List<bool>() { false, false, false, false },
                new List<int>() { 0, 0, 0, 0 },
                new List<int>() { 25000, 25000, 25000, 25000 });
            
            SendRyukyoku(ryukyokuMsgobj);
        }


        //ここからメッセージを送信するための関数
        //モデル操作完了後に呼び出される。

        public void SendStartGame()
        {
            for (int i = 0; i < playerNames.Count; i++)
            {
                serverRouter.SendStartGame(playerNames[i], new MJsonMessageStartGame(serverMjModel.turns[i], playerNames));
                DebugUtil.ServerDebug(JsonConvert.SerializeObject(new MJsonMessageStartGame(serverMjModel.turns[i], playerNames)));
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
                    msgobj.doraMarker,
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


        public void SendDora(MJsonMessageDora msgobj)
        {
            foreach (var name in playerNames)
            {
                serverRouter.SendDora(name, msgobj);
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

    }
}
