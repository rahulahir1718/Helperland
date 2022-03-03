var currentlyOpen;
$(document).ready(function () {
  $("#go-up").hide();
  $("#get-message").hide();

  loadLoginPopUp(loginPopUp);

  $(".l-6").click(function () {
    $(window).scrollTop(0);
    if (currentlyOpen != "Login") {
      loadLoginPopUp("True");
    } else {
      showLoginPopUp();
    }
  });
});

function loadLoginPopUp(openPopUp) {
  var url = "/account/login";
  $.get(url, function (data) {
    $("#exampleModal").html(data);
    if (openPopUp == "True") {
      showLoginPopUp();
    }
  });
}

function showLoginPopUp() {
  $("#exampleModal").modal("show");
  currentlyOpen = "Login";
}

function openForgetPasswordPopUp() {
  var url = "/account/forgotpassword";
  $.get(url, function (data) {
    $("#exampleModal").html(data);
    $("#exampleModal").modal("show");
    currentlyOpen = "ForgotPassword";
  });
}

function PostRequest() {
  var url = "/account/login";
  var valdata = $("#loginForm").serialize();
  //to get alert popup
  $.post(url, valdata, function (data) {
    var url = data.split("returnUrl=");
    if (url[1] != null) {
      window.location.href = url[1];
    } else {
      $("#exampleModal").html(data);
      $("#exampleModal").modal("show");
    }
  });
}

function forgotPasswordPostRequest() {
  var url = "/account/forgotpassword";
  var valdata = $("#forgetPasswordForm").serialize();
  $.post(url, valdata, function (data) {
    $("#exampleModal").html(data);
    $("#exampleModal").modal("show");
  });
}

$(".dropdown-item").click(function () {
  var src = $(this).children().eq(0).attr("src");
  $("#flagImage").attr("src", src);
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
