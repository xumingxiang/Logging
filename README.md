# Plu.Logging 分布式日志系统接入步骤

1. 拉取git

  https://github.com/xumingxiang/Logging.git

2. 引用dll  
    ~\Logging\release\1.0\Logging.Client.dll
3. 基本配置
```xml
    <add key="AppId" value="{AppId}"/>    
    <add key="LoggingServerHost" value="http://***:88/server"/>  <!--测试环境: http://***:88/server-->
    <add key="LoggingEnabled" value="true"/>
```
4. 高级配置
```xml
    <!--设置日志缓冲时间-->
    <add key="LoggingBlockElapsed" value="20000"/>
    <!--设置日志队列长度-->
    <add key="LoggingQueueLength" value="5000"/>
    <!--设置日志发送的线程数量-->
    <add key="LoggingTaskNum" value="1"/>
    <!--设置日志缓冲大小-->
    <add key="LoggingBufferSize" value="300"/>
```

5. 使用Settings配置类
    配置项除了使用config配置文件外也可以使用Settings类
```csharp
    Settings.Startup(4002, "http://***:88/server",300,5000);
```

6. 基本使用方法
```csharp
    private static ILog logger = LogManager.GetLogger(typeof(QiniuStreamModel));
    logger.Debug("title","message");
    logger.Info("title","message");
    logger.Warm("title","message");
    logger.Error("title","message");
    logger.Metric("metric_name", 100,tags);
    logger.Flush();//冲刷队列中所有的日志
```
7. 高级使用方法(给日志加Tag属性)   
```csharp
    private static ILog logger = LogManager.GetLogger(typeof(QiniuStreamModel));
    var tags = new Dictionary<string, string>();
    tags.Add("roomid", "360850");
    tags.Add("userId", "123456");
    logger.Error("title","message",tags);
```
```
    给日志加Tag的作用
    1. 可以对日志轨迹进行追踪
    2. 给日志建索引，方便查询，提升效率
```

8.  查看日志

    生产环境： http://***:9090/server/logviewer.html

    测试环境： http://***:88/server/logviewer.html   

9. java 客户端使用    
   
    java 客户端使用和C#完全相同,so easy !
```java
    Settings.startup(4001, "http://***:9092/server",10,5000);
    ......
    private   ILog logger=LogManager.getLogger(Main.class);
    ......
    logger.debug("java test debug");
    
    logger.flush();//冲刷队列中所有的日志
```

10. golang客户端使用
#10.1）：在App根目录添加配置文件 	loggingclient.config：
	    {
            "AppId" :6001,
        
        	"LoggingEnabled" :true,
        
        	"LoggingServerHost":"http://***:88/server"
        }
	
# 10.2）：编码

<pre><code>
    logger := loggingclient.GetLogger("main")

	logger.Debug("test message")

	logger.Debug("test title", "test message")

	tags := make(map[string]string)
	tags["tag1"] = "tag1"
	tags["tag2"] = "tag2"
	logger.Debug("test title", "test message", tags)

	err := errors.New("this is a error")
	logger.Debug(err)
	
	
	logger.Metric("metric_name", 100,nil)   //metric打点
	
	//logger.Info(params ...interface{})
    //logger.Warm(params ...interface{})
    //logger.Error(params ...interface{})
	//同logger.Debug(params ...interface{})
	
</code></pre>
