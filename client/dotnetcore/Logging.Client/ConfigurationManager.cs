using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace Logging.Client
{
    public class ConfigurationManager
    {

        static Dictionary<string, string> _appSettings { get; set; }


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
