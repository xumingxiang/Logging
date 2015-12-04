using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text;
using System.Threading;

namespace Logging.Client
{
    public class FileLogger
    {
        private static ConcurrentQueue<string> _queue = new ConcurrentQueue<string>();
        private static DateTime _lastTime = DateTime.MinValue;
        private static TimeSpan _flushInterval = new TimeSpan(0, 0, 30);
        private const string log_path = "d:\\log\\Logging_Client\\";

        public int AppId { get; set; }

        public FileLogger(int appId)
        {
            this.AppId = appId;
        }

        private string _getLogPath()
        {
            return log_path + this.AppId;
        }

        private string _getLogFileName()
        {
            return Path.Combine(log_path, this.AppId + "\\" + DateTime.Now.ToString("yyyyMMdd") + ".txt");
        }

        private static string _logCurrentTime()
        {
            return DateTime.Now.ToString("HH:mm:ss");
        }

        private static readonly object _lock = new object();

        public void Log(Exception e)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(e.Message);
            sb.AppendLine(e.Source);
            sb.AppendLine(e.StackTrace);
            Log(sb.ToString());
            if (e.InnerException != null)
            {
                Log(e.InnerException);
            }
        }

        public void Log(string content)
        {
            _queue.Enqueue(_logCurrentTime() + content);

            if (_queue.Count > 0 && DateTime.Now - _lastTime > _flushInterval)
            {
                _lastTime = DateTime.Now;
                ConcurrentQueue<string> temp = _queue;
                _queue = new ConcurrentQueue<string>();
                ThreadPool.QueueUserWorkItem(_FlushLog, temp);
            }
        }

        private void _FlushLog(object logs)
        {
            ConcurrentQueue<string> queue = (ConcurrentQueue<string>)logs;

            try
            {
                string path = _getLogPath();
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string file = _getLogFileName();
                string content = null;
                StringBuilder sb = new StringBuilder();
                while (queue.TryDequeue(out content))
                {
                    sb.AppendLine(content);
                }
                File.AppendAllText(file, sb.ToString());
            }
            catch
            {
            }
        }
    }
}