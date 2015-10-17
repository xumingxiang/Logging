$(function () {
    function getOnOffs() {
        var query_url = "GetLogOnOffs.ashx";
        var log_on_off_temp = $("#log_on_off_temp").html();
        $.get(query_url, function (result) {
            var onOffs = eval("(" + result + ")");
            for (var i = 0; i < onOffs.length; i++) {
                var onOffHtml = log_on_off_temp
                .replace(/{appId}/g, onOffs[i].AppId)
                .replace("{checked_debug}", onOffs[i].Debug == 1 ? "checked" : "")
                .replace("{checked_info}", onOffs[i].Info == 1 ? "checked" : "")
                .replace("{checked_warm}", onOffs[i].Warm == 1 ? "checked" : "")
                .replace("{checked_error}", onOffs[i].Error == 1 ? "checked" : "");
                $("#logonoffwarp").append(onOffHtml);
            }
        });
    }

    function setOnOff(event, appid) {
        var levels = $(event).parent().prev().find(":checkbox");
        var debug = $(levels[0]).prop("checked") ? 1 : 0;
        var info = $(levels[1]).prop("checked") ? 1 : 0;
        var warm = $(levels[2]).prop("checked") ? 1 : 0;
        var error = $(levels[3]).prop("checked") ? 1 : 0;
  
        $.post("SetLogOnOff.ashx", {
            appid: appid,
            debug: debug,
            info: info,
            warm: warm,
            error: error
        }, function () {
            alert("OK");
            history.go(0);
        });
    }

    $("#logonoffwarp").delegate("button", "click", function () {
        var appid = $(this).data("appid");
        setOnOff(this,appid);
    });

    $("#addnoff").click(function () {
        var log_on_off_temp = $("#log_on_off_temp").html();
        var onOffHtml = log_on_off_temp.replace("{appId}", "<input type=\"text\" placeholder=\"AppId\" class=\"form-control\" style=\"width: 100px\"/>")
              .replace("{checked_debug}","checked")
              .replace("{checked_info}", "checked")
              .replace("{checked_warm}", "checked")
              .replace("{checked_error}", "checked");
        var $onOffHtml = $(onOffHtml);
        $onOffHtml.find("button").click(function () {

            var txt_appId = $($(this).parent().prev().prev().children()[0]);
            var appId = txt_appId.val();
            if (appId <= 0) {
                alert("AppId必须大于0");
                txt_appId.focus();
                return;
            }
            setOnOff(this, appId);
           
        });
        $("#logonoffwarp").append($onOffHtml);

    });

    getOnOffs();
});