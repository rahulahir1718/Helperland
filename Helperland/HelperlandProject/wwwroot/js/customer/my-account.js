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

  $("#my-details-div").load("/customer/mydetails");
  $("#my-addresses-div").load("/customer/myaddresses", function () {
    myAddressEvents();
  });
});

function myAddressEvents() {
  $(".editBtn").click(function () {
    $.ajax({
      url: "/customer/editaddress",
      type: "GET",
      data: {
        id: $(this).attr("data-id"),
      },
      success: function (result) {
        $("#customerModal").html(result);
        $("#customerModal").modal("show");
      },
      error: function () {
        alert("error");
      },
    });
  });

  $(".deleteBtn").click(function () {
    $.ajax({
      url: "/customer/deleteaddress",
      type: "POST",
      data: {
        id: $(this).attr("data-id"),
      },
      success: function (result) {
        $("#my-addresses-div").load("/customer/myaddresses", function () {
          myAddressEvents();
        });
      },
      error: function () {
        alert("error");
      },
    });
  });

  $("#newAddressBtn").click(function () {
    $.ajax({
      url: "/customer/editaddress",
      type: "GET",
      success: function (result) {
        $("#customerModal").html(result);
        $("#customerModal").modal("show");
      },
      error: function () {
        alert("error");
      },
    });
  });
}

function editCompleted() {
  $("#customerModal").modal("hide");
  $("#my-addresses-div").load("/customer/myaddresses", function () {
    myAddressEvents();
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
