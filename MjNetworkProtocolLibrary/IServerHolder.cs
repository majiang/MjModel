using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MjNetworkProtocolLibrary
{
    public delegate void GetMessageHandler(string message);
    // save server infomation on client side
    public interface IServerHolder
    {

        event GetMessageHandler OnGetMessageFromServer;
        void GetMessageFromServer(string message);
        void MakeConnection();
        void SendMessageToServer(string message);
        void SendMJsonObject(MJsonMessageJoin jsonmsg);
        void SendMJsonObject(MJsonMessagePon jsonmsg);
        void SendMJsonObject(MJsonMessageChi jsonmsg);
        void SendMJsonObject(MJsonMessageDaiminkan jsonmsg);
        void SendMJsonObject(MJsonMessageNone jsonmsg);
        void SendMJsonObject(MJsonMessageHora jsonmsg);
        void SendMJsonObject(MJsonMessageDahai jsonmsg);
        void SendMJsonObject(MJsonMessageAnkan jsonmsg);
        void SendMJsonObject(MJsonMessageReach jsonmsg);
        void SendMJsonObject(MJsonMessageKakan jsonmsg);
    }
}
