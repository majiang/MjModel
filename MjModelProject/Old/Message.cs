using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MjServer
{
    public static class MsgType
    {
        public const string HELLO = "hello";
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
        public const string END_GAME = "end_game";
        public const string RYUKYOKU = "ryukyoku";
        public const string NONE = "none";
        public const string ERROR = "error";

        public static List<string> MsgTypeList = new List<string>()
        {
            MsgType.JOIN,
            MsgType.START_GAME,
            MsgType.START_KYOKU,
            MsgType.TSUMO,
            MsgType.DAHAI ,
            MsgType.PON,
            MsgType.CHI,
            MsgType.ANKAN,
            MsgType.KAKAN,
            MsgType.DAIMINKAN,
            MsgType.DORA,
            MsgType.REACH,
            MsgType.REACH_ACCEPTED,
            MsgType.HORA,
            MsgType.END_KYOKU,
            MsgType.END_GAME,
            MsgType.RYUKYOKU,
            MsgType.NONE,
            MsgType.ERROR
        };
    }

    public class MjsonMessageBase {
        public string type;
        public bool IsJOIN()
        {
            return type == MsgType.JOIN;
        }
        public bool IsSTART_GAME()
        {
            return type == MsgType.START_GAME;
        }
        public bool IsSTART_KYOKU()
        {
            return type == MsgType.START_KYOKU;
        }
        public bool IsTSUMO()
        {
            return type == MsgType.TSUMO;
        }
        public bool IsDAHAI()
        {
            return type == MsgType.DAHAI;
        }
        public bool IsPON()
        {
            return type == MsgType.PON;
        }
        public bool IsCHI()
        {
            return type == MsgType.CHI;
        }
        public bool IsANKAN()
        {
            return type == MsgType.ANKAN;
        }
        public bool IsKAKAN()
        {
            return type == MsgType.KAKAN;
        }
        public bool IsDAIMINKAN()
        {
            return type == MsgType.DAIMINKAN;
        }
        public bool IsDORA()
        {
            return type == MsgType.DORA;
        }
        public bool IsREACH()
        {
            return type == MsgType.REACH;
        }
        public bool IsHORA()
        {
            return type == MsgType.HORA;
        }
        public bool IsEND_KYOKU()
        {
            return type == MsgType.END_KYOKU;
        }
        public bool IsEND_GAME()
        {
            return type == MsgType.END_GAME;
        }
        public bool IsRYUKYOKU()
        {
            return type == MsgType.RYUKYOKU;
        }
        public bool IsNONE()
        {
            return type == MsgType.NONE;
        }
    }

  

    public class MJsonMessageHello : MjsonMessageBase
    {
        public string protocol;
        public int protocol_version;
        public MJsonMessageHello(string protocol = "mjsonp", int protocol_version =1)
        {
            type = MsgType.HELLO;
            this.protocol = protocol;
            this.protocol_version = protocol_version;
        }
    }


    public class MJsonMessageJoin : MjsonMessageBase
    {
        public string name;
        public string room;
        public MJsonMessageJoin(string name, string room)
        {
            type = MsgType.JOIN;
            this.name = name;
            this.room = room;
        }
    }
    public class MJsonMessageStartGame : MjsonMessageBase
    {
        public int id;//プレーヤid
        public List<string> names;
        public MJsonMessageStartGame(int id, List<string> names)
        {
            type = MsgType.START_GAME;
            this.id = id;
            this.names = names; 
        }
    }
    public class MJsonMessageStartKyoku : MjsonMessageBase
    {
        public string bakaze;
        public int kyoku;
        public int honba;
        public int kyotaku;
        public int oya;
        public string dora_marker;
        public List<List<string>> tehais;
        public MJsonMessageStartKyoku(string bakaze, int kyoku, int honba, int kyotaku, int oya, string dora_marker, List<List<string>> tehais)
        {
            type = MsgType.START_KYOKU;
            this.bakaze = bakaze;
            this.kyoku = kyoku;
            this.honba = honba;
            this.kyotaku = kyotaku;
            this.oya = oya;
            this.dora_marker = dora_marker;
            this.tehais = tehais;
        }
    }

    public class MJsonMessageTsumo : MjsonMessageBase
    {
        public int actor;
        public string pai;
        public MJsonMessageTsumo(int actor, string pai)
        {
            type = MsgType.TSUMO;
            this.actor = actor;
            this.pai = pai;
        }

    }

    public class MJsonMessageDahai : MjsonMessageBase
    {
       
        public int actor;
        public string pai;
        public bool tsumogiri;
        public MJsonMessageDahai(int actor, string pai,bool tsumogiri)
        {
            type = MsgType.DAHAI;
            this.actor = actor;
            this.pai = pai;
            this.tsumogiri = tsumogiri;
        }

    }

    public class MJsonMessagePon : MjsonMessageBase
    {
        public int actor;
        public int target;
        public string pai;
        public List<string> consumed;
        public MJsonMessagePon(int actor, int target, string pai, List<string> consumed)
        {
            type = MsgType.PON;
            this.actor = actor;
            this.target = target;
            this.pai = pai;
            this.consumed = consumed;
        }
    }
    public class MJsonMessageChi : MjsonMessageBase
    {
        public int actor;
        public int target;
        public string pai;
        public List<string> consumed;
        public MJsonMessageChi(int actor, int target, string pai, List<string> consumed)
        {
            type = MsgType.CHI;
            this.actor = actor;
            this.target = target;
            this.pai = pai;
            this.consumed = consumed;
        }
    }
    public class MJsonMessageKakan : MjsonMessageBase
    {
        public int actor;
        public string pai;
        public List<string> consumed;
        public MJsonMessageKakan(int actor,string pai, List<string> consumed)
        {
            type = MsgType.KAKAN;
            this.pai = pai;
            this.consumed = consumed;
        }
    }
    public class MJsonMessageAnkan : MjsonMessageBase
    {
        public int actor;
        public string pai;
        public List<string> consumed;
        public MJsonMessageAnkan(int actor, string pai, List<string> consumed)
        {
            type = MsgType.ANKAN;
            this.actor = actor;
            this.pai = pai;
            this.consumed = consumed;
        }
    }
    public class MJsonMessageDaiminkan : MjsonMessageBase
    {
        public int actor;
        public int target;
        public string pai;
        public List<string> consumed;
        public MJsonMessageDaiminkan(int actor, int target, string pai, List<string> consumed)
        {
            type = MsgType.DAIMINKAN;
            this.actor = actor;
            this.target = target;
            this.pai = pai;
            this.consumed = consumed;
        }
    }

    public class MJsonMessageReach : MjsonMessageBase
    {
        public int actor;

        public MJsonMessageReach(int actor)
        {
            type = MsgType.REACH;
            this.actor = actor;
        }
    }
    public class MJsonMessageReachAccept : MjsonMessageBase
    {
        public int actor;
        public List<int> deltas;
        public List<int> scores;

        public MJsonMessageReachAccept(int actor,List<int> deltas,List<int> scores)
        {
            type = MsgType.REACH_ACCEPTED;
            this.actor = actor;
            this.deltas = deltas;
            this.scores = scores;
        }
    }

    public class MJsonMessageHora : MjsonMessageBase
    {
     
        public int actor;
        public int target;
        public string pai;
        public List<string> uradora_markers;
        public List<string> hora_tehais;
        public List<List<object>> yakus;
        public int fu;
        public int fan;
        public int hora_points;
        public List<int> deltas;
        public List<int> scores;

        public MJsonMessageHora(int actor,int target, string pai, List<string> uradora_markers, List<string> hora_tehais, List<List<object>> yakus, int fu, int fan, int hora_points, List<int> deltas, List<int> scores)
        {
            type = MsgType.HORA;
            this.actor = actor;
            this.target = target;
            this.pai = pai;
            this.uradora_markers = uradora_markers;
            this.hora_tehais = hora_tehais;
            this.yakus = yakus;
            this.fu = fu;
            this.fan = fan;
            this.hora_points = hora_points;
            this.deltas = deltas;
            this.scores = scores;
        }
        public MJsonMessageHora(int actor, int target, string pai)
        {
            type = MsgType.HORA;
            this.actor = actor;
            this.target = target;
            this.pai = pai;
        }

    }
    


        public class MJsonMessageRyukyoku : MjsonMessageBase
    {
        
        public string reason;
        public List<List<string>> tehais;
        public List<bool> tenpais;
        public List<int> deltas;
        public List<int> scores;

        public MJsonMessageRyukyoku(string reason, List<List<string>> tehais, List<bool> tenpais, List<int> deltas, List<int> scores)
        {
            type = MsgType.RYUKYOKU;
            this.reason = reason;
            this.tehais = tehais;
            this.tenpais = tenpais;
            this.deltas = deltas;
            this.scores = scores;
        }
    }
    public class MJsonMessageDora : MjsonMessageBase
    {
        
        public string dora_marker;
        public MJsonMessageDora(string dora_marker)
        {
            type = MsgType.DORA;
            this.dora_marker = dora_marker;
        }
    }

    public class MJsonMessageNone : MjsonMessageBase
    {
        
        public MJsonMessageNone()
        {
            type = MsgType.NONE;
        }
    }

    public class MJsonMessageEndkyoku : MjsonMessageBase
    {
        
        public MJsonMessageEndkyoku()
        {
            type = MsgType.END_KYOKU;
        }
    }

    public class MJsonMessageEndgame : MjsonMessageBase
    {
        public MJsonMessageEndgame()
        {
            type = MsgType.END_GAME;
        }
    }

    public class MJsonMessageError : MjsonMessageBase
    {
        string error_message;
        public MJsonMessageError(string message)
        {
            type = MsgType.ERROR;
            this.error_message = message;
        }
    }


    //サーバー側での受信メッセージパース用
    public class MjsonMessageAll : MjsonMessageBase
    {
        
        public string name;
        public string room;
        public int id;
        public List<string> names;
        public string bakaze;
        public int kyoku;
        public int honba;
        public int kyotaku;
        public int oya;
        public string dora_marker;

        public int actor;
        public bool tsumogiri;
        public int target;
        public string pai;
        public List<string> consumed;


        public List<int> details;//点数移動
        public List<int> scores;//点数移動結果
        public List<string> uradora_markers;
        public List<string> hora_tehais;
        public List<List<object>> yakus;
        public List<bool> tenpais;
        public int fu;
        public int fan;
        public int hora_points;
        public string reason;
        public List<List<string>> tehais;
        public List<int> deltas;

    }
}
