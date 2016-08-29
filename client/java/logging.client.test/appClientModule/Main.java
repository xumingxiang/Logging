import logging.client.ILog;
import logging.client.LogManager;
import logging.client.Settings;

import java.util.Date;

public class Main {


    static {
        System.out.println("aa:" + new Date());
    }

    public static void main(String[] args) {
        System.out.println("bb:" + new Date());
        Settings.startup(4005, "http://183.131.23.176:9092/server/", 1000, 100 * 1000);
        System.out.println(Settings.LoggingQueueLength);
        System.out.println("ThriftTest begin");
        ThriftTest();
        System.out.println("ThriftTest end");
    }

    public static void ThriftTest() {
        ILog logger = LogManager.getLogger(Main.class);
        for (int i = 0; i < 1; i++) {
            logger.debug("java test debug");
            logger.info("java test info");
            logger.warm("java test warm");
            logger.error("java test error");
            logger.metric("metric_java_test", 1, null);
        }

        try {
            Thread.sleep(200);
        } catch (InterruptedException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        }

        for (int i = 0; i < 1; i++) {
            logger.debug("java test debug");
            logger.info("java test info");
            logger.warm("java test warm");
            logger.error("java test error");
            logger.metric("metric_java_test", 1, null);
        }

        try {
            Thread.sleep(200);
        } catch (InterruptedException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        }

        for (int i = 0; i < 1; i++) {
            logger.debug("java test debug");
            logger.info("java test info");
            logger.warm("java test warm");
            logger.error("java test error");
            logger.metric("metric_java_test", 1, null);
        }

        try {
            Thread.sleep(200);
        } catch (InterruptedException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        }

        for (int i = 0; i < 1; i++) {
            logger.debug("java test debug");
            logger.info("java test info");
            logger.warm("java test warm");
            logger.error("java test error");
            logger.metric("metric_java_test", 1, null);
        }

        try {
            Thread.sleep(200);
        } catch (InterruptedException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        }

        logger.debug("java test debug");
        logger.info("java test info");
        logger.warm("java test warm");
        logger.error(new Exception("ex test"));
        logger.metric("metric_java_test", 1, null);

//	    	try {
//				Thread.sleep(200);
//			} catch (InterruptedException e) {
//				// TODO Auto-generated catch block
//				e.printStackTrace();
//			}

        logger.debug("java test Debug  over");
        logger.flush();
    }


    /* (non-Java-doc)
     * @see java.lang.Object#Object()
     */
    public Main() {


        //super();
        System.out.println("aa:" + new Date());
    }

}