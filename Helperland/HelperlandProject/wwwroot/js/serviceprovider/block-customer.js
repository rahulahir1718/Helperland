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

  $("#block-customer").addClass("active-link");

  findParameters();
});

function findParameters() {
  var table = $("#table_id").DataTable({
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
    pagingType: "full_numbers",
    language: {
      paginate: {
        next: '<img src="/images/keyboard-right-arrow-button.png" />',
        previous: '<img src="/images/keyboard-left-arrow-button.png" />',
        first: '<img src="/images/left_play.png" alt="" />',
        last: '<img src="/images/right_play.png"/>',
      },
    },
  });
  var entries = table.page.info().recordsTotal;
  $("#table_id_length label").append(" Total Record: " + entries);

  clickEvents();
}

function clickEvents() {
  $(".block-btn").click(function () {
    $.ajax({
      url: "/serviceprovider/blockcustomer",
      type: "POST",
      data: {
        customerId: $(this).attr("data-id"),
      },
      success: function (result) {
        window.location.href = "/serviceprovider/blockcustomer";
      },
      error: function () {
        alert("error");
      },
    });
  });

  $(".unblock-btn").click(function () {
    $.ajax({
      url: "/serviceprovider/unblockcustomer",
      type: "POST",
      data: {
        customerId: $(this).attr("data-id"),
      },
      success: function (result) {
        window.location.href = "/serviceprovider/blockcustomer";
      },
      error: function () {
        alert("error");
      },
    });
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

$(".bottom-buttons > button").click(function () {
  $(".bottom-buttons > button").removeClass("active");
  $(".bottom-buttons > button").css("color", "#777777");
  $(this).addClass("active");
  $(this).css("color", "white");
});
