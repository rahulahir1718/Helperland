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

  findParameters();

  $("#service-history").addClass("active-link");

  $(".rateit-reset").hide();
});

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

function findParameters() {
  var table = $("#table_id").DataTable({
    searching: false,
    info: false,
    responsive: true,
    buttons: ["excelHtml5"],
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
  $(".buttons-excel").hide();
  var entries = table.page.info().recordsTotal;
  $("#table_id_length label").append(" Total Record: " + entries);

  clickEvents();
}

function clickEvents() {
  $("tbody .serviceId").click(function () {
    $.ajax({
      url: "/customer/servicerequestdetail",
      type: "GET",
      data: {
        id: $(this).text(),
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

  $(".rate-sp").click(function () {
    $.ajax({
      url: "/customer/editrating",
      type: "GET",
      data: {
        id: $(this).attr("data-id"),
      },
      success: function (result) {
        $("#ratingSubmit").attr("data-id", result.ratingId);
        $("#modal-sp-name").html(
          result.ratingToNavigation.firstName +
            " " +
            result.ratingToNavigation.lastName
        );
        $("#modalPeofileImage").attr(
          "src",
          result.ratingToNavigation.userProfilePicture
        );
        $(".rateit-average").rateit({ value: result.ratings, readonly: true });
        $(".rateit-ontime").rateit({ value: result.onTimeArrival });
        $(".rateit-friendly").rateit({ value: result.friendly });
        $(".rateit-quality").rateit({ value: result.qualityOfService });
        $(".rateit-reset").hide();
        $("#ratingModal #average-rating-text").html(result.ratings);
        $("#ratingModal").modal("show");
        $("ratingSubmit").attr("data-id", result.ratingId);
        $("#ratingSubmit").click(function () {
          var data = {
            RatingId: $(this).attr("data-id"),
            OnTimeArrival: $(".rateit-ontime").rateit("value"),
            Friendly: $(".rateit-friendly").rateit("value"),
            QualityOfService: $(".rateit-quality").rateit("value"),
            Ratings:
              ($(".rateit-ontime").rateit("value") +
                $(".rateit-friendly").rateit("value") +
                $(".rateit-quality").rateit("value")) /
              3,
            Comments: $("#feedback").val(),
          };
          $.ajax({
            type: "POST",
            url: "/customer/editrating",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(data),
              success: function (result) {
                  window.location.href = "/customer/servicehistory";
            },
            error: function () {
              alert("Failed to receive the Data");
              console.log("Failed ");
            },
          });
        });
      },
      error: function () {
        alert("error");
      },
    });
  });

  $("#export").on("click", function () {
    $(".buttons-excel").trigger("click");
  });
}
