using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
namespace MjModelProject
{
    public class ServerState
    {

        public ServerController serverController;
        public BlockingCollection<MjsonMessageAll> getMsgList = new BlockingCollection<MjsonMessageAll>();
        public bool canExecute = false;

        public virtual ServerState GetMessage(MjsonMessageAll msgobj) { return this; }
        public virtual ServerState Execute() { return this; }//無くす方向で


    }

    public class AfterInitialiseState : ServerState
    {
   
        public AfterInitialiseState(ServerController sc)
        {
            this.serverController = sc;
        }
        public AfterInitialiseState(ServerState ss)
        {
            this.serverController = ss.serverController;
        }

        public override ServerState GetMessage(MjsonMessageAll msgobj)
        {

            if (msgobj.type == MsgType.JOIN)
            {
                getMsgList.Add(msgobj);

                if ((getMsgList.Count == Constants.PLAYER_NUM))
                {
                    canExecute = true;
                }
            }
            else
            {
                serverController.SendErrorToRoomMember(msgobj);
            }
            return this;
        }

        public override ServerState Execute()
        {
            if (canExecute)
            {
                serverController.StartGame();
                return new AfterStartGameState(this);
            }
            else
            {
                return this;
            }
        }

    }


    public class AfterStartGameState : ServerState
    {
      


        public AfterStartGameState(ServerState ss)
        {
            this.serverController = ss.serverController;
        }

        public override ServerState GetMessage(MjsonMessageAll msgobj)
        {
            
            if (msgobj.type == MsgType.NONE)
            {
                getMsgList.Add(msgobj);
            
                if ((getMsgList.Count == Constants.PLAYER_NUM))
                {
                    canExecute = true;
                }
            }
            else
            {
                //errorhandring
                serverController.SendErrorToRoomMember(msgobj);
            }
            return this;
        }

        public override ServerState Execute()
        {
            if (canExecute)
            {
                serverController.StartKyoku();
                getMsgList.Dispose();
                return new AfterStartKyokuState(this);
            }
            else
            {
                return this;
            }
        }
    }

    public class AfterStartKyokuState : ServerState
    {
   
        public AfterStartKyokuState(ServerState ss)
        {
            this.serverController = ss.serverController;
        }
        public override ServerState GetMessage(MjsonMessageAll msgobj)
        {
            
            if (msgobj.type == MsgType.NONE)
            {
                getMsgList.Add(msgobj);
             
                if ((getMsgList.Count == Constants.PLAYER_NUM))
                {
                    canExecute = true;
                }
            }
            else
            {
                //errorhandring
                serverController.SendErrorToRoomMember(msgobj);
            }
            return this;
        }

        public override ServerState Execute()
        {
            if (canExecute)
            {
                serverController.Tsumo();
                getMsgList.Dispose();
                return new AfterTsumoState(this);
            }
            else
            {
                return this;
            }
        }
    }


    public class AfterTsumoState : ServerState
    {
     
        public AfterTsumoState(ServerState ss)
        {
            this.serverController = ss.serverController;
        }
        public override ServerState GetMessage(MjsonMessageAll msgobj)
        {
            
            if ((msgobj.type == MsgType.DAHAI)
                || (msgobj.type == MsgType.REACH)
                || (msgobj.type == MsgType.ANKAN)
                || (msgobj.type == MsgType.KAKAN)
                || (msgobj.type == MsgType.NONE))
            {
                getMsgList.Add(msgobj);
                
                if (getMsgList.Count == Constants.PLAYER_NUM)
                {
                    
                    if (getMsgList.Count(e => e.type == MsgType.DAHAI) == 1 &&
                        getMsgList.Count(e => e.type == MsgType.NONE) == Constants.PLAYER_NUM - 1)
                    {
                        var dahaiObj = (MjsonMessageAll)getMsgList.First(e => e.type == MsgType.DAHAI);
                        serverController.Dahai(dahaiObj.actor, dahaiObj.pai, dahaiObj.tsumogiri);
                        getMsgList.Dispose();
                        return new AfterDahaiState(this);
                    }
                    else if (getMsgList.Count(e => e.type == MsgType.REACH) == 1 &&
                            getMsgList.Count(e => e.type == MsgType.NONE) == Constants.PLAYER_NUM - 1)
                    {
                        var dahaiObj = (MjsonMessageAll)getMsgList.First(e => e.type == MsgType.REACH);
                        serverController.Reach(dahaiObj.actor);
                        getMsgList.Dispose();
                        return new AfterReachState(this);
                    }

                }
            }
            else
            {
                //error handring
                serverController.SendErrorToRoomMember(msgobj);
            }
            return this;
        }
    }

