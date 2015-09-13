
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

    function numberToIp(number) {
        if (number <= 0) { return ""; }

        return (Math.floor(number / (256 * 256 * 256))) + "." +
        (Math.floor(number % (256 * 256 * 256) / (256 * 256))) + "." +
        (Math.floor(number % (256 * 256) / 256)) + "." +
        (Math.floor(number % 256));
    }

    function getLogs(start, end, clear) {
        var query_url = "logviewer.ashx";
        var appid = $("#appid").val() || 0;
        var source = $("#source").val() || "";
        var title = $("#title").val() || "";
        var msg = $("#msg").val() || "";

        if (!start) {
            start = $("#start").val() || "0000-00-00";
            start = (new Date(start)).valueOf() * 10000;
        }
        if (!end) {
            end = $("#end").val() || new Date().Format("yyyy-MM-dd hh:mm:ss", false);
            end = (new Date(end)).valueOf() * 10000;
        }

      
      

        var ip = $("#ip").val() || "";

        var level_cb = $('#level-warp').find(":checked");
        var level = "";
        level_cb.each(function () {
            var level_val = $(this).val();
            if (level_val != "") {
                level = level + $(this).val() + ",";
            }
        });
        level = level.substr(0, level.length - 1);


        var tags_inputs = $("#tag_warp").find(":text");
        var tags = "";
        tags_inputs.each(function () {
            var tag_val = $(this).val();
            if (tag_val != "") {
                tags = tags + $(this).val() + ",";
            }
        });
        tags = tags.substr(0, tags.length - 1);



        var item_temp = $("#log_item_temp").html();


        $.get(query_url, {
            appid: appid,
            source: source,
            title: title,
            msg: msg,
            start: start,
            end: end,
            ip: ip,
            level: level,
            tags: tags
        }, function (result) {
            var log_json = eval("(" + result + ")");
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
                 .replace("{time}", new Date(log.Time / 10000).Format("yyyy-MM-dd hh:mm:ss.S"))
                 .replace("{level_class}", level_class);
                $("#log_warp").append(item_html);
            }
        });
    }

    function getStatistics(start, end, appId) {
        var query_url = "StatisticsViewer.ashx";
        $.get(query_url, {
            start: start,
            end: end,
            appId: appId
        }, function (result) {
            var s_json = eval("(" + result + ")");
            var s_length = s_json.length;
            if (s_length <= 0) { return; }
            $("#statistics_warp").empty();
            var s_item_temp = $("#s_item_temp").html();
            for (var i = 0; i < s_length; i++) {
                var s = s_json[i];

                var debug_num = s.Debug;
                var info_num = s.Info;
                var warm_num = s.Warm;
                var error_num = s.Error;

                if (debug_num >= 100000) {
                    debug_num = (Math.round(debug_num / 100) / 100) + "w";
                } else if (debug_num >= 10000) {
                    debug_num = (Math.round(debug_num / 10) / 100) + "k";
                }

                if (info_num >= 10000) {
                    info_num = (Math.round(info_num / 100) / 100) + "w";
                } else if (info_num >= 10000) {
                    info_num = (Math.round(info_num / 10) / 100) + "k";
                }

                if (warm_num >= 10000) {
                    warm_num = (Math.round(warm_num / 100) / 100) + "w";
                } else if (warm_num >= 10000) {
                    warm_num = (Math.round(warm_num / 10) / 100) + "k";
                }

                if (error_num >= 100000) {
                    error_num = (Math.round(error_num / 100) / 100) + "k";
                } else if (error_num >= 10000) {
                    error_num = (Math.round(error_num / 10) / 100) + "k";
                }


                var s_item_html = s_item_temp
              .replace("{appId}", s.AppId)
              .replace("{debug_num}", debug_num)
              .replace("{info_num}", info_num)
              .replace("{warm_num}", warm_num)
              .replace("{error_num}", error_num);

                $("#statistics_warp").append(s_item_html);
            }
        });
    }

    function addTag() {
        var tag_warp = $("#tag_warp");
        var tag_rows = tag_warp.find(".tag_row");
        if (tag_rows.length == 0) {
            tag_warp.append("<div class=\"tag_row row mt-2\"></div>");
        }
        var last_row = tag_warp.find(".tag_row").last();
        var last_row_cols = last_row.find(".col-lg-6");
        if (last_row_cols.length <= 1) {
            last_row.append($("#tag_item_temp").html());
        } else {
            tag_warp.append("<div class=\"tag_row row mt-2\">" + $("#tag_item_temp").html() + "</div>");
        }
    }

    $("#btn_query").click(function () {
        getLogs(null, null, true);
    });
    $("#more_query").click(function () {
        var cursor = $("#more_query").attr("cursor");
        getLogs(null, cursor, false);
    });

    $("#addTag").click(function () {
        addTag();
    });

    $("#tag_warp").delegate(".remove-tag", "click", function () {
        var row = $(this).parent().parent().parent();
        $(this).parent().parent().remove();

        if (row.find(".col-lg-6").length == 0) {
            row.remove();
        }
    });



    //$('.left').BootSideMenu({ side: "left", autoClose: false });

    var now = new Date();
    var default_start_time = new Date((now.setHours(now.getHours() - 1))).Format("yyyy-MM-dd hh:mm:ss");
    var default_end_time = new Date().Format("yyyy-MM-dd hh:mm:ss");

    var default_start_timestamp = (new Date(default_start_time)).valueOf() * 10000;
    var default_end_timestamp = (new Date(default_end_time)).valueOf() * 10000;

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

    getStatistics(default_start_timestamp, default_end_timestamp);
    getLogs(null, null, true);
});