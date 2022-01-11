alert(
  "footer html file will be loaded using jquery..so make sure that this page is loaded using web server like Apache or VS Code plugin Live Server because header and footer will not be shown directly on Google Chrome or any other browser due to file access constraint."
);

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

$(window).resize(function () {
  updateContainer();
});

$(".vertical-navbar ul li a").click(function () {
  closeNavbar();
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

$(".bottom-buttons > button").click(function () {
  $(".bottom-buttons > button").removeClass("active");
  $(".bottom-buttons > button").css("color", "#777777");
  $(this).addClass("active");
  $(this).css("color", "white");
});

$("#export").on("click", function () {
  $(".buttons-pdf").trigger("click");
});

$(document).ready(function () {
  var table = $("#table_id").DataTable({
    searching: false,
    info: false,
    responsive: true,
    buttons: ["pdfHtml5"],
    stripeClasses: [],
    aLengthMenu: [
      [5, 10, 15, -1],
      [5, 10, 25, "All"],
    ],
    dom: '<"float-left"B><"float-right"f>rt<"row"<"col-sm-4"l><"col-sm-4"i><"col-sm-4"p>>',
    pageLength: 10,
    paging: "true",
    pagingType: "full_numbers",
    language: {
      paginate: {
        next: '<img src="assets/images/keyboard-right-arrow-button.png" />',
        previous: '<img src="assets/images/keyboard-left-arrow-button.png" />',
        first: '<img src="assets/images/left_play.png" alt="" />',
        last: '<img src="assets/images/right_play.png"/>',
      },
    },
  });
  $(".buttons-pdf").hide();
  var entries = table.page.info().recordsTotal;
  $("#table_id_length label").append(" Total Record: " + entries);
});
