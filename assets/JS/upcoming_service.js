$("#footer").load("footer.html", function () {
  $(".newslater-area").hide();
});

$("#openbtn").hide();
$("#closebtn").hide();

$("#openbtn").click(function () {
  openNavbar();
});

$("#closebtn").click(function () {
  closeNavbar();
});

$(".vertical-navbar ul li a").click(function () {
  closeNavbar();
});

$(window).resize(function () {
  updateContainer();
});

function closeNavbar() {
  $(".vertical-navbar").animate(
    {
      width: "0px",
    },
    500,
    "swing"
  );
  $(".vertical-navbar").hide();
  $("#openbtn").show();
}
function openNavbar() {
  $(".vertical-navbar").animate(
    {
      width: "261px",
    },
    500,
    "swing"
  );
  $(".vertical-navbar").show();
  $("#openbtn").hide();
}
function updateContainer() {
  var $width = $(window).width();
  if ($width <= 1139) {
    $(".vertical-navbar").addClass("closed");
    $(".vertical-navbar").hide();
    $("#openbtn").show();
    $("#closebtn").show();
  } else {
    $(".vertical-navbar").removeClass("closed");
    $(".vertical-navbar").show();
    $("#openbtn").hide();
    $("#closebtn").hide();
  }
}
