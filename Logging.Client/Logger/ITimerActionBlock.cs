namespace Logging.Client
{
    internal interface ITimerActionBlock<T>
    {
        void Enqueue(T item);
    }
}