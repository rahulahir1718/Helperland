$(document).ready(function () {
  $("#go-up").hide();
  $("#get-message").hide();
});

$("#acceptance > a").click(function () {
  $("#acceptance").fadeOut(500);
});

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

  if (scroll > 800) {
    $("#go-up").fadeIn(800);
  } else {
    $("#go-up").fadeOut(500);
  }

  if ($(window).width() < 500) {
    if (scroll > 5500) {
      $("#get-message").fadeIn(800);
    } else {
      $("#get-message").fadeOut(500);
    }
  } else {
    if (scroll > 2500) {
      $("#get-message").fadeIn(800);
    } else {
      $("#get-message").fadeOut(500);
    }
  }
});
