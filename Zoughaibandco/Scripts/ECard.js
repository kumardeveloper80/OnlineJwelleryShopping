$(document).ready(function () {
    $('#ecardDate').datetimepicker({
        minDateTime: 0
    });
    $("#ecardAmount").ForceNumericOnly();
});

function AddEcard(UserId) {
    if (UserId > 0 ) {
        if (CheckValidation()) {
            $('.ajax-loader').css("visibility", "visible");
            var requestData = Object;            requestData.Amount = parseInt($("#ecardAmount").val());            requestData.ToEmail = $('#ecardEmailTo').val();            requestData.ToFirstName = $("#ecardFirstNameTo").val();            requestData.ToLastName = $("#ecardLastNameTo").val();            requestData.ToPhoneNo = $("#ecardPhoneNumber").val();            requestData.DeliverDate = $("#ecardDate").val();            requestData.Address = $("#ecardDeliveryaddress").val();            requestData.Description = $("#ecardDescription").val();            var routeURL = location.protocol + '//' + location.host;            $.ajax({
                url: routeURL + '/api/AjaxCall/SaveEGiftCard',
                type: 'POST',
                data: requestData,
                success: function (result) {
                    if (result.status == 1) {
                        $('#ecardForm')[0].reset();
                        window.location.href = "/Home/EGiftPayment";
                    }
                    else {
                        alert(result.message);
                    }
                $('.ajax-loader').css("visibility", "hidden");
                },
                error: function (xhr) {
                    alert("Error", xhr.statusText);
                },
            });                   }
    }
    else {
        login();
    }
}


function CheckValidation() {
    var IsValid = true;
    var emailRegx = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/;

    if (isNaN(parseInt($("#ecardAmount").val()))) {
        IsValid = false;
        $("#amtLbl").css({ color: "red" });
    }
    else if (parseInt($("#ecardAmount").val()) < 50) {
        IsValid = false;
        $("#amtLbl").css({ color: "red" });
        alert("Amount should be higher than 50 USD");
    }
    else if (parseInt($("#ecardAmount").val()) > 5000) {
        IsValid = false;
        $("#amtLbl").css({ color: "red" });
        alert("Amount should be lower than 5000 USD");
    }
    else {
        $("#amtLbl").css({ color: "#231F20" });
    }

    if (jQuery.trim($("#ecardPhoneNumber").val()) == "") {
        IsValid = false;
        $("#ecardPhoneNumber").parent().find("label").css({ color: "red" });
    }

    if (jQuery.trim($("#ecardFirstNameTo").val()) == "") {
        IsValid = false;
        $("#ecardFirstNameTo").parent().find("label").css({ color: "red" });
    }
    else {
        $("#ecardFirstNameTo").parent().find("label").css({ color: "#231F20" });
    }

    if (jQuery.trim($("#ecardLastNameTo").val()) == "") {
        IsValid = false;
        $("#ecardLastNameTo").parent().find("label").css({ color: "red" });
    }
    else {
        $("#ecardLastNameTo").parent().find("label").css({ color: "#231F20" });
    }

    if (jQuery.trim($("#ecardDate").val()) == "") {
        IsValid = false;
        $("#ecardDateLbl").css({ color: "red" });
    }
    else {
        $("#ecardDateLbl").css({ color: "#231F20" });
    }
    

    if ($("#ecardDeliveryaddress").val() == "") {
        IsValid = false;
        $("#ecardDeliveryaddress").parent().parent().find(".ecard_bold_label").css({ color: "red" });
    }
    else {
        $("#ecardDeliveryaddress").parent().parent().find(".ecard_bold_label").css({ color: "#231F20" });
    }

        if ($.trim($('#ecardEmailTo').val()) == "") {
        IsValid = false;
        $("#ecardEmailTo").parent().find("label").css({ color: "red" });
    }
    else if (!emailRegx.test($.trim($('#ecardEmailTo').val()))) {
        IsValid = false;
        $("#ecardEmailTo").parent().find("label").css({ color: "red" });
    }
    else {
        $("#ecardEmailTo").parent().find("label").css({ color: "#231F20" });
    }    if ($.trim($('#ConfirmEmail').val()) == "") {
        IsValid = false;
        $("#ConfirmEmail").parent().find("label").css({ color: "red" });
    }
    else if (!emailRegx.test($.trim($('#ConfirmEmail').val()))) {
        IsValid = false;
        $("#ConfirmEmail").parent().find("label").css({ color: "red" });
    }
    else {
        $("#ConfirmEmail").parent().find("label").css({ color: "#231F20" });
    }    if ($("#ecardEmailTo").val() == "" || $("#ecardEmailTo").val() != $("#ConfirmEmail").val()) {
        $("#ecardEmailTo").parent().find("label").css({ color: "red" });
        $("#ConfirmEmail").parent().find("label").css({ color: "red" });
        IsValid = false;
    }    return IsValid;
}