using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace MjServer
{

    class ClientUsingTcpHolder : ClientHolderInterface
    {
        TcpClient tcpClient;
        ClientUsingTcpHolder(TcpClient tcpClient)
        {
            this.tcpClient = tcpClient;
        }

        public event GetMessageFromClient OnGetMessageFromClient;

        public void Disconnect()
        {
            throw new NotImplementedException();
        }

        public void SendMessage(string message)
        {
            throw new NotImplementedException();
        }
    }
}
