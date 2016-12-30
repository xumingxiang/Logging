using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace Logging.Client
{
    internal static class Utils
    {
        //   readonly static DateTime START_TIME = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));

        private static readonly DateTime START_TIME = new System.DateTime(1970, 1, 1).ToLocalTime();

        /// <summary>
        /// DateTime时间格式转换为Unix时间戳格式精确到17位。
        ///
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static long GetTimeTicks(DateTime time)
        {
            return (time - START_TIME).Ticks;
        }

        /// <summary>
        /// DateTime时间格式转换为Unix时间戳格式精确到10位。
        ///
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static long GetUnixTime(DateTime time)
        {
            return (long)(time - START_TIME).TotalSeconds;
        }

        /// <summary>
        /// 将IPv4格式的字符串转换为int型表示
        /// </summary>
        /// <param name="strIPAddress">IPv4格式的字符</param>
        /// <returns></returns>
        public static long IPToNumber(string strIPAddress)
        {
            if (string.IsNullOrWhiteSpace(strIPAddress)) { return 0; }
            string[] arrayIP = strIPAddress.Split('.');
            long sip1 = long.Parse(arrayIP[0]);
            long sip2 = long.Parse(arrayIP[1]);
            long sip3 = long.Parse(arrayIP[2]);
            long sip4 = long.Parse(arrayIP[3]);

            return (sip1 << 24) + (sip2 << 16) + (sip3 << 8) + sip4;
        }

        /// <summary>
        /// 获取服务器IP
        /// </summary>
        /// <returns></returns>
        public static string GetServerIP()
        {
            string str = "127.0.0.1";
            try
            {
                string hostName = Dns.GetHostName();

                var hostEntity = Dns.GetHostEntryAsync(hostName).Result;
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

        ///// <summary>
        ///// 将int型表示的IP还原成正常IPv4格式。
        ///// </summary>
        ///// <param name="intIPAddress">int型表示的IP</param>
        ///// <returns></returns>
        //public static string NumberToIP(long intIPAddress)
        //{
        //    long tempIPAddress;
        //    //将目标整形数字intIPAddress转换为IP地址字符串
        //    //-1062731518 192.168.1.2
        //    //-1062731517 192.168.1.3
        //    if (intIPAddress >= 0)
        //    {
        //        tempIPAddress = intIPAddress;
        //    }
        //    else
        //    {
        //        tempIPAddress = intIPAddress + 1;
        //    }
        //    long s1 = tempIPAddress / 256 / 256 / 256;
        //    long s21 = s1 * 256 * 256 * 256;
        //    long s2 = (tempIPAddress - s21) / 256 / 256;
        //    long s31 = s2 * 256 * 256 + s21;
        //    long s3 = (tempIPAddress - s31) / 256;
        //    long s4 = tempIPAddress - s3 * 256 - s31;
        //    if (intIPAddress < 0)
        //    {
        //        s1 = 255 + s1;
        //        s2 = 255 + s2;
        //        s3 = 255 + s3;
        //        s4 = 255 + s4;
        //    }
        //    string strIPAddress = s1.ToString() + "." + s2.ToString() + "." + s3.ToString() + "." + s4.ToString();
        //    return strIPAddress;
        //}
    }
}