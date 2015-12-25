using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Logging.Server
{
    internal static class FileLogger
    {
        static ConcurrentQueue<string> _queue = new ConcurrentQueue<string>();
        static DateTime _lastTime = DateTime.MinValue;
        static TimeSpan _flushInterval = new TimeSpan(0, 1, 0);

        const string log_path = "c:\\log\\Logging_Server\\";

        static string _getLogPath()
        {
            return log_path;
        }
        static string _getLogFileName()
        {
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

            if (_queue.Count > 0 && DateTime.Now - _lastTime > _flushInterval)
            {
                _lastTime = DateTime.Now;
                _FlushLog(_queue);
                _queue = new ConcurrentQueue<string>();
            }
        }

        private static void _FlushLog(ConcurrentQueue<string> queue)
        {
            try
            {
                string path = _getLogPath();
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string content = null;
                StringBuilder sb = new StringBuilder();
                while (queue.TryDequeue(out content))
                {
                    sb.AppendLine(content);
                }

                string file = _getLogFileName();
                if (!File.Exists(file))
                {
                    using (var fs = File.Create(file)) { }
                }

                using (var fs = new FileStream(file, FileMode.Append, FileAccess.Write))
                {
                    using (var sw = new StreamWriter(fs))
                    {
                        sw.WriteLine(sb.ToString());
                        sw.Flush();
                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
            }
        }
    }
}
