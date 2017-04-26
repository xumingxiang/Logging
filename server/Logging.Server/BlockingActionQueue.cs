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
        ///// <summary>
        ///// 当前队列长度
        ///// </summary>
        //public int QueueLength { get; private set; }

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

            s_Queue = new BlockingCollection<T>();
            this.Action = action;
            this.QueueMaxLength = queueMaxLength;
            this.Tasks = new Task[taskNum];
            for (int i = 0; i < taskNum; i++)
            {
                int temp_i = i;
                this.Tasks[temp_i] = Task.Factory.StartNew(this.DequeueProcess, TaskCreationOptions.LongRunning);
            }
        }

        /// <summary>
        /// 入队处理
        /// </summary>
        /// <param name="item"></param>
        public int Enqueue(T item)
        {
            int queueLen = s_Queue.Count;
            int over_count = 0;
            if (queueLen >= this.QueueMaxLength)
            {
                over_count = (queueLen - this.QueueMaxLength) + 1;
                for (int i = 0; i < over_count; i++)
                {
                    this.s_Queue.Take();//超过队列长度，扔掉
                }
            }
            // this.s_Queue.Enqueue(item);
            this.s_Queue.Add(item);
            return over_count;
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
                }
                catch (ThreadAbortException tae)
                {
                    Thread.ResetAbort();
                    FileLogger.Log(tae);//TODO:此处写文件件会造成巨大文件
                    //do exception...
                }
                catch (Exception ex)
                {
                    FileLogger.Log(ex);//TODO:此处写文件件会造成巨大文件
                }
            }
        }
    }
}