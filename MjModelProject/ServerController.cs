using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


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

        public bool CanStart()
        {
            return playerNames.Count == Constants.PLAYER_NUM;
        }

        public void StartGame()
        {
            StartGame();
        }

        public void StartKyoku()
        {
            //初期座席配置作成
            var turn = new List<int> { 0, 1, 2, 3 };
            startPositionID = new List<int>(turn.OrderBy(i => Guid.NewGuid()));

            
            //modelへ指示
            serverMjModel = new ServerMjModel();//resetにするかも
            serverMjModel.StartGame();


            
        }

        //ここからメッセージを受け取った際の関数
        //モデルの操作後にビューを変更する。
        public void Tsumo()
        {
             serverMjModel.Tsumo();
        }

        public void Dahai(int actor, string pai, bool tsumogiri)
        {
            throw new NotImplementedException();
        }

        public void Pon(int actor, int target, string pai, List<string> list)
        {
            throw new NotImplementedException();
        }

        internal void Chi(int actor, int target, string pai, List<string> list)
        {
            throw new NotImplementedException();
        }

        internal void Kakan(int actor, int target, string pai, List<string> list)
        {
            throw new NotImplementedException();
        }

        internal void Ankan(int actor, int target, string pai, List<string> list)
        {
            throw new NotImplementedException();
        }

        internal void Daiminkan(int actor, int target, string pai, List<string> list)
        {
            throw new NotImplementedException();
        }

        internal void Reach(int actor)
        {
            throw new NotImplementedException();
        }

        internal void Hora(int actor, int target, string pai)
        {
            throw new NotImplementedException();
        }



        //ここからメッセージを送信するための関数
        //モデル操作完了後に呼び出される。
        public void SendTsumo(int actor, string pai)
        {
            foreach(var name in playerNames){
                if (name == playerNames[actor])
                {
                    serverRouter.SendTsumo( playerNames[actor] , new MJsonMessageTsumo(actor, pai) );
                }
                else
                {
                    serverRouter.SendTsumo( playerNames[actor] , new MJsonMessageTsumo(actor, "?"));
                }
            }
        }

        public void SendStartKyoku()
        {
            //modelへの指示後にクライアントへモデルの状態を送信。
            //自身の手配しか見えない状態に加工して送信
            var tehais = serverMjModel.tehais;
            for (int i = 0; i < playerNames.Count; i++)
            {
                var unknownTehais = new List<List<string>> { Tehai.UNKNOWN_TEHAI_STRING, Tehai.UNKNOWN_TEHAI_STRING, Tehai.UNKNOWN_TEHAI_STRING, Tehai.UNKNOWN_TEHAI_STRING };
                unknownTehais[i] = serverMjModel.tehais[i].GetTehaiString();
                serverRouter.SendStartKyoku(playerNames[i], new MJsonMessageStartKyoku(0, 0, 0, 0, 0, 0, unknownTehais));
            }
        }

    }
}
