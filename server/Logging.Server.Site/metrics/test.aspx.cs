
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
            //List<MetricEntity> lst = new List<MetricEntity>();
            //for (int i = 0; i < 10; i++)
            //{
            //    var tag = new Dictionary<string, string>();
            //    tag.Add("tag1", "bb");
            //    lst.Add(new MetricEntity { Name = "jjjj", Value = 11, Time = Utils.GetUnixTime(DateTime.Now),Tags= tag});
            //}
            //InfluxdbWriter w = new InfluxdbWriter();
            //w.Write(lst);

            //InfluxDBClient client = new InfluxDBClient("http://10.25.69.161:8086", "root","root");
            //var points = new List<IInfluxDatapoint>();
            //var valMixed = new InfluxDatapoint<InfluxValueField>();
            //valMixed.UtcTimestamp = DateTime.UtcNow;
            //var time = DateTime.Now;
            //valMixed.Tags.Add("TestDate", time.ToShortDateString());
            //valMixed.Tags.Add("TestTime", time.ToShortTimeString());
            //valMixed.Fields.Add("Doublefield", new InfluxValueField(123.8));
            //valMixed.Fields.Add("Stringfield", new InfluxValueField("eqeq"));
            //valMixed.Fields.Add("Boolfield", new InfluxValueField(true));
            //valMixed.Fields.Add("Int Field", new InfluxValueField(2313));

            //valMixed.MeasurementName = "test7";


            //points.Add(valMixed);
            //points.Add(valMixed);
            //var valDouble = new InfluxDatapoint<double>();
            //valDouble.UtcTimestamp = DateTime.UtcNow;
            //valDouble.Tags.Add("TestDate", "today");
            //valDouble.Tags.Add("TestTime", DateTime.Now.ToString());
            //valDouble.Fields.Add("Doublefield", 123);
            //valDouble.Fields.Add("Doublefield2",434);
            //valDouble.MeasurementName = "test8";
            //valDouble.Precision = TimePrecision.Nanoseconds;
            //points.Add(valDouble);
            //points.Add(valDouble);

            //var time = DateTime.Now;
            //var today = DateTime.Now.ToShortDateString();
            //var now = DateTime.Now.ToShortTimeString();


            //var valDouble = new InfluxDatapoint<double>();
            //valDouble.UtcTimestamp = DateTime.UtcNow;
            //valDouble.Tags.Add("TestDate", today);
            //valDouble.Tags.Add("TestTime", now);
            //valDouble.Fields.Add("Doublefield", DataGen.RandomDouble());
            //valDouble.Fields.Add("Doublefield2", DataGen.RandomDouble());
            //valDouble.MeasurementName = measurementName;
            //valDouble.Precision = TimePrecision.Nanoseconds;
            //points.Add(valDouble);

            //valDouble = new InfluxDatapoint<double>();
            //valDouble.UtcTimestamp = DateTime.UtcNow;
            //valDouble.Tags.Add("TestDate", today);
            //valDouble.Tags.Add("TestTime", now);
            //valDouble.Fields.Add("Doublefield", DataGen.RandomDouble());
            //valDouble.Fields.Add("Doublefield2", DataGen.RandomDouble());
            //valDouble.MeasurementName = measurementName;
            //valDouble.Precision = TimePrecision.Microseconds;
            //points.Add(valDouble);

            //var valInt = new InfluxDatapoint<int>();
            //valInt.UtcTimestamp = DateTime.UtcNow;
            //valInt.Tags.Add("TestDate", today);
            //valInt.Tags.Add("TestTime", now);
            //valInt.Fields.Add("Intfield", DataGen.RandomInt());
            //valInt.Fields.Add("Intfield2", DataGen.RandomInt());
            //valInt.MeasurementName = measurementName;
            //valInt.Precision = TimePrecision.Milliseconds;
            //points.Add(valInt);

            //valInt = new InfluxDatapoint<int>();
            //valInt.UtcTimestamp = DateTime.UtcNow;
            //valInt.Tags.Add("TestDate", today);
            //valInt.Tags.Add("TestTime", now);
            //valInt.Fields.Add("Intfield", DataGen.RandomInt());
            //valInt.Fields.Add("Intfield2", DataGen.RandomInt());
            //valInt.MeasurementName = measurementName;
            //valInt.Precision = TimePrecision.Seconds;
            //points.Add(valInt);

            //var valBool = new InfluxDatapoint<bool>();
            //valBool.UtcTimestamp = DateTime.UtcNow;
            //valBool.Tags.Add("TestDate", today);
            //valBool.Tags.Add("TestTime", now);
            //valBool.Fields.Add("Booleanfield", time.Ticks % 2 == 0);
            //valBool.MeasurementName = measurementName;
            //valBool.Precision = TimePrecision.Minutes;
            //points.Add(valBool);

            //var valString = new InfluxDatapoint<string>();
            //valString.UtcTimestamp = DateTime.UtcNow;
            //valString.Tags.Add("TestDate", today);
            //valString.Tags.Add("TestTime", now);
            //valString.Fields.Add("Stringfield", DataGen.RandomString());
            //valString.MeasurementName = measurementName;
            //valString.Precision = TimePrecision.Hours;
            //points.Add(valString);


            //client.PostPointsAsync("metrics", points);

        }
    }
}