using System;


namespace Logging.Server
{
    public static class Utils
    {
        readonly static DateTime START_TIME = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));

        /// <summary>
        /// DateTime时间格式转换为Unix时间戳格式。精确到17位,即100纳秒
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static long GetTimeStamp(DateTime time)
        {
            return (time - START_TIME).Ticks;
        }


        public static DateTime GetDateTime(long timestamp)
        {
            DateTime newDateTime = START_TIME.AddTicks(timestamp);
            return newDateTime;
        }


        /// <summary>
        /// 将IPv4格式的字符串转换为int型表示
        /// </summary>
        /// <param name="strIPAddress">IPv4格式的字符</param>
        /// <returns></returns>
        public static long IPToNumber(string strIPAddress)
        {

            //System.Net.IPAddress ipaddress = System.Net.IPAddress.Parse("216.20.222.72");
            //long dreamduip = ipaddress.;// 结果 1222513880

            if (string.IsNullOrWhiteSpace(strIPAddress)) { return 0; }

            //将目标IP地址字符串strIPAddress转换为数字
            string[] arrayIP = strIPAddress.Split('.');
            long sip1 = long.Parse(arrayIP[0]);
            long sip2 = long.Parse(arrayIP[1]);
            long sip3 = long.Parse(arrayIP[2]);
            long sip4 = long.Parse(arrayIP[3]);
            long tmpIpNumber;
            tmpIpNumber = sip1 * 256 * 256 * 256 + sip2 * 256 * 256 + sip3 * 256 + sip4;
            return tmpIpNumber;
        }


        /// <summary>
        /// 将int型表示的IP还原成正常IPv4格式。
        /// </summary>
        /// <param name="intIPAddress">int型表示的IP</param>
        /// <returns></returns>
        public static string NumberToIP(long intIPAddress)
        {
            long tempIPAddress;
            //将目标整形数字intIPAddress转换为IP地址字符串
            //-1062731518 192.168.1.2 
            //-1062731517 192.168.1.3 
            if (intIPAddress >= 0)
            {
                tempIPAddress = intIPAddress;
            }
            else
            {
                tempIPAddress = intIPAddress + 1;
            }
            long s1 = tempIPAddress / 256 / 256 / 256;
            long s21 = s1 * 256 * 256 * 256;
            long s2 = (tempIPAddress - s21) / 256 / 256;
            long s31 = s2 * 256 * 256 + s21;
            long s3 = (tempIPAddress - s31) / 256;
            long s4 = tempIPAddress - s3 * 256 - s31;
            if (intIPAddress < 0)
            {
                s1 = 255 + s1;
                s2 = 255 + s2;
                s3 = 255 + s3;
                s4 = 255 + s4;
            }
            string strIPAddress = s1.ToString() + "." + s2.ToString() + "." + s3.ToString() + "." + s4.ToString();
            return strIPAddress;
        }

        const int BKDRHashSeed = 131;
        // BKDR Hash Function
        public static long BKDRHash(string str)
        {
            //int seed = 131; // 31 131 1313 13131 131313 etc..
            long hash = 0;
            int len = str.Length;
            for (int i = 0; i < len; i++)
            {
                hash = hash * BKDRHashSeed + (str[i]);
            }
            return (hash & 0x7FFFFFFF);
        }

        public static long Time33(string str)
        {
            long hash = 0;
            int len = str.Length;
            for (int i = 0; i < len; i++)
            {
                hash = ((hash << 5) + hash) + (long)str[i];
            }
            return hash;
        }

    }
}