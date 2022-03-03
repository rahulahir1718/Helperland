$(window).scroll(function () {
  var sticky = $("header"),
    scroll = $(window).scrollTop();

  if (scroll > 64) sticky.addClass("fixed");
  else sticky.removeClass("fixed");
});


$(document).ready(function () {
    $("#fileUpload").on("change", function () {
        var fileName = $(this).val().split("\\").pop();
        $("#file-chosen").html(fileName);
        $("#forFileUpload").addClass("uploaded");
    });

    $("header .navbar").css("background", "rgba(82, 82, 82, 0.8)");
    $("header .navbar-brand img").height(54).width(73);
    $("header .l-1").removeClass("nlbt").addClass("nlbb").html("Book now").width(111);
    $("header .l-2").html("Prices & services").addClass("nlbt");
    $("header .l-3").html("Warranty");
    $("header .l-5").html("Contact");
    $("header .l-6").removeClass("nlbt").addClass("nlbb").attr('href', "/home/index/true");
    $("header .l-7").removeClass("nlbt").addClass("nlbb");
    $("header .l-8").hide();
});

$('#myCheckBox').change(function () {
    $('#submit-btn').prop("disabled", !this.checked);
}).change()