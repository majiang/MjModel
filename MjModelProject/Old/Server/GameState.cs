using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
namespace MjServer
{
    public class GameState
    {

        public GameController serverController;
        public BlockingCollection<MjsonMessageAll> getMsgList = new BlockingCollection<MjsonMessageAll>();
        public virtual GameState GetMessage(MjsonMessageAll msgobj) { return this; }

        protected MjsonMessageAll GetNearestHoraPlayerMsg(BlockingCollection<MjsonMessageAll> getMsgList)
        {
            return getMsgList.Where(e => e.IsHORA())
                                .OrderBy(e => (e.actor > e.target ? e.actor - 4 : e.actor))//頭ハネを表現している
                                .First();
        }
    }

    class AfterInitialiseState : GameState
    {
        public AfterInitialiseState(GameController sc)
        {
            this.serverController = sc;
        }
        public AfterInitialiseState(GameState ss)
        {
            this.serverController = ss.serverController;
        }

        public override GameState GetMessage(MjsonMessageAll msgobj)
        {

            if (msgobj.type == MsgType.JOIN)
            {
                getMsgList.Add(msgobj);

                if ((getMsgList.Count == Constants.PLAYER_NUM))
                {
                    serverController.StartGame();
                    return new AfterStartGameState(this);
                }
            }
            else
            {
                serverController.SendErrorToRoomMember(msgobj);
            }
            return this;
        }
    }


    class AfterStartGameState : GameState
    {
      
        public AfterStartGameState(GameState ss)
        {
            this.serverController = ss.serverController;
        }

        public override GameState GetMessage(MjsonMessageAll msgobj)
        {
            
            if (msgobj.type == MsgType.NONE)
            {
                getMsgList.Add(msgobj);
            
                if ((getMsgList.Count == Constants.PLAYER_NUM))
                {
                    serverController.StartKyoku();
                    getMsgList.Dispose();
                    return new AfterStartKyokuState(this);
                }
            }
            else
            {
                //errorhandling
                serverController.SendErrorToRoomMember(msgobj);
            }
            return this;
        }

    }

    class AfterStartKyokuState : GameState
    {
   
        public AfterStartKyokuState(GameState ss)
        {
            this.serverController = ss.serverController;
        }
        public override GameState GetMessage(MjsonMessageAll msgobj)
        {
            
            if (msgobj.type == MsgType.NONE)
            {
                getMsgList.Add(msgobj);
             
                if ((getMsgList.Count == Constants.PLAYER_NUM))
                {
                    serverController.Tsumo();
                    getMsgList.Dispose();
                    return new AfterTsumoState(this);
                }
            }
            else
            {
                //errorhandling
                serverController.SendErrorToRoomMember(msgobj);
            }
            return this;
        }
    }


    class AfterTsumoState : GameState
    {
     
        public AfterTsumoState(GameState ss)
        {
            this.serverController = ss.serverController;
        }
        public override GameState GetMessage(MjsonMessageAll msgobj)
        {
            
            if ((msgobj.IsDAHAI())
                || (msgobj.IsREACH())
                || (msgobj.IsANKAN())
                || (msgobj.IsKAKAN())
                || (msgobj.IsHORA())
                || (msgobj.IsNONE()))
            {
                getMsgList.Add(msgobj);
                
                if (getMsgList.Count == Constants.PLAYER_NUM)
                {

                    //
                    if (getMsgList.Count(e => e.IsHORA()) >= 1 )
                    {
                        var horaObj = GetNearestHoraPlayerMsg(getMsgList);
                        serverController.Hora(horaObj.actor, horaObj.target, horaObj.pai);
                        getMsgList.Dispose();
                        return new AfterHoraState(this);
                    }

                    //RinshanFlggOff
                    //serverController.ResetRinshanFlag();
                    //serverController.ResetIppatsuFlag();

                    //TODO Kakan


                    if (getMsgList.Count(e => e.IsDAHAI()) == 1 &&
                        getMsgList.Count(e => e.IsNONE()) == Constants.PLAYER_NUM - 1)
                    {
                        var dahaiObj = getMsgList.Where(e => e.IsDAHAI())
                            .First();
                        serverController.Dahai(dahaiObj.actor, dahaiObj.pai, dahaiObj.tsumogiri);
                        getMsgList.Dispose();
                        return new AfterDahaiState(this);
                    }
                    else if (getMsgList.Count(e => e.IsREACH()) == 1 &&
                            getMsgList.Count(e => e.IsNONE()) == Constants.PLAYER_NUM - 1)
                    {
                        var reachObj = getMsgList.First(e => e.IsREACH());
                        serverController.Reach(reachObj.actor);
                        getMsgList.Dispose();
                        return new AfterReachState(this);
                    }
                    else if (getMsgList.Count(e => e.IsANKAN()) == 1 &&
                            getMsgList.Count(e => e.IsNONE()) == Constants.PLAYER_NUM - 1)
                    {
                        var kanObj = getMsgList.First(e => e.IsREACH());
                        serverController.Ankan(kanObj.actor, kanObj.pai, kanObj.consumed);
                        getMsgList.Dispose();
                        return new AfterKanState(this);
                    }

                }
            }
            else
            {
                //error handling
                serverController.SendErrorToRoomMember(msgobj);
            }
            return this;
        }
    }

