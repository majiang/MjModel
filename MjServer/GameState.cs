using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MjNetworkProtocolLibrary;
namespace MjServer
{
    interface GameState
    {
        bool ValidateMessage(MJsonMessageAll msg);
        bool ExecuteAction(GameContext context, List<MJsonMessageAll> msgList);
        void SetLastActor(int actor);
    }
    class StateBase
    {
        protected int lastActor = 0;
        public void SetLastActor(int actor)
        {
            lastActor = actor;
        }

        protected MJsonMessageAll SelectHighPriorityMessage(List<MJsonMessageAll> msgList)
        {
            // priority is as follows.
            // hora > daiminkan > pon > chi > none
            if (msgList.Exists(e => e.IsHORA()))
            {
                return msgList.Where(e => e.IsHORA())
                              .OrderBy(e => (e.actor > e.target ? e.actor - 4 : e.actor))// sort by ATAMAHANE
                              .First();
            }
            else if (msgList.Exists(e => e.IsDAIMINKAN()))
            {
                return msgList.First(e => e.IsDAIMINKAN());
            }
            else if (msgList.Exists(e => e.IsPON()))
            {
                return msgList.First(e => e.IsPON());
            }
            else if (msgList.Exists(e => e.IsCHI()))
            {
                return msgList.First(e => e.IsCHI());
            }

            return msgList.First(e => e.IsNONE());

        }
    }

    class AfterInitialiseState : StateBase, GameState
    {
        public static event StartKyokuHandler OnStartKyoku;


        static AfterInitialiseState state = new AfterInitialiseState();
        private AfterInitialiseState() { }
        public static GameState GetInstance()
        {
            return state;
        }

        public bool ValidateMessage(MJsonMessageAll msg)
        {
            return msg.IsNONE();
        }

        public bool ExecuteAction(GameContext context, List<MJsonMessageAll> msg)
        {
            return OnStartKyoku();
        }

    }


    class AfterStartKyokuState : StateBase, GameState
    {
        public static event TsumoHandler OnTsumo;
        static GameState state = new AfterStartKyokuState();
        private AfterStartKyokuState() { }

        public static GameState GetInstance()
        {
            return state;
        }

        public bool ValidateMessage(MJsonMessageAll msg)
        {
            return msg.IsNONE();
        }

        public bool ExecuteAction(GameContext context, List<MJsonMessageAll> msg)
        {
            return OnTsumo();
        }

    }

    class AfterTsumoState : StateBase, GameState
    {
        public static event ReachHandler OnReach;
        public static event AnkanHandler OnAnkan;
        public static event KakanHandler OnKakan;
        public static event HoraHandler OnHora;
        public static event DahaiHandler OnDahai;

        static AfterTsumoState state = new AfterTsumoState();
        private AfterTsumoState() { }

        public static GameState GetInstance()
        {
            return state;
        }

        public bool ValidateMessage(MJsonMessageAll msg)
        {
            if (msg.actor == lastActor)
            {
                return msg.IsREACH() || msg.IsANKAN() || msg.IsKAKAN() || msg.IsHORA() || msg.IsDAHAI();
            }
            else
            {
                return msg.IsNONE();
            }

        }

        public bool ExecuteAction(GameContext context, List<MJsonMessageAll> msgList)
        {

            var nextAction = SelectHighPriorityMessage(msgList);

            if (nextAction.IsREACH())
            {
                return OnReach(nextAction.actor);
            }
            else if (nextAction.IsANKAN())
            {
                return OnAnkan(nextAction.actor, nextAction.consumed);
            }
            else if (nextAction.IsKAKAN())
            {
                return OnKakan(nextAction.actor, nextAction.pai, nextAction.consumed);
            }
            else if (nextAction.IsHORA())
            {
                return OnHora(nextAction.actor, nextAction.target, nextAction.pai);
            }
            else if (nextAction.IsDAHAI())
            {
                return OnDahai(nextAction.actor, nextAction.pai, nextAction.tsumogiri);
            }
            else
            {
                // it is error if this line executed;
                return false;
            }

        }



    }


