var routeURL = location.protocol + '//' + location.host;

EGiftCardList();

function EGiftCardList() {
    $('.ajax-loader').css("visibility", "visible");
    $.ajax({
        url: routeURL + '/api/AdminApi/EGiftCardList',
        type: 'GET',
        success: function (data) {
            if (data.status == 1) {
                $("#ecardListDiv").html(data.dataenum);
            }
            else if (data.status == 0) {
                $("#ecardListDiv").html("Data not found");
            }
            else {
                $("#ecardListDiv").html("Data not found");
                alert("Something went wrong with your internet connection, please try again later!");
            }
            $('.ajax-loader').css("visibility", "hidden");
        }
    });
}

function DeleteEGiftCard(eGiftCardId) {
    $.ajax({
        url: routeURL + '/api/AdminApi/DeleteEGiftCard?EGiftCardId=' + eGiftCardId,
        type: 'GET',
        success: function (data) {
            if (data.status == 1) {
                EGiftCardList();
            }
            else if (data.status == 0) {
                alert("EGiftCard not deleted.");
            }
            else {
                alert("Something went wrong with your internet connection, please try again later!");
            }
            $('.ajax-loader').css("visibility", "hidden");
        }
    });
}

function UpdateEGiftCard(eGiftCardId) {
    var requestData = Object;    var DeliverDateTime = $("#DeliverDate").val().replace(/\//g, '-');    requestData.Amount = parseInt($("#Amount").val());    requestData.ToEmail = $('#ToEmail').val();    requestData.ToFirstName = $("#ToFirstName").val();    requestData.ToLastName = $("#ToLastName").val();    requestData.DeliverDateTime = DeliverDateTime;    requestData.Address = CKEDITOR.instances.Address.getData();    requestData.Description = CKEDITOR.instances.Description.getData();
    requestData.UserId = $("#UserId").val();
    requestData.ToPhoneNo = $("#ToPhoneNo").val();
    requestData.Id = $("#Id").val();

    if ($('#IsDeliver').is(':checked')) {
        requestData.IsDeliver = true;
    }
    else {
        requestData.IsDeliver = false;
    }

    if ($('#IsPublished').is(':checked')) {
        requestData.IsPublished = true;
    }
    else {
        requestData.IsPublished = false;
    }

    $.ajax({
        url: routeURL + '/api/AdminApi/UpdateEGiftCard',
        type: 'POST',
        data: requestData,
        success: function (data) {
            if (data.status == 1) {
                location.href = "/Admin/GiftCardRequest";
            }
            else if (data.status == 0) {
                alert("EGiftCard not updated.");
            }
            else {
                alert("Something went wrong with your internet connection, please try again later!");
            }
        }
    });
}

jQuery.fn.ForceNumericOnly =
    function () {
        return this.each(function () {
            $(this).keydown(function (e) {
                var key = e.charCode || e.keyCode || 0;
                // allow backspace, tab, delete, arrows, numbers and keypad numbers ONLY
                return (
                    key == 8 ||
                    key == 9 ||
                    key == 46 ||
                    (key >= 37 && key <= 40) ||
                    (key >= 48 && key <= 57) ||
                    (key >= 96 && key <= 105));
            })
        })
    };