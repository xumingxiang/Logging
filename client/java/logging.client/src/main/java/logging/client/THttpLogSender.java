package logging.client;

import java.util.List;

import org.apache.thrift.protocol.TCompactProtocol;
import org.apache.thrift.transport.THttpClient;
import org.apache.thrift.transport.TTransport;

public class THttpLogSender extends LogSenderBase {

    @Override
    public void Send(List<ILogEntity> logEntities) {


        try {
            if (logEntities == null || logEntities.size() <= 0) {
                return;
            }
            TLogPackage logPackage = CreateLogPackage(logEntities);

            TTransport httpClient = new THttpClient(Settings.LoggingServerHost + "/Reciver.ashx");

            TCompactProtocol protocol = new TCompactProtocol(httpClient);
            httpClient.open();
            LogTransferService.Client client = new LogTransferService.Client(protocol);
            client.Log(logPackage);
            httpClient.close();
        } catch (Exception e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        }
    }


}