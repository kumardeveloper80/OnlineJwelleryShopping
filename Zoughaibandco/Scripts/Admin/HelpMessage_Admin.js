var routeURL = location.protocol + '//' + location.host;

HelpMsgList();

function HelpMsgList() {
    $('.ajax-loader').css("visibility", "visible");
    $.ajax({
        url: routeURL + '/api/AdminApi/HelpMsgList',
        type: 'GET',
        success: function (data) {
            if (data.status == 1) {
                $("#helpMsgDiv").html(data.dataenum);
            }
            else if (data.status == 0) {
                $("#helpMsgDiv").html("Data not found");
            }
            else {
                $("#helpMsgDiv").html("Data not found");
                alert("Something went wrong with your internet connection, please try again later!");
            }
            $('.ajax-loader').css("visibility", "hidden");
        }
    });
}

function DeleteHelpMsg(msgId) {
    $.ajax({
        url: routeURL + '/api/AdminApi/DeleteHelpMsg?HelpId=' + msgId,
        type: 'GET',
        success: function (data) {
            if (data.status == 1) {
                HelpMsgList();
            }
            else if (data.status == 0) {
                alert("Help message not deleted.");
            }
            else {
                alert("Something went wrong with your internet connection, please try again later!");
            }
            $('.ajax-loader').css("visibility", "hidden");
        }
    });
}