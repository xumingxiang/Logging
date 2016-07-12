using Logging.Server.Metric.Processor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Logging.Server.Site.Test
{
    public partial class MetricTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            var metric = MetricProcessorManager.GetMetricProcessor();

            IList<MetricEntity> logs = new List<MetricEntity>();
            logs.Add(new MetricEntity { Name = "plu_test_request_9", Value = 4110, Time = Utils.GetUnixTime(DateTime.Now) });
            logs.Add(new MetricEntity { Name = "plu_test_request_8", Value = 4110, Time = Utils.GetUnixTime(DateTime.Now) });
            logs.Add(new MetricEntity { Name = "plu_test_request_7", Value = 4110, Time = Utils.GetUnixTime(DateTime.Now) });
            logs.Add(new MetricEntity { Name = "plu_test_request_6", Value = 4110, Time = Utils.GetUnixTime(DateTime.Now) });
            metric.Process(logs);

            Response.Write(Utils.GetTimeStamp(DateTime.Now));
        }
    }
}