using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace MjModelProject
{
    class ServerController
    {
        
        private ServerRouter serverRouter;
        private string roomName;
       
        public List<string> playerNames;//入室した順番でプレイヤー名が入っている
        public List<int> startPositionID;//入室した順番で初期配置が入っている
        public Yama yama { get; set; }
        public List<Kawa> kawas { get; set; }
        public List<Tehai> tehais { get; set; }
        public Field field { get; set; }
        public List<int> turnds;
       // public List<>//ipaddresとポートが入る？
        


        public ServerController(ServerRouter sr, string rn) {
            serverRouter = sr;
            roomName = rn;
            
            playerNames = new List<string>();
            yama = new Yama();
            kawas = new List<Kawa> { new Kawa(),  new Kawa(), new Kawa(), new Kawa() };
            tehais = new List<Tehai> { new Tehai(), new Tehai(), new Tehai(), new Tehai() };
            field = new Field();
        }


        public bool CanJoin()
        {
            return playerNames.Count < Constants.PLAYER_NUM;//4人まで参加可能
        }
        public void Join(string name)
        {
            playerNames.Add(name);
            if (playerNames.Count == 4)
            {
                StartKyoku();
            }
        }
        
        
        private void StartKyoku()
        {
            //初期座席配置作成
            var turn = new List<int> { 0, 1, 2, 3 };
            startPositionID = new List<int>(turn.OrderBy(i => Guid.NewGuid()));

            
            //modelへ指示
            var stubTehai = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            var unknownTehai = new List<int> { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            var stubTehais = new List<List<int>> { stubTehai, unknownTehai, unknownTehai, unknownTehai };


            //modelへの指示後にクライアントへモデルの状態を送信。
            for (int i = 0; i < startPositionID.Count; i++)
            {
                serverRouter.SendStartKyoku(playerNames[i], 0, 0, 0, 0, 0, 0, stubTehais);
            }
        }








        public void Sendxxx()
        {

        }


        internal void Dahai(int p1, int p2, bool p3)
        {
            throw new NotImplementedException();
        }

        internal void Pon(int p1, int p2, int p3, List<int> list)
        {
            throw new NotImplementedException();
        }

        internal void Chi(int p1, int p2, int p3, List<int> list)
        {
            throw new NotImplementedException();
        }

        internal void Kakan(int p1, int p2, int p3, List<int> list)
        {
            throw new NotImplementedException();
        }

        internal void Ankan(int p1, int p2, int p3, List<int> list)
        {
            throw new NotImplementedException();
        }

        internal void Daiminkan(int p1, int p2, int p3, List<int> list)
        {
            throw new NotImplementedException();
        }

        internal void Reach(int p)
        {
            throw new NotImplementedException();
        }

        internal void Hora(int p1, int p2, int p3)
        {
            throw new NotImplementedException();
        }

        internal void None()
        {
            throw new NotImplementedException();
        }
    }
}