    class AfterDahiState : StateBase, GameState
    {
        public static event TsumoHandler OnTsumo;
        public static event PonHandler OnPon;
        public static event ChiHandler OnChi;
        public static event HoraHandler OnHora;
        public static event DaiminkanHandler OnDaiminkan;



        static AfterDahiState state = new AfterDahiState();
        private AfterDahiState() { }

        public static GameState GetInstance()
        {
            return state;
        }

        public bool ValidateMessage(MJsonMessageAll msg)
        {
            if (msg.actor == lastActor)
            {
                return msg.IsNONE();
            }
            else
            {
                return msg.IsNONE() || msg.IsPON() || msg.IsCHI() || msg.IsHORA() || msg.IsDAIMINKAN();
            }

        }

        public bool ExecuteAction(GameContext context, List<MJsonMessageAll> msgList)
        {

            var nextAction = SelectHighPriorityMessage(msgList);

            if (nextAction.IsNONE())
            {
                return OnTsumo();
            }
            else if (nextAction.IsPON())
            {
                return OnPon(nextAction.actor, nextAction.target, nextAction.pai, nextAction.consumed);
            }
            else if (nextAction.IsCHI())
            {
                return OnChi(nextAction.actor, nextAction.target, nextAction.pai, nextAction.consumed);
            }
            else if (nextAction.IsHORA())
            {
                return OnHora(nextAction.actor, nextAction.target, nextAction.pai);
            }
            else if (nextAction.IsDAIMINKAN())
            {
                return OnDaiminkan(nextAction.actor, nextAction.target, nextAction.pai, nextAction.consumed);
            }
            else
            {
                return false;  // If this line executed, It is Error ;
            }

        }


    }

    class AfterAnKanState : StateBase, GameState
    {
        public static event OpenDoraHandler OnOpenDora;

        static AfterAnKanState state = new AfterAnKanState();
        private AfterAnKanState() { }

        public bool ValidateMessage(MJsonMessageAll msg)
        {
            return msg.IsNONE();
        }
        public bool ExecuteAction(GameContext context, List<MJsonMessageAll> msgList)
        {
            return OnOpenDora();
        }
    }

    class AfterKaKanState : StateBase, GameState
    {
        public static event OpenDoraHandler OnOpenDora;
        public static event HoraHandler OnHora;

        static AfterKaKanState state = new AfterKaKanState();

        public bool ValidateMessage(MJsonMessageAll msg)
        {
            if (msg.actor == lastActor)
            {
                return msg.IsNONE();
            }
            else
            {
                return msg.IsNONE() || msg.IsHORA();
            }
        }
        public bool ExecuteAction(GameContext context, List<MJsonMessageAll> msgList)
        {
            var nextAction = SelectHighPriorityMessage(msgList);

            if (nextAction.IsNONE())
            {
                return OnOpenDora();
            }
            else if (nextAction.IsHORA())
            {
                return OnHora(nextAction.actor, nextAction.target, nextAction.pai);
            }
            else
            {
                return false;  // If this line executed, It is Error ;
            }
        }


    }

    class AfterDaiminKanState : StateBase, GameState
    {
        public static event OpenDoraHandler OnOpenDora;

        static AfterDaiminKanState state = new AfterDaiminKanState();
        private AfterDaiminKanState() { }

        public bool ValidateMessage(MJsonMessageAll msg)
        {
            return msg.IsNONE();
        }
        public bool ExecuteAction(GameContext context, List<MJsonMessageAll> msgList)
        {
            return OnOpenDora();
        }
    }



    class AfterOpenDoraState : StateBase, GameState
    {
        public static event RinshanHandler OnRinshan;

        static AfterOpenDoraState state = new AfterOpenDoraState();

