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

  $("#service-schedule").addClass("active-link");

  var events = [];
  $.ajax({
    type: "GET",
    url: "/serviceprovider/getrequests",
    success: function (data) {
      $.each(data.data, function (i, v) {
        events.push({
          title: v.title,
          start: v.start,
          end: v.End != null ? v.End : null,
          color: v.color,
          id: v.id,
          allDay: true,
        });
      });
      GenerateCalender(events);
    },
    error: function (error) {
      alert("failed");
    },
  });
});

function GenerateCalender(events) {
  $("#calender").fullCalendar("destroy");
  $("#calender").fullCalendar({
    defaultDate: new Date(),
    timeFormat: "h(:mm)a",
    header: {
      left: "prev,next title",
      right: "my text",
    },
    eventLimit: true,
    eventColor: "#378006",
    events: events,
    eventClick: function (calEvent, jsEvent, view) {
      loadSPModal(calEvent.id);
    },
  });
}

function loadSPModal(id) {
  $.ajax({
    url: "/serviceprovider/servicerequestdetail",
    type: "GET",
    data: {
      id: id,
    },
    success: function (result) {
      $("#spModal").html(result);
      $("#spModal").modal("show");
      $(".modal-cancel-btn").click(function () {
        sendPostRequest(
          $(this).attr("data-id"),
          $(this).children("span").text()
        );
      });
      $(".modal-complete-btn").click(function () {
        sendPostRequest(
          $(this).attr("data-id"),
          $(this).children("span").text()
        );
      });
    },
    error: function () {
      alert("error");
    },
  });
}

function sendPostRequest(id, process) {
  $.ajax({
    url: "/serviceprovider/servicerequestdetail",
    type: "POST",
    data: {
      requestId: id,
      process: process,
    },
    success: function (result) {
      $("#spModal").html(result);
    },
    error: function () {
      alert("error");
    },
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
