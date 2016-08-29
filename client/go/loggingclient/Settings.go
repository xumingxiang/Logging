package loggingclient

import (
	"encoding/json"
	"fmt"
	"io/ioutil"
	"time"
)

var LoggingSettings *Settings

func initLoggingSettings() *Settings {
	if LoggingSettings == nil {
		LoggingSettings = getSettingsFromFile()
	}
	return LoggingSettings
}

// StartUp 启动LoggintClient,如果存在配置文件，无需调用此方法
func StartUp(appId int, loggingServerHost string) {
	if LoggingSettings == nil {
		LoggingSettings = NewSettings(appId, loggingServerHost)
	}
	fmt.Println(time.Now().String() + "：LoggingClient StartUp ！")
}

func init() {
	initLoggingSettings()
}

// Settings 设置选项
type Settings struct {

	//应用号
	AppId int32

	//是否启用日志
	LoggingEnabled bool

	//设置日志服务器
	LoggingServerHost string

	//队列最大长度
	QueueMaxLength int

	//元素包的大小
	BufferSize int

	//队列阻塞时间。单位：秒
	BlockElapsed int

	//开关缓存时间。单位：分钟
	LogOnOffCacheTimeOut int
}

// NewSettings 创建一个NewSettings 引用对象
func NewSettings(appId int, loggingServerHost string) *Settings {
	var settings = new(Settings)
	settings.AppId = int32(appId)
	settings.LoggingServerHost = loggingServerHost
	settings.LoggingEnabled = true
	settings.QueueMaxLength = 50000
	settings.BufferSize = 300
	settings.BlockElapsed = 5
	settings.LogOnOffCacheTimeOut = 5
	fmt.Println(time.Now().String() + "初始化LoggingClient配置项")
	return settings
}

//读取配置文件
func getSettingsFromFile() *Settings {

	settingsFilePath := "loggingclient.config"
	b, err := ioutil.ReadFile(settingsFilePath)
	if err != nil {
		fmt.Println(err)
		return nil
	}
	text := string(b)
	var settings Settings
	if err2 := json.Unmarshal([]byte(text), &settings); err2 == nil {

	} else {
		fmt.Println(err2)
	}
	if settings.QueueMaxLength <= 0 {
		settings.QueueMaxLength = 5000
	}
	if settings.BufferSize <= 0 {
		settings.BufferSize = 300
	}
	if settings.BlockElapsed <= 0 {
		settings.BlockElapsed = 5
	}
	if settings.LogOnOffCacheTimeOut <= 0 {
		settings.LogOnOffCacheTimeOut = 5
	}
	fmt.Println(time.Now().String() + "初始化LoggingClient配置项FromFile")
	return &settings
}
