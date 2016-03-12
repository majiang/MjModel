using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MjClient.Client.Logger
{
    class ClientLogger
    {
        static ClientLogger singletonClientLogger;

        private ClientLogger(){}

        public static ClientLogger GetInstance()
        {
            if(singletonClientLogger == null)
            {
                singletonClientLogger = new ClientLogger();
            }

            return singletonClientLogger;
        }


        /// <summary>
        /// log for client as follows
        /// clientLog:<message>
        /// </summary>
        /// <param name="message">log message</param>
        public static void Log(string message)
        {
            Console.WriteLine("clientLog:{0}",message);
        }
    }
}
