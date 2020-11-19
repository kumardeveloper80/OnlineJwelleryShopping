function onSubmit() {
    if (CheckValidation()) {
        var routeURL = location.protocol + '//' + location.host;
        $('.ajax-loader').css("visibility", "visible");
        var requestData = {
            FirstName: $.trim($('#firstName').val()),
            LastName: $.trim($('#lastName').val()),
            Email: $.trim($('#email').val()),
            Phone: $.trim($('#phone').val()),
            Messsage: $.trim($('#helpMessage').val())
        };

        $.ajax({
            url: routeURL + '/api/AjaxCall/SaveHelp',
            type: 'POST',
            data: JSON.stringify(requestData),
            contentType: 'application/json',
            success: function (result) {
                $('.ajax-loader').css("visibility", "hidden");
                if (result.status == 1) {
                    swal("Success", "Thank you for contacting us. We will get back to you as soon as possible !");
                }
                else if (result.status == -3) {
                    swal("Error", result.Messsage);
                }
                else {
                    swal("Error", "Something went wrong with your internet connection, please try again later!");
                }
                onCancel();
            },
            error: function (xhr) {
                $('.ajax-loader').css("visibility", "hidden");
                onCancel();
                swal("Error", xhr.statusText);
            },
        });
    }
}

function onCancel() {
    document.getElementById("helpForm").reset();
}

function CheckValidation() {
    var IsValid = true;
    var emailRegx = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/;

    if ($.trim($('#firstName').val()) == "") {
        IsValid = false;
        $("#firstNameLabel").addClass("InValid");
    }
    else {
        $("#firstNameLabel").removeClass("InValid");
    }

    if ($.trim($('#lastName').val()) == "") {
        IsValid = false;
        $("#lastNameLabel").addClass("InValid");
    }
    else {
        $("#lastNameLabel").removeClass("InValid");
    }

    if ($.trim($('#email').val()) == "") {
        IsValid = false;
        $("#emailLabel").addClass("InValid");
    }
    else if (!emailRegx.test($.trim($('#email').val()))) {
        IsValid = false;
        $("#emailLabel").addClass("InValid");
    }
    else {
        $("#emailLabel").removeClass("InValid");
    }

    if ($.trim($('#helpMessage').val()) == "") {
        IsValid = false;
        $("#helpMessageLabel").addClass("InValid");
    }
    else {
        $("#helpMessageLabel").removeClass("InValid");
    }

    return IsValid;
}