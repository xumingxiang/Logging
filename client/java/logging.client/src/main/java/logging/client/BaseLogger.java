package logging.client;

import java.io.IOException;
import java.io.PrintWriter;
import java.io.StringWriter;
import java.util.Map;

public abstract class BaseLogger implements ILog {

	private String Source;

	static TimerActionBlock<ILogEntity> block;

	static LogSenderBase logSender;

	public BaseLogger(String source) {
		this.Source = source;
		if (logSender == null) {
			logSender = new THttpLogSender();
		}
		if (block == null) {
			block = new TimerActionBlock<>(logSender, Settings.LoggingQueueLength, Settings.LoggingBufferSize,
					Settings.LoggingBlockElapsed);
		}
	}

	public void debug(String message) {
		// TODO Auto-generated method stub
		debug("", message);
	}

	public void debug(String title, String message) {
		// TODO Auto-generated method stub
		debug(title, message, null);
	}

	public void debug(String title, String message, Map<String, String> tags) {
		// TODO Auto-generated method stub
		Log(title, message, null, 1);
	}

	public void debugWithTags(String title, String message, String[] tags) {
		// TODO Auto-generated method stub

	}

	public void info(String message) {
		// TODO Auto-generated method stub
		info("", message);
	}

	public void info(String title, String message) {
		// TODO Auto-generated method stub
		info(title, message, null);
	}

	public void info(String title, String message, Map<String, String> tags) {
		// TODO Auto-generated method stub
		Log(title, message, null, 2);
	}

	public void infoWithTags(String title, String message, String[] tags) {
		// TODO Auto-generated method stub

	}

	public void warm(String message) {
		// TODO Auto-generated method stub
		warm("", message);
	}

	public void warm(String title, String message) {
		// TODO Auto-generated method stub
		warm(title, message, null);
	}

	public void warm(String title, String message, Map<String, String> tags) {
		// TODO Auto-generated method stub
		Log(title, message, null, 3);
	}

	public void warmWithTags(String title, String message, String[] tags) {
		// TODO Auto-generated method stub

	}

	public void error(String message) {
		// TODO Auto-generated method stub
		error("", message);
	}

	public void error(String title, String message) {
		// TODO Auto-generated method stub
		error(title, message, null);
	}

	public void error(String title, String message, Map<String, String> tags) {
		// TODO Auto-generated method stub
		Log(title, message, tags, 4);
	}

	public void errorWithTags(String title, String message, String[] tags) {
		// TODO Auto-generated method stub

	}

	public void error(Exception ex) {
		error(ex.toString(), ex);

	}

	public void error(String title, Exception ex) {
		error(title, ex, null);

	}

	public void error(Exception ex, Map<String, String> tags) {
		error(ex.toString(), ex, tags);

	}

	public void error(String title, Exception ex, Map<String, String> tags) {

		PrintWriter pw = null;
		StringWriter sw = null;
		try {
			sw = new StringWriter();
			pw = new PrintWriter(sw);
			ex.printStackTrace(pw); // 将出错的栈信息输出到printWriter中
			pw.flush();
			sw.flush();
		} finally {
			if (sw != null) {
				try {
					sw.close();
				} catch (IOException e1) {
					e1.printStackTrace();
				}
			}
			if (pw != null) {
				pw.close();
			}
		}

		error(title, sw.toString(), tags);

	}

	public void metric(String name, double value, Map<String, String> tags) {

		if (!Settings.LoggingEnabled) {
			return;
		}

		MetricEntity Metric = new MetricEntity();
		Metric.Type = 2;
		Metric.Name = name;
		Metric.Value = value;
		Metric.Tags = tags;
		Metric.Time = System.currentTimeMillis()/1000;
		block.Enqueue(Metric);
	}

	public String getLogs(long start, long end, int appId, int[] level, String title, String msg, String source,
			String ip, Map<String, String> tags, int limit) {
		// TODO Auto-generated method stub
		return null;
	}

	public  void  flush(){

		block.flush();

	}

	protected void Log(String title, String message, Map<String, String> tags, int level) {
		if (!Settings.LoggingEnabled) {
			return;
		}
		
		 LogOnOff onOff = LogOnOffManager.GetLogOnOff();

         if (level == 1 && onOff.Debug != 1) { return; }
         if (level == 2 && onOff.Info != 1) { return; }
         if (level == 3 && onOff.Warm != 1) { return; }
         if (level == 4 && onOff.Error != 1) { return; }
		
		LogEntity log = this.CreateLog(this.Source, title, message, tags, level);
		block.Enqueue(log);

	}

	protected LogEntity CreateLog(String source, String title, String message, Map<String, String> tags, int level) {
		LogEntity log = new LogEntity();
		log.Type = 1;
		log.Level = level;
		log.Message = message;
		log.Tags = tags;
		log.Time = System.currentTimeMillis() * 10000;
		log.Title = title;
		log.Source = source;
		log.Thread = (int) Thread.currentThread().getId();
		return log;
	}
}
