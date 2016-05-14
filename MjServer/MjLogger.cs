using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace MjServer
{
    public class MjLogger
    {
        bool isDebug = true;
        Logger logger;

        public MjLogger()
        {
            if (isDebug)
            {
                logger = new DebugConsoleLogger();
            }
            else
            {
                logger = new FileLogger();
            }
                
        }

        public void Log(string msg)
        {
            logger.Log(msg);
        }

    }

    interface Logger
    {
        void Log(string msg);
    }

    class DebugConsoleLogger : Logger
    {
        public void Log(string msg)
        {
            Debug.WriteLine(msg);
        }
    }

    class FileLogger : Logger
    {
        public void Log(string msg)
        {
            throw new NotImplementedException();
        }
    }

}
