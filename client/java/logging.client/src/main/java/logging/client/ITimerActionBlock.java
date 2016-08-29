package logging.client;

public interface ITimerActionBlock<T> {
	 void Enqueue(T item);
}
