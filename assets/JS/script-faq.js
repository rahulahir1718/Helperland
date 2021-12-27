$(document).ready(initialization());

function initialization() {
  $("#header").load("header.html");
  $("#footer").load("footer.html");
  $("#fsp").hide();
  $(".btn-1").addClass("active-btn");
  $(".btn-2").addClass("non-active-btn");

  $(".btn-1 a").click(function () {
    btn1Clicked();
  });
  $(".btn-2 a").click(function () {
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

let rotation1 = 0;
function rotateimg1() {
  rotation1 += 90;
  if (rotation1 === 180) {
    rotation1 = 0;
  }
  document.querySelector("#img1").style.transform = `rotate(${rotation1}deg)`;
}

let rotation2 = 0;
function rotateimg2() {
  rotation2 += 90;
  if (rotation2 === 180) {
    rotation2 = 0;
  }
  document.querySelector("#img2").style.transform = `rotate(${rotation2}deg)`;
}

let rotation3 = 0;
function rotateimg3() {
  rotation3 += 90;
  if (rotation3 === 180) {
    rotation3 = 0;
  }
  document.querySelector("#img3").style.transform = `rotate(${rotation3}deg)`;
}

let rotation4 = 0;
function rotateimg4() {
  rotation4 += 90;
  if (rotation4 === 180) {
    rotation4 = 0;
  }
  document.querySelector("#img4").style.transform = `rotate(${rotation4}deg)`;
}

let rotation_1 = 0;
function rotateimg_1() {
  rotation_1 += 90;
  if (rotation_1 === 180) {
    rotation_1 = 0;
  }
  document.querySelector("#img-1").style.transform = `rotate(${rotation_1}deg)`;
}

let rotation_2 = 0;
function rotateimg_2() {
  rotation_2 += 90;
  if (rotation_2 === 180) {
    rotation_2 = 0;
  }
  document.querySelector("#img-2").style.transform = `rotate(${rotation_2}deg)`;
}

$(window).scroll(function () {
  var sticky = $("#header"),
    scroll = $(window).scrollTop();

  if (scroll > 64) sticky.addClass("fixed");
  else sticky.removeClass("fixed");
});
