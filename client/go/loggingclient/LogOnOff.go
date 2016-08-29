package loggingclient

//日志开关
type LogOnOff struct {
	AppId int

	Debug byte

	Info byte

	Warm byte

	Error byte
}