    class AfterDahaiState : GameState
    {

        public AfterDahaiState(GameState ss)
        {
            this.serverController = ss.serverController;
        }
        public override GameState GetMessage(MjsonMessageAll msgobj)
        {
            
            if ((msgobj.type == MsgType.NONE)
                || (msgobj.type == MsgType.PON)
                || (msgobj.type == MsgType.DAIMINKAN)
                || (msgobj.type == MsgType.CHI)
                || (msgobj.type == MsgType.HORA))
            {
                getMsgList.Add(msgobj);
                if (getMsgList.Count == Constants.PLAYER_NUM)
                {
                    //４人共アクションがない場合
                    if (getMsgList.Count(e => e.IsNONE()) == Constants.PLAYER_NUM)
                    {
                        //終局判定
                        if (serverController.CanFinishKyoku())
                        {
                            serverController.Ryukyoku();
                            getMsgList.Dispose();
                            return new AfterRyukyokuState(this);
                        }
                        else
                        {
                            //ツモ状態へ移行
                            serverController.Tsumo();
                            return new AfterTsumoState(this);
                        }
                    }
                    //以下実装予定
                    //hora

                    else if (getMsgList.Count(e => e.IsHORA()) >= 1)
                    {
                        //頭ハネ
                        var horaObj = GetNearestHoraPlayerMsg(getMsgList);

                        serverController.Hora(horaObj.actor, horaObj.target, horaObj.pai);
                        getMsgList.Dispose();
                        return new AfterHoraState(this);
                    }
                    else if (getMsgList.Count(e => e.IsDAIMINKAN()) == 1)
                    {
                        var daiminkanObj = getMsgList.First(e => e.IsDAIMINKAN());
                        serverController.Daiminkan(daiminkanObj.actor, daiminkanObj.target, daiminkanObj.pai, daiminkanObj.consumed);
                        getMsgList.Dispose();
                        return new AfterKanState(this);
                    }
                    else if (getMsgList.Count(e => e.IsPON()) == 1)
                    {
                        var ponObj = getMsgList.First(e => e.IsPON());
                        serverController.Pon(ponObj.actor, ponObj.target, ponObj.pai, ponObj.consumed);
                        getMsgList.Dispose();
                        return new AfterTsumoState(this);
                    }
                    else if (getMsgList.Count(e => e.IsCHI()) == 1)
                    {
                        var chiObj = getMsgList.First(e => e.IsCHI());
                        serverController.Chi(chiObj.actor, chiObj.target, chiObj.pai, chiObj.consumed);
                        getMsgList.Dispose();
                        return new AfterTsumoState(this);
                    }

                }
            }
            else
            {
                //errorhandling
                serverController.SendErrorToRoomMember(msgobj);
            }
            return this;
        }
    }




    class AfterKanState : GameState
    {
       
