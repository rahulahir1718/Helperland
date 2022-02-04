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
});