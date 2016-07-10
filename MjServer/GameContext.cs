using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MjNetworkProtocolLibrary;
using Newtonsoft.Json;

namespace MjServer
{


    public class GameContext
    {


        GameState gameState;

        List<MJsonMessageAll> messageList = new List<MJsonMessageAll>();

        protected int lastActor = 0;
        public void SetLastActor(int actor)
        {
            lastActor = actor;
        }

        public GameContext()
        {
            gameState = AfterInitialiseState.GetInstance();

        }     


        
        public bool ValidateMessage(MJsonMessageAll msg)
        {
            return gameState.ValidateMessage(msg, lastActor);
        }


        public bool ExecuteAction()
        {
            return gameState.ExecuteAction(this, messageList);
        }


        public void RegisterMessage(MJsonMessageAll msg)
        {
            messageList.Add(msg);
        }

        public bool HasRecievedMessageFromAllClients()
        {
            return messageList.Count == MjModelLibrary.Constants.PLAYER_NUM;
        }
        
        public string GetMessages()
        {
            StringBuilder sb = new StringBuilder();
            messageList.Select(e => JsonConvert.SerializeObject(e))
                       .ToList()
                       .ForEach(e => sb.Append(e));

            return sb.ToString();
        }

        
        public void ChangeState(MJsonMessageStartKyoku sentMessage)
        {
            gameState = AfterStartKyokuState.GetInstance();
            SetLastActor(sentMessage.oya);
            messageList.Clear();
        }
        public void ChangeState(MJsonMessageTsumo sentMessage)
        {
            gameState = AfterTsumoState.GetInstance();
            SetLastActor(sentMessage.actor);
            messageList.Clear();
        }
        public void ChangeState(MJsonMessageDahai sentMessage, bool isReachDahai)
        {
            if (isReachDahai)
            {
                gameState = AfterReachDahaiState.GetInstance();
            }
            else
            {
                gameState = AfterDahiState.GetInstance();
            }
            SetLastActor(sentMessage.actor);
            messageList.Clear();
        }
        public void ChangeState(MJsonMessagePon sentMessage)
        {
            gameState = AfterPonState.GetInstance();
            SetLastActor(sentMessage.actor);
            messageList.Clear();
        }
        public void ChangeState(MJsonMessageChi sentMessage)
        {
            gameState = AfterChiState.GetInstance();
            SetLastActor(sentMessage.actor);
            messageList.Clear();
        }
        public void ChangeState(MJsonMessageDaiminkan sentMessage)
        {
            gameState = AfterDaiminkanState.GetInstance();
            SetLastActor(sentMessage.actor);
            messageList.Clear();
        }
        public void ChangeState(MJsonMessageAnkan sentMessage)
        {
            gameState = AfterAnkanState.GetInstance();
            SetLastActor(sentMessage.actor);
            messageList.Clear();
        }
        public void ChangeState(MJsonMessageKakan sentMessage)
        {
            gameState = AfterKakanState.GetInstance();
            SetLastActor(sentMessage.actor);
            messageList.Clear();
        }
        public void ChangeState(MJsonMessageDora sentMessage)
        {
            gameState = AfterOpenDoraState.GetInstance();
            messageList.Clear();
        }
        
        public void ChangeState(MJsonMessageHora sentMessage)
        {
            gameState = AfterHoraState.GetInstance();
            messageList.Clear();
        }
        public void ChangeState(MJsonMessageReach sentMessage)
        {
            gameState = AfterReachState.GetInstance();
            SetLastActor(sentMessage.actor);
            messageList.Clear();
        }
        public void ChangeState(MJsonMessageReachAccept sentMessage)
        {
            gameState = AfterReachAcceptState.GetInstance();
            messageList.Clear();
        }
        public void ChangeState(MJsonMessageRyukyoku sentMessage)
        {
            gameState = AfterRyukyokuState.GetInstance();
            messageList.Clear();
        }
        public void ChangeState(MJsonMessageEndkyoku sentMessage)
        {
            gameState = AfterEndKyokuState.GetInstance();
            messageList.Clear();
        }
        public void ChangeState(MJsonMessageEndgame sentMessage)
        {
            gameState = AfterEndGameState.GetInstance();
            messageList.Clear();
        }
        public void ChangeState(MJsonMessageSetScene sentMessage)
        {
            gameState = AfterDahiState.GetInstance();
            SetLastActor(sentMessage.actor);
            messageList.Clear();
        }

        public void PrintState()
        {
            System.Diagnostics.Debug.WriteLine(gameState.GetType().ToString());
        }

        public event StartKyokuHandler StartKyoku;
        public event TsumoHandler Tsumo;
        public event DahaiHandler Dahai;
        public event ChiHandler Chi;
        public event PonHandler Pon;
        public event KakanHandler Kakan;
        public event DaiminkanHandler Daiminkan;
        public event AnkanHandler Ankan;
        public event OpenDoraHandler OpenDora;
        public event RinshanHandler Rinshan;
        public event ReachHandler Reach;
        public event ReachDahaiHandler ReachDahai;
        public event ReachAcceptHandler ReachAccept;
        public event HoraHandler Hora;
        public event RyukyokuHandler Ryukyoku;
        public event EndKyokuHandler Endkyoku;
        public event EndGameHandler EndGame;
        public event CheckIsEndGameHandler CheckIsEndGame;
        public event CheckIsEndKyokuHandler CheckIsRyuKyoku;


        public bool OnStartKyoku()
        {
            return StartKyoku();
        }
        public bool OnTsumo()
        {
            return Tsumo();
        }
        public bool OnDahai(int actor, string pai, bool tsumogiri)
        {
            return Dahai(actor, pai, tsumogiri);
        }
        public bool OnChi(int actor, int target, string pai, List<string> consumed)
        {
            return Chi(actor, target, pai, consumed);
        }
        public bool OnPon(int actor, int target, string pai, List<string> consumed)
        {
            return Pon(actor, target, pai, consumed);
        }
        public bool OnKakan(int actor, string pai, List<string> consumed)
        {
            return Kakan(actor, pai, consumed);
        }
        public bool OnDaiminkan(int actor, int target, string pai, List<string> consumed)
        {
            return Daiminkan(actor, target, pai, consumed);
        }
        public bool OnAnkan(int actor,List<string> consumed)
        {
            return Ankan(actor, consumed);
        }
        public bool OnOpenDora()
        {
            return OpenDora();
        }
        public bool OnRinshan()
        {
            return Rinshan();
        }
        public bool OnReach(int actor)
        {
            return Reach(actor);
        }
        public bool OnReachDahai(int actor, string pai, bool tsumogiri)
        {
            return ReachDahai(actor, pai, tsumogiri);
        }
        public bool OnReachAccept()
        {
            return ReachAccept();
        }
        public bool OnHora(int actor, int target, string pai)
        {
            return Hora(actor, target, pai);
        }
        public bool OnRyukyoku()
        {
            return Ryukyoku();
        }
        public bool OnEndKyoku()
        {
            return Endkyoku();
        }
        public bool OnEndGame()
        {
            return EndGame();
        }
        public bool OnCheckIsRyukyoku()
        {
            return CheckIsRyuKyoku();
        }
        public bool OnCheckIsEndGame()
        {
            return CheckIsEndGame();
        }
    }


}
