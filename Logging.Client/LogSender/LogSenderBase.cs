using System;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.Net.Sockets;

namespace Logging.Client
{
    internal abstract class LogSenderBase
    {
        protected static int SENDER_TIMEOUT = 5000;

        public abstract void Send(IList<LogEntity> logEntities);


        #region 私有成员

        private static long serverIPNum;


        protected static long ServerIPNum
        {
            get
            {
                if (serverIPNum <= 0)
                {
                    string serverIP = GetServerIP();
                    serverIPNum = Utils.IPToNumber(serverIP);
                }
                return serverIPNum;
            }
        }

        /// <summary>
        /// 获取服务器IP
        /// </summary>
        /// <returns></returns>
        private static string GetServerIP()
        {
            string str = "127.0.0.1";
            try
            {
                string hostName = Dns.GetHostName();
                var hostEntity = Dns.GetHostEntry(hostName);
                var ipAddressList = hostEntity.AddressList;
                var ipAddress = ipAddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);

                if (ipAddress != null)
                {
                    str = ipAddress.ToString();
                }
                return str;
            }
            catch (Exception) { str = string.Empty; }
            return str;
        }

        #endregion 私有成员

    }
}