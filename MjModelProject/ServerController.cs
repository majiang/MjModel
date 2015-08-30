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
        public Dictionary<string,int> playerId { get; set; }
        public Yama yama { get; set; }
        public List<Kawa> kawas { get; set; }
        public List<Tehai> tehais { get; set; }
        public Field field { get; set; }
       // public List<>//ipaddresとポートが入る？


        public ServerController(ServerRouter sr, string rn) {
            serverRouter = sr;
            roomName = rn;
            playerId = new Dictionary<string, int>();
            yama = new Yama();
            kawas = new List<Kawa> { new Kawa(),  new Kawa(), new Kawa(), new Kawa() };
            tehais = new List<Tehai> { new Tehai(), new Tehai(), new Tehai(), new Tehai() };
            field = new Field();
        }



        public async void Join(string name)
        {
            playerId.Add(name, playerId.Count);
            if (CanStartGame())
            {
                var startMsg = new MJsonMessageStartGame();
                startMsg.id
                serverRouter.SendMessage(roomName, JsonConvert.SerializeObject(  ));
            }
        }
        public bool CanJoin()
        {
            return playerId.Count < 4;//4人まで参加可能
        }
        public bool CanStartGame()
        {
            return playerId.Count == 4;
        }
    }
}
