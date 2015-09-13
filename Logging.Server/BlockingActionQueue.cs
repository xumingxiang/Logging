using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Logging.Server
{
    /// <summary>
    /// 多线程消费队列（BlockingCollection实现）
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class BlockingActionQueue<T>
    {

        /// <summary>
        /// 当前队列长度
        /// </summary>
        public int QueueLength { get; private set; }

        public Task[] Tasks { get; set; }

        private Action<T> Action { get; set; }

        private BlockingCollection<T> s_Queue;

        /// <summary>
        /// 阻塞队列的最大长度
        /// </summary>
        private int QueueMaxLength { get; set; }


        /// <summary>
        /// 多线程消费队列
        /// </summary>
        /// <param name="taskNum">处理队列出队的线程数量</param>
        /// <param name="action">处理委托</param>
        /// <param name="queueMaxLength">设置队列最大长度</param>
        public BlockingActionQueue(int taskNum, Action<T> action, int queueMaxLength)
        {
            if (taskNum <= 0) { taskNum = 1; }
            if (queueMaxLength <= 0) { queueMaxLength = int.MaxValue; }

            s_Queue = new BlockingCollection<T>(new ConcurrentQueue<T>(), queueMaxLength);
            this.Action = action;
            this.QueueMaxLength = queueMaxLength;
            this.Tasks = new Task[taskNum];
            for (int i = 0; i < taskNum; i++)
            {
                int temp_i = i;
                this.Tasks[temp_i] = Task.Factory.StartNew(this.DequeueProcess);
            }
        }

        /// <summary>
        /// 入队处理
        /// </summary>
        /// <param name="item"></param>
        public void Enqueue(T item)
        {
            int queueLen = s_Queue.Count;
            if (queueLen >= this.QueueMaxLength)
            {
                for (int i = 0; i < (queueLen - this.QueueMaxLength) + 1; i++)
                {
                    this.s_Queue.Take();
                }

            }
            this.s_Queue.Add(item);
        }

        /// <summary>
        /// 出队处理函数
        /// </summary>
        private void DequeueProcess()
        {
            while (true)
            {
                try
                {
                    T item = s_Queue.Take();
                 
                    this.Action(item);
                    //  Console.WriteLine("again");
                }
                catch (ThreadAbortException)
                {
                    Thread.ResetAbort();
                    //do exception...
                }
                catch (Exception ex)
                {

                }
            }
        }
    }
}
