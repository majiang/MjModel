using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MjModelProject
{
    public class Client
    {
        public ClientRouter clientRouter;
        public MjModel model;
        public ClientController clientController;

        public Client()
        {
            clientRouter = new ClientRouter();
            model = new MjModel();
            clientController = new ClientController();
        }
    }
}
