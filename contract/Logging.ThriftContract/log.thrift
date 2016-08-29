namespace csharp Logging.ThriftContract   #  ×¢ÊÍ1

struct TLogEntity {   #  ×¢ÊÍ2 
    1: string Title 
    2: string Message 
    3: byte   Level 
    4: i64    Time 
	5: string    Source
	6: i32       Thread
    7: map<string,string> Tags
  }

struct TMetricEntity {   #  ×¢ÊÍ2 
    1: string Name 
    2: double Value 
    3: i64    Time 
    4: map<string,string> Tags
  }

  struct TLogPackage {   #  ×¢ÊÍ2 
    1:i64		IP
	2:i32    AppId
	3:list<TLogEntity>    LogItems
	4:list<TMetricEntity> MetricItems
  }

  


service LogTransferService {  #  ×¢ÊÍ3 
   
   void Log(1:TLogPackage logPackage)  
}
