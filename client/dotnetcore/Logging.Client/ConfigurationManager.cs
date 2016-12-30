using System.Collections.Generic;
using System.Collections.Specialized;

namespace Logging.Client
{
    public class ConfigurationManager
    {
        private static Dictionary<string, string> _appSettings { get; set; }

        static ConfigurationManager()
        {
            // _appSettings = new Dictionary<string, string>();

            AppSettings = new NameValueCollection();
        }

        public static NameValueCollection AppSettings { get; set; }

        //public static Dictionary<string, string> AppSettings
        //{
        //    get { return _appSettings; }
        //}
    }
}