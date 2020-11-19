var routeURL = location.protocol + '//' + location.host;
var filterType = "";

OrderList("","","");

function OrderList(paymentType, startDate, endDate) {
    $("#orderListDiv").html("");
    $('.ajax-loader').css("visibility", "visible");
    $.ajax({
        url: routeURL + '/api/AdminApi/OrderList?paymentType=' + paymentType + '&startDate=' + startDate + '&endDate=' + endDate ,
        type: 'GET',
        success: function (data) {
            if (data.status == 1) {
                $("#orderListDiv").html(data.dataenum);
            }
            else if (data.status == 0) {
                $("#orderListDiv").html("Data not found");
            }
            else {
                $("#orderListDiv").html("Data not found");
                alert("Something went wrong with your internet connection, please try again later!");
            }
            $('.ajax-loader').css("visibility", "hidden");
        }
    });
}

//function onPaymentTypeChange(paymentType) {
//    debugger;
//    var startDate = $("#startDate").val();
//    var endDate = $("#endDate").val();

//    debugger;
//    if (startDate != "" && endDate != "") {
//        if (Date.parse(startDate) > Date.parse(endDate)) {
//            OrderList(paymentType, startDate, endDate);
//        } else {
//            OrderList(paymentType, "", "");
//        }
//    }
//    else {
//        OrderList(paymentType, "", "");
//    }

//}

function onSubmit() {
    debugger;
    var paymentType = $("#paymentType").val();
    var startDate = $("#startDate").val();
    var endDate = $("#endDate").val();

    //if (startDate == "" && endDate == "") {
    //    alert("Please select start and end date");
    //}
    //else if (startDate == "") {
    //    alert("Please select start date");
    //}
    //else if (endDate == "") {
    //    alert("Please select end date");
    //}
    //else if (Date.parse(startDate) > Date.parse(endDate)) {
    //    alert("start date must be less than end date");
    //}
    //else{
    //    OrderList(paymentType, startDate, endDate);
    //}

    OrderList(paymentType, startDate, endDate);
}

function onReset() {
    $('#filterForm').trigger("reset");
    OrderList("", "", "");
}