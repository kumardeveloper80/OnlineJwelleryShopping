$(document).ready(function () {
    $('#searchTxt').on("input", function () {
        var input = this.value;
        CollectionList(filterType);
    });

    $('.ajax-loader').css("visibility", "hidden");

    $('[id^=SingleImage]').change(function (e) {        
        var id = this.id.replace('SingleImage', '');
        var file;
        if ((file = this.files[0])) {
            var fileExtension = file.name.split('.').pop().toLowerCase();
            var validExtensions = ['png', 'jpg', 'jpeg', 'svg'];
            if ($.inArray(fileExtension, validExtensions) == -1) {
                $("#SingleImage" + id).val('');
                $('#collectionImg' + id).attr("src", '');
                $("#deleteImg" + id).hide();
                alert("Invalid file type");
            }
            else if (file.size / 1024 > 1500) {
                $("#SingleImage" + id).val('');
                $('#collectionImg' + id).attr("src", '');
                $("#deleteImg" + id).hide();
                alert("IMAGE SIZE SHOULD BE UNDER 1500 KB");
            }
            else {
                var reader = new FileReader();
                var imgtag = document.getElementById("collectionImg" + id);
                imgtag.title = file.name;

                reader.onload = function (event) {
                    imgtag.src = event.target.result;
                };

                reader.readAsDataURL(file);
                $("#deleteImg" + id).show();
            }
        }
    });

});

var routeURL = location.protocol + '//' + location.host;
var filterType = "";

CollectionList("", "");

function CollectionList(filter) {
    filterType = filter;
    var search = $('#searchTxt').val();
    $('.ajax-loader').css("visibility", "visible");
    $.ajax({
        url: routeURL + '/api/AdminApi/CollectionList?search=' + search + '&filter=' + filterType,
        type: 'GET',
        success: function (data) {
            if (data.status == 1) {
                $("#collectionListDiv").html(data.dataenum);
                $('.ajax-loader').css("visibility", "hidden");
            }
            else if (data.status == 0) {
                $("#collectionListDiv").html("<div style='padding: 40px;color: #d8d8d8;text-align: center;'>Data not found</div>");
            }
            else {
                $("#collectionListDiv").html("<div style='padding: 40px;color: #d8d8d8;text-align: center;'>Data not found</div>");
                alert("Something went wrong with your internet connection, please try again later!");
            }
            $('.ajax-loader').css("visibility", "hidden");
        }
    });
    $('.ajax-loader').css("visibility", "hidden");
}

function DeleteCollection(productId) {
    $.ajax({
        url: routeURL + '/api/AdminApi/DeleteCollection?CollectionId=' + productId,
        type: 'GET',
        success: function (data) {
            if (data.status == 1) {
                CollectionList("");
            }
            else if (data.status == 0) {
                alert("Collection not deleted.");
            }
            else {
                alert("Something went wrong with your internet connection, please try again later!");
            }
            $('.ajax-loader').css("visibility", "hidden");
        }
    });
}

