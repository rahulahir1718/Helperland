$(document).ready(function () {
  $(".newslater-area").hide();
  $("header .navbar").css("background", "rgba(82, 82, 82, 0.8)");
  $("header .navbar-brand img").height(54).width(73);
  $("header .l-1")
    .removeClass("nlbt")
    .addClass("nlbb")
    .html("Book now")
    .width(111);
  $("header .l-2").html("Prices & services");
  $("header .l-3").html("Warranty");
  $("header .l-5").html("Contact");
  $("header .l-6")
    .removeClass("nlbt")
    .addClass("nlbb")
    .attr("href", "/home/index/true");
  $("header .l-7").removeClass("nlbt").addClass("nlbb");
  $("header .l-8").hide();
  $("header .l-1").hide();

  $("#my-details-div").load("/serviceprovider/mydetails", function () {
    myDetailsEvents();
  });
});

function myDetailsEvents() {
  $(".avtar-list").each(function () {
    if ($(this).attr("src") == $("#mainProfileImg").attr("src")) {
      $(this).addClass("active-avtar");
    }
  });

  $(".avtar-list").click(function () {
    $(".avtar-list").removeClass("active-avtar");
    $(this).addClass("active-avtar");
    $("#mainProfileImg").attr("src", $(this).attr("src"));
    $("#mainProfileImgValue").val($(this).attr("src"));
  });
}

$(window).scroll(function () {
  var sticky = $("header"),
    scroll = $(window).scrollTop();

  if (scroll > 64) sticky.addClass("fixed");
  else sticky.removeClass("fixed");
});

$("#openbtn").click(function () {
  openNavbar();
});

$("#closebtn").click(function () {
  closeNavbar();
});

function closeNavbar() {
  $(".sidebar").animate(
    {
      width: "0px",
    },
    500,
    "swing"
  );
  $(".sidebar").hide();
  $("#openbtn").show();
}
function openNavbar() {
  $(".sidebar").animate(
    {
      width: "261px",
    },
    500,
    "swing"
  );
  $(".sidebar").show();
  $("#openbtn").hide();
}
