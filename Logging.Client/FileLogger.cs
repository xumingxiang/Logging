using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Logging.Client
{
    public static class FileLogger
    {
        static ConcurrentQueue<string> _queue = new ConcurrentQueue<string>();
        static DateTime _lastTime = DateTime.MinValue;
        static TimeSpan _flushInterval = new TimeSpan(0, 1, 0);

        const string log_path = "d:\\log\\Logging_Client\\";

        static string _getLogPath()
        {
            //return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs\\");
            return log_path;
        }
        static string _getLogFileName()
        {
            //return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs\\", DateTime.Now.ToString("yyyyMMdd") + ".txt");
            return Path.Combine(log_path, DateTime.Now.ToString("yyyyMMdd") + ".txt");
        }

        static string _logCurrentTime()
        {
            return DateTime.Now.ToString("HH:mm:ss");
        }

        static readonly object _lock = new object();

        public static void Log(Exception e)
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

        public static void Log(string content)
        {
            _queue.Enqueue(_logCurrentTime() + content);
#if DEBUG
            _FlushLog(_queue);
#else

            if (_queue.Count > 0
                && DateTime.Now - _lastTime > _flushInterval)
            {
                _lastTime = DateTime.Now;
                ConcurrentQueue<string> temp = _queue;
                _queue = new ConcurrentQueue<string>();
                ThreadPool.QueueUserWorkItem(_FlushLog, temp);
            }
#endif
        }

        static void _FlushLog(object logs)
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
