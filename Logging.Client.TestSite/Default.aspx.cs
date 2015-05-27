using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Logging.Client.TestSite
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ILog logger = Logging.Client.LogManager.GetLogger(typeof(Default));

            logger.Debug("test");
            logger.Info("test","test");
            logger.Warm("test", "test",null);
            Dictionary<string,string> tags=new Dictionary<string,string>();
            tags.Add("a","a");
            logger.Error("test","test",tags);
        }
    }
}