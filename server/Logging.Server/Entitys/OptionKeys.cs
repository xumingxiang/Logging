using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logging.Server.Entitys
{
    public enum OptionKeys
    {
        /// <summary>
        /// 错误数量报警线
        /// </summary>
        ALERTING_APPERROR_ERRORCOUNTLIMIT,

        /// <summary>
        /// 错误增长速度报警线(环比)
        /// </summary>
        ALERTING_APPERROR_ERRORGROWTHLIMIT,

        /// <summary>
        /// 报警间隔时间.单位：分钟
        /// </summary>
        ALERTING_APPERROR_INTERVAL,

        /// <summary>
        /// 异常报警邮件收件人
        /// </summary>
        ALERTING_APPERROR_EMAILRECEIVERS
    }
}
