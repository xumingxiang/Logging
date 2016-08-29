package loggingclient

type LogEntity struct {
	Type int

	Title string

	Message string

	Level int8

	Time int64

	Source string

	Thread int32

	Tags map[string]string
}

func NewLogEntity() *LogEntity {

	var logEntity = new(LogEntity)
	logEntity.Type = 1
	logEntity.Thread = 0 //Thread.CurrentThread.ManagedThreadId
	return logEntity
}

func (this *LogEntity) getType() int {
	return 1
}
