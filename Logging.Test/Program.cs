using Logging.Client;
using Logging.Client.Widgets;
using Logging.Server;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logging.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            //string str = "！@##￥&*&……%￥%adadad阿达add第三大大豆1233123ASASASAS";
            //var result1 = Logging.Server.Utils.BKDRHash(str);//1236678004
            //long result2 = Logging.Server.Utils.Time33(str);//5480260620523375082
            //Console.WriteLine(result1);
            //Console.WriteLine(result2);
            //Console.ReadLine();

            //ILog logger = LogManager.GetLogger("");
            //var resp = logger.GetLogs(14421175343224598, 14421175343174565, 1002);

            //var logvm = Newtonsoft.Json.JsonConvert.DeserializeObject<JObject>(resp);

            //Console.WriteLine(Utils.GetDateTime(14420752600137110));
            //Console.WriteLine(int.MaxValue);
            //Console.WriteLine(long.MaxValue);
            System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            Parallel.For(0,100000000,(i)=> {

                WebApiMetric.Count("http://www.cnblogs.com/PurpleTide/archive");

            });

            watch.Stop();
            var ms=WebApiMetric.metrics;

           // Console.WriteLine(ms["http://www.cnblogs.com/PurpleTide/archive"]);
            Console.WriteLine(watch.ElapsedMilliseconds);
            Console.ReadLine();
        }
    }
}
