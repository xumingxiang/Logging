$(function () {
    var init = function () {
        $.get("api/GetAppErrOpts", function (data) {
            var result = eval("(" + data + ")")
            $("#app_err_interval").val(result.ALERTING_APPERROR_INTERVAL);
            $("#app_err_errorCountLimit").val(result.ALERTING_APPERROR_ERRORCOUNTLIMIT);
            $("#app_err_errorGrowthLimit").val(result.ALERTING_APPERROR_ERRORGROWTHLIMIT);
            $("#app_err_emailReceivers").val(result.ALERTING_APPERROR_EMAILRECEIVERS);
        });

        $("#btn_ok").click(function () {
            setOpts();
        });
    }

    var setOpts = function () {
        var interval = $("#app_err_interval").val();
        var errorCountLimit = $("#app_err_errorCountLimit").val();
        var errorGrowthLimit = $("#app_err_errorGrowthLimit").val();
        var emailReceivers = $("#app_err_emailReceivers").val();

        var data = {
            interval: interval,
            errorCountLimit: errorCountLimit,
            errorGrowthLimit: errorGrowthLimit,
            emailReceivers: emailReceivers
        };

        $.get("api/SetAppErrOpts", data, function () {
            alert("OK");
        });
    }

    init();
});