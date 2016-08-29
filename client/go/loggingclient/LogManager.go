package loggingclient

import (
	"fmt"
	"time"
)

// var _instance *object

// type object struct {
//     name string
// }

// func Instance() *object {
//    if _instance == nil {
//        _instance = new(object)
//    }
//    return _instance
// }

// func (p *object) Setname(name string) {
//     p.name = name
// }

// func (p *object) Say() {
// 	fmt.Println(p.name)
// }

// type LogManager struct {
// 	Logs map[string]ILog
// }
var logs map[string]ILog

func init() {
	logs = make(map[string]ILog)
}

// GetLogger 获取ILog对象
func GetLogger(source string) (ret ILog) {

	if log, ok := logs[source]; ok {
		return log
	}
	newLog := NewBaseLogger(source)
	fmt.Println(time.Now().String() + ":创建ILog " + source)
	logs[source] = newLog
	return newLog
}

func loggingEnabled() bool {
	return LoggingSettings != nil && LoggingSettings.LoggingEnabled
}