    public class AfterDahaiState : ServerState
    {

        public AfterDahaiState(ServerState ss)
        {
            this.serverController = ss.serverController;
        }
        public override ServerState GetMessage(MjsonMessageAll msgobj)
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
                    if (getMsgList.Count(e => e.type == MsgType.NONE) == Constants.PLAYER_NUM)
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

                    else if (getMsgList.Count(e => e.type == MsgType.HORA) >= 1)
                    {
                        var horaObj = getMsgList.First(e => e.type == MsgType.HORA);
                        serverController.Hora(horaObj.actor, horaObj.target, horaObj.pai);
                        getMsgList.Dispose();
                        return new AfterHoraState(this);
                    }
                    else if (getMsgList.Count(e => e.type == MsgType.DAIMINKAN) == 1)
                    {
                        var daiminkanObj = getMsgList.First(e => e.type == MsgType.DAIMINKAN);
                        serverController.Daiminkan(daiminkanObj.actor, daiminkanObj.target, daiminkanObj.pai, daiminkanObj.consumed);
                        getMsgList.Dispose();
                        return new AfterDaiminkanState(this);
                    }
                    else if (getMsgList.Count(e => e.type == MsgType.PON) == 1)
                    {
                        var ponObj = getMsgList.First(e => e.type == MsgType.PON);
                        serverController.Pon(ponObj.actor, ponObj.target, ponObj.pai, ponObj.consumed);
                        getMsgList.Dispose();
                        return new AfterTsumoState(this);
                    }
                    else if (getMsgList.Count(e => e.type == MsgType.CHI) == 1)
                    {
                        var chiObj = getMsgList.First(e => e.type == MsgType.CHI);
                        serverController.Chi(chiObj.actor, chiObj.target, chiObj.pai, chiObj.consumed);
                        getMsgList.Dispose();
                        return new AfterTsumoState(this);
                    }

                }
            }
            else
            {
                //errorhandring
                serverController.SendErrorToRoomMember(msgobj);
            }
            return this;
        }
    }




    public class AfterDaiminkanState : ServerState
    {
       
        public AfterDaiminkanState(ServerState ss)
        {
            this.serverController = ss.serverController;
        }
        public override ServerState GetMessage(MjsonMessageAll msgobj)
        {
            
            if (msgobj.type == MsgType.NONE)
            {
                getMsgList.Add(msgobj);
                
                if ((getMsgList.Count == Constants.PLAYER_NUM))
                {
                    canExecute = true;
                }
            }
            else
            {
                //errorhandring
                serverController.SendErrorToRoomMember(msgobj);
            }
            return this;
        }

        public override ServerState Execute()
        {
            if (canExecute)
            {
                serverController.Rinshan();
                getMsgList.Dispose();
                return new AfterDaiminkanRinshanState(this);
            }
            else
            {
                return this;
            }
        }
    }

    public class AfterDaiminkanRinshanState : ServerState
    {
       
        public AfterDaiminkanRinshanState(ServerState ss)
        {
            this.serverController = ss.serverController;
        }
        public override ServerState GetMessage(MjsonMessageAll msgobj)
        {
            
            if (msgobj.type == MsgType.DAHAI && msgobj.actor == serverController.serverMjModel.currentActor)
            {
                getMsgList.Add(msgobj);
                
                if ((getMsgList.Count == 1))
                {
                    canExecute = true;
                }
            }
            else
            {
                //errorhandring
                serverController.SendErrorToRoomMember(msgobj);
            }
            return this;
        }

        public override ServerState Execute()
        {
            if (canExecute)
            {
                serverController.OpenDora();
                return new AfterDaiminkanRinshanDoraOpenState(this, getMsgList);//打俾オブジェクトを受け渡し
            }
            else
            {
                return this;
            }
        }
    }

    public class AfterDaiminkanRinshanDoraOpenState : ServerState
    {

        BlockingCollection<MjsonMessageAll> dahaiMsgList;
        public AfterDaiminkanRinshanDoraOpenState(ServerState ss)
        {
            this.serverController = ss.serverController;
            dahaiMsgList = new BlockingCollection<MjsonMessageAll>();
        }
        public AfterDaiminkanRinshanDoraOpenState(ServerState ss, BlockingCollection<MjsonMessageAll> msgList)
        {
            this.serverController = ss.serverController;
            this.dahaiMsgList = msgList;
        }
        public override ServerState GetMessage(MjsonMessageAll msgobj)
        {
            
            if (msgobj.type == MsgType.NONE)
            {
                getMsgList.Add(msgobj);

                if ((getMsgList.Count == Constants.PLAYER_NUM))
                {
                    canExecute = true;
                }
            }
            else
            {
                //errorhandring
                serverController.SendErrorToRoomMember(msgobj);
            }
            return this;
        }

        public override ServerState Execute()
        {
            if (canExecute)
            {
                var dahaiObj = dahaiMsgList.First();//打俾オブジェクトは1個しかないことを前提
                serverController.Dahai(dahaiObj.actor, dahaiObj.pai, dahaiObj.tsumogiri);
                getMsgList.Dispose();
                return new AfterDahaiState(this);
            }
            else
            {
                return this;
            }
        }
    }


    public class AfterReachState : ServerState
    {
        public AfterReachState(ServerState ss)
        {
            this.serverController = ss.serverController;
        }
        public override ServerState GetMessage(MjsonMessageAll msgobj)
        {

            if ((msgobj.type == MsgType.DAHAI)
              || (msgobj.type == MsgType.NONE))
            {
                if (getMsgList.Count(e => e.type == MsgType.DAHAI) == 1 &&
                    getMsgList.Count(e => e.type == MsgType.NONE) == Constants.PLAYER_NUM - 1)
                {
                    var dahaiObj = (MjsonMessageAll)getMsgList.First(e => e.type == MsgType.DAHAI);
                    serverController.Dahai(dahaiObj.actor, dahaiObj.pai, dahaiObj.tsumogiri);
                    getMsgList.Dispose();
                    return new AfterDahaiState(this);
                    getMsgList.Add(msgobj);
                }
                if ((getMsgList.Count == Constants.PLAYER_NUM))
                {
                    canExecute = true;
                }
            }
            else
            {
                //errorhandring
                serverController.SendErrorToRoomMember(msgobj);
            }
            return this;
        }

        public override ServerState Execute()
        {
            if (canExecute)
            {
                serverController.SendEndkyoku();
                getMsgList.Dispose();

                return new AfterEndKyokuState(this);
            }
            else
            {
                return this;
            }
        }

    }



    public class AfterHoraState : ServerState
    {
        public AfterHoraState(ServerState ss)
        {
            this.serverController = ss.serverController;
        }
        public override ServerState GetMessage(MjsonMessageAll msgobj)
        {

            if (msgobj.type == MsgType.NONE)
            {
                getMsgList.Add(msgobj);
                
                if ((getMsgList.Count == Constants.PLAYER_NUM))
                {
                    canExecute = true;
                }
            }
            else
            {
                //errorhandring
                serverController.SendErrorToRoomMember(msgobj);
            }
            return this;
        }

        public override ServerState Execute()
        {
            if (canExecute)
            {
                serverController.SendEndkyoku();
                getMsgList.Dispose();

                return new AfterEndKyokuState(this);
            }
            else
            {
                return this;
            }
        }

    }

    public class AfterRyukyokuState : ServerState
    {

        public AfterRyukyokuState(ServerState ss)
        {
            this.serverController = ss.serverController;
        }
        public override ServerState GetMessage(MjsonMessageAll msgobj)
        {

            if (msgobj.type == MsgType.NONE)
            {
                getMsgList.Add(msgobj);

                if ((getMsgList.Count == Constants.PLAYER_NUM))
                {
                    canExecute = true;
                }
            }
            else
            {
                //errorhandring
                serverController.SendErrorToRoomMember(msgobj);
            }
            return this;
        }

        public override ServerState Execute()
        {
            if (canExecute)
            {
                serverController.SendEndkyoku();
                getMsgList.Dispose();

                return new AfterEndKyokuState(this);
            }
            else
            {
                return this;
            }
        }
    }

    public class AfterEndKyokuState : ServerState
    {
        public AfterEndKyokuState(ServerState ss)
        {
            this.serverController = ss.serverController;
        }

        public override ServerState GetMessage(MjsonMessageAll msgobj)
        {

            if (msgobj.type == MsgType.NONE)
            {
                getMsgList.Add(msgobj);

                if ((getMsgList.Count == Constants.PLAYER_NUM))
                {
                    canExecute = true;
                }
            }
            else
            {
                //errorhandring
                serverController.SendErrorToRoomMember(msgobj);
            }
            return this;
        }

        public override ServerState Execute()
        {
            if (canExecute)
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
            else
            {
                return this;
            }
        }

    }



    public class EndState : ServerState
    {
        public EndState() { }
        public EndState(ServerState ss)
        {
            this.serverController = ss.serverController;
        }
        public override ServerState GetMessage(MjsonMessageAll msgobj)
        {
            return this;
        }

        public override ServerState Execute()
        {
            return this;
        }
    }




}
