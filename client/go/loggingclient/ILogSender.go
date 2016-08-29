package loggingclient

import "container/list"

type ILogSender interface {
	send(iLogEntities list.List)
}
