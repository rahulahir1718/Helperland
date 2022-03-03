$(document).ready(function () {
  $("#go-up").hide();
    $("#get-message").hide();

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

$(window).scroll(function () {
  var sticky = $("header"),
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
