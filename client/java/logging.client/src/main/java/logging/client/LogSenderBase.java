package logging.client;

import java.util.ArrayList;
import java.util.List;

import logging.client.TimerActionBlock.DequeueAction;

public abstract class LogSenderBase implements DequeueAction<ILogEntity> {

    public abstract void Send(List<ILogEntity> logEntities);


    protected TLogPackage CreateLogPackage(List<ILogEntity> logEntities) {

        List<LogEntity> logs = new ArrayList<>();
        List<MetricEntity> metrics = new ArrayList<>();

        for (int i = 0; i < logEntities.size(); i++) {
            ILogEntity item = logEntities.get(i);
            if (item.Type == 1) {
                logs.add((LogEntity) item);
            } else if (item.Type == 2) {
                metrics.add((MetricEntity) item);
            }
        }

        int logs_cnt = logs.size();
        List<TLogEntity> tlogs = new ArrayList<>();
        for (int i = 0; i < logs_cnt; i++) {
            LogEntity _log = logs.get(i);
            TLogEntity tlog = new TLogEntity();
            tlog.Level = (byte) _log.Level;
            tlog.Message = _log.Message;
            tlog.Source = _log.Source;
            tlog.Tags = _log.Tags;
            tlog.Thread = _log.Thread;
            tlog.Time = _log.Time;
            tlog.Title = _log.Title;
            tlogs.add(tlog);
        }

        List<TMetricEntity> tmetrics = new ArrayList<>();
        for (int i = 0; i < metrics.size(); i++) {
            MetricEntity _metric = metrics.get(i);
            TMetricEntity tmetric = new TMetricEntity();
            tmetric.Name = _metric.Name;
            tmetric.Tags = _metric.Tags;
            tmetric.Time = _metric.Time;
            tmetric.Value = _metric.Value;
            tmetrics.add(tmetric);
        }

        TLogPackage logpackage = new TLogPackage();
        logpackage.AppId = Settings.AppId;
        logpackage.IP = 0;
        logpackage.LogItems = tlogs;
        logpackage.MetricItems = tmetrics;
        return logpackage;
    }

    public void execute(List<ILogEntity> items) {
        // TODO Auto-generated method stub
        try {
            Send(items);
            LogOnOffManager.RefreshLogOnOff();
        } catch (Exception e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        }
    }

}
