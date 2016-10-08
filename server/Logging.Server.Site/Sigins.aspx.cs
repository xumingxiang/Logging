using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace Logging.Server.Site
{
    public partial class Sigins : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void sign_Click(object sender, EventArgs e)
        {
            string phone = "15380703848";
            string pwd = "p@ssw0rd01";

            int uid = ClientLogin(phone, pwd);
            string cookie = RealLogin(phone, pwd, uid);
            string resp = Sigin(cookie);
        }

        /// <summary>
        /// 签到
        /// </summary>
        /// <param name="cookie"></param>
        /// <returns></returns>
        public string Sigin(string cookie)
        {
            string url = "http://www.yijitongoa.com:9090/yjtoa/s/signins";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.UserAgent = "yjtos/1.1.4 (iPhone; iOS 9.3.4; Scale/3.00)";
            request.Accept = "*/*";
            //    var JSESSIONID = this.ViewState["JSESSIONID"] as string;
            //   string cookie = this.ViewState["cookie"] as string;
            //   request.Headers.Add("cookie", "JSESSIONID="+ JSESSIONID + "; yjtToken=\"MzU1MTU3NToxNDczNDAwNDExODg1OnAzMEVKeGtEblJTeEtkTzZ5RnNLR1E9PQ ==\"; lbcooki_-536595_9090=172_25_1_180-9090");

            request.Headers.Add("cookie", cookie);

            //{ "cardTime":"50089","actrualData":"","signResult":0,"id":0,"actrualPOI":"上海市闸北区广中西路777弄11","openCellHeight":0,"positionData":"31.285454,121.445568","machineName":"上海市闸北区广中西路777弄11"}

            string data = @"{""actrualData"":"""",""signResult"":0,""id"":0,""actrualPOI"":""上海市闸北区广中西路777弄11"",""openCellHeight"":0,""positionData"":""31.285454,121.445568"",""machineName"":""上海市闸北区广中西路777弄11""}";

            StreamWriter writer = new StreamWriter(request.GetRequestStream(), Encoding.UTF8);
            writer.Write(data);
            writer.Flush();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string encoding = response.ContentEncoding;
            if (encoding == null || encoding.Length < 1)
            {
                encoding = "UTF-8"; //默认编码
            }
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encoding));
            string resp = reader.ReadToEnd();
            Response.Write(resp);
            response.Close();

            return resp;
        }

        /// <summary>
        /// 登录并返回cookie
        /// </summary>
        /// <returns></returns>
        public string RealLogin(string phone, string pwd, int uid)
        {
            string url = "http://www.yijitongoa.com:9090/yjtoa/s/reallogin";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.UserAgent = "yjtos/1.1.4 (iPhone; iOS 9.3.4; Scale/3.00)";
            request.Accept = "*/*";

            //request.Headers.Add("cookie", "JSESSIONID=8AE71F23BE5EBB6EB0A46B70E1E8E343; yjtToken=\"MzU1MTU3NToxNDczNDAwNDExODg1OnAzMEVKeGtEblJTeEtkTzZ5RnNLR1E9PQ ==\"; lbcooki_-536595_9090=172_25_1_180-9090");

            string data = "{\"phone\":\"" + phone + "\",\"password\":\"" + pwd + "\",\"userId\":3551575,\"contentId\":0,\"custVCode\":0,\"custName\":\"苏州游视网络科技有限公司\",\"iccid\":\"0E97DB25918C4289B35687C3187CA88C\"}";

            StreamWriter writer = new StreamWriter(request.GetRequestStream(), Encoding.UTF8);
            writer.Write(data);
            writer.Flush();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string encoding = response.ContentEncoding;
            if (encoding == null || encoding.Length < 1)
            {
                encoding = "UTF-8"; //默认编码
            }
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encoding));
            string resp = reader.ReadToEnd();
            //Response.Write(resp);
            //response.Close();

            string cookie = response.Headers["Set-Cookie"];

            return cookie;
        }

        /// <summary>
        /// 登录并返回uid
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public int ClientLogin(string phone, string pwd)
        {
            string url = "http://www.yijitongoa.com:9090/yjtoa/s/clientlogin";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.UserAgent = "yjtos/1.1.4 (iPhone; iOS 9.3.4; Scale/3.00)";
            request.Accept = "*/*";

            //request.Headers.Add("cookie", "JSESSIONID=8AE71F23BE5EBB6EB0A46B70E1E8E343; yjtToken=\"MzU1MTU3NToxNDczNDAwNDExODg1OnAzMEVKeGtEblJTeEtkTzZ5RnNLR1E9PQ ==\"; lbcooki_-536595_9090=172_25_1_180-9090");

            string data = "{\"password\":\"" + pwd + "\",\"id\":0,\"loginName\":\"" + phone + "\",\"iccid\":\"0E97DB25918C4289B35687C3187CA88C\"}";

            StreamWriter writer = new StreamWriter(request.GetRequestStream(), Encoding.UTF8);
            writer.Write(data);
            writer.Flush();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string encoding = response.ContentEncoding;
            if (encoding == null || encoding.Length < 1)
            {
                encoding = "UTF-8"; //默认编码
            }
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encoding));
            string resp = reader.ReadToEnd();

            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject(resp) as JObject;
            string str_uid = (obj.GetValue("payload")[0] as JObject).GetValue("userId").ToString();
            return Convert.ToInt32(str_uid);
        }

        protected void login_Click(object sender, EventArgs e)
        {
            //Login();
        }
    }
}