using System;
using System.Collections.Generic;

namespace Logging.Client.Widgets
{
    /// <summary>
    /// weiapi接口统计Metric
    /// </summary>
    public class WebApiMetric : IWidget
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(WebApiMetric));

        private static DateTime LastSendTime = DateTime.Now;

        public static Dictionary<string, long> metrics = new Dictionary<string, long>();

        private static object lockthis = new object();

        private static int TotalCount = 0;

        //时间刻度，毫秒
        private static readonly int Scale = 1000 * 60;

        /// <summary>
        /// 超过1千万必须send
        /// </summary>
        private static readonly int TotalCountLimit = 10000000;

        /// <summary>
        ///统计数量
        /// </summary>
        /// <param name="name"></param>
        public static void Count(string name)
        {
            lock (lockthis)
            {
                long count = 0;
                if (metrics.ContainsKey(name))
                {
                    metrics.TryGetValue(name, out count);
                }
                metrics[name] = count + 1;
                TotalCount += 1;
                if ((DateTime.Now - LastSendTime).TotalMilliseconds >= Scale || TotalCount >= TotalCountLimit)
                {
                    //send
                    foreach (var item in metrics)
                    {
                        logger.Metric(item.Key, item.Value);
                    }
                    metrics.Clear();
                    LastSendTime = DateTime.Now;
                    Console.WriteLine("send" + TotalCount);
                    TotalCount = 0;
                }
            }
        }
    }
}