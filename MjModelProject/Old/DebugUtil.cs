using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace OldMjServer
{
    public static class DebugUtil
    {
        public static void ServerDebug(string msg)
        {
            Debug.WriteLine("Server:"+msg);
        }

    }
}