function AddEditCollection() {
    if (Validation()) {
        var requestData = new FormData();
        var CollectionId = $("#CollectionId").val();
        var IsAdminA = $("#IsAdminAdded").val();
        if (IsAdminA == "") {
            requestData.append('IsAdminAdded', true);
        }
        else {
            requestData.append('IsAdminAdded', IsAdminA);
        }        
        requestData.append('CollectionId', CollectionId);
        requestData.append('Name', $.trim($('#CollectionName').val()));
        requestData.append('RouteName', $.trim($('#RouteName').val()));
        if ($('#Active').is(':checked')) {
            requestData.append('Active', true);
        }
        else {
            requestData.append('Active', false);
        }
        //requestData.append('BannersId', $('#BannersId').val());
        var sd = $('#SideId').val();
        if (sd == "1") {
            requestData.append('Side', "Right");
        }
        else {
            requestData.append('Side', "Left");
        }
        requestData.append('Order', $('#Order').val());

        var ImgName1 = document.getElementById('SingleImage1');
        if (ImgName1.files.length > 0) {
            requestData.append("BannerImage", ImgName1.files[0]);
        }

        var ImgName2 = document.getElementById('SingleImage2');
        if (ImgName2.files.length > 0) {
            requestData.append("BackgroundImage", ImgName2.files[0]);
        }

        var ImgName3 = document.getElementById('SingleImage3');
        if (ImgName3.files.length > 0) {
            requestData.append("ForegroundImage", ImgName3.files[0]);
        }

        var ImgName4 = document.getElementById('SingleImage4');
        if (ImgName4.files.length > 0) {
            requestData.append("LogoImage", ImgName4.files[0]);
        }

        var ImgName5 = document.getElementById('SingleImage5');
        if (ImgName5.files.length > 0) {
            requestData.append("PageImage1", ImgName5.files[0]);
        }

        var ImgName6 = document.getElementById('SingleImage6');
        if (ImgName6.files.length > 0) {
            requestData.append("PageImage2", ImgName6.files[0]);
        }

        var ImgName7 = document.getElementById('SingleImage7');
        if (ImgName7.files.length > 0) {
            requestData.append("PageImage3", ImgName7.files[0]);
        }

        var ImgName8 = document.getElementById('SingleImage8');
        if (ImgName8.files.length > 0) {
            requestData.append("PageImage4", ImgName8.files[0]);
        }

        requestData.append('AboutText', CKEDITOR.instances.AboutText.getData());

        var ImgName9 = document.getElementById('SingleImage9');
        if (ImgName9.files.length > 0) {
            requestData.append("PageImage5", ImgName9.files[0]);
        }

        var ImgName10 = document.getElementById('SingleImage10');
        if (ImgName10.files.length > 0) {
            requestData.append("PageImage6", ImgName10.files[0]);
        }

        var ImgName11 = document.getElementById('SingleImage11');
        if (ImgName11.files.length > 0) {
            requestData.append("PageImage7", ImgName11.files[0]);
        }

        var ImgName12 = document.getElementById('SingleImage12');
        if (ImgName12.files.length > 0) {
            requestData.append("PageImage8", ImgName12.files[0]);
        }

        var BreadCrumColor = $("#Breadcrumcolour").val();
        var LookBookTextColor = $("#LookBookTextcolour").val();
        var LookBookBorderColor = $("#LookBookBorderColor").val();
        var ShopTheCollectionTextColor = $("#ShopTheCollectionTextColor").val();

       
        requestData.append('BreadCrumColor', BreadCrumColor);
        requestData.append('LookBookTextColor', LookBookTextColor);
        requestData.append('LookBookBorderColor', LookBookBorderColor);
        requestData.append('ShopTheCollectionTextColor', ShopTheCollectionTextColor);


        $.ajax({
            url: routeURL + '/api/AdminApi/SaveCollection',
            type: 'POST',
            data: requestData,
            dataType: 'json',
            contentType: false,
            processData: false,
            success: function (result) {
                if (result.status == 1) {
                    location.href = "/Admin/Collection";
                }
                else if (result.status == 0) {
                    if (CollectionId > 0) {
                        alert("Collection details not updated");
                    }
                    else {
                        alert("Collection details not added");
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
    if ($.trim($('#CollectionName').val()) == "") {
        IsValid = false;
        $("label[for=CollectionName]").addClass("InValid");
    }
    else {
        $("label[for=CollectionName]").removeClass("InValid");
    }

    if ($.trim($('#RouteName').val()) == "") {
        IsValid = false;
        $("label[for=RouteName]").addClass("InValid");
    }
    else {
        $("label[for=RouteName]").removeClass("InValid");
    }

    if ($.trim($('#Order').val()) == "") {
        IsValid = false;
        $("label[for=Order]").addClass("InValid");
    }
    else {
        $("label[for=Order]").removeClass("InValid");
    }
    
    if ($.trim($('#collectionImg1').attr("src")) == "") {
        IsValid = false;
        $("label[for=BannerImage]").addClass("InValid");
    }
    else {
        $("label[for=BannerImage]").removeClass("InValid");
    }
    if ($.trim($('#collectionImg2').attr("src")) == "") {
        IsValid = false;
        $("label[for=BackgroundImage]").addClass("InValid");
    }
    else {
        $("label[for=BackgroundImage]").removeClass("InValid");
    }

    if ($.trim($('#collectionImg3').attr("src")) == "") {
        IsValid = false;
        $("label[for=ForegroundImage]").addClass("InValid");
    }
    else {
        $("label[for=ForegroundImage]").removeClass("InValid");
    }

    //LogoImage
    if ($.trim($('#collectionImg4').attr("src")) == "") {
        IsValid = false;
        $("label[for=LogoImage]").addClass("InValid");
    }
    else {
        $("label[for=LogoImage]").removeClass("InValid");
    }

    ////Image1
    //if ($.trim($('#collectionImg5').attr("src")) == "") {
    //    IsValid = false;
    //    $("label[for=Image1]").addClass("InValid");
    //}
    //else {
    //    $("label[for=Image1]").removeClass("InValid");
    //}

    ////Image2
    //if ($.trim($('#collectionImg6').attr("src")) == "") {
    //    IsValid = false;
    //    $("label[for=Image2]").addClass("InValid");
    //}
    //else {
    //    $("label[for=Image2]").removeClass("InValid");
    //}

    ////Image3
    //if ($.trim($('#collectionImg7').attr("src")) == "") {
    //    IsValid = false;
    //    $("label[for=Image3]").addClass("InValid");
    //}
    //else {
    //    $("label[for=Image3]").removeClass("InValid");
    //}

    ////Image4
    //if ($.trim($('#collectionImg8').attr("src")) == "") {
    //    IsValid = false;
    //    $("label[for=Image4]").addClass("InValid");
    //}
    //else {
    //    $("label[for=Image4]").removeClass("InValid");
    //}
    
    //if ($.trim(CKEDITOR.instances.AboutText.getData()) == "") {
    //    IsValid = false;
    //    $("label[for=Description]").addClass("InValid");
    //}
    //else {
    //    $("label[for=Description]").removeClass("InValid");
    //}
    
    ////Image5
    //if ($.trim($('#collectionImg9').attr("src")) == "") {
    //    IsValid = false;
    //    $("label[for=Image5]").addClass("InValid");
    //}
    //else {
    //    $("label[for=Image5]").removeClass("InValid");
    //}

    ////Image6
    //if ($.trim($('#collectionImg10').attr("src")) == "") {
    //    IsValid = false;
    //    $("label[for=Image6]").addClass("InValid");
    //}
    //else {
    //    $("label[for=Image6]").removeClass("InValid");
    //}

    ////Image7
    //if ($.trim($('#collectionImg11').attr("src")) == "") {
    //    IsValid = false;
    //    $("label[for=Image7]").addClass("InValid");
    //}
    //else {
    //    $("label[for=Image7]").removeClass("InValid");
    //}

    ////Image8
    //if ($.trim($('#collectionImg12').attr("src")) == "") {
    //    IsValid = false;
    //    $("label[for=Image8]").addClass("InValid");
    //}
    //else {
    //    $("label[for=Image8]").removeClass("InValid");
    //}

    var CollectionId = $("#CollectionId").val();

    return IsValid;
}

function RemoveImg(cid, cname, id) {    
    if (cid != "" && cname != "") {
        $.ajax({
            url: routeURL + '/api/AdminApi/DeleteImage?CollectionId=' + cid + '&ColumnName=' + cname,
            type: 'GET',
            success: function (data) {
                if (data.status == 1) {
                    $("#SingleImage" + id).val('');
                    $('#collectionImg' + id).attr("src", '');
                    $("#deleteImg" + id).hide();
                }
                else if (data.status == 0) {
                    alert("image not deleted.");
                }
                else {
                    alert("Something went wrong with your internet connection, please try again later!");
                }
                $('.ajax-loader').css("visibility", "hidden");
            }
        });
    }
    else {
        $("#SingleImage" + id).val('');
        $('#collectionImg' + id).attr("src", '');
        $("#deleteImg" + id).hide();
    } 
}

function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode != 46 && charCode > 31
        && (charCode < 48 || charCode > 57))
        return false;
    return true;
}
