using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

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
    public class MJsonMessageJoin
    {
        public string type = MsgType.JOIN;
        public string name;
        public string room;
        public MJsonMessageJoin(string name, string room)
        {
            this.name = name;
            this.room = room;
        }
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
        public string bakaze;
        public int kyoku;
        public int honba;
        public int kyotaku;
        public int oya;
        public string doraMarker;
        public List<List<string>> tehais;
        public MJsonMessageStartKyoku(string bakaze, int kyoku, int honba, int kyotaku, int oya, string doraMarker, List<List<string>> tehais)
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
        public int actor;
        public string pai;
        public MJsonMessageTsumo(int actor, string pai)
        {
            this.actor = actor;
            this.pai = pai;
        }

    }

    public class MJsonMessageDahai
    {
        public string type = MsgType.DAHAI;
        public int actor;
        public string pai;
        public bool tsumogiri;
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
        public int actor;
        public int target;
        public string pai;
        public List<string> consumed;
        public MJsonMessagePon(int actor, int target, string pai, List<string> consumed)
        {
            this.actor = actor;
            this.target = target;
            this.pai = pai;
            this.consumed = consumed;
        }
    }
    public class MJsonMessageChi
    {
        public string type = MsgType.CHI;
        public int actor;
        public int target;
        public string pai;
        public List<string> consumed;
        public MJsonMessageChi(int actor, int target, string pai, List<string> consumed)
        {
            this.actor = actor;
            this.target = target;
            this.pai = pai;
            this.consumed = consumed;
        }
    }
    public class MJsonMessageKakan
    {
        public string type = MsgType.KAKAN;
        public int actor;
        public int target;
        public string pai;
        public List<string> consumed;
        public MJsonMessageKakan(int actor, int target, string pai, List<string> consumed)
        {
            this.actor = actor;
            this.target = target;
            this.pai = pai;
            this.consumed = consumed;
        }
    }
    public class MJsonMessageAnkan
    {
        public string type = MsgType.ANKAN;
        public int actor;
        public int target;
        public string pai;
        public List<string> consumed;
        public MJsonMessageAnkan(int actor, int target, string pai, List<string> consumed)
        {
            this.actor = actor;
            this.target = target;
            this.pai = pai;
            this.consumed = consumed;
        }
    }
    public class MJsonMessageDaiminkan
    {
        public string type = MsgType.DAIMINKAN;
        public int actor;
        public int target;
        public string pai;
        public List<string> consumed;
        public MJsonMessageDaiminkan(int actor, int target, string pai, List<string> consumed)
        {
            this.actor = actor;
            this.target = target;
            this.pai = pai;
            this.consumed = consumed;
        }
    }

    public class MJsonMessageReach
    {
        public string type = MsgType.REACH;
        public int actor;

        public MJsonMessageReach(int actor)
        {
            this.actor = actor;
        }
    }
    public class MJsonMessageReachAccept
    {
        public string type = MsgType.REACH_ACCEPTED;
        public int actor;
        public List<int> deltas;
        public List<int> scores;

        public MJsonMessageReachAccept(int actor,List<int> deltas,List<int> scores)
        {
            this.actor = actor;
            this.deltas = deltas;
            this.scores = scores;
        }
    }

    public class MJsonMessageHora
    {
        public string type = MsgType.HORA;
        public int actor;
        public int target;
        public string pai;
        public List<string> uradoraMarkers;
        public List<string> horaTehais;
        public Dictionary<string, int> yakus;
        public int fu;
        public int fan;
        public List<int> deltas;
        public List<int> scores;

        public MJsonMessageHora(int actor,int target, string pai, List<string> uradoraMarkers, List<string> horaTehais, Dictionary<string, int> yakus, int fu, int fan, List<int> deltas, List<int> scores)
        {
            this.actor = actor;
            this.target = target;
            this.pai = pai;
            this.uradoraMarkers = uradoraMarkers;
            this.horaTehais = horaTehais;
            this.yakus = yakus;
            this.fu = fu;
            this.fan = fan;
            this.deltas = deltas;
            this.scores = scores;
        }
    }

    public class MJsonMessageRyukyoku
    {
        public string type = MsgType.RYUKYOKU;
        public string reason;
        public List<List<string>> tehais;
        public List<bool> tenpais;
        public List<int> deltas;
        public List<int> scores;

        public MJsonMessageRyukyoku(string reason, List<List<string>> tehais, List<bool> tenpais, List<int> deltas, List<int> scores)
        {
            this.reason = reason;
            this.tehais = tehais;
            this.tenpais = tenpais;
            this.deltas = deltas;
            this.scores = scores;
        }
    }
    public class MJsonMessageDora
    {
        public string type = MsgType.DORA;
        public string doraMarker;
        public MJsonMessageDora(string doraMarker)
        {
            this.doraMarker = doraMarker;
        }
    }

    public class MJsonMessageNone
    {
        public string type = MsgType.NONE;
    }

    public class MJsonMessageEndkyoku
    {
        public string type = MsgType.END_KYOKU;
    }
}
