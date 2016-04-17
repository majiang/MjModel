using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MjNetworkProtocolLibrary;

namespace MjServer
{
    // server action is decisioned by clients action
    public delegate bool StartKyokuHandler();
    public delegate bool TsumoHandler();
    public delegate bool DahaiHandler(int actor, string pai, bool tsumogiri);
    public delegate bool ChiHandler(int actor, int target, string pai, List<string> consumed);
    public delegate bool PonHandler(int actor, int target, string pai, List<string> consumed);
    public delegate bool KakanHandler(int actor, string pai, List<string> consumed);
    public delegate bool DaiminkanHandler(int actor, int target, string pai, List<string> consumed);
    public delegate bool AnkanHandler(int actor, List<string> consumed);
    public delegate bool OpenDoraHandler();
    public delegate bool RinshanHandler();
    public delegate bool ReachHandler(int actor);
    public delegate bool ReachDahaiHandler(int actor, string pai, bool tsumogiri);
    public delegate bool ReachAcceptHandler();
    public delegate bool HoraHandler(int actor, int target, string pai);
    public delegate bool RyukyokuHandler();
    public delegate bool EndKyokuHandler();
    public delegate bool EndGameHandler();

    public class GameContext
    {


        GameState gameState;

        List<MJsonMessageAll> messageList;
//        public event GameStart GameStartHandler;
        MJsonMessageAll ReachActionBuffer;


        public GameContext()
        {
            gameState = AfterInitialiseState.GetInstance();
        }     
        
        public bool ValidateMessage(MJsonMessageAll msg)
        {
            return gameState.ValidateMessage(msg);
        }

        public void RegisterMessage(MJsonMessageAll msg)
        {
            messageList.Add(msg);
        }

        public bool HasRecievedMessageFromAllClients()
        {
            return messageList.Count == MjModelLibrary.Constants.PLAYER_NUM;
        }

        public bool ExecuteAction()
        {
            return gameState.ExecuteAction(this, messageList);
        }
        



        public List<MJsonMessageAll> GetMessageList()
        {
            return messageList;
        }
 
        // overrode
        /*
        public void ChangeState(MJsonMessageTsumo msg)
        {
            gameState = AfterTsumoState.GetInstance();
            gameState.SetLastActor(msg.actor);
        }
        public void ChangeState(MJsonMessageTsumo msg)
        {
            gameState = AfterTsumoState.GetInstance();
            gameState.SetLastActor(msg.actor);
        }
        public void ChangeState(MJsonMessageTsumo msg)
        {
            gameState = AfterTsumoState.GetInstance();
            gameState.SetLastActor(msg.actor);
        }
        public void ChangeState(MJsonMessageTsumo msg)
        {
            gameState = AfterTsumoState.GetInstance();
            gameState.SetLastActor(msg.actor);
        }
        public void ChangeState(MJsonMessageTsumo msg)
        {
            gameState = AfterTsumoState.GetInstance();
            gameState.SetLastActor(msg.actor);
        }
        public void ChangeState(MJsonMessageTsumo msg)
        {
            gameState = AfterTsumoState.GetInstance();
            gameState.SetLastActor(msg.actor);
        }
        */
    }


}
