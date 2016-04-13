using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MjNetworkProtocolLibrary;

namespace MjServer
{
    class MessageValidator
    {
        public MessageValidator() { }

        GameContext gameContext = new GameContext();
        



        public void SetAction(MJsonMessageAnkan msg)
        {
            // set agtion ;
            ServerState = AfterAnkanState();
            
        }
        public void SetAction(MJsonMessageChi msg)
        {
            // set agtion ;
            ServerState = AfterChiState();
            
        }
    }
}
