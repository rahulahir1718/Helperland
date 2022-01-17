alert(
  "header and footer html files will be loaded using jquery..so make sure that this page is loaded using web server like Apache or VS Code plugin Live Server because header and footer will not be shown directly on Google Chrome or any other browser due to file access constraint."
);
$(document).ready(function () {
  $("#go-up").hide();
  $("#get-message").hide();
});
$("#header").load("header.html");
$("#footer").load("footer.html");
$(window).scroll(function () {
  var sticky = $("#header"),
    scroll = $(window).scrollTop();

  if (scroll > 64) sticky.addClass("fixed");
  else sticky.removeClass("fixed");

  if (scroll > 300) {
    $("#go-up").fadeIn(800);
  } else {
    $("#go-up").fadeOut(500);
  }

  if ($(window).width() < 500) {
    if (scroll > 250) {
      $("#get-message").fadeIn(800);
    } else {
      $("#get-message").fadeOut(500);
    }
  } else {
    if (scroll > 1500) {
      $("#get-message").fadeIn(800);
    } else {
      $("#get-message").fadeOut(500);
    }
  }
});
