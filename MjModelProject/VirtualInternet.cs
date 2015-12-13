using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace MjModelProject
{
    public class VirtualInternet
    {
        Dictionary<IPAddress, RouterInterface> ipaddressRouterDictionary;

        public VirtualInternet()
        {
            ipaddressRouterDictionary = new Dictionary<IPAddress, RouterInterface>();
        }

        public void AddRouter(IPAddress ipaddress, RouterInterface routerinterface){
            ipaddressRouterDictionary.Add(ipaddress, routerinterface);
        }

        public void RoutePacket(Packet packet)
        {
            ipaddressRouterDictionary[packet.toIpAddress].AddPacket(packet);
        }

    }
}
