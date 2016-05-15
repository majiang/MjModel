using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MjNetworkProtocolLibrary;
using System.Diagnostics;
using MjModelLibrary;

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
            // hora > daiminkan > pon > chi > others > none
            if (msgList.Exists(e => e.IsHORA()))
            {
                return msgList.Where(e => e.IsHORA())
                              .OrderBy(e => (e.actor > e.target ? e.actor - Constants.PLAYER_NUM : e.actor))// nearest player hora is selected 
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
            else if (msgList.Exists(e=> e.IsNONE() == false))
            {
                return msgList.First(e => e.IsNONE() == false);
            }

            return msgList.First(e => e.IsNONE());

        }
    }

    class AfterInitialiseState : StateBase, GameState
    {
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
            return context.OnStartKyoku();
        }

    }


    class AfterStartKyokuState : StateBase, GameState
    {
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
            return context.OnTsumo();
        }

    }

    class AfterTsumoState : StateBase, GameState
    {

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
                return context.OnReach(nextAction.actor);
            }
            else if (nextAction.IsANKAN())
            {
                return context.OnAnkan(nextAction.actor, nextAction.consumed);
            }
            else if (nextAction.IsKAKAN())
            {
                return context.OnKakan(nextAction.actor, nextAction.pai, nextAction.consumed);
            }
            else if (nextAction.IsHORA())
            {
                return context.OnHora(nextAction.actor, nextAction.target, nextAction.pai);
            }
            else if (nextAction.IsDAHAI())
            {
                return context.OnDahai(nextAction.actor, nextAction.pai, nextAction.tsumogiri);
            }
            else
            {
                // it is error if this line executed;
                Debug.Assert(false);
                return false;
            }

        }



    }


    class AfterDahiState : StateBase, GameState
    {
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

            // check hora
            if (nextAction.IsHORA())
            {
                return context.OnHora(nextAction.actor, nextAction.target, nextAction.pai);
            }
            // check kyokuend
            if ( context.OnCheckIsRyukyoku() )
            {
                return context.OnRyukyoku();
            }
            
            // execute action
            if (nextAction.IsPON())
            {
                return context.OnPon(nextAction.actor, nextAction.target, nextAction.pai, nextAction.consumed);
            }
            else if (nextAction.IsCHI())
            {
                return context.OnChi(nextAction.actor, nextAction.target, nextAction.pai, nextAction.consumed);
            }
            else if (nextAction.IsDAIMINKAN())
            {
                return context.OnDaiminkan(nextAction.actor, nextAction.target, nextAction.pai, nextAction.consumed);
            }
            else if (nextAction.IsNONE())
            {
                return context.OnTsumo();
            }
            else
            {
                Debug.Assert(false);
                return false;  // If this line executed, It is Error ;
            }

        }


    }

    class AfterAnkanState : StateBase, GameState
    {

        static AfterAnkanState state = new AfterAnkanState();
        private AfterAnkanState() { }
        public static GameState GetInstance()
        {
            return state;
        }

        public bool ValidateMessage(MJsonMessageAll msg)
        {
            return msg.IsNONE();
        }
        public bool ExecuteAction(GameContext context, List<MJsonMessageAll> msgList)
        {
            return context.OnOpenDora();
        }
    }

    class AfterKakanState : StateBase, GameState
    {
        static AfterKakanState state = new AfterKakanState();
        private AfterKakanState() { }
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
                return msg.IsNONE() || msg.IsHORA();// for chankan hora
            }
        }
        public bool ExecuteAction(GameContext context, List<MJsonMessageAll> msgList)
        {
            var nextAction = SelectHighPriorityMessage(msgList);

            if (nextAction.IsNONE())
            {
                return context.OnOpenDora();
            }
            else if (nextAction.IsHORA())
            {
                return context.OnHora(nextAction.actor, nextAction.target, nextAction.pai);
            }
            else
            {
                Debug.Assert(false);
                return false;  // If this line executed, It is Error ;
            }
        }


    }


    class AfterPonState : StateBase, GameState
    {

        static AfterPonState state = new AfterPonState();
        private AfterPonState() { }
        public static GameState GetInstance()
        {
            return state;
        }
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
            var nextAction = SelectHighPriorityMessage(msgList);
            return context.OnDahai(nextAction.actor, nextAction.pai, nextAction.tsumogiri);
        }
    }


    class AfterChiState : StateBase, GameState
    {

        static AfterChiState state = new AfterChiState();
        private AfterChiState() { }
        public static GameState GetInstance()
        {
            return state;
        }
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
            var nextAction = SelectHighPriorityMessage(msgList);
            return context.OnDahai(nextAction.actor, nextAction.pai, nextAction.tsumogiri);
        }
    }

    class AfterDaiminkanState : StateBase, GameState
    {

        static AfterDaiminkanState state = new AfterDaiminkanState();
        private AfterDaiminkanState() { }
        public static GameState GetInstance()
        {
            return state;
        }
        public bool ValidateMessage(MJsonMessageAll msg)
        {
            return msg.IsNONE();
        }
        public bool ExecuteAction(GameContext context, List<MJsonMessageAll> msgList)
        {
            return context.OnOpenDora();
        }
    }



    class AfterOpenDoraState : StateBase, GameState
    {

        static AfterOpenDoraState state = new AfterOpenDoraState();
        private AfterOpenDoraState() { }
        public static GameState GetInstance()
        {
            return state;
        }
        public bool ValidateMessage(MJsonMessageAll msg)
        {
            return msg.IsNONE();
        }
        public bool ExecuteAction(GameContext context, List<MJsonMessageAll> msgList)
        {
            return context.OnRinshan();
        }
    }

    class AfterReachState : StateBase, GameState
    {

        static AfterReachState state = new AfterReachState();
        private AfterReachState() { }
        public static GameState GetInstance()
        {
            return state;
        }
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
            return context.OnReachDahai(nextAction.actor, nextAction.pai, nextAction.tsumogiri);
        }
    }

    class AfterReachDahaiState : StateBase, GameState
    {

        static AfterReachDahaiState state = new AfterReachDahaiState();
        private AfterReachDahaiState() { }
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
            if (msgList.Exists(e => e.IsHORA()))
            {
                var nextAction = msgList.First(e => e.IsHORA());
                return context.OnHora(nextAction.actor, nextAction.target, nextAction.pai);
            }
            else
            {
                return context.OnReachAccept();
            }
        }

    }

    class AfterReachAcceptState : StateBase, GameState
    {
        static AfterReachAcceptState state = new AfterReachAcceptState();
        private AfterReachAcceptState() { }
        public static GameState GetInstance()
        {
            return state;
        }
        public bool ValidateMessage(MJsonMessageAll msg)
        {
            return msg.IsNONE();
        }

        public bool ExecuteAction(GameContext context, List<MJsonMessageAll> msgList)
        {

            var nextAction = SelectHighPriorityMessage(msgList);

            if (nextAction.IsNONE())
            {
                return context.OnTsumo();
            }
            else if (nextAction.IsPON())
            {
                return context.OnPon(nextAction.actor, nextAction.target, nextAction.pai, nextAction.consumed);
            }
            else if (nextAction.IsCHI())
            {
                return context.OnChi(nextAction.actor, nextAction.target, nextAction.pai, nextAction.consumed);
            }
            else if (nextAction.IsDAIMINKAN())
            {
                return context.OnDaiminkan(nextAction.actor, nextAction.target, nextAction.pai, nextAction.consumed);
            }
            else
            {
                Debug.Assert(false);
                return false;  // If this line executed, It is Error ;
            }

        }
    }

    class AfterHoraState : StateBase, GameState
    {
        static AfterHoraState state = new AfterHoraState();
        private AfterHoraState() { }
        public static GameState GetInstance()
        {
            return state;
        }
        public bool ValidateMessage(MJsonMessageAll msg)
        {
            return msg.IsNONE();
        }

        public bool ExecuteAction(GameContext context, List<MJsonMessageAll> msgList)
        {
            return context.OnEndKyoku();
        }
    }

    class AfterRyukyokuState : StateBase, GameState
    {
        static AfterRyukyokuState state = new AfterRyukyokuState();
        private AfterRyukyokuState() { }
        public static GameState GetInstance()
        {
            return state;
        }
        public bool ValidateMessage(MJsonMessageAll msg)
        {
            return msg.IsNONE();
        }

        public bool ExecuteAction(GameContext context, List<MJsonMessageAll> msgList)
        {
            return context.OnEndKyoku();
        }
    }

    class AfterEndKyokuState : StateBase, GameState
    {
        static AfterEndKyokuState state = new AfterEndKyokuState();
        private AfterEndKyokuState() { }
        public static GameState GetInstance()
        {
            return state;
        }
        public bool ValidateMessage(MJsonMessageAll msg)
        {
            return msg.IsNONE();
        }

        public bool ExecuteAction(GameContext context, List<MJsonMessageAll> msgList)
        {
            if (context.OnCheckIsEndGame())
            {
                return context.OnEndGame();
            }
            else
            {
                return context.OnStartKyoku();
            }
        }
    }

    class AfterEndGameState : StateBase, GameState
    {
        static AfterEndGameState state = new AfterEndGameState();
        private AfterEndGameState() { }
        public static GameState GetInstance()
        {
            return state;
        }

        public bool ValidateMessage(MJsonMessageAll msg)
        {
            return msg.IsNONE();
        }

        public bool ExecuteAction(GameContext context, List<MJsonMessageAll> msgList)
        {
            Debug.Assert(false);
            return false;
        }
    }

}
