using System;
using System.Net;

namespace Logging.Client
{
    public class WebClientEx //: WebClient
    {
        private int _timeout;

        /// <summary>
        /// 超时时间(毫秒)
        /// </summary>
        public int Timeout
        {
            get
            {
                return _timeout;
            }
            set
            {
                _timeout = value;
            }
        }

        public WebClientEx()
        {
            this._timeout = 60000;
        }

        public WebClientEx(int timeout)
        {
            this._timeout = timeout;
        }

        public string GetWebRequest(Uri address)
        {
            //HttpWebResponse resp = new HttpWebResponse();

            var req = HttpWebRequest.CreateHttp(address);
            req.ContinueTimeout = this._timeout;

            var resp = req.GetResponseAsync().Result;
            var stream = resp.GetResponseStream();
            byte[] b = new byte[stream.Length];
            stream.Read(b, 0, (int)stream.Length);

            var result = System.Text.Encoding.UTF8.GetString(b);
            return result;

            ////  resp.Result

            //var result = base.GetWebRequest(address);
            //result.Timeout = this._timeout;
            //return result;
        }
    }
}