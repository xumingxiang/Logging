package loggingclient

import (
	"io/ioutil"
	"net/http"
	"strconv"
	"strings"
	"time"
)

// LogOnOffManager 日志开关管理类
type LogOnOffManager struct {
	LogOnOffCacheTimeOut int

	LogOnOffGetURL string

	LastUpdateTime time.Time

	LogOnOff *LogOnOff
}

// NewLogOnOffManager 实例化一个LogOnOffManager引用对象
func NewLogOnOffManager() *LogOnOffManager {

	var loom = new(LogOnOffManager)
	loom.LogOnOffCacheTimeOut = LoggingSettings.LogOnOffCacheTimeOut
	loom.LogOnOffGetURL = LoggingSettings.LoggingServerHost + "/GetLogOnOff.ashx?appId=" + strconv.Itoa(int(LoggingSettings.AppId))
	loom.LogOnOff = &LogOnOff{Debug: 1, Error: 1, Info: 1, Warm: 1}
	loom.LastUpdateTime = time.Now()
	return loom
}

// GetLogOnOff 获取日志开关
func (loom *LogOnOffManager) getLogOnOff() (ret *LogOnOff) {
	if &loom.LogOnOff == nil {
		return &LogOnOff{Debug: 1, Error: 1, Info: 1, Warm: 1}
	}
	return loom.LogOnOff
}

// RefreshLogOnOff 从服务端获取并刷新日志开关,10分钟缓存
func (loom *LogOnOffManager) RefreshLogOnOff() {

	var timeDiff = time.Now().Sub(loom.LastUpdateTime).Minutes()
	if int(timeDiff) < loom.LogOnOffCacheTimeOut {
		return
	}
	response, _ := http.Get(loom.LogOnOffGetURL)

	body, _ := ioutil.ReadAll(response.Body)
	var resp = string(body)
	if resp != "" {
		var arr = strings.Split(resp, ",")

		var _debug, _ = strconv.Atoi(arr[0])
		var _info, _ = strconv.Atoi(arr[1])
		var _warm, _ = strconv.Atoi(arr[2])
		var _error, _ = strconv.Atoi(arr[3])

		loom.LogOnOff.Debug = byte(_debug)
		loom.LogOnOff.Info = byte(_info)
		loom.LogOnOff.Warm = byte(_warm)
		loom.LogOnOff.Error = byte(_error)

	}
	loom.LastUpdateTime = time.Now()
	defer response.Body.Close()
}
