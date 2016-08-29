package logging.client;

import java.util.ArrayList;
import java.util.Date;
import java.util.List;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;
import java.util.concurrent.LinkedBlockingQueue;
import java.util.concurrent.Callable;

public class TimerActionBlock<T> implements ITimerActionBlock<T> {

    private LinkedBlockingQueue<T> queue;

    /// <summary>
    /// 阻塞队列的最大长度
    /// </summary>
    private int QueueMaxLength;

    /// <summary>
    /// 元素包的大小
    /// </summary>
    private int BufferSize;

    /// <summary>
    /// 上一次打包处理的时间
    /// </summary>
    private Date LastActionTime;

    private ExecutorService pool = Executors.newSingleThreadExecutor();

    private int BlockElapsed;

    private DequeueAction<T> Action;

    public TimerActionBlock(DequeueAction<T> action, int queueMaxLength, int bufferSize, int blockElapsed) {

        this.BufferSize = bufferSize;
        this.LastActionTime = new Date();
        this.BlockElapsed = blockElapsed;
        this.Action = action;
        this.QueueMaxLength = queueMaxLength;
        this.queue = new LinkedBlockingQueue<>(this.QueueMaxLength);

    }

    public void Enqueue(T item) {
        int queueLen = this.queue.size();
        if (queueLen > 0 && (queueLen >= BufferSize || (new Date().getTime() - this.LastActionTime.getTime()) >= this.BlockElapsed)) {
            List<T> buffer = new ArrayList<>();
            for (int i = 0; i < queueLen; i++) {
                try {
                    buffer.add(this.queue.take());
                    buffer.add(item);
                } catch (InterruptedException e) {
                    e.printStackTrace();
                }
            }
            pool.submit(new DequeueActionTask(buffer));
            this.LastActionTime = new Date();
        } else {
            queue.add(item);
        }
    }

    public void flush() {
        int queueLen = this.queue.size();
        List<T> buffer = new ArrayList<>();
        for (int i = 0; i < queueLen; i++) {
            try {
                buffer.add(this.queue.take());
            } catch (InterruptedException e) {
                e.printStackTrace();
            }
        }
        this.Action.execute(buffer);
    }

    private final class DequeueActionTask implements Callable<String> {
        List<T> _data;

        public DequeueActionTask(List<T> data) {
            _data = data;
        }

        public String call() throws Exception {
            Action.execute(_data);
            return "ok";
        }
    }

    public interface DequeueAction<T> {
        void execute(List<T> items);
    }
}
