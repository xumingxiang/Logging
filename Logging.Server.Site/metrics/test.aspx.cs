using Logging.Server.Metric.Writer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Logging.Server.Site.metrics
{
    public partial class test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            List<MetricEntity> lst = new List<MetricEntity>();
            for (int i = 0; i < 100; i++)
            {
                var tag = new Dictionary<string, string>();
                tag.Add("tag1", "aa");
                lst.Add(new MetricEntity { Name = "test", Value = 11, Time = Utils.GetUnixTime(DateTime.Now) });
            }
            InfluxdbWriter w = new InfluxdbWriter();
            w.WriteAsync(lst);
        }
    }
}