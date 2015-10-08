using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Web;
using System.Threading;

namespace Logging.Client.Widgets
{
    /// <summary>
    /// 聚合未捕获异常
    /// </summary>
    public class HttpUnhandledExceptionAggregater
    {
        readonly static ILog logger = LogManager.GetLogger(typeof(HttpUnhandledExceptionAggregater));

        private int ReportCount { get;  set; }

        private int ReportElapsed { get;  set; }

        private DateTime LastReportTime { get; set; }

        /// <summary>
        /// 聚合未捕获异常
        /// </summary>
        /// <param name="reportCount">一次上报的数量</param>
        /// <param name="reportElapsed">上报间隔时间。单位：秒</param>
        public HttpUnhandledExceptionAggregater(int reportCount, int reportElapsed)
        {
            this.ReportCount = reportCount;
            this.ReportElapsed = reportElapsed;
            this.ErrorCollection = new ConcurrentDictionary<string, Tuple<int, Exception>>();
            this.ErrorCount = 0;
            this.LastReportTime = DateTime.Now;
        }

        private HttpUnhandledExceptionAggregater() { }

        private int ErrorCount;

        private ConcurrentDictionary<string, Tuple<int, Exception>> ErrorCollection { get; set; }

        //  readonly  object lockthis = new object();

        public void Aggregate()
        {
            try
            {
                var url = HttpContext.Current.Request.Url;
                Exception ex = HttpContext.Current.Server.GetLastError();
                string key = url.Scheme + "://" + url.Authority + url.AbsolutePath;

                Tuple<int, Exception> item_ex;

                var has = this.ErrorCollection.TryGetValue(key, out item_ex);
                if (has)
                {
                    this.ErrorCollection[key] = new Tuple<int, Exception>(item_ex.Item1 + 1, ex);
                }
                else
                {
                    this.ErrorCollection.TryAdd(key, new Tuple<int, Exception>(1, ex));
                }

                Interlocked.Increment(ref this.ErrorCount);
                if (this.ErrorCount >= this.ReportCount || (DateTime.Now - LastReportTime).Seconds >= this.ReportElapsed)
                {
                    foreach (var err in this.ErrorCollection)
                    {
                        Dictionary<string, string> log_tags = new Dictionary<string, string>();
                        log_tags.Add("url", err.Key);
                        log_tags.Add("count", err.Value.Item1.ToString());
                        log_tags.Add("widgets", "HttpUnhandledExceptionAggregater");
                        logger.Error(err.Value.Item2.Message + "(" + err.Value.Item1.ToString() + ")", err.Value.Item2, log_tags);
                    }
                    logger.Metric("HttpUnhandledException", this.ErrorCount);
                    this.ErrorCollection.Clear();
                    Interlocked.Add(ref this.ErrorCount, -this.ErrorCount);
                    this.LastReportTime = DateTime.Now;
                }
            }
            catch 
            {

            }
        }
    }
}
