using Logging.Server.Entitys;
using Logging.Server.Viewer;
using Logging.Server.Writer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Logging.Server.Alerting
{
    /// <summary>
    /// 应用错误增速提醒，一分钟
    /// </summary>
    public class AppErrorthAlerting : BaseAlerting
    {
        private static string[] OptKeys = {
             OptionKeys.ALERTING_APPERROR_INTERVAL.ToString(),
             OptionKeys.ALERTING_APPERROR_ERRORCOUNTLIMIT.ToString(),
             OptionKeys.ALERTING_APPERROR_ERRORGROWTHLIMIT.ToString(),
             OptionKeys.ALERTING_APPERROR_EMAILRECEIVERS.ToString()
        };

        public AppErrorthAlerting() :
            base(AlertingType.AppError)
        {
            var opts = GetOptions();
            if (opts.Count == 0) { return; }
            this.Interval = Convert.ToInt64(opts[OptionKeys.ALERTING_APPERROR_INTERVAL.ToString()]);
            this.ErrorCountLimit = Convert.ToInt32(opts[OptionKeys.ALERTING_APPERROR_ERRORCOUNTLIMIT.ToString()]);
            this.ErrorGrowthLimit = Convert.ToInt32(opts[OptionKeys.ALERTING_APPERROR_ERRORGROWTHLIMIT.ToString()]);
            this.EmailReceivers = opts[OptionKeys.ALERTING_APPERROR_EMAILRECEIVERS.ToString()].Split(',');
        }

        public static void SetOptions(int interval, int errorCountLimit, int errorGrowthLimit, string emailReceivers)
        {
            List<Options> opts = new List<Options>();
            opts.Add(new Options { Key = OptionKeys.ALERTING_APPERROR_INTERVAL.ToString(), Value = interval.ToString(), UpdateTime = Utils.GetUnixTime(DateTime.Now) });
            opts.Add(new Options { Key = OptionKeys.ALERTING_APPERROR_ERRORCOUNTLIMIT.ToString(), Value = errorCountLimit.ToString(), UpdateTime = Utils.GetUnixTime(DateTime.Now) });
            opts.Add(new Options { Key = OptionKeys.ALERTING_APPERROR_ERRORGROWTHLIMIT.ToString(), Value = errorGrowthLimit.ToString(), UpdateTime = Utils.GetUnixTime(DateTime.Now) });
            opts.Add(new Options { Key = OptionKeys.ALERTING_APPERROR_EMAILRECEIVERS.ToString(), Value = emailReceivers, UpdateTime = Utils.GetUnixTime(DateTime.Now) });
            LogWriterManager.GetLogWriter().SetOptions(opts);
        }

        public static Dictionary<string, string> GetOptions()
        {
            var opts = LogViewerManager.GetLogViewer().GetOptions(OptKeys);
            if (opts.Count == 0)
            {
                return new Dictionary<string, string>();
            }
            else
            {
                return opts.ToDictionary(x => x.Key, x => x.Value);
            }
        }

        /// <summary>
        /// 报警间隔时间.单位：分钟
        /// </summary>
        public long Interval { get; set; }

        /// <summary>
        /// 错误数量报警线
        /// </summary>
        public int ErrorCountLimit { get; set; }

        /// <summary>
        /// 错误增长速度报警线(环比)
        /// </summary>
        public double ErrorGrowthLimit { get; set; }

        public string[] EmailReceivers { get; set; }

        public override void Alert()
        {
            if (this.EmailReceivers == null || this.EmailReceivers.Length == 0)
            {
                return;
            }

            long start1 = Utils.GetUnixTime(DateTime.Now.AddMinutes(-1)) * 10000000;
            long end1 = Utils.GetUnixTime(DateTime.Now) * 10000000;

            long start2 = Utils.GetUnixTime(DateTime.Now.AddMinutes(-2)) * 10000000;
            long end2 = start1;

            var data1 = LogViewerManager.GetLogViewer().GetStatistics(start1, end1, 0);

            var data2 = LogViewerManager.GetLogViewer().GetStatistics(start2, end2, 0);

            var data_grp1 = data1.GroupBy(x => x.AppId);

            foreach (var item in data_grp1)
            {
                var appId = item.Key;


                var lastAH = LogViewerManager.GetLogViewer().GetLastAlertingHistory(appId, AlertingType.AppError);
                if (lastAH != null)
                {
                    if ((Utils.GetDateTimeFromUnix(lastAH.Time) - DateTime.Now).TotalMinutes < this.Interval)
                    {
                        continue;
                    }
                }

                int error_count1 = item.Sum(x => x.Error);
                int error_count2 = data2.Where(x => x.AppId == appId).Sum(x => x.Error);
                int error_count = error_count1 + error_count2;

                string msg_body_fmt = string.Empty;
                if ((error_count) > ErrorCountLimit)
                {
                    //报警
                    msg_body_fmt = "应用{0}程序异常过多，请及时处理";
                }
                else if (error_count2 > 0 && error_count1 > 10 * this.Interval && ((error_count1 - error_count2) / error_count2) * 100 > ErrorGrowthLimit)
                {
                    //报警
                    msg_body_fmt = "应用{0}程序异常增长很快，请及时处理";
                }
                else
                {
                    continue;
                }

                string appName = appId.ToString();
                var loo = LogViewerManager.GetLogViewer().GetLogOnOff(appId);
                if (loo != null)
                {
                    appName = $"{appId}({ loo.AppName})";
                }

                string msg_subject = "【报警】应用" + appName + "程序异常";

                for (int i = 0; i < EmailReceivers.Length; i++)
                {
                    MailHelper.SendMail(msg_subject, EmailReceivers[i], string.Format(msg_body_fmt, appName), true);
                }

                AlertingHistory ah = new AlertingHistory();
                ah.AlertingType = AlertingType.AppError;
                ah.Time = Utils.GetUnixTime(DateTime.Now);
                ah.ObjId = appId;
                ILogWriter w = LogWriterManager.GetLogWriter();
                w.RecordAlerting(ah);
            }
        }
    }
}