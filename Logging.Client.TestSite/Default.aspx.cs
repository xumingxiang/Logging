using System;
using System.Collections.Generic;

namespace Logging.Client.TestSite
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ILog logger = LogManager.GetLogger(typeof(Default));

            logger.Debug("test");
            logger.Info("aabbbbbcc","test");
            logger.Warm("test", "大大的打算打算大大",null);
            Dictionary<string,string> tags=new Dictionary<string,string>();
            tags.Add("a","a");
            logger.Error("test","test",tags);
        }
    }
}