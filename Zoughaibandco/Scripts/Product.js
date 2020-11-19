var routeURL = location.protocol + '//' + location.host;

function AddToWishList(UserId, ProductId) {
    if (UserId > 0) {
        $.ajax({
            url: routeURL + '/api/AjaxCall/AddItemToWishList?ProductId=' + ProductId,
            type: 'GET',
            success: function (data) {
                $("#widhItemsCount").html(data.dataenum.WishListCount);
                $("#cartItemsCount").html(data.dataenum.CartCount);
                if (data.status == 1) {
                    swal("Success", "Product successfully added to you wishlist");
                }
                else if (data.status == -2) {
                    swal("Error", "Product is already in your wishlist");
                }
            }
        });
    }
    else {
        $(".login_popup").fadeIn(200);
        $("#widhItemsCount").html(0);
    }
}

function AddToGuestWishList(UserId, ProductId) {
    $.ajax({
        url: routeURL + '/api/AjaxCall/AddItemToGuestWishList?ProductId=' + ProductId,
        type: 'GET',
        success: function (data) {
            $("#GuestWidhItemsCount").html(data.dataenum.WishListCount);
            $("#GuestCartItemsCount").html(data.dataenum.CartCount);
            if (data.status == 1) {
                swal("Success", "Product successfully added to you wishlist");
            }
            else if (data.status == -2) {
                swal("Error", "Product is already in your wishlist");
            }
        }
    });
}

function AddItemToCart(UserId, ProductId) {
    if (UserId > 0) {
        $('.ajax-loader').css("visibility", "visible");
        $.ajax({
            url: routeURL + '/api/AjaxCall/AddToCart?ProductId=' + ProductId,
            type: 'GET',
            success: function (data) {
                $("#widhItemsCount").html(data.dataenum.WishListCount);
                $("#cartItemsCount").html(data.dataenum.CartCount);
                if (data.status == 1) {
                    AddToCartSuccess();
                }
                else {
                    swal("Error", "Something went wrong with your internet connection, please try again later!");
                }
                $('.ajax-loader').css("visibility", "hidden");
            }
        });
    }
    else {
        $(".login_popup").fadeIn(200);
        $("#widhItemsCount").html(0);
    }
}

function AddToCartSuccess() {
    swal({
        title: "Success",
        text: "Product added to cart",
        type: "success",
        showCancelButton: true,
        confirmButtonColor: '#DD6B55',
        confirmButtonText: 'Checkout',
        cancelButtonText: "Continue Shopping",
        closeOnConfirm: false,
        closeOnCancel: true
    },
        function (isConfirm) {
            if (isConfirm) {
                location.href = "/Product/Cart";
            }
            else {
            }
        });
}

function AddItemToGuestCart(GuestUserId, ProductId) {
    $('.ajax-loader').css("visibility", "visible");
    $.ajax({
        url: routeURL + '/api/AjaxCall/AddToGuestCart?ProductId=' + ProductId,
        type: 'GET',
        success: function (data) {
            $("#GuestWidhItemsCount").html(data.dataenum.WishListCount);
            $("#GuestCartItemsCount").html(data.dataenum.CartCount);
            if (data.status == 1) {
                AddToGuestCartSuccess();
            }
            else {
                swal("Error", "Something went wrong with your internet connection, please try again later!");
            }
            $('.ajax-loader').css("visibility", "hidden");
        }
    });
}

function AddToGuestCartSuccess() {
    swal({
        title: "Success",
        text: "Product added to cart",
        type: "success",
        showCancelButton: true,
        confirmButtonColor: '#DD6B55',
        confirmButtonText: 'Checkout',
        cancelButtonText: "Continue Shopping",
        closeOnConfirm: false,
        closeOnCancel: true
    },
        function (isConfirm) {
            if (isConfirm) {
                location.href = "/Product/GuestCart";
            }
            else {
            }
        });
}

