
Date.prototype.Format = function (fmt, utc) {
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

    var query_url = "logviewer.ashx";


    function numberToIp(number) {
        if (number <= 0) { return ""; }

        return (Math.floor(number / (256 * 256 * 256))) + "." +
        (Math.floor(number % (256 * 256 * 256) / (256 * 256))) + "." +
        (Math.floor(number % (256 * 256) / 256)) + "." +
        (Math.floor(number % 256));
    }

    function getlogs(clear, start) {

        var appid = $("#appid").val() || 0;
        var source = $("#source").val() || "";
        var title = $("#title").val() || "";
        var msg = $("#msg").val() || "";

        if (!start) {
            start = $("#start").val() || "2015-08-29 16:16:16";
        }
        var end = $("#end").val() || new Date().Format("yyyy-MM-dd hh:mm:ss", false);

        var ip = $("#ip").val() || "";
        var level_cb = $('#level-warp').find(":checked");
        var level = "";

        level_cb.each(function () { level = level + $(this).val() + ","; });
        level = level.substr(0, level.length - 1);
        var item_temp = $("#log_item_temp").html();

        $.get(query_url, {
            appid: appid,
            source: source,
            title: title,
            msg: msg,
            start: start,
            end: end,
            ip: ip,
            level: level
        }, function (result) {
            var log_json = eval("(" + result + ")")
            var log_length = log_json.List.length;
            var log_cursor = log_json.Cursor;
            var log_end = log_json.End;

            if (new Date(log_cursor) >= new Date(log_end) || log_length >= 100) {
                $("#more_query").show();
                $("#more_query").attr("cursor", log_cursor);
            } else {
                $("#more_query").hide();
            }
            if (log_length == 0) {
                $("#empty").show();
                $("#record").hide();

            } else {
                $("#empty").hide();
                $("#record").show();
            }
            $("#record_num").text(log_length);

            if (clear) {
                $("#log_warp").empty();
            }
            for (var i = 0; i < log_length; i++) {
                var log = log_json.List[i];
                var level_class = "";
                if (log.Level == 2) {
                    level_class = "list-group-item-info";
                } else if (log.Level == 3) {
                    level_class = "list-group-item-warning";
                } else if (log.Level == 4) {
                    level_class = "list-group-item-danger";
                }
                var item_html = item_temp
                 .replace("{title}", log.Title)
                 .replace("{msg}", log.Message)
                 .replace("{source}", log.Source)
                 .replace("{ip}", numberToIp(log.IP))
                 .replace("{appid}", log.AppId)
                 .replace("{thread}", log.Thread)
                 .replace("{time}", new Date(log.Time).Format("yyyy-MM-dd hh:mm:ss.S"))
                 .replace("{level_class}", level_class);
                $("#log_warp").append(item_html);
            }
        });
    }

    $("#btn_query").click(function () {
        getlogs(true);
    });
    $("#more_query").click(function () {
        var cursor = $("#more_query").attr("cursor");
        getlogs(false, cursor);
    });

    var now = new Date();
    var default_start_time = new Date((now.setHours(now.getHours() - 1))).Format("yyyy-MM-dd hh:mm:ss");
    var default_end_time = new Date().Format("yyyy-MM-dd hh:mm:ss");

    $("#start")
       .val(default_start_time)
       .datetimepicker({
           defaultValue: default_start_time,
           dateFormat: 'yy-mm-dd',
           showSecond: true,
           timeFormat: 'HH:mm:ss',
           currentText: '当前',
           closeText: '确定'
       });

    $("#end")
        .val(default_end_time)
        .datetimepicker({
            defaultValue: default_end_time,
            dateFormat: 'yy-mm-dd',
            showSecond: true,
            timeFormat: 'HH:mm:ss',
            currentText: '当前',
            closeText: '确定'
        });


});