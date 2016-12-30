using System;
using System.Collections.Generic;

namespace Logging.Client
{
    public partial interface ILog
    {
        void Debug(string message);

        void Debug(string title, string message);

        void Debug(string title, string message, Dictionary<string, string> tags);

        /// <summary>
        ///
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="tags">key=val格式的字符串数组</param>
        void DebugWithTags(string title, string message, string[] tags);

        void Info(string message);

        void Info(string title, string message);

        void Info(string title, string message, Dictionary<string, string> tags);

        /// <summary>
        ///
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="tags">key=val格式的字符串数组</param>
        void InfoWithTags(string title, string message, string[] tags);

        void Warn(string message);

        void Warn(string title, string message);

        void Warn(string title, string message, Dictionary<string, string> tags);

        /// <summary>
        ///
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="tags">key=val格式的字符串数组</param>
        void WarnWithTags(string title, string message, string[] tags);

        void Error(string message);

        void Error(string title, string message);

        void Error(string title, string message, Dictionary<string, string> tags);

        /// <summary>
        ///
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="tags">key=val格式的字符串数组</param>
        void ErrorWithTags(string title, string message, string[] tags);

        void Error(Exception ex);

        void Error(string title, Exception ex);

        void Error(Exception ex, Dictionary<string, string> tags);

        void Error(string title, Exception ex, Dictionary<string, string> tags);

        void Metric(string name, double value, Dictionary<string, string> tags = null);

        void Metric(string name, Dictionary<string, string> tags = null);

        void Metric(string name, double value, DateTime time, Dictionary<string, string> tags = null);

        /// <summary>
        /// 获取日志。
        /// 本方法为了不引入第三方序列化框架，请客户程序自行返回结果反序列化
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="appId"></param>
        /// <param name="level"></param>
        /// <param name="title"></param>
        /// <param name="msg"></param>
        /// <param name="source"></param>
        /// <param name="ip"></param>
        /// <param name="tags"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        string GetLogs(long start, long end, int appId, int[] level = null, string title = "", string msg = "", string source = "", string ip = "", Dictionary<string, string> tags = null, int limit = 100);

        void Flush();
    }
}