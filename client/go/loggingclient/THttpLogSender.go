package loggingclient

import (
	"container/list"
	"fmt"
	"git.corp.plu.cn/plugo/infrastructure/loggingclient/thrift"
	"time"
)

type THttpLogSender struct {
	sendURL string
}

// NewTHttpLogSender 初始化一个THttpLogSender引用对象
func NewTHttpLogSender() *THttpLogSender {
	var sender = new(THttpLogSender)
	sender.sendURL = LoggingSettings.LoggingServerHost + "/Reciver.ashx"
	return sender
}

func (sender *THttpLogSender) createLogPackage(iLogEntities list.List) *TLogPackage {

	tlogSlice := make([]*TLogEntity, 0, iLogEntities.Len())
	tmetricSlice := make([]*TMetricEntity, 0, iLogEntities.Len())
	for e := iLogEntities.Front(); e != nil; e = e.Next() {
		var logType = e.Value.(ILogEntity).getType()
		switch logType {
		case 1:
			var item = e.Value.(*LogEntity)
			tlogSlice = append(tlogSlice, sender.converToTLogEntity(item))
		case 2:
			var item = e.Value.(*MetricEntity)
			tmetricSlice = append(tmetricSlice, sender.converToTMetricEntity(item))
		default:
			fmt.Println(logType)
		}
	}
	ipAddr := getInternalIP()
	ipNum := ipToNum(ipAddr)
	var logPackage = NewTLogPackage()
	logPackage.AppId = LoggingSettings.AppId
	logPackage.IP = ipNum
	logPackage.LogItems = tlogSlice
	logPackage.MetricItems = tmetricSlice
	return logPackage
}

func (sender *THttpLogSender) converToTLogEntity(logEntity *LogEntity) *TLogEntity {
	var result = TLogEntity{}
	result.Level = logEntity.Level
	result.Message = logEntity.Message
	result.Source = logEntity.Source
	result.Tags = logEntity.Tags
	result.Thread = logEntity.Thread
	result.Time = logEntity.Time
	result.Title = logEntity.Title
	return &result
}

func (sender *THttpLogSender) converToTMetricEntity(logMetric *MetricEntity) *TMetricEntity {
	var result = TMetricEntity{}
	result.Value = logMetric.Value
	result.Time = logMetric.Time
	result.Name = logMetric.Name
	result.Tags = logMetric.Tags
	return &result
}

func (sender *THttpLogSender) send(iLogEntities list.List) {
	var logPackage = sender.createLogPackage(iLogEntities)
	transport, err := thrift.NewTHttpPostClient(sender.sendURL)
	if err != nil {
		fmt.Println(time.Now().String() + "：Error NewTHttpPostClient " + sender.sendURL + err.Error())
		return
	}
	protocolFactory := thrift.NewTCompactProtocolFactory()
	if err := transport.Open(); err != nil {
		fmt.Println(time.Now().String() + "：Error transport open " + sender.sendURL + err.Error())
		return
	}
	client := NewLogTransferServiceClientFactory(transport, protocolFactory)
	if err := client.Log(logPackage); err != nil {
		fmt.Println(time.Now().String() + "：Error Log " + sender.sendURL + err.Error())
		return
	}
	defer transport.Close()
}
