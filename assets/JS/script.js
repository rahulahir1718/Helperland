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
let rotation = 0;

$("#img1").click(function () {
  alert("Hello");
});
function rotateimg1() {
  rotation += 90;
  if (rotation === 180) {
    rotation = 0;
  }
  document.querySelector("#img1").style.transform = `rotate(${rotation}deg)`;
}
function rotateimg2() {
  rotation += 90;
  if (rotation === 180) {
    rotation = 0;
  }
  document.querySelector("#img2").style.transform = `rotate(${rotation}deg)`;
}
function rotateimg3() {
  rotation += 90;
  if (rotation === 180) {
    rotation = 0;
  }
  document.querySelector("#img3").style.transform = `rotate(${rotation}deg)`;
}
function rotateimg4() {
  rotation += 90;
  if (rotation === 180) {
    rotation = 0;
  }
  document.querySelector("#img4").style.transform = `rotate(${rotation}deg)`;
}
