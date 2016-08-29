package logging.client;

import java.util.Map;

public interface ILog {


    void debug(String message);

    void debug(String title, String message);

    void debug(String title, String message, Map<String, String> tags);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="title"></param>
    /// <param name="message"></param>
    /// <param name="tags">key=val格式的字符串数组</param>
    void debugWithTags(String title, String message, String[] tags);

    void info(String message);

    void info(String title, String message);

    void info(String title, String message, Map<String, String> tags);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="title"></param>
    /// <param name="message"></param>
    /// <param name="tags">key=val格式的字符串数组</param>
    void infoWithTags(String title, String message, String[] tags);

    void warm(String message);

    void warm(String title, String message);

    void warm(String title, String message, Map<String, String> tags);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="title"></param>
    /// <param name="message"></param>
    /// <param name="tags">key=val格式的字符串数组</param>
    void warmWithTags(String title, String message, String[] tags);

    void error(String message);

    void error(String title, String message);

    void error(String title, String message, Map<String, String> tags);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="title"></param>
    /// <param name="message"></param>
    /// <param name="tags">key=val格式的字符串数组</param>
    void errorWithTags(String title, String message, String[] tags);

    void error(Exception ex);

    void error(String title, Exception ex);

    void error(Exception ex, Map<String, String> tags);

    void error(String title, Exception ex, Map<String, String> tags);

    void metric(String name, double value, Map<String, String> tags );

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
    String getLogs(long start, long end, int appId, int[] level , String title , String msg, String source, String ip , Map<String, String> tags , int limit );

    void flush();
}