        public bool ValidateMessage(MJsonMessageAll msg)
        {
            return msg.IsNONE();
        }
        public bool ExecuteAction(GameContext context, List<MJsonMessageAll> msgList)
        {
            return OnRinshan();
        }
    }

    class AfterReachState : StateBase, GameState
    {
        public static event ReachDahaiHandler OnReachDahai;

        static AfterReachState state = new AfterReachState();

        public bool ValidateMessage(MJsonMessageAll msg)
        {
            if (msg.actor == lastActor)
            {
                return msg.IsDAHAI();
            }
            else
            {
                return msg.IsNONE();
            }
        }
        public bool ExecuteAction(GameContext context, List<MJsonMessageAll> msgList)
        {
            var nextAction = msgList.First(e => e.IsDAHAI());
            return OnReachDahai(nextAction.actor, nextAction.pai, nextAction.tsumogiri);
        }
    }

    class AfterReachDahaiState : StateBase, GameState
    {
        public static event ReachAcceptHandler OnReachAccept;
        public static event HoraHandler OnHora;

        static AfterReachDahaiState state = new AfterReachDahaiState();
        private AfterReachDahaiState(){}

        public bool ValidateMessage(MJsonMessageAll msg)
        {
            if (msg.actor == lastActor)
            {
                return msg.IsNONE();
            }
            else
            {
                return msg.IsNONE() || msg.IsPON() || msg.IsCHI() || msg.IsHORA() || msg.IsDAIMINKAN();
            }

        }

        public bool ExecuteAction(GameContext context, List<MJsonMessageAll> msgList)
        {
            if ( msgList.Exists(e => e.IsHORA()) )
            {
                var nextAction = msgList.First(e => e.IsHORA());
                return OnHora(nextAction.actor, nextAction.target, nextAction.pai);
            }
            else
            {
                return OnReachAccept();
            }
        }

    }

    class AfterReachAccept : StateBase, GameState
    {
        public static event TsumoHandler OnTsumo;
        public static event PonHandler OnPon;
        public static event ChiHandler OnChi;
        public static event DaiminkanHandler OnDaiminkan;

        static AfterReachAccept state = new AfterReachAccept();
        private AfterReachAccept() { }

        public bool ValidateMessage(MJsonMessageAll msg)
        {
            return msg.IsNONE();
        }
        
        public bool ExecuteAction(GameContext context, List<MJsonMessageAll> msgList)
        {

            var nextAction = SelectHighPriorityMessage(msgList);

            if (nextAction.IsNONE())
            {
                return OnTsumo();
            }
            else if (nextAction.IsPON())
            {
                return OnPon(nextAction.actor, nextAction.target, nextAction.pai, nextAction.consumed);
            }
            else if (nextAction.IsCHI())
            {
                return OnChi(nextAction.actor, nextAction.target, nextAction.pai, nextAction.consumed);
            }
            else if (nextAction.IsDAIMINKAN())
            {
                return OnDaiminkan(nextAction.actor, nextAction.target, nextAction.pai, nextAction.consumed);
            }
            else
            {
                return false;  // If this line executed, It is Error ;
            }

        }
    }

    class AfterHoraState : StateBase, GameState
    {
        public static event EndKyokuHandler OnEndKyoku;
        public static event EndGameHandler OnEndGame;
        public static event CheckEndGameHandler OnCheckEndGame;

        static AfterHoraState state = new AfterHoraState();
        private AfterHoraState() { }

        public bool ValidateMessage(MJsonMessageAll msg)
        {
            return msg.IsNONE();
        }

        public bool ExecuteAction(GameContext context, List<MJsonMessageAll> msgList)
        {
            if ( OnCheckEndGame() )
            {
                return OnEndGame();
            }
            else
            {
                return OnEndKyoku();
            }
        }
    }

    class EndState : StateBase
    {
        static EndState state = new EndState();
        private EndState() { }
    }

}
