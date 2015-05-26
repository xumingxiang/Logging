namespace java com.javabloger.gen.code   #  ×¢ÊÍ1

struct LogEntity {   #  ×¢ÊÍ2 
    1: string Title 
    2: string Message 
    3: byte   Level 
    4: i64    Time 
    5: string IP
    6: map<string,string> Tags
  }


service LogTransferService {  #  ×¢ÊÍ3 
   
   void Log(1:list<LogEntity > logEntities)  
}