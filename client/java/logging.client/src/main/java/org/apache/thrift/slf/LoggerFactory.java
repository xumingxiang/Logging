package org.apache.thrift.slf;

public class LoggerFactory {
	public static Logger getLogger(String logName){
		
		return new Logger();
		
	}
	
public static Logger getLogger(Class type){
		
	return getLogger(type.getName());
		
	}
}
