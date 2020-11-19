$(document).ready(function () {
    $('#searchTxt').on("input", function () {
        var input = this.value;
        ProductList(filterType);
    });

    $("#SingleImage").change(function (e) {
        var file;
        if ((file = this.files[0])) {
            var fileExtension = file.name.split('.').pop().toLowerCase();
            var validExtensions = ['png', 'jpg', 'jpeg'];
            if ($.inArray(fileExtension, validExtensions) == -1) {
                $("#SingleImage").val('');
                $('#productImg').attr("src", '');
                $("#deleteImg").hide();
                alert("Invalid file type");
            }
            else if (file.size / 1024 > 500) {
                $("#SingleImage").val('');
                $('#productImg').attr("src", '');
                $("#deleteImg").hide();
                alert("IMAGE SIZE SHOULD BE UNDER 500 KB");
            }
            else {
                var reader = new FileReader();
                var imgtag = document.getElementById("productImg");
                imgtag.title = file.name;

                reader.onload = function (event) {
                    imgtag.src = event.target.result;
                };

                reader.readAsDataURL(file);
                $("#deleteImg").show();
            }
        }
    });

    $("#addColor").on("click", function () {
        addColor($(".jscolor").val());
    });

    $(".removeColor").click(function () {
        $("#colorString").val($("#colorString").val().replace($(this).parent().attr("colorValue"), ""));
        $(this).parent().remove();
        console.log($("#colorString").val());
    });
});

var routeURL = location.protocol + '//' + location.host;
var filterType = "";

ProductList("", "");

function ProductList(filter) {
    filterType = filter;
    var search = $('#searchTxt').val();
    $('.ajax-loader').css("visibility", "visible");
    $.ajax({
        url: routeURL + '/api/AdminApi/ProductList?search=' + search + '&filter=' + filterType,
        type: 'GET',
        success: function (data) {
            if (data.status == 1) {
                $("#productListDiv").html(data.dataenum);
            }
            else if (data.status == 0) {
                $("#productListDiv").html("Data not found");
            }
            else {
                $("#productListDiv").html("Data not found");
                alert("Something went wrong with your internet connection, please try again later!");
            }
            $('.ajax-loader').css("visibility", "hidden");
        }
    });
}

function DeleteProduct(productId) {
    $.ajax({
        url: routeURL + '/api/AdminApi/DeleteProduct?ProductId=' + productId,
        type: 'GET',
        success: function (data) {
            if (data.status == 1) {
                ProductList("");
            }
            else if (data.status == 0) {
                alert("Product not deleted.");
            }
            else {
                alert("Something went wrong with your internet connection, please try again later!");
            }
            $('.ajax-loader').css("visibility", "hidden");
        }
    });
}

