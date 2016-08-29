package loggingclient

func getLogSender() ILogSender {

	var iLogSender ILogSender = NewTHttpLogSender()
	return iLogSender
}
