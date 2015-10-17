using System;
using System.Net;
using System.Text;

namespace Logging.Client
{
    internal sealed class LogOnOffManager
    {
        private LogOnOffManager()
        {
        }

        private readonly static int LogOnOffCackeTimeOut = 10;//单位:分钟

        private readonly static string GetLogOnOffUrl = Settings.LoggingServerHost + "/GetLogOnOff.ashx?appId=" + Settings.AppId;

        private static DateTime LastUpdateTime;

        private static LogOnOff logOnOff = null;

        public static LogOnOff GetLogOnOff()
        {
            if (logOnOff == null)
            {
                logOnOff = new LogOnOff();
                logOnOff.Debug = 1;
                logOnOff.Info = 1;
                logOnOff.Warm = 1;
                logOnOff.Error = 1;
            }
            return logOnOff;
        }

        /// <summary>
        /// 从服务端获取并刷新日志开关,10分钟缓存
        /// </summary>
        /// <returns></returns>
        public static void RefreshLogOnOff()
        {
            if ((DateTime.Now - LastUpdateTime).TotalMinutes < LogOnOffCackeTimeOut)
            {
                return;
            }

            string resp = string.Empty;
            using (WebClient _client = new WebClient())
            {
                byte[] resp_byte = _client.DownloadData(GetLogOnOffUrl);
                resp = Encoding.UTF8.GetString(resp_byte);
            }
            if (!string.IsNullOrWhiteSpace(resp))
            {
                logOnOff = new LogOnOff();
                string[] arr = resp.Split(',');
                logOnOff.Debug = Convert.ToByte(arr[0]);
                logOnOff.Info = Convert.ToByte(arr[1]);
                logOnOff.Warm = Convert.ToByte(arr[2]);
                logOnOff.Error = Convert.ToByte(arr[3]);

                LastUpdateTime = DateTime.Now;
            }
        }
    }
}