package loggingclient

// ILog loggingclient对外提供的接口
type ILog interface {
	Debug(params ...interface{})

	Info(params ...interface{})

	Warm(params ...interface{})

	Error(params ...interface{})

	Debugf(format string, params ...interface{})

	Infof(format string, params ...interface{})

	Warmf(format string, params ...interface{})

	Errorf(format string, params ...interface{})

	Metric(name string, value float64, tags map[string]string)
}
