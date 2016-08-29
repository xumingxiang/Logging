package loggingclient

type LogLevel int

const (
	No LogLevel = iota
	Debug
	Info
	Warm
	Error
)
