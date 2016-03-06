using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace MjServer
{
    public class Packet
    {
        public TcpClient TcpClient;
        public IPAddress fromIpAddress;
        public EndPoint fromPort;
        public IPAddress toIpAddress;
        public EndPoint toPort;
        public string JsonMessage;
        public Packet(IPAddress fromIp, IPAddress toIp, string jsonmsg)
        {
            fromIpAddress = fromIp;
            toIpAddress = toIp;
            JsonMessage = jsonmsg;
        }
        public Packet(IPAddress fromIp, EndPoint fromPt, IPAddress toIp, EndPoint toPt, string jsonmsg)
        {
            fromIpAddress = fromIp;
            fromPort = fromPt;
            toIpAddress = toIp;
            toPort = toPt;
            JsonMessage = jsonmsg;
        }
        public Packet(TcpClient client, string jsonmsg)
        {
            TcpClient = client;
            JsonMessage = jsonmsg;
        }
    }
}
