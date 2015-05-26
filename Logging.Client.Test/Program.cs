using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Logging.Client.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            TimerBatchBlockTest();

            Console.ReadLine();
        }

        static void TimerBatchBlockTest()
        {
            TimerBatchBlock<int> b = new TimerBatchBlock<int>(1, (batch) =>
            {
                string txt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + ":";
                foreach (var item in batch)
                {
                    txt += item + ",";
                }

                Console.WriteLine(txt);

            }, 1000, 10, 100);

            int i = 0;
            while (i < 105)
            {
                i++;
                b.Enqueue(i);
            }
        }
    }
}
