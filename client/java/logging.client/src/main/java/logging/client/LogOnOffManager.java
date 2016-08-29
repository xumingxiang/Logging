package logging.client;

import java.util.Date;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;

import org.apache.http.HttpEntity;
import org.apache.http.client.methods.CloseableHttpResponse;
import org.apache.http.client.methods.HttpGet;
import org.apache.http.impl.client.CloseableHttpClient;
import org.apache.http.impl.client.HttpClients;

final class LogOnOffManager {

	private LogOnOffManager() {
	}

	private static int LogOnOffCackeTimeOut = 10 * 60 * 1000;// 单位:毫秒

	private static String GetLogOnOffUrl = Settings.LoggingServerHost + "/GetLogOnOff.ashx?appId=" + Settings.AppId;

	private static Date LastUpdateTime;

	private static LogOnOff logOnOff = null;

	public static LogOnOff GetLogOnOff() {
		if (logOnOff == null) {
			logOnOff = new LogOnOff();
			logOnOff.Debug = 1;
			logOnOff.Info = 1;
			logOnOff.Warm = 1;
			logOnOff.Error = 1;
		}
		return logOnOff;
	}

	/// <summary>
	/// 从服务端获取并刷新日志开关,10分钟缓存
	/// </summary>
	/// <returns></returns>
	public static void RefreshLogOnOff() {
		if (LastUpdateTime != null && (new Date().getTime() - LastUpdateTime.getTime()) < LogOnOffCackeTimeOut) {
			return;
		}

		String resp = "";

		// 创建HttpClient实例
		CloseableHttpClient httpclient = HttpClients.createDefault();
		// 创建Get方法实例
		HttpGet httpgets = new HttpGet(GetLogOnOffUrl);
		CloseableHttpResponse response;
		try {
			response = httpclient.execute(httpgets);
			HttpEntity entity = response.getEntity();
			if (entity != null) {
				 InputStream instreams = entity.getContent();
				resp = convertStreamToString(instreams);
				response.close();
				System.out.println(resp);
			}
		} catch (IOException e2) {
			// TODO Auto-generated catch block
			e2.printStackTrace();
			LastUpdateTime = new Date();
		}

		if (!resp.isEmpty()) {
			logOnOff = new LogOnOff();
			String[] arr = resp.split(",");
			logOnOff.Debug = Byte.parseByte(arr[0]);
			logOnOff.Info = Byte.parseByte(arr[1]);
			logOnOff.Warm = Byte.parseByte(arr[2]);
			logOnOff.Error = Byte.parseByte(arr[3]);
			LastUpdateTime = new Date();
		}
	}

	public static String convertStreamToString(InputStream is) {
		BufferedReader reader = new BufferedReader(new InputStreamReader(is));
		StringBuilder sb = new StringBuilder();

		String line;
		try {
			while ((line = reader.readLine()) != null) {
				sb.append(line + "\n");
			}
		} catch (IOException e) {
			e.printStackTrace();
		} finally {
			try {
				is.close();
			} catch (IOException e) {
				e.printStackTrace();
			}
		}
		return sb.toString();
	}

}
