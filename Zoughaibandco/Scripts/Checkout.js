var country = "AF";
$(document).ready(function () {
    $('#deliveryTime').timepicker();
    $('#deliveryDate').datetimepicker({
        minDateTime: 0
    });

    var resizeFunction = function () {
        $(".checkoutStepDiv").css({ "width": $(".checkoutAllStepsContainer").width() + "px" });
        $(".checkoutStepDivSlider").css({ "width": ($(".checkoutStepDivSlider>div").width() * $(".checkoutStepDivSlider>div").length) + "px" });
    }
    resizeFunction();


    $(".formButtons span").click(function () {
        if ($(this).attr('disabled')) {
            return false;
        };

        var target = $(this).attr("rel");
        var okToGo = 1;
        if (target == "1") {
            if ($("#billingPrimaryAddress").val() == "") {
                $("#billingPrimaryAddress").parent().find("label").css({ color: "red" });
                okToGo = 0;
            }
            else {
                $("#billingPrimaryAddress").parent().find("label").css({ color: "#75797b" });
            }
            if ($("#billingCity").val() == "") {
                $("#billingCity").parent().find("label").css({ color: "red" });
                okToGo = 0;
            }
            else {
                $("#billingCity").parent().find("label").css({ color: "#75797b" });
            }
            if ($("#billingPrimaryPhone").val() == "") {
                $("#billingPrimaryPhone").parent().find("label").css({ color: "red" });
                okToGo = 0;
            }
            else {
                $("#billingPrimaryPhone").parent().find("label").css({ color: "#75797b" });
            }
        }
        else if (target == "2") {
            if ($("#deliveryDate").val() == "") {
                $("#deliveryDate").parent().find("label").css({ color: "red" });
                okToGo = 0;
            }
            else {
                $("#deliveryDate").parent().find("label").css({ color: "#75797b" });
            }
            if ($("#deliveryTime").val() == "") {
                $("#deliveryTime").parent().find("label").css({ color: "red" });
                okToGo = 0;
            }
            else {
                $("#deliveryTime").parent().find("label").css({ color: "#75797b" });
            }
        }
        if (okToGo == 1) {
            $(".checkoutStepsWrapper>div").removeClass("activeStep")
            $(".checkoutStepsWrapper>div").eq(target).addClass("activeStep")

            $("html, body").animate({ scrollTop: $(".yourBagSafe").offset().top - ($(".white_header").height() + $(".orange_header").height()) }, 500, function () {
                $(".checkoutStepDivSlider").stop().clearQueue().animate({ "left": -($(".checkoutStepDivSlider>div").width() * target) + "px" }, 400);
            });
        }
        else if (okToGo == 0) {
            $('html, body').animate({
                scrollTop: $("#billingPrimaryAddress").parent().find("label").offset().top - ($(".white_header").height() + $(".orange_header").height())
            }, 50);
        }

    });


    $(".cancelWishlist").first().click(function () {
        document.getElementById("checkoutForm").reset();
    });

    $(".sameAsBilling").first().click(function () {
        if ($(this).find(".fillMeIn").attr("class").indexOf("checkbox_filled") == -1) {
            $(this).find(".fillMeIn").addClass("checkbox_filled");
            $("#deliveryAddress").val($("#billingPrimaryAddress").val());
        }
        else {
            $(this).find(".fillMeIn").removeClass("checkbox_filled");
            $("#deliveryAddress").val("");
        }
    });
    //1 : Online | 2: COD
    $(".sameAsBilling").last().click(function () {
        $("#paymentMethod").val(2);
        $(".sameAsBillingLeft").last().removeClass("paymentSelected");
        $(this).addClass("paymentSelected");
    });
    $(".sameAsBillingLeft").click(function () {
        var amt = $("#amount").val();
        if (amt > 5000) {
            swal("Warning", "Please select COD because the payment amount is greater then 5000 USD");
        }
        else {
            $("#paymentMethod").val(1);
            $(".sameAsBilling").last().removeClass("paymentSelected");
            $(this).addClass("paymentSelected");
        }        
        //alert("If you choose this method, you will be redirected to our payment platform.\n\n Do not close your window even after payment is complete,\n until you are redirected to our website, or your request will fail !\n\n");
    });
    $(".sameAsBilling").last().click();
});

function finish() {
    $('.ajax-loader').css("visibility", "visible");
    var routeURL = location.protocol + '//' + location.host;
    var okToGo = 1;
    var payMet = $("#paymentMethod").val();

    var formData = $("#checkoutForm").serializeArray();
    $.ajax({
        url: routeURL + '/api/AjaxCall/Checkout',
        type: 'POST',
        data: formData,
        success: function (result) {
            if (result.status == 1) {
                if (result.dataenum == 1) {
                    $("#widhItemsCount").html(0);
                }
                else if (result.dataenum == 2) {
                    $("#cartItemsCount").html(0);
                }

                if (payMet == "2") {
                    swal({
                        title: "Thank you",
                        text: "Your order has been placed. We will contact you shortly to proceed.",
                        type: "warning",
                        showCancelButton: false,
                        confirmButtonColor: '#DD6B55',
                        confirmButtonText: 'oK',
                        closeOnConfirm: true
                    },
                        function (isConfirm) {
                            if (isConfirm) {
                                document.getElementById("checkoutForm").reset();
                                location.href = "/Home/Index";
                            }
                        });
                }
                else if (payMet == "1") {
                    $("#payment_confirmation").submit();
                }
                else {
                    location.href = "/Home/Index";
                }
            }
            else if (result.status == 0) {
                swal("Connection problem", "Something is wrong with your internet connection, please try again later.");
                location.href = "/Home/Index";
            }
            else if (result.status == -1) {
                location.href = "/Home/Index";
            }
            else {
                alert(result.message);
            }
            $('.ajax-loader').css("visibility", "hidden");
        },
        error: function (xhr) {
            alert("Error", xhr.statusText);
        },
    });
    $('.ajax-loader').css("visibility", "hidden");
}

function onCountryChange() {
    country = $("#billingCountry").val();
    if (country === "LB") {
        $("#paymentMethod").val(2);
        $("#divOnline").removeClass("paymentSelected");
        $("#divCOD").addClass("paymentSelected");
        $("#divOnline").hide();
    } else {
        $("#divOnline").show();
    }
}