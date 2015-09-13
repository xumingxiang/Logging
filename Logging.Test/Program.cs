using Logging.Server;
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

            Console.WriteLine(Utils.GetDateTime(14420752600137110));
            Console.WriteLine(int.MaxValue);
            Console.WriteLine(long.MaxValue);
            Console.ReadLine();
        }
    }
}
