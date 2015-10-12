using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MjModelProject
{
    public class ServerState
    {
        public ServerController serverController;
        public List<MjsonMessageAll> getMsgList = new List<MjsonMessageAll>();
        public bool canExecute = false;

        public virtual ServerState GetMessage(MjsonMessageAll msgobj) { return this; }
        public virtual ServerState Execute() { return this; }


    }

    public class AfterInitialiseState : ServerState
    {
        private int joinNum;
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
            getMsgList.Add(msgobj);
            if (msgobj.type == MsgType.JOIN)
            {
                joinNum++;
                if ((getMsgList.Count == Constants.PLAYER_NUM) && (joinNum == Constants.PLAYER_NUM))
                {
                    canExecute = true;
                }
            }
            else
            {
                //errorhandring
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
        private int noneNum = 0;


        public AfterStartGameState(ServerState ss)
        {
            this.serverController = ss.serverController;
        }

        public override ServerState GetMessage(MjsonMessageAll msgobj)
        {
            getMsgList.Add(msgobj);
            if (msgobj.type == MsgType.NONE)
            {
                noneNum++;
                if ((getMsgList.Count == Constants.PLAYER_NUM) && (noneNum == Constants.PLAYER_NUM))
                {
                    canExecute = true;
                }
            }
            else
            {
                //errorhandring
            }
            return this;
        }

        public override ServerState Execute()
        {
            if (canExecute)
            {
                serverController.StartKyoku();
                getMsgList.Clear();
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
        private int noneNum = 0;
        public AfterStartKyokuState(ServerState ss)
        {
            this.serverController = ss.serverController;
        }
        public override ServerState GetMessage(MjsonMessageAll msgobj)
        {
            getMsgList.Add(msgobj);
            if (msgobj.type == MsgType.NONE)
            {
                noneNum++;
                if ((getMsgList.Count == Constants.PLAYER_NUM) && (noneNum == Constants.PLAYER_NUM))
                {
                    canExecute = true;
                }
            }
            else
            {
                //errorhandring
            }
            return this;
        }

        public override ServerState Execute()
        {
            if (canExecute)
            {
                serverController.Tsumo();
                getMsgList.Clear();
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
            getMsgList.Add(msgobj);
            if ((msgobj.type == MsgType.DAHAI) || (msgobj.type == MsgType.NONE))
            {
                if (getMsgList.Count == Constants.PLAYER_NUM)
                {
                    
                    if (getMsgList.Count(e => e.type == MsgType.DAHAI) == 1 &&
                        getMsgList.Count(e => e.type == MsgType.NONE) == Constants.PLAYER_NUM - 1)
                    {
                        var dahaiObj = (MjsonMessageAll)getMsgList.First(e => e.type == MsgType.DAHAI);
                        serverController.Dahai(dahaiObj.actor, dahaiObj.pai, dahaiObj.tsumogiri);
                        getMsgList.Clear();
                        return new AfterDahaiState(this);
                    }
                }
            }
            else
            {
                //error handring
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
            getMsgList.Add(msgobj);
            if ((msgobj.type == MsgType.NONE)
                || (msgobj.type == MsgType.PON)
                || (msgobj.type == MsgType.DAIMINKAN)
                || (msgobj.type == MsgType.CHI)
                || (msgobj.type == MsgType.HORA))
            {
                if (getMsgList.Count == Constants.PLAYER_NUM)
                {
                    if (getMsgList.Count(e => e.type == MsgType.NONE) == Constants.PLAYER_NUM)
                    {
                        //終局判定
                        if (serverController.CanFinishKyoku())
                        {
                            serverController.Ryukyoku();
                            getMsgList.Clear();
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
                        getMsgList.Clear();
                        return new AfterDaiminkanState(this);
                    }
                    else if (getMsgList.Count(e => e.type == MsgType.DAIMINKAN) == 1)
                    {
                        var daiminkanObj = getMsgList.First(e => e.type == MsgType.DAIMINKAN);
                        serverController.Daiminkan(daiminkanObj.actor, daiminkanObj.target, daiminkanObj.pai, daiminkanObj.consumed);
                        getMsgList.Clear();
                        return new AfterDaiminkanState(this);
                    }
                    else if (getMsgList.Count(e => e.type == MsgType.PON) == 1)
                    {
                        var ponObj = getMsgList.First(e => e.type == MsgType.PON);
                        serverController.Pon(ponObj.actor, ponObj.target, ponObj.pai, ponObj.consumed);
                        getMsgList.Clear();
                        return new AfterTsumoState(this);
                    }
                    else if (getMsgList.Count(e => e.type == MsgType.CHI) == 1)
                    {
                        var chiObj = getMsgList.First(e => e.type == MsgType.CHI);
                        serverController.Chi(chiObj.actor, chiObj.target, chiObj.pai, chiObj.consumed);
                        getMsgList.Clear();
                        return new AfterTsumoState(this);
                    }

                }
            }
            else
            {
                //errorhandring
            }
            return this;
        }
    }

    public class AfterRyukyokuState : ServerState
    {
        int noneNum = 0;
        public AfterRyukyokuState(ServerState ss)
        {
            this.serverController = ss.serverController;
        }
        public override ServerState GetMessage(MjsonMessageAll msgobj)
        {
            getMsgList.Add(msgobj);
            if (msgobj.type == MsgType.NONE)
            {
                noneNum++;
                if ((getMsgList.Count == Constants.PLAYER_NUM) && (noneNum == Constants.PLAYER_NUM))
                {
                    canExecute = true;
                }
            }
            else
            {
                //errorhandring
            }
            return this;
        }

        public override ServerState Execute()
        {
            if (canExecute)
            {
                serverController.SendEndkyoku();
                getMsgList.Clear();

                return new END_STATE(this);
            }
            else
            {
                return this;
            }
        }
    }


    public class AfterDaiminkanState : ServerState
    {
        int noneNum = 0;
        public AfterDaiminkanState(ServerState ss)
        {
            this.serverController = ss.serverController;
        }
        public override ServerState GetMessage(MjsonMessageAll msgobj)
        {
            getMsgList.Add(msgobj);
            if (msgobj.type == MsgType.NONE)
            {
                noneNum++;
                if ((getMsgList.Count == Constants.PLAYER_NUM) && (noneNum == Constants.PLAYER_NUM))
                {
                    canExecute = true;
                }
            }
            else
            {
                //errorhandring
            }
            return this;
        }

        public override ServerState Execute()
        {
            if (canExecute)
            {
                serverController.Rinshan();
                getMsgList.Clear();
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
        int noneNum = 0;
        public AfterDaiminkanRinshanState(ServerState ss)
        {
            this.serverController = ss.serverController;
        }
        public override ServerState GetMessage(MjsonMessageAll msgobj)
        {
            getMsgList.Add(msgobj);
            if (msgobj.type == MsgType.DAHAI && msgobj.actor == serverController.serverMjModel.currentActor)
            {
                noneNum++;
                if ((getMsgList.Count == 1) && (noneNum == 1))
                {
                    canExecute = true;
                }
            }
            else
            {
                //errorhandring
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
        int noneNum = 0;
        List<MjsonMessageAll> dahaiMsgList;
        public AfterDaiminkanRinshanDoraOpenState(ServerState ss)
        {
            this.serverController = ss.serverController;
            dahaiMsgList = new List<MjsonMessageAll>();
        }
        public AfterDaiminkanRinshanDoraOpenState(ServerState ss, List<MjsonMessageAll> msgList)
        {
            this.serverController = ss.serverController;
            this.dahaiMsgList = msgList;
        }
        public override ServerState GetMessage(MjsonMessageAll msgobj)
        {
            getMsgList.Add(msgobj);
            if (msgobj.type == MsgType.NONE)
            {
                noneNum++;
                if ((getMsgList.Count == Constants.PLAYER_NUM) && (noneNum == Constants.PLAYER_NUM))
                {
                    canExecute = true;
                }
            }
            else
            {
                //errorhandring
            }
            return this;
        }

        public override ServerState Execute()
        {
            if (canExecute)
            {
                var dahaiObj = dahaiMsgList.First();//打俾オブジェクトは1個しかないことを前提
                serverController.Dahai(dahaiObj.actor, dahaiObj.pai, dahaiObj.tsumogiri);
                getMsgList.Clear();
                return new AfterDahaiState(this);
            }
            else
            {
                return this;
            }
        }
    }



    public class END_STATE : ServerState
    {
        public END_STATE() { }
        public END_STATE(ServerState ss)
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
