using Logging.Server.Viewer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logging.Server.Alerting
{
    /// <summary>
    /// 应用错误增速提醒，一分钟
    /// </summary>
    public class AppErrorthAlerting : BaseAlerting
    {
        public AppErrorthAlerting() :
            base(AlertingType.AppError)
        {
        }

        /// <summary>
        /// 错误数量报警线
        /// </summary>
        public int ErrorCountLimit { get; set; }


        /// <summary>
        /// 错误增长速度报警线
        /// </summary>
        public int ErrorGrowthLimit { get; set; }

        protected override void Alert()
        {
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
                int error_count1 = item.Sum(x => x.Error);
                int error_count2 = data2.Where(x => x.AppId == appId).Sum(x => x.Error);
                int error_count = error_count1 + error_count2;
                double growth = (error_count1 - error_count2) / error_count2;
                string msg_content = string.Empty;
                if ((error_count) > ErrorCountLimit)
                {
                    //报警
                }
                else if (growth > 1)
                {
                    //报警
                }
                else
                {
                    break;
                }


            }
        }

        /// <summary>
        /// 报警间隔时间
        /// </summary>
        public int Interval { get; set; }

    }
}
