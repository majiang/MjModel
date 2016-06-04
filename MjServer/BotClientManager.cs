using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MjClient;

namespace MjServer
{
    public class BotClientManager
    {
        public void GenerateClient(string roomName, int needNum)
        {
            for(int i = 0; i < needNum; i++)
            {
                var client = new ClientFacade("bot_" + roomName, roomName);
                client.SetLoggable(false);
                client.StartClient();
            }
        }
    }
}
