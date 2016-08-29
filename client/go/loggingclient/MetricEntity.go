package loggingclient

type MetricEntity struct {
	Type int

	Name string

	Value float64

	Time int64

	Tags map[string]string
}

func (this *MetricEntity) getType() int {
	return 2
}

// NewMetricEntity 实例化一个MetricEntity引用对象
func NewMetricEntity() *MetricEntity {

	var metricEntity = new(MetricEntity)
	metricEntity.Type = 2
	return metricEntity
}