        public AfterKanState(GameState ss)
        {
            this.serverController = ss.serverController;
        }
        public override GameState GetMessage(MjsonMessageAll msgobj)
        {
            
            if (msgobj.IsNONE())
            {
                getMsgList.Add(msgobj);
                
                if ((getMsgList.Count == Constants.PLAYER_NUM))
                {
                    serverController.OpenDora(); 
                    getMsgList.Dispose();
                    return new AfterOpenDoraState(this);
                }
            }
            else
            {
                //errorhandling
                serverController.SendErrorToRoomMember(msgobj);
            }
            return this;
        }
    }

    class AfterOpenDoraState : GameState
    {
        public AfterOpenDoraState(GameState ss)
        {
            this.serverController = ss.serverController;
        }
        public override GameState GetMessage(MjsonMessageAll msgobj)
        {

            if (msgobj.IsNONE())
            {
                getMsgList.Add(msgobj);

                if ((getMsgList.Count == Constants.PLAYER_NUM))
                {
                    serverController.Rinshan();
                    getMsgList.Dispose();
                    return new AfterTsumoState(this);
                }
            }
            else
            {
                //errorhandling
                serverController.SendErrorToRoomMember(msgobj);
            }
            return this;
        }

    }




    class AfterReachState : GameState
    {
        public AfterReachState(GameState ss)
        {
            this.serverController = ss.serverController;
        }
        public override GameState GetMessage(MjsonMessageAll msgobj)
        {

            if ((msgobj.IsDAHAI())
              || (msgobj.IsNONE()))
            {
                getMsgList.Add(msgobj);

                if (getMsgList.Count(e => e.IsDAHAI()) == 1 &&
                    getMsgList.Count(e => e.IsNONE()) == Constants.PLAYER_NUM - 1)
                {
                    var dahaiObj = getMsgList.First(e => e.IsDAHAI());
                    serverController.Dahai(dahaiObj.actor, dahaiObj.pai, dahaiObj.tsumogiri);
                    getMsgList.Dispose();
                    return new AfterReachDahaiState(this);
                }
                if ((getMsgList.Count == Constants.PLAYER_NUM))
                {
                    serverController.SendErrorToRoomMember(msgobj);
                }
            }
            else
            {
                //errorhandling
                serverController.SendErrorToRoomMember(msgobj);
            }
            return this;
        }

    }


    class AfterReachDahaiState : GameState
    {

        public AfterReachDahaiState(GameState ss)
        {
            this.serverController = ss.serverController;
        }
        public override GameState GetMessage(MjsonMessageAll msgobj)
        {

            if ((msgobj.type == MsgType.NONE)
                || (msgobj.type == MsgType.PON)
                || (msgobj.type == MsgType.DAIMINKAN)
                || (msgobj.type == MsgType.CHI)
                || (msgobj.type == MsgType.HORA))
            {
                getMsgList.Add(msgobj);
                if (getMsgList.Count == Constants.PLAYER_NUM)
                {



                    if(getMsgList.Count(e => e.IsHORA()) > 0)
                    {
                        var horaObj = GetNearestHoraPlayerMsg(getMsgList);
                        serverController.Hora(horaObj.actor, horaObj.target, horaObj.pai);
                        getMsgList.Dispose();
                        return new AfterHoraState(this);
                    }

                    serverController.ReachAccept();
                    return new AfterReachAccceptState(this, getMsgList);
                }
            }
            else
            {
                //errorhandling
                serverController.SendErrorToRoomMember(msgobj);
            }
            return this;
        }
    }

    class AfterReachAccceptState : GameState
    {
        BlockingCollection<MjsonMessageAll> prevMsgList = new BlockingCollection<MjsonMessageAll>();

        public AfterReachAccceptState(GameState ss, BlockingCollection<MjsonMessageAll> getMsgList)
        {
            this.serverController = ss.serverController;
            this.prevMsgList = getMsgList;
        }

        public override GameState GetMessage(MjsonMessageAll msgobj)
        {
             if (msgobj.IsNONE())
            {
                getMsgList.Add(msgobj);
                if(getMsgList.Count < Constants.PLAYER_NUM)
                {
                    return this;
                }

                //受け継いだメッセージ内に４人共アクションがない場合
                if (prevMsgList.Count(e => e.IsNONE()) == Constants.PLAYER_NUM)
                {
                    //終局判定
                    if (serverController.CanFinishKyoku())
                    {
                        serverController.Ryukyoku();
                        getMsgList.Dispose();
                        return new AfterRyukyokuState(this);
                    }
                    else
                    {
                        //ツモ状態へ移行
                        serverController.Tsumo();
                        return new AfterTsumoState(this);
                    }
                }
                //hora
                
                else if (prevMsgList.Count(e => e.IsDAIMINKAN()) == 1)
                {
                    var daiminkanObj = prevMsgList.First(e => e.IsDAIMINKAN());
                    serverController.Daiminkan(daiminkanObj.actor, daiminkanObj.target, daiminkanObj.pai, daiminkanObj.consumed);
                    prevMsgList.Dispose();
                    return new AfterKanState(this);
                }
                else if (prevMsgList.Count(e => e.IsPON()) == 1)
                {
                    var ponObj = prevMsgList.First(e => e.IsPON());
                    serverController.Pon(ponObj.actor, ponObj.target, ponObj.pai, ponObj.consumed);
                    prevMsgList.Dispose();
                    return new AfterTsumoState(this);
                }
                else if (prevMsgList.Count(e => e.IsCHI()) == 1)
                {
                    var chiObj = prevMsgList.First(e => e.IsCHI());
                    serverController.Chi(chiObj.actor, chiObj.target, chiObj.pai, chiObj.consumed);
                    prevMsgList.Dispose();
                    return new AfterTsumoState(this);
                }
            }
            else
            {
                //errorhandling
                serverController.SendErrorToRoomMember(msgobj);
            }
            return this;
        }
    }



    class AfterHoraState : GameState
    {
        public AfterHoraState(GameState ss)
        {
            this.serverController = ss.serverController;
        }
        public override GameState GetMessage(MjsonMessageAll msgobj)
        {

            if (msgobj.IsNONE())
            {
                getMsgList.Add(msgobj);
                
                if ((getMsgList.Count == Constants.PLAYER_NUM))
                {
                    serverController.SendEndkyoku();
                    getMsgList.Dispose();

                    return new AfterEndKyokuState(this);
                }
            }
            else
            {
                //errorhandling
                serverController.SendErrorToRoomMember(msgobj);
            }
            return this;
        }


    }

    class AfterRyukyokuState : GameState
    {

        public AfterRyukyokuState(GameState ss)
        {
            this.serverController = ss.serverController;
        }
        public override GameState GetMessage(MjsonMessageAll msgobj)
        {

            if (msgobj.IsNONE())
            {
                getMsgList.Add(msgobj);

                if ((getMsgList.Count == Constants.PLAYER_NUM))
                {
                    serverController.SendEndkyoku();
                    getMsgList.Dispose();

                    return new AfterEndKyokuState(this);
                }
            }
            else
            {
                //errorhandling
                serverController.SendErrorToRoomMember(msgobj);
            }
            return this;
        }


    }

    class AfterEndKyokuState : GameState
    {
        public AfterEndKyokuState(GameState ss)
        {
            this.serverController = ss.serverController;
        }

        public override GameState GetMessage(MjsonMessageAll msgobj)
        {

            if (msgobj.IsNONE())
            {
                getMsgList.Add(msgobj);

                if ((getMsgList.Count == Constants.PLAYER_NUM))
                {
                    if (serverController.CanEndGame())
                    {
                        serverController.SendEndgame();
                        return new EndState(this);
                    }
                    else
                    {
                        serverController.StartKyoku();
                        return new AfterStartKyokuState(this);
                    }
                }
            }
            else
            {
                //errorhandling
                serverController.SendErrorToRoomMember(msgobj);
            }
            return this;
        }


    }



    class EndState : GameState
    {
        public EndState() { }
        public EndState(GameState ss)
        {
            this.serverController = ss.serverController;
        }
        public override GameState GetMessage(MjsonMessageAll msgobj)
        {
            
            return this;
        }
    }




}
