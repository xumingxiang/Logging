package loggingclient

import (
	"container/list"
	"fmt"
	"time"
)

// BaseLogger ILog 实现基类
type BaseLogger struct {
	Source string
}

var _block *TimerActionBlock

func getBlock() *TimerActionBlock {
	if _block == nil {
		_block = NewTimerActionBlock(
			send,
			LoggingSettings.QueueMaxLength,
			LoggingSettings.BufferSize,
			LoggingSettings.BlockElapsed)
		go _block.dequeue()
		fmt.Println(time.Now().String() + "初始化日志队列")
	}
	return _block
}

func send(buffer list.List) {
	logSender := getLogSender()
	logSender.send(buffer)
	loom := NewLogOnOffManager()
	loom.RefreshLogOnOff()

}

// NewBaseLogger 实例化一个BaseLogger引用对象
func NewBaseLogger(source string) *BaseLogger {
	var baseLogger = new(BaseLogger)
	baseLogger.Source = source
	//baseLogger.LoggingEnabled = loggingEnabled()
	return baseLogger
}

// Debug 实现ILog.Debug
func (bl *BaseLogger) Debug(params ...interface{}) {
	title, message, tags := bl.getLogParams(params)
	bl.log(title, message, tags, 1)
}

// Info 实现ILog.Info
func (bl *BaseLogger) Info(params ...interface{}) {
	title, message, tags := bl.getLogParams(params)
	bl.log(title, message, tags, 2)
}

// Warm 实现ILog.Warm
func (bl *BaseLogger) Warm(params ...interface{}) {
	title, message, tags := bl.getLogParams(params)
	bl.log(title, message, tags, 3)
}

// Error 实现ILog.Error
func (bl *BaseLogger) Error(params ...interface{}) {
	title, message, tags := bl.getLogParams(params)
	bl.log(title, message, tags, 4)
}

// Debugf 实现ILog.Debugf
func (bl *BaseLogger) Debugf(format string, params ...interface{}) {
	message := fmt.Sprintf(format, params...)
	bl.log("", message, nil, 1)
}

// Infof 实现ILog.Infof
func (bl *BaseLogger) Infof(format string, params ...interface{}) {
	message := fmt.Sprintf(format, params...)
	bl.log("", message, nil, 2)
}

// Warmf 实现ILog.Warmf
func (bl *BaseLogger) Warmf(format string, params ...interface{}) {
	message := fmt.Sprintf(format, params...)
	bl.log("", message, nil, 3)
}

// Errorf 实现ILog.Errorf
func (bl *BaseLogger) Errorf(format string, params ...interface{}) {
	message := fmt.Sprintf(format, params...)
	bl.log("", message, nil, 4)
}

// Metric 实现ILog.Metric
func (bl *BaseLogger) Metric(name string, value float64, tags map[string]string) {
	if !LoggingSettings.LoggingEnabled {
		return
	}
	var Metric = NewMetricEntity()
	Metric.Name = name
	Metric.Value = value
	Metric.Tags = tags
	Metric.Time = time.Now().Unix()
	var block = getBlock()
	block.enqueue(Metric)
}

// Metric2 实现ILog.Metric2
func (bl *BaseLogger) Metric2(name string, value float64, tags map[string]string) {
	if !LoggingSettings.LoggingEnabled {
		return
	}
	var Metric = NewMetricEntity()
	Metric.Name = name
	Metric.Value = value
	Metric.Tags = tags
	Metric.Time = time.Now().Unix()
	var block = getBlock()
	block.enqueue(Metric)
}

func (bl *BaseLogger) getLogParams(params interface{}) (title string, message string, tags map[string]string) {
	var _title = ""
	var _message = ""
	var _tags map[string]string
	var _parmas = params.([]interface{})
	var argLen = len(_parmas)
	if argLen == 1 {
		_message = bl.getLogParamStr(_parmas[0])
	} else if argLen == 2 {
		_title = bl.getLogParamStr(_parmas[0])
		_message = bl.getLogParamStr(_parmas[1])
	} else if argLen == 3 {
		_title = bl.getLogParamStr(_parmas[0])
		_message = bl.getLogParamStr(_parmas[1])
		_tags = _parmas[2].(map[string]string)
	} else {
		_title = bl.getLogParamStr(_parmas[0])
		_message = fmt.Sprint(_parmas)
	}
	return _title, _message, _tags
}

func (bl *BaseLogger) getLogParamStr(param interface{}) (str string) {
	switch param.(type) {
	case string:
		str = param.(string)
	case error:
		str = param.(error).Error()
	default:
		str = fmt.Sprint(param)
	}
	return str
}

func (bl *BaseLogger) createLog(source string, title string, message string, tags map[string]string, level int8) (ret *LogEntity) {
	var log = NewLogEntity()
	log.Level = level
	log.Message = message
	log.Tags = tags
	log.Time = time.Now().UnixNano() / 100
	log.Title = title
	log.Source = source
	return log
}

func (bl *BaseLogger) log(title string, message string, tags map[string]string, level int8) {
	if !LoggingSettings.LoggingEnabled {
		return
	}
	var loom = NewLogOnOffManager()
	var onOff = loom.getLogOnOff()
	if level == 1 && onOff.Debug != 1 {
		return
	}
	if level == 2 && onOff.Info != 1 {
		return
	}
	if level == 3 && onOff.Warm != 1 {
		return
	}
	if level == 4 && onOff.Error != 1 {
		return
	}
	var log = bl.createLog(bl.Source, title, message, tags, level)
	var block = getBlock()
	block.enqueue(log)
}
