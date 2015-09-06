using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MjModelProject
{
    public class Server
    {
        public ServerRouter serverRouter;
        public MjModel model;

        public Server()
        {
            serverRouter = new ServerRouter();
            model = new MjModel();
        }
    }
}
