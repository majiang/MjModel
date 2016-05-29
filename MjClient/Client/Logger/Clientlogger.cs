using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace MjClient.Logger
{
    public class ClientLogger
    {
        private static ClientLogger singletonClientLogger = new ClientLogger();
        public bool EnableLog;

        public static ClientLogger GetInstance()
        {
            return singletonClientLogger;
        }

        private ClientLogger() { }

        /// <summary>
        /// log for client as follows
        /// clientLog:<message>
        /// </summary>
        /// <param name="message">log message</param>
        public void Log(string message)
        {
            if (EnableLog)
            {
                Debug.WriteLine("clientLog:{0}", message);
            }
        }
    }
}
