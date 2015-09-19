namespace csharp Logging.Client   #  ×¢ÊÍ1

struct TLogItem {   #  ×¢ÊÍ2 
    1: string Title 
    2: string Message 
    3: byte   Level 
    4: i64    Time 
	5: string    Source
	6: i32       Thread
    7: map<string,string> Tags
  }

  struct TLogEntity {   #  ×¢ÊÍ2 
    1:i64		IP
	2:i32    AppId
	3:list<TLogItem> Items
  }

  


service LogTransferService {  #  ×¢ÊÍ3 
   
   void Log(1:TLogEntity logEntity)  
}