function AddEditProduct() {
    if (Validation()) {
        let colorList;
        if ($('#colorString').val().length > 0) {
            colorList = $('#colorString').val();
        }

        var requestData = new FormData();
        var ProductImg = document.getElementById('SingleImage');
        if (ProductImg.files.length > 0) {
            requestData.append(ProductImg.files[0].name, ProductImg.files[0]);
        }

        if ($('#IsPublished').is(':checked')) {
            requestData.append('IsPublished', true);
        }
        else {
            requestData.append('IsPublished', false);
        }

        var ProductId = $("#ProductId").val();
        requestData.append('ProductId', ProductId);
        requestData.append('Title', $.trim($('#Title').val()));
        requestData.append('ReferenceNO', $.trim($('#ReferenceNO').val()));
        requestData.append('Price', $.trim($('#Price').val()));
        requestData.append('PriceDiscounted', $.trim($('#PriceDiscounted').val()));
        requestData.append('Description', CKEDITOR.instances.Description.getData());
        requestData.append('CategoryId', $('#CategoryId').val());
        requestData.append('SubCategoryId', $('#SubCategoryId').val());
        requestData.append('CollectionId', $('#CollectionId').val());
        requestData.append('ColorList', colorList);
        //added later
        requestData.append('Material', $.trim($('#Material').val()));
        requestData.append('GoldWeight', $.trim($('#GoldWeight').val()));
        requestData.append('Round', $.trim($('#Round').val()));
        requestData.append('Marquis', $.trim($('#Marquis').val()));
        requestData.append('Ruby', $.trim($('#Ruby').val()));
        requestData.append('Sapphire', $.trim($('#Sapphire').val()));
        requestData.append('Emerald', $.trim($('#Emerald').val()));
        requestData.append('Fancy', $.trim($('#Fancy').val()));
        requestData.append('Princes', $.trim($('#Princes').val()));
        requestData.append('Baguette', $.trim($('#Baguette').val()));
        requestData.append('Triangle', $.trim($('#Triangle').val()));
        requestData.append('Pear', $.trim($('#Pear').val()));
        requestData.append('black', $.trim($('#black').val()));
        requestData.append('semiprecious', $.trim($('#semiprecious').val()));
        requestData.append('diamond', $.trim($('#diamond').val()));
        requestData.append('parentId', $('#ParentId').val());

        $.ajax({
            url: routeURL + '/api/AdminApi/SaveProduct',
            type: 'POST',
            data: requestData,
            dataType: 'json',
            contentType: false,
            processData: false,
            success: function (result) {
                if (result.status == 1) {
                    location.href = "/Admin/Product";
                }
                else if (result.status == 0) {
                    if (ProductId > 0) {
                        alert("Product details not updated");
                    } else {
                        alert("Product details not added");
                    }
                }
                else {
                    alert(result.message);
                }
            },
            error: function (xhr) {
                swal("Error", xhr.statusText);
            },
        });
    }
}

function Validation() {
    IsValid = true;
    if ($.trim($('#Title').val()) == "") {
        IsValid = false;
        $("label[for=Title]").addClass("InValid");
    }
    else {
        $("label[for=Title]").removeClass("InValid");
    }

    if ($.trim($('#Price').val()) == "") {
        IsValid = false;
        $("label[for=Price]").addClass("InValid");
    }
    else {
        $("label[for=Price]").removeClass("InValid");
    }

    var ProductId = $("#ProductId").val();
    var file = document.getElementById('SingleImage');
    var imgSrc = $('#productImg').attr("src");

    //For Update Product
    if (ProductId > 0) {
        if (imgSrc == "") {
            $("#imgLbl").addClass("InValid");
            IsValid = false;
        }
        else {
            $("#imgLbl").removeClass("InValid");
        }
    }
    else {
        //For Create Product
        if (file.files.length == 0) {
            IsValid = false;
            $("#imgLbl").addClass("InValid");
        }
        else {
            $("#imgLbl").removeClass("InValid");
        }
    }



    return IsValid;
}

function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode != 46 && charCode > 31
        && (charCode < 48 || charCode > 57))
        return false;
    return true;
}


function isNumberKeyDecimal(evt, obj) {

    var charCode = (evt.which) ? evt.which : event.keyCode
    var value = obj.value;
    var dotcontains = value.indexOf(".") != -1;
    if (dotcontains)
        if (charCode == 46) return false;
    if (charCode == 46) return true;
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;
    return true;
}

function RemoveImg() {
    $("#SingleImage").val('');
    $('#productImg').attr("src", '');
    $("#deleteImg").hide();
}

function addColor(color) {
    if ($("#colorString").val().indexOf(color) == -1) {
        $(".colorsHolder").append("<div class='colorDiv' colorValue='#" + color + "' style='background-color:#" + color + "'>" + "<div class='removeColor'>X</div></div>");
        $("#colorString").val($("#colorString").val() + "#" + color);

        $(".removeColor").click(function () {
            $("#colorString").val($("#colorString").val().replace($(this).parent().attr("colorValue"), ""));
            $(this).parent().remove();
        });
    }
}