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
            HttpLogSender logsender = new HttpLogSender();
            List<LogEntity> logs = new List<LogEntity>();
            logs.Add(new LogEntity { IP="192.168.1.1", Level=1, Message="test_message", Title= "test_title" });
            logsender.Send(logs);
        }
    }
}