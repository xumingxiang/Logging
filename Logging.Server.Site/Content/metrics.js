// 对Date的扩展，将 Date 转化为指定格式的String
// 月(M)、日(d)、小时(h)、分(m)、秒(s)、季度(q) 可以用 1-2 个占位符， 
// 年(y)可以用 1-4 个占位符，毫秒(S)只能用 1 个占位符(是 1-3 位的数字) 
// 例子： 
// (new Date()).Format("yyyy-MM-dd hh:mm:ss.S") ==> 2006-07-02 08:09:04.423 
// (new Date()).Format("yyyy-M-d h:m:s.S")      ==> 2006-7-2 8:9:4.18 
Date.prototype.Format = function (fmt, utc) { //author: meizz 
    var o = {

        "M+": (utc ? this.getUTCMonth() : this.getMonth()) + 1, //月份 
        "d+": (utc ? this.getUTCDate() : this.getDate()), //日 
        "h+": (utc ? this.getUTCHours() : this.getHours()), //小时 
        "m+": (utc ? this.getUTCMinutes() : this.getMinutes()), //分 
        "s+": (utc ? this.getUTCSeconds() : this.getSeconds()), //秒 
        "q+": Math.floor(((utc ? this.getUTCMonth() : this.getMonth()) + 3) / 3), //季度 
        "S": utc ? this.getUTCMilliseconds() : this.getMilliseconds() //毫秒 
    };
    if (/(y+)/.test(fmt)) fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt)) fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
}

