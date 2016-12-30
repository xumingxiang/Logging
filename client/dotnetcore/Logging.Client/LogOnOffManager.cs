using System;

namespace Logging.Client
{
    internal sealed class LogOnOffManager
    {
        private LogOnOffManager()
        {
        }

        private readonly static int LogOnOffCacheTimeOut = 10;//单位:分钟

        private readonly static string GetLogOnOffUrl = Settings.LoggingServerHost + "/GetLogOnOff.ashx?appId=" + Settings.AppId;

        private readonly static LogOnOff Default = new LogOnOff { Debug = 1, Error = 1, Info = 1, Warm = 1 };

        private static DateTime LastUpdateTime;

        private static LogOnOff logOnOff = null;

        public static LogOnOff GetLogOnOff()
        {
            if (logOnOff == null) { return Default; }
            return logOnOff;
        }

        /// <summary>
        /// 从服务端获取并刷新日志开关,10分钟缓存
        /// </summary>
        /// <returns></returns>
        public static void RefreshLogOnOff()
        {
            if ((DateTime.Now - LastUpdateTime).TotalMinutes < LogOnOffCacheTimeOut) { return; }

            string resp = string.Empty;

            try
            {
                //using (WebClientEx _client = new WebClientEx(10 * 1000))
                //{
                //    byte[] resp_byte = _client.DownloadData(GetLogOnOffUrl);
                //    resp = Encoding.UTF8.GetString(resp_byte);
                //}
                WebClientEx _client = new WebClientEx();
                resp = _client.GetWebRequest(new Uri(GetLogOnOffUrl));
            }
            catch { }
            if (!string.IsNullOrWhiteSpace(resp))
            {
                logOnOff = new LogOnOff();
                string[] arr = resp.Split(',');
                logOnOff.Debug = Convert.ToByte(arr[0]);
                logOnOff.Info = Convert.ToByte(arr[1]);
                logOnOff.Warm = Convert.ToByte(arr[2]);
                logOnOff.Error = Convert.ToByte(arr[3]);
            }
            LastUpdateTime = DateTime.Now;
        }
    }
}