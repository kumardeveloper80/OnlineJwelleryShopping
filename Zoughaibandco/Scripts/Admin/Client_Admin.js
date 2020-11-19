$(document).ready(function () {
    $('#searchTxt').on("input", function () {
        var input = this.value;
        ClientList(input);
    });
});

var routeURL = location.protocol + '//' + location.host;

ClientList("");

function ClientList(searchstring) {
    var search = searchstring;
    $('.ajax-loader').css("visibility", "visible");
    $.ajax({
        url: routeURL + '/api/AdminApi/ClientList?search=' + search,
        type: 'GET',
        success: function (data) {
            if (data.status == 1) {
                $("#clientListDiv").html(data.dataenum);
            }
            else if (data.status == 0) {
                $("#clientListDiv").html("Data not found");
            }
            else {
                $("#clientListDiv").html("Data not found");
                alert("Something went wrong with your internet connection, please try again later!");
            }
            $('.ajax-loader').css("visibility", "hidden");
        }
    });
}

function DeleteClient(clientId) {
    $.ajax({
        url: routeURL + '/api/AdminApi/DeleteClient?ClientId=' + clientId,
        type: 'GET',
        success: function (data) {
            if (data.status == 1) {
                ClientList("");
            }
            else if (data.status == 0) {
                alert("Client not deleted.");
            }
            else {
                alert("Something went wrong with your internet connection, please try again later!");
            }
            $('.ajax-loader').css("visibility", "hidden");
        }
    });
}