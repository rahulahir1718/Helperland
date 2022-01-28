$(window).scroll(function () {
  var sticky = $("#header"),
    scroll = $(window).scrollTop();

  if (scroll > 64) {
    sticky.addClass("fixed");
    $(".navbar-brand img").height(54).width(73);
    $(".navbar").css("background", "rgba(82, 82, 82, 0.8)");
    $(".l-1").removeClass("nlbt");
    $(".l-1").addClass("nlbb");
    $(".l-6").removeClass("nlbt");
    $(".l-6").addClass("nlbb");
    $(".l-7").removeClass("nlbt");
    $(".l-7").addClass("nlbb");
  } else {
    sticky.removeClass("fixed");
    $(".navbar-brand img").height(120).width(156);
    $(".navbar").css("background", "transparent");
    $(".l-1").removeClass("nlbb");
    $(".l-1").addClass("nlbt");
    $(".l-6").removeClass("nlbb");
    $(".l-6").addClass("nlbt");
    $(".l-7").removeClass("nlbb");
    $(".l-7").addClass("nlbt");
  }
});

$(".dropdown-item").click(function () {
  var src = $(this).children().eq(0).attr("src");
  $("#flagImage").attr("src", src);
});
