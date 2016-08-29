# 一：在App根目录添加配置文件 	loggingclient.config：
	    {
            "AppId" :6001,
        
        	"LoggingEnabled" :true,
        
        	"LoggingServerHost":"http://172.16.9.220:88/server"
        }
	
# 二：编码

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
    
# 三：备注
    1）生产环境LoggingServerHost http://192.168.9.20:88/server
       测试环境LoggingServerHost http://172.16.9.220:88/server
        
    2）AppId golang 项目从数字6开头，如6001、6002。1,2,3,4,5开头的已被占用
    
    3)如不添加配置文件，也可在项目入口处调用
      loggingclient.StartUp(6001, "http://172.16.9.220:88/server")
      
    4)LogViewer
        生产环境http://183.131.23.176:9090/server/logviewer.html#
        测试环境http://172.16.9.220:88/server/logviewer.html#
    
    5)更多：http://git.corp.plu.cn/pluplatform/PLU.Logging/blob/master/README.md
      
    
    