$(document).ready(initialization());

function initialization() {
  $("#header").load("header.html");
  $("#footer").load("footer.html");
  $("#fsp").hide();
  $(".btn-1").addClass("active-btn");
  $(".btn-2").addClass("non-active-btn");

  $(".btn-1").click(function () {
    btn1Clicked();
  });
  $(".btn-2").click(function () {
    btn2Clicked();
  });
  alert(
    "header and footer html files will be loaded using jquery..so make sure that this page is loaded using web server like Apache or VS Code plugin Live Server because header and footer will not be shown directly on Google Chrome or any other browser due to file access constraint."
  );
}

function btn1Clicked() {
  if (!$(".btn-1").hasClass("active-btn")) {
    $(".btn-1").addClass("active-btn");
    $(".btn-1").removeClass("non-active-btn");
    $(".btn-2").removeClass("active-btn");
    $(".btn-2").addClass("non-active-btn");
    $("#fc").show();
    $("#fsp").hide();
  }
}

function btn2Clicked() {
  if (!$(".btn-2").hasClass("active-btn")) {
    $(".btn-2").addClass("active-btn");
    $(".btn-2").removeClass("non-active-btn");
    $(".btn-1").removeClass("active-btn");
    $(".btn-1").addClass("non-active-btn");
    $("#fsp").show();
    $("#fc").hide();
  }
}

function getRotationDegrees(obj) {
  var matrix =
    obj.css("-webkit-transform") ||
    obj.css("-moz-transform") ||
    obj.css("-ms-transform") ||
    obj.css("-o-transform") ||
    obj.css("transform");
  if (matrix !== "none") {
    var values = matrix.split("(")[1].split(")")[0].split(",");
    var a = values[0];
    var b = values[1];
    var angle = Math.round(Math.atan2(b, a) * (180 / Math.PI));
  } else {
    var angle = 0;
  }
  return angle < 0 ? angle + 360 : angle;
}

$("#fc img").click(function () {
  rotate($(this));
});

$("#fsp img").click(function () {
  rotate($(this));
});

var rotation = 0;
function rotate(obj) {
  var currentRotation = getRotationDegrees(obj);
  if (currentRotation == 90) {
    rotation = 0;
  } else {
    rotation = 90;
  }
  obj.css({
    "-webkit-transform": "rotate(" + rotation + "deg)",
    "-moz-transform": "rotate(" + rotation + "deg)",
    "-ms-transform": "rotate(" + rotation + "deg)",
    transform: "rotate(" + rotation + "deg)",
  });
}

$(window).scroll(function () {
  var sticky = $("#header"),
    scroll = $(window).scrollTop();

  if (scroll > 64) sticky.addClass("fixed");
  else sticky.removeClass("fixed");
});
