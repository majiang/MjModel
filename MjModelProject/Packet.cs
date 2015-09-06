using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace MjModelProject
{
    public class Packet
    {
        public IPAddress fromIpAddress;
        public IPAddress toIpAddress;
        public string jsonMessage;
        public Packet(IPAddress fromIp, IPAddress toIp,  string jsonmsg)
        {
            fromIpAddress = fromIp;
            toIpAddress = toIp;
            jsonMessage = jsonmsg;
        }
    }
}