$(function () {

    //var host = "http://172.16.9.10:8086";
    //var db = "metrics";
    //var u = "root";
    //var p = "root";
    var query_base = "../metrics/MetricsQuery.ashx?cmd=";
    function queryAsLine() {

        var query_metric_name = $("#metric_name").val();
        var group_by_time = $("#group_by_time").val() || "1m";
        var aggr = $("#aggr").val();
        var group_by_tag = $("#grp_tag").val();

        var start_time_str = $("#start_time").val();
        var start_time = new Date(start_time_str);
        var utc_start_time_fm = start_time.Format("yyyy-MM-dd hh:mm:ss", true);

        var end_time_str = $("#end_time").val() || new Date().Format("yyyy-MM-dd hh:mm:ss");
        var end_time = new Date(end_time_str);
        var utc_end_time_fm = end_time.Format("yyyy-MM-dd hh:mm:ss", true);
        var filter_name = $("#flt_tag").val();
        var filter_val = $("#flite_by_tag").val();
        var displayType = $("#displayType").val();

        if (!query_metric_name || query_metric_name == "") {
            alert("Metric Name 不能为空");
            return;
        }

        //if (group_by_tag != "0") {
        //    if (query_metric_name.split(',').length >= 2) {
        //        alert("折线图 Group 功能暂时只支持单条Metric Name 查询");
        //        return;
        //    }
        //}

        if (end_time <= start_time) {
            alert("结束时间不能大于开始时间");
            return;
        }

        if (!!myChart) {
            myChart.clear();
            myChart.dispose();
        }

        var myChart = echarts.init(document.getElementById('chart-panel'));
        $("#empty_point").hide();
        myChart.showLoading({
            text: '正在努力的读取数据中...',    //loading话术
        });



        var query_str = query_base + 'select ' + aggr + '( value )' + ' from ' + query_metric_name + ' where ';
        if (start_time != "") {
            query_str += ' time > \'' + utc_start_time_fm + '\' and ';
        }

        query_str += ' time < \'' + utc_end_time_fm + '\' ';

        if (filter_name != "0" && filter_val != "") {
            query_str += ' and ' + filter_name + ' = \'' + filter_val + '\' ';
        }

        query_str += ' group%20by%20';
        if (group_by_tag != "") {
            query_str += group_by_tag + '%20%2C';
        }
        query_str += ' time(' + group_by_time + ')';

        $.ajax({
            'type': 'get',
            'url': query_str,
            'success': function (resp) {
                var result = eval("(" + resp + ")");
                if (!result || result.length == 0) {
                    myChart.hideLoading();
                    $("#empty_point").text("没有相关查到数据").show();
                    return;
                }

                var legend = [];
                var legend_map = [];
                var series = [];
                var xAxis = [];
                var xAxis_timestamp = [];
                var group_index = 2;

                //计算legend
                for (var i = 0; i < result.length; i++) {
                    var columns = result[i].columns;
                    var metric_name = result[i].name;
                    var legend_map_item = {};
                    legend_map_item.metric_name = metric_name;
                    legend_map_item.legend = [];
                    legend_map_item.data_index = i;

                    if (columns.length < 3) {

                        legend_map_item.legend.push(metric_name);
                        legend.push(metric_name);
                    } else {
                        var points = result[i].points;
                        for (var j = 0; j < points.length; j++) {
                            var group_val = points[j][group_index];
                            if (group_val != null && group_val != "") {
                                group_val = metric_name + "." + group_val;
                            } else {
                                group_val = metric_name
                            }

                            if (legend_map_item.legend.indexOf(group_val) < 0) {
                                legend_map_item.legend.push(group_val);

                            }

                            if (legend.indexOf(group_val) < 0) {
                                legend.push(group_val);
                            }
                        }
                    }
                    legend_map.push(legend_map_item);
                }

                //计算xAxis_timestamp、xAxis
                for (var i = 0; i < result.length; i++) {
                    var points = result[i].points;
                    var points_len = points.length;
                    for (var j = 0; j < points_len; j++) {
                        var timestamp = points[points_len - j - 1][0];
                        if (xAxis_timestamp.indexOf(timestamp) < 0) {
                            xAxis_timestamp.push(timestamp);
                        }
                    }
                }
                $.each(xAxis_timestamp, function (index, item) {
                    xAxis.push(new Date(item).Format("yy-MM-dd hh:mm:ss"));
                });

                //计算series
                for (var k = 0; k < legend_map.length; k++) {
                    var legend_item = legend_map[k].legend;
                    var metric_name = legend_map[k].metric_name;
                    for (var i = 0; i < legend_item.length; i++) {
                        var legend_item_val = legend_item[i];

                        var serie = {};
                        serie.name = legend_item_val;
                        serie.type = "line";
                        serie.data = [];
                        var points = result[legend_map[k].data_index].points;
                        var points_len = points.length;

                        //初始化折线点
                        var serie_data_map = new Map();
                        $.each(xAxis_timestamp, function (index, item) {
                            serie_data_map.set(item, "-");
                        });

                        for (var j = points_len - 1; j >= 0; j--) {
                            var point = points[j];
                            var group_val = point[group_index];
                            if (legend_item_val == metric_name || legend_item_val == metric_name + "." + group_val) {
                                serie_data_map.set(point[0], point[1]);
                            }
                        }

                        serie_data_map.forEach(function (i, key) {
                            serie.data.push(serie_data_map.get(key));
                        });

                        series.push(serie);
                    }

                }

                var option = {
                    tooltip: {
                        trigger: 'axis'
                    },
                    legend: {
                        data: legend
                    },
                    toolbox: {
                        show: true,
                        feature: {
                            mark: { show: true },
                            dataView: { show: true, readOnly: true },
                            magicType: { show: true, type: ['line', 'bar', 'stack', 'tiled'] },
                            restore: { show: true },
                            saveAsImage: { show: true }
                        }
                    },
                    calculable: true,
                    xAxis: [
                        {
                            type: 'category',
                            data: xAxis,
                        }
                    ],
                    yAxis: [
                        {
                            type: 'value',
                            splitArea: { show: true }
                        }
                    ],
                    series: series
                };

                myChart.hideLoading();
                myChart.setOption(option);
                window.onresize = myChart.resize;
            },
            'error': function (result) {
                myChart.hideLoading();
                $("#empty_point").text("Sorry，查询失败，请调整参数重试").show();
                return;
            }
        });
    }

    function queryAsPie() {

        var query_metric_name = $("#metric_name").val();
        var group_by_time = $("#group_by_time").val() || "1m";
        var aggr = $("#aggr").val();
        var group_by_tag = $("#grp_tag").val();

        var start_time_str = $("#start_time").val();
        var start_time = new Date(start_time_str);
        var utc_start_time_fm = start_time.Format("yyyy-MM-dd hh:mm:ss", true);

        var end_time_str = $("#end_time").val() || new Date().Format("yyyy-MM-dd hh:mm:ss");
        var end_time = new Date(end_time_str);
        var utc_end_time_fm = end_time.Format("yyyy-MM-dd hh:mm:ss", true);
        var filter_name = $("#flt_tag").val();
        var filter_val = $("#flite_by_tag").val();


        if (!query_metric_name || query_metric_name == "") {
            alert("Metric Name 不能为空");
            return;
        }

        if (end_time <= start_time) {
            alert("结束时间不能大于开始时间");
            return;
        }

        if (!!myChart) {
            myChart.clear();
            myChart.dispose();
        }

        var myChart = echarts.init(document.getElementById('chart-panel'));
        $("#empty_point").hide();
        myChart.showLoading({
            text: '正在努力的读取数据中...',    //loading话术
        });

        var query_str = query_base + 'select ' + aggr + '( value )' + ' from ' + query_metric_name + ' where ';
        if (start_time != "") {
            query_str += ' time > \'' + utc_start_time_fm + '\' and ';
        }

        query_str += ' time < \'' + utc_end_time_fm + '\' ';
        if (filter_name != "0" && filter_val != "") {
            query_str += ' and ' + filter_name + ' = \'' + filter_val + '\' ';
        }
        if (group_by_tag != "0") {
            query_str += ' group%20by%20';
            query_str += group_by_tag + '%20';
        }


        $.ajax({
            'type': 'get',
            'url': query_str,
            'success': function (resp) {
                var result = eval("(" + resp + ")");
                if (!result || result.length == 0) {
                    myChart.hideLoading();
                    $("#empty_point").text("没有相关查到数据").show();
                    return;
                }

                var legend = [];
                var serie = {};

                serie.name = "";

                serie.type = "pie";
                serie.radius = '55%';
                serie.center = ['50%', '60%'];

                serie.label = {
                    normal: {
                        show: true,
                        formatter: '{b} : {c} ({d}%)'
                    }
                };


                serie.itemStyle = {
                    emphasis: {
                        shadowBlur: 10,
                        shadowOffsetX: 0,
                        shadowColor: 'rgba(0, 0, 0, 0.5)'
                    },
                    normal: {
                        label: {
                            show: true,
                            formatter: '{b} : {c} ({d}%)'
                        },
                        labelLine: { show: true }
                    }
                };



                serie.data = [];
                for (var k = 0; k < result.length; k++) {
                    var points = result[k].points;
                    var metric_name = result[k].name;
                    var points_len = points.length;
                    for (var i = 0; i < points_len; i++) {
                        var legend_name = metric_name;
                        if (points[i].length >= 3) {
                            legend_name = metric_name + "." + points[i][2];
                        }
                        serie.data.push({ value: points[i][1], name: legend_name });
                        legend.push(legend_name);
                    }
                }

                var option = {
                    tooltip: {
                        trigger: 'item',
                        formatter: "{b} : {c} ({d}%)"
                    },
                    legend: {
                        data: legend
                    },
                    series: serie
                };

                myChart.hideLoading();
                myChart.setOption(option);
                window.onresize = myChart.resize;
            },
            'error': function (result) {
                myChart.hideLoading();
                $("#empty_point").text("Sorry，查询失败，请调整参数重试").show();
                return;
            }
        });
    }

    var now = new Date();
    var default_start_time = new Date((now.setHours(now.getHours() - 1))).Format("yyyy-MM-dd hh:mm:ss");
    var default_end_time = new Date().Format("yyyy-MM-dd hh:mm:ss");

    $("#start_time")
        .val(default_start_time)
        .datetimepicker({
            defaultValue: default_start_time,
            dateFormat: 'yy-mm-dd',
            showSecond: true,
            timeFormat: 'HH:mm:ss',
            currentText: '当前',
            closeText: '确定'
        });

    $("#end_time")
        .val(default_end_time)
        .datetimepicker({
            defaultValue: default_end_time,
            dateFormat: 'yy-mm-dd',
            showSecond: true,
            timeFormat: 'HH:mm:ss',
            currentText: '当前',
            closeText: '确定'
        });


    $("#metric_name").autocomplete({
        source: function (request, responseFn) {
            var metric_name = request.term;
            var query_str = query_base + 'list+series+%2F' + metric_name + '%2F';
            $.get(query_str, function (resp) {
                var result = eval("(" + resp + ")");
                var points = result[0].points;
                var series = $.map(points, function (item) {
                    return item[1];
                });
                responseFn(series);
            });
        },
        select: function (event, ui) {
            var metricName = ui.item.value;
            setTags(metricName);
        }
    });

    $("#group_by_tag").autocomplete({
        source: function (request, responseFn) {
            var metric_name = $("#metric_name").val();
            if (!metric_name || metric_name == "") { return false; }
            var kw = request.term;
            var query_str = query_base + 'select+*+from+%22' + metric_name + '%22+limit+1';
            $.get(query_str, function (resp) {
                var result = eval("(" + resp + ")");
                var columns = result[0].columns;
                var series = [];
                for (var i = 3; i < columns.length; i++) {
                    var tag = columns[i];
                    if (tag.indexOf(kw) >= 0) {
                        series.push(tag);
                    }
                }
                responseFn(series);
            });
        }
    });

    $("#btn_query").click(function () {
        var displayType = $("#displayType").val();
        if (displayType == "pie") {
            queryAsPie();
        } else {
            queryAsLine();
        }
    });



    function setTags(metricName) {
        var query_str = query_base + 'select+*+from+%22' + metricName + '%22+limit+1';
        $.get(query_str, function (resp) {
            var result = eval("(" + resp + ")");
            var columns = result[0].columns;
            $("#flt_tag,#grp_tag").empty();
            $("#flt_tag").append("<option  value=\"0\">未选择</option>");
            $("#grp_tag").append("<option  value=\"0\">未选择</option>");
            for (var i = 0; i < columns.length; i++) {
                var tag = columns[i];
                //"time", "sequence_number", "AppId", "IP", "value"
                if (tag != "time" && tag != "sequence_number" && tag != "IP" && tag != "value") {
                    $("#flt_tag").append("<option  value=\"" + tag + "\">" + tag + "</option>");
                    $("#grp_tag").append("<option  value=\"" + tag + "\">" + tag + "</option>");
                }
            }
        });
    }

});


//http://192.168.49.132:8086/db/metrics/series?p=root&q=list+series+%2Fwebapi_metrics_point_te%2F&u=root
//http://192.168.49.132:8086/db/metrics/series?u=root&p=root&q=select+*+from+%22webapi_metrics_point_test%22+limit+1
//http://192.168.49.132:8086/db/metrics/series?u=root&p=root&q=select+*+from+%22webapi_metrics_point_test%22+limit+1
