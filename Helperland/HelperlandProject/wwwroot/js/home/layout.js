$(document).ready(function () {
    $(".l-6").click(function () {
        $(window).scrollTop(0);
        var url = $(this).data('url');
        openLoginPopUp(url);
    });
});


function openLoginPopUp(url) {
    $.get(url, function (data) {
        $("#exampleModal").html(data);
        $("#exampleModal").modal("show");
    });
}

function openForgetPasswordPopUp() {
    var url = "/account/forgotpassword";
    $.get(url, function (data) {
        $("#exampleModal").html(data);
        $("#exampleModal").modal("show");
    });
}