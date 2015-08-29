using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MjModelProject
{
    class ServerController
    {
        public Dictionary<string,int> playerId { get; set; }
        public Yama yama { get; set; }
        public List<Kawa> kawas { get; set; }
        public List<Tehai> tehais { get; set; }
        public Field field { get; set; }
       // public List<>//ipaddresとポートが入る？


        public ServerController() {
            playerId = new Dictionary<string, int>();
            yama = new Yama();
            kawas = new List<Kawa> { new Kawa(),  new Kawa(), new Kawa(), new Kawa() };
            tehais = new List<Tehai> { new Tehai(), new Tehai(), new Tehai(), new Tehai() };
            field = new Field();
        }

        public void Join(string name, string room)
        {
            
            playerId.Add(name, playerId.Count);

        }
        public bool CanJoin(string name)
        {
            return playerId.Count < 4 && !playerId.ContainsKey(name);//4人まで参加可能＆同じ名前はNG
        }
        public bool CanStartGame()
        {
            return playerId.Count == 4;
        }
    }
}
