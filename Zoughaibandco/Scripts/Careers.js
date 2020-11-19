function onSubmit() {
    if (CheckValidation()) {
        var routeURL = location.protocol + '//' + location.host;
        $('.ajax-loader').css("visibility", "visible");
        var AttachFile = document.getElementById('fileex');
        var requestData = new FormData();

        if (AttachFile.files.length > 0) {
            requestData.append(AttachFile.files[0].name, AttachFile.files[0]); 
        }
        requestData.append('FirstName', $.trim($('#firstName').val()));
        requestData.append('LastName', $.trim($('#lastName').val()));
        requestData.append('Email', $.trim($('#email').val()));
        requestData.append('Phone', $.trim($('#phone').val()));
        requestData.append('Address', $.trim($('#address').val()));
        requestData.append('Position', $.trim($('#position').val()));
        requestData.append('AboutUs', $.trim($('#aboutUs').val()));
        $.ajax({
            url: routeURL + '/api/AjaxCall/SaveCareer',
            type: 'POST',
            data: requestData,
            dataType: 'json',
            contentType: false,
            processData: false,
            success: function (result) {
                if (result.status != 1) {
                    swal(result.message);
                }
                onCancel();
            },
            error: function (xhr) {
                onCancel();
                swal("Error", xhr.statusText);
            },
        });
    }
}

function onCancel() {
    $('.ajax-loader').css("visibility", "hidden");
    $('#firstName').val("");
    $('#lastName').val("");
    $('#email').val("");
    $('#phone').val("");
    $('#address').val("");
    $('#position').val("");
    $('#aboutUs').val("");
    $("#fileex").val(null);
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
    return IsValid;
}