using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MjModelProject
{
    public static class MsgType
    {
        public const string JOIN = "join";
        public const string START_GAME = "start_game";
        public const string START_KYOKU = "start_kyoku";
        public const string TSUMO = "tsumo";
        public const string DAHAI = "dahai";
        public const string PON = "pon";
        public const string CHI = "chi";
        public const string KAKAN = "kakan";
        public const string ANKAN = "ankan";
        public const string DAIMINKAN = "daiminkan";
        public const string DORA = "dora";
        public const string REACH = "reach";
        public const string REACH_ACCEPTED = "reach_accepted";
        public const string HORA = "hora";
        public const string END_KYOKU = "end_kyoku";
        public const string RYUKYOKU = "ryukyoku";
        public const string NONE = "none";
    }

    public class MjsonMessageAll
    {
        public string type;
        public string name;
        public string room;
        public int id;
        public List<string> names;
        public string bakaze;
        public int kyoku;
        public int actor;
        public bool tsumogiri;
        public int target;
        public string pai;
        public List<string> consumed;
        public string doraMarker;
        public List<int> details;//点数移動
        public List<int> scores;//点数移動結果
        public List<string> uradoraMarkers;
        public List<string> horaTehais;
        public Dictionary<string, int> yakus;
        public int fu;
        public int fan;
        public int horaPoints;
        public string reason;
        public List<List<string>> tehais;
        public int honba;
        public int kyotaku;
        public int oya;
        public List<int> deltas;

    }

    public class MJsonMessageStartGame
    {
        public string type = MsgType.START_GAME;
        public int id;//プレーヤid
        public List<string> names;
        public MJsonMessageStartGame(int id, List<string> names)
        {
            this.id = id;
            this.names = names; 
        }
    }
    public class MJsonMessageStartKyoku
    {
        public string type = MsgType.START_KYOKU;
        int bakaze;
        int kyoku;
        int honba;
        int kyotaku;
        int oya;
        int doraMarker;
        List<List<string>> tehais;
        public MJsonMessageStartKyoku(int bakaze, int kyoku, int honba, int kyotaku, int oya, int doraMarker, List<List<string>> tehais)
        {
            this.bakaze = bakaze;
            this.kyoku = kyoku;
            this.honba = honba;
            this.kyotaku = kyotaku;
            this.oya = oya;
            this.doraMarker = doraMarker;
            this.tehais = tehais;
        }
    }

    public class MJsonMessageTsumo
    {
        public string type = MsgType.TSUMO;
        int actor;
        string pai;
        public MJsonMessageTsumo(int actor, string pai)
        {
            this.actor = actor;
            this.pai = pai;
        }

    }

    public class MJsonMessageDahai
    {
        public string type = MsgType.DAHAI;
        int actor;
        string pai;
        bool tsumogiri;
        public MJsonMessageDahai(int actor, string pai,bool tsumogiri)
        {
            this.actor = actor;
            this.pai = pai;
            this.tsumogiri = tsumogiri;
        }

    }

    public class MJsonMessagePon
    {
        public string type = MsgType.PON;
        int actor;
        int target;
        string pai;
        List<string> list;
        public MJsonMessagePon(int actor, int target, string pai, List<string> list)
        {
            this.actor = actor;
            this.target = target;
            this.pai = pai;
            this.list = list;
        }
    }




    public class MJsonMessageNone
    {
        public string type = MsgType.NONE;
    }
}
