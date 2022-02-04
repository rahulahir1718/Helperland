$(document).ready(function () {
  $("#header").load("header.html");
  $("#footer").load("footer.html");
  $("#tab-nav .nav-item .nav-link").removeClass("active");
});

$("#tab-nav .nav-item").click(function () {
  var link = $(this).children().eq(0);
  if (link.hasClass("nav-item-active")) {
  } else {
    link.addClass("nav-item-active");
    var img = $(this).children().children().eq(0);
    var imgPath = img.attr("src").split(".");
    img.attr("src", imgPath[0] + "-white.png");
  }
});