function GuestEmptyToCard(GuestUserId, CartId) {

    swal({
        title: "Remove all cart products",
        text: "Are you sure you want to delete all products from your cart ?",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: '#DD6B55',
        confirmButtonText: 'Yes, I am sure!',
        cancelButtonText: "No, cancel it!",
        closeOnConfirm: false,
        closeOnCancel: true
    },
        function (isConfirm) {
            if (isConfirm) {
                $.ajax({
                    url: routeURL + '/api/AjaxCall/EmptyGuestCart',
                    type: 'POST',
                    data: '',
                    success: function (data) {
                        if (data.status > 0) {
                            $("#GuestCartItemsCount").html("0");
                            $("#NoCart").show();
                            $("#GuestWidhItemsCount").html("0");
                            swal("Success", "Your cart has been emptied.");
                        }
                        else if (data.status == 0) {
                            swal("Error", "Your cart already emptied.");
                        }
                        else {
                            swal("Connection problem", "Something is wrong with your internet connection, please try again later.");
                        }
                    }
                });
            }
        });
}function CartProductToRemove(UserId, CartDetailsId) {    if (UserId > 0) {
        $.ajax({
            url: routeURL + '/api/AjaxCall/CartProductRemove?CartDetailsId=' + CartDetailsId,
            type: 'GET',
            success: function (result) {
                if (result.status == 1) {
                    CartProRemoveSuccess();
                }
                else {
                    swal("Connection problem", "Something is wrong with your internet connection, please try again later.");
                }
            }
        });
    }
    else {
        $(".login_popup").fadeIn(200);
        $("#widhItemsCount").html(0);
    }
}function CartProRemoveSuccess() {
    swal({
        title: "Success",
        text: "Item has been removed.",
        confirmButtonColor: '#DD6B55',
        confirmButtonText: 'OK, GOT IT!',
    },
        function (isConfirm) {
            location.href = "/Product/Cart";
        });
}function GuestCartProductToRemove(GuestUserId, CartDetailsId) {
    $.ajax({
        url: routeURL + '/api/AjaxCall/GuestCartProductRemove?CartDetailsId=' + CartDetailsId,
        type: 'GET',
        success: function (result) {
            if (result.status == 1) {
                CartProGuestRemoveSuccess();
            }
            else {
                swal("Connection problem", "Something is wrong with your internet connection, please try again later.");
            }
        }
    });
}function CartProGuestRemoveSuccess() {
    swal({
        title: "Success",
        text: "Item has been removed.",
        confirmButtonColor: '#DD6B55',
        confirmButtonText: 'OK, GOT IT!',
    },
        function (isConfirm) {
            location.href = "/Product/GuestCart";
        });
}function EmptyToWishList(UserId) {    if (UserId > 0) {
        swal({
            title: "Remove all wishlist products",
            text: "Are you sure you want to delete all your wishlist products ?",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: '#DD6B55',
            confirmButtonText: 'Yes, I am sure!',
            cancelButtonText: "No, cancel it!",
            closeOnConfirm: false,
            closeOnCancel: true
        },
            function (isConfirm) {
                if (isConfirm) {
                    var routeURL = location.protocol + '//' + location.host;
                    $.ajax({
                        url: routeURL + '/api/AjaxCall/CancelWishlist',
                        type: 'POST',
                        data: '',
                        success: function (data) {
                            if (data.status > 0) {
                                $("#widhItemsCount").html("0");
                                swal("Success", "Your wishlist has been emptied.");
                                $("#NoWishList").show();
                                $("#wishListDiv").hide();
                            }
                            else if (data.status == 0) {
                                $("#widhItemsCount").html("0");
                                swal("Success", "Your wishlist is already empty.");
                            }
                            else {
                                swal("Connection problem", "Something is wrong with your internet connection, please try again later.");
                            }
                        }
                    });
                }
            });
    }
    else {
        $(".login_popup").fadeIn(200);
        $("#widhItemsCount").html(0);
    }
}

