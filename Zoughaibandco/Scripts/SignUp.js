var IsNewUserRequest = true;
var bookAppointmentOkToGo = 0;
var Id = 0;
var routeURL = location.protocol + '//' + location.host;

function signup() {
    bookAppointmentOkToGo = 0;
    FormValidation();
    if (bookAppointmentOkToGo == 0) {
        $('.ajax-loader').css("visibility", "visible");
        if (IsNewUserRequest == true) {
            poststr = $("#signupForm").serialize();
            $.ajax({
                url: routeURL + '/api/AjaxCall/Signup',
                type: 'POST',
                data: '' + poststr,
                success: function (data) {
                    if (data.status == "1") {

                        document.getElementById("signupForm").reset();
                        window.location = routeURL + "/Home";
                    }
                    else if (data.status == "-2") {
                        alert("User already exists");
                    }
                    else {
                        alert("Something went wrong with your internet connection, please try again later!");
                    }
                    $('.ajax-loader').css("visibility", "hidden");
                }
            });
        }
        else {
            updateUser();
        }
    }
}

function getUser() {
    $('.ajax-loader').css("visibility", "visible");
    $.ajax({
        url: routeURL + '/api/AjaxCall/GetUser',
        type: 'GET',
        success: function (data) {
            if (data.status == 1) {
                Id = data.dataenum.Id;
                $("#firstName").val(data.dataenum.FirstName);
                $("#lastName").val(data.dataenum.LastName);
                $("#password").val(data.dataenum.Password);
                $("#email").val(data.dataenum.Email);
                $("#ConfirmEmail").val(data.dataenum.Email);
                $("#email").attr("disabled", "disabled");
                $("#ConfirmEmail").attr("disabled", "disabled"); 
                IsNewUserRequest = false;
            }
            else if (data.status == -2) {
                alert("User does not exists");
            }
            else {
                alert("Something went wrong with your internet connection, please try again later!");
            }
            $('.ajax-loader').css("visibility", "hidden");
        }
    });
}

function updateUser() {
    poststr = $("#signupForm").serialize();
    poststr += "&Id=" + Id + "&Email=" + $("#email").val();
    $.ajax({
        url: routeURL + '/api/AjaxCall/UpdateUser',
        type: 'POST',
        data: '' + poststr,
        success: function (data) {
            if (data.status == 1) {
                document.getElementById("signupForm").reset();
                window.location = routeURL + "/Home";
            }
            else if (data.status == -2) {
                alert("User does not exists");
            }
            else {
                alert("Something went wrong with your internet connection, please try again later!");
            }
            $('.ajax-loader').css("visibility", "hidden");
        }
    });
}

function FormValidation() {
    var email = $('#email').val();
    var AtPos = email.indexOf("@");
    var StopPos = email.lastIndexOf(".");
    var StopSecondPos = email.length;

    $("label").css({ color: "#000" });

    if (jQuery.trim($("#firstName").val()) == "") {
        bookAppointmentOkToGo = 1;
        $("#firstName").parent().find("label").css({ color: "red" });
    }
    else {
        $("#firstName").parent().find("label").css({ color: "#000" });
    }
    if (jQuery.trim($("#lastName").val()) == "") {
        bookAppointmentOkToGo = 1;
        $("#lastName").parent().find("label").css({ color: "red" });
    }
    else {
        $("#lastName").parent().find("label").css({ color: "#000" });
    }
    if (jQuery.trim($("#password").val()) == "") {
        bookAppointmentOkToGo = 1;
        $("#password").parent().find("label").css({ color: "red" });
    }
    else {
        $("#password").parent().find("label").css({ color: "#000" });
    }
    if ((jQuery.trim($('#email').val()) == "" || (AtPos == -1 || StopPos == -1 || (StopPos != -1 && StopPos == StopSecondPos - 1)))) {
        bookAppointmentOkToGo = 1;
        $("#email").parent().find("label").css({ color: "red" });
    }
    else {
        $("#email").parent().find("label").css({ color: "#000" });
    }

    if ($("#email").val() != "" && $("#email").val() != $("#ConfirmEmail").val()) {
        alert("Emails do not match");
        $("#email").parent().find("label").css({ color: "red" });
        $("#ConfirmEmail").parent().find("label").css({ color: "red" });
        bookAppointmentOkToGo = 1;
    }
    if ($("#password").val() != "" && $("#password").val() != $("#ConfirmPassword").val()) {
        alert("Passwords do not match");
        $("#password").parent().find("label").css({ color: "red" });
        $("#ConfirmPassword").parent().find("label").css({ color: "red" });
        bookAppointmentOkToGo = 1;
    }
}