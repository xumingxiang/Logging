package logging.client;

import java.util.Hashtable;

public class LogManager {

	 static Hashtable<String, ILog> _logs = new Hashtable<String,ILog>();


     private LogManager()
     { }

     /// <summary>
     /// 通过类型名获取ILog实例。
     /// </summary>
     /// <param name="type">The type.</param>
     /// <returns>ILog instance</returns>
     public static ILog getLogger(Class type)
     {        	 
         if (type == null)
         {
             return getLogger("NoName");
         }
         else
         {
             return getLogger(type.getName());
         }
     }

     /// <summary>
     /// 通过字符串名获取ILog实例。
     /// </summary>
     /// <param name="name">The name.</param>
     /// <returns>ILog instance</returns>
     public static ILog getLogger(String name)
     {
         ILog log = _logs.get(name);
         if (log==null)
         {
             log = new SimpleLogger(name);
             _logs.put(name, log);
         }
         return log;
     }
	
}
