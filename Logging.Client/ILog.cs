using System.Collections.Generic;

namespace Logging.Client
{
    public interface ILog
    {
        void Debug(string message);

        void Debug(string title, string message);

        void Debug(string title, string message, Dictionary<string, string> tags);

        void Info(string message);

        void Info(string title, string message);

        void Info(string title, string message, Dictionary<string, string> tags);

        void Warm(string message);

        void Warm(string title, string message);

        void Warm(string title, string message, Dictionary<string, string> tags);

        void Error(string message);

        void Error(string title, string message);

        void Error(string title, string message, Dictionary<string, string> tags);
    }
}