alert(
  "footer html file will be loaded using jquery..so make sure that this page is loaded using web server like Apache or VS Code plugin Live Server because header and footer will not be shown directly on Google Chrome or any other browser due to file access constraint."
);
$(document).ready(function () {
  var $width = $(window).width();
  $(window).resize(function () {
    $width = $(window).width();
    updateContainer($width);
  });

  $("table").DataTable({
    searching: false,
    info: false,
    responsive: true,
    stripeClasses: [],
    aLengthMenu: [
      [5, 10, 15, -1],
      [5, 10, 25, "All"],
    ],
    dom: '<"float-left"B><"float-right"f>rt<"row"<"col-sm-4"l><"col-sm-4"i><"col-sm-4"p>>',
    pageLength: 10,
    paging: "true",
    pagingType: "simple_numbers",
    language: {
      paginate: {
        next: '<img src="assets/images/polygon-2-copy-5.png" />',
        previous: '<img src="assets/images/polygon-1-copy-5.png" />',
      },
    },
  });
});

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
  var $width = $(window).width();
  updateContainer($width);
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
function updateContainer(width) {
  if (width <= 1139) {
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
