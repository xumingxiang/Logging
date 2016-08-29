package logging.client;

import java.util.Map;

public class LogEntity extends ILogEntity {

//       public LogEntity() {Type = 1; }

       public String Title;

       public String Message;

       public int Level;

       public long Time;

       public String Source;

       public int Thread;

       public Map<String, String> Tags;
	
}
