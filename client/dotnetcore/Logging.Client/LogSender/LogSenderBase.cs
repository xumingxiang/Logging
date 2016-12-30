using Logging.ThriftContract;
using System.Collections.Generic;
using System.Linq;

namespace Logging.Client
{
    internal abstract class LogSenderBase
    {
        protected static int SENDER_TIMEOUT = 5000;

        public abstract long Send(IList<ILogEntity> logEntities);

        protected TLogPackage CreateLogPackage(IList<ILogEntity> logEntities)
        {
            var logs = logEntities.Where(x => x.Type == 1);
            var metrics = logEntities.Where(x => x.Type == 2);

            List<TLogEntity> tlogs = new List<TLogEntity>();
            foreach (var log in logs)
            {
                var _log = log as LogEntity;
                TLogEntity tlog = new TLogEntity();
                tlog.Level = (sbyte)_log.Level;
                tlog.Message = _log.Message;
                tlog.Source = _log.Source;
                tlog.Tags = _log.Tags;
                tlog.Thread = _log.Thread;
                tlog.Time = _log.Time;
                tlog.Title = _log.Title;
                tlogs.Add(tlog);
            }

            List<TMetricEntity> tmetrics = new List<TMetricEntity>();

            foreach (var metric in metrics)
            {
                var _metric = metric as MetricEntity;
                TMetricEntity tmetric = new TMetricEntity();
                tmetric.Name = _metric.Name;
                tmetric.Tags = _metric.Tags;
                tmetric.Time = _metric.Time;
                tmetric.Value = _metric.Value;
                tmetrics.Add(tmetric);
            }

            TLogPackage package = new TLogPackage();
            package.AppId = Settings.AppId;
            package.IP = ServerIPNum;
            package.LogItems = tlogs;
            package.MetricItems = tmetrics;
            return package;
        }

        #region 私有成员

        private static long serverIPNum;

        protected static long ServerIPNum
        {
            get
            {
                if (serverIPNum <= 0)
                {
                    string serverIP = Utils.GetServerIP();
                    serverIPNum = Utils.IPToNumber(serverIP);
                }
                return serverIPNum;
            }
        }

        #endregion 私有成员
    }
}