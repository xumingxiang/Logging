package loggingclient

import (
	list "container/list"
	"time"
)

type TimerActionBlock struct {
	Action func(buffer list.List)

	Queue chan interface{}

	// /// <summary>
	// /// 最近一次异常报告时间
	// /// </summary>
	// LastReportTime time.Time

	// /// <summary>
	// /// 出现的异常数量
	// /// </summary>
	// ExceptionCount int

	// /// <summary>
	// /// 溢出数量
	// /// </summary>
	// OverCount int

	// /// <summary>
	// /// 最近一次异常
	// /// </summary>
	// LastException error

	/// <summary>
	/// 阻塞队列的最大长度
	/// </summary>
	QueueMaxLength int

	Buffer list.List

	/// <summary>
	/// 元素包的大小
	/// </summary>
	BufferSize int

	/// <summary>
	/// 上一次打包处理的时间
	/// </summary>
	LastActionTime time.Time

	//队列阻塞时间。单位：秒
	BlockElapsed int
}

// NewTimerActionBlock 实例化一个TimerActionBlock引用对象
func NewTimerActionBlock(action func(buffer list.List), queueMaxLength int, bufferSize int, blockElapsed int) *TimerActionBlock {
	var block = new(TimerActionBlock)
	block.Action = action
	block.BlockElapsed = blockElapsed
	block.BufferSize = bufferSize
	block.QueueMaxLength = queueMaxLength
	block.Buffer = list.List{}
	block.LastActionTime = time.Now()
	block.Queue = make(chan interface{}, block.QueueMaxLength)
	return block
}

func (block *TimerActionBlock) enqueue(item interface{}) {
	queueLen := len(block.Queue)
	if queueLen >= block.QueueMaxLength {
		for i := 0; i <= queueLen-block.QueueMaxLength; i++ {
			<-block.Queue
		}
	} else {
		block.Queue <- item
	}
}

func (block *TimerActionBlock) dequeue() {
	for true {
		item, has := block.popQueue()
		if has {
			block.Buffer.PushBack(item)
		}
		block.checkBufferSize()
		block.checkBlockElapsed()
	}
}

func (block *TimerActionBlock) popQueue() (item interface{}, has bool) {
	select {
	case item, _ := <-block.Queue:
		return item, true
	case <-time.After(time.Millisecond * 200):
		return nil, false
	}
}

func (block *TimerActionBlock) checkBufferSize() {
	if block.Buffer.Len() >= block.BufferSize {
		block.executeAction()
	}
}

func (block *TimerActionBlock) checkBlockElapsed() {
	if block.Buffer.Len() <= 0 {
		return
	}
	var timeDiff = time.Now().Sub(block.LastActionTime).Seconds()
	if int(timeDiff) > block.BlockElapsed {
		block.executeAction()
	}
}

func (block *TimerActionBlock) flush() {
	for true {
		item, has := block.popQueue()
		if has {
			block.Buffer.PushBack(item)
		}
	}
	block.executeAction()
}

func (block *TimerActionBlock) executeAction() {
	block.Action(block.Buffer)
	block.Buffer.Init()
	block.LastActionTime = time.Now()
}