function EmptyToGustWishList(GuestUserId) {
    swal({
        title: "Remove all wishlist products",
        text: "Are you sure you want to delete all your wishlist products ?",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: '#DD6B55',
        confirmButtonText: 'Yes, I am sure!',
        cancelButtonText: "No, cancel it!",
        closeOnConfirm: false,
        closeOnCancel: true
    },
        function (isConfirm) {
            if (isConfirm) {
                var routeURL = location.protocol + '//' + location.host;
                $.ajax({
                    url: routeURL + '/api/AjaxCall/CancelGuestWishlist',
                    type: 'POST',
                    data: '',
                    success: function (data) {
                        if (data.status > 0) {
                            $("#GuestWidhItemsCount").html("0");
                            swal("Success", "Your wishlist has been emptied.");
                            $("#NoWishList").show();
                            $("#wishListDiv").hide();
                        }
                        else if (data.status == 0) {
                            $("#widhItemsCount").html("0");
                            swal("Success", "Your wishlist is already empty.");
                        }
                        else {
                            swal("Connection problem", "Something is wrong with your internet connection, please try again later.");
                        }
                    }
                });
            }
        });
}

function WishListProductToRemove(UserId, WishListId, Element) {
    if (UserId > 0) {
        $.ajax({
            url: routeURL + '/api/AjaxCall/WishListProductToRemove?WishListId=' + WishListId,
            type: 'GET',
            success: function (result) {
                if (result.status == 1) {
                    var wishListItemCount = parseInt($("#widhItemsCount").html());
                    if (wishListItemCount > 0) {
                        $("#widhItemsCount").html(wishListItemCount - 1);
                    }                    $(Element).parent().parent().fadeOut(400, function () {
                        $(Element).remove();
                        resizeFunction();                        $("#" + WishListId).remove();                        if ($("#wishListDiv ul").children().length == 0) {                            $("#wishListDiv").remove();
                            $("#NoWishList").show();
                        }                    });                }
                else {
                    swal("Connection problem", "Something is wrong with your internet connection, please try again later.");
                }
            }
        });
    }
    else {
        $(".login_popup").fadeIn(200);
        $("#widhItemsCount").html(0);
    }
}

function GuestWishListProductToRemove(UserId, WishListId, Element) {
    $.ajax({
        url: routeURL + '/api/AjaxCall/GuestWishListProductToRemove?WishListId=' + WishListId,
        type: 'GET',
        success: function (result) {
            if (result.status == 1) {
                var wishListItemCount = parseInt($("#GuestWidhItemsCount").html());
                if (wishListItemCount > 0) {
                    $("#GuestWidhItemsCount").html(wishListItemCount - 1);
                }                $(Element).parent().parent().fadeOut(400, function () {
                    $(Element).remove();
                    resizeFunction();                    $("#" + WishListId).remove();                    if ($("#wishListDiv ul").children().length == 0) {                        $("#wishListDiv").remove();
                        $("#NoWishList").show();
                    }                });            }
            else {
                swal("Connection problem", "Something is wrong with your internet connection, please try again later.");
            }
        }
    });
}

function UpdateToCartQty(UserId, CartDetailsId, Qty) {
    if (UserId > 0) {
        $('.ajax-loader').css("visibility", "visible");
        $.ajax({
            url: routeURL + '/api/AjaxCall/UpdateToCartQty?CartDetailsId=' + CartDetailsId + '&&Qty=' + Qty,
            type: 'GET',
            success: function (data) {
                if (data.status == 1) {
                    location.reload();
                }
                else {
                    swal("Error", "Something went wrong with your internet connection, please try again later!");
                }
                $('.ajax-loader').css("visibility", "hidden");
            }
        });
    }
    else {
        $(".login_popup").fadeIn(200);
        $("#widhItemsCount").html(0);
    }
}

function GuestUpdateToCartQty(UserId, CartDetailsId, Qty) {
    $('.ajax-loader').css("visibility", "visible");
    $.ajax({
        url: routeURL + '/api/AjaxCall/GuestUpdateToCartQty?CartDetailsId=' + CartDetailsId + '&&Qty=' + Qty,
        type: 'GET',
        success: function (data) {
            if (data.status == 1) {
                location.reload();
            }
            else {
                swal("Error", "Something went wrong with your internet connection, please try again later!");
            }
            $('.ajax-loader').css("visibility", "hidden");
        }
    });
}




///Function for Wishlist items resize
var resizeFunction = function () {
    rightMargin = 22;
    $(".wishlistItemsContainer ul").css({ "width": ($(".wishlistItemsContainer ul li").length * ($(".wishlistItemsContainer ul li").width() + rightMargin)) + "px" })
}