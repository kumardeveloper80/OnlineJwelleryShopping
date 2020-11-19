// Check Login username and password
function checkLogin() {
    var routeURL = location.protocol + '//' + location.host;

    var userNamePassReadyTogo = 0;

    if (jQuery.trim($("#Body_txtUsername").val()) == "") {
        userNamePassReadyTogo = 1;
        $("#usernameLabel").css({ color: "red" });
    }
    else {
        $("#usernameLabel").css({ color: "#808080" });
    }

    if (jQuery.trim($("#Body_txtPassword").val()) == "") {
        userNamePassReadyTogo = 1;
        $("#passwordLabel").css({ color: "red" });
    }
    else {
        $("#passwordLabel").css({ color: "#808080" });
    }

    if (userNamePassReadyTogo == 0) {
        $("#loginLoader").show();

        // Connect to logincheck to authenticate user
        poststr = "LoginUsername=" + $("#Body_txtUsername").val() + "&LoginPassword=" + $("#Body_txtPassword").val();
        $.ajax({
            url: routeURL + '/api/AdminApi/Login',
            type: 'POST',
            data: '' + poststr,
            success: function (data) {
                if (data.status == 1) {
                   
                    $("#loginLoader").hide();
                    $("#incorrectUserNamePass").animate({ height: "0px" }, 500);
                    window.location = routeURL + '/Admin/Product';
                }
                else {
                    $("#loginLoader").hide();
                    $("#incorrectUserNamePass").animate({ height: "30px" }, 500);
                }
            }
        });
    }
}

//////////////////////////////////// Allow only numeric ///////////////////////////////////////////////
// Numeric only control handler
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
$(document).ready(function () {
    //$("#year").ForceNumericOnly();
});
//////////////////////////////////// Allow only numeric ///////////////////////////////////////////////