$("#openbtn").click(function () {
  openNavbar();
});

$("#closebtn").click(function () {
  closeNavbar();
});

$(window).resize(function () {
  var $width = $(window).width();
  updateContainer($width);
});

function closeNavbar() {
  $(".sidebar").animate(
    {
      width: "0px",
    },
    500,
    "swing"
  );

  $("#openbtn").show();
}
function openNavbar() {
  $(".sidebar").animate(
    {
      width: "273px",
    },
    500,
    "swing"
  );
  $(".sidebar").show();
  $("#openbtn").hide();
}

function updateContainer(width) {
  if (width <= 1000) {
    $(".sidebar").addClass("fixed");
    $(".sidebar").hide();
    $("#openbtn").show();
    $("#closebtn").show();
  } else {
    $(".sidebar").show();
    $(".sidebar").removeClass("fixed");
    $("#openbtn").hide();
    $("#closebtn").hide();
  }
}

$(document).ready(function () {
  var $width = $(window).width();
  $(window).resize(function () {
    $width = $(window).width();
    updateContainer($width);
  });

  if ($width <= 1000) {
    closeNavbar();
  } else {
    $("#openbtn").hide();
    $("#closebtn").hide();
  }

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
    pagingType: "simple_numbers",
    language: {
      paginate: {
        next: '<img src="assets/images/polygon-2-copy-5.png" />',
        previous: '<img src="assets/images/polygon-1-copy-5.png" />',
      },
    },
  });
});

var rotation = 0;
$(".sidebar-links").click(function () {
  $(".sidebar-links").removeClass("active-link");
  $(this).addClass("active-link");
  var element = $(this).children().eq(0);
  var currentRotation = getRotationDegrees(element);
  if (currentRotation == 90) {
    rotation = 0;
  } else {
    rotation = 90;
  }
  element.css({
    "-webkit-transform": "rotate(" + rotation + "deg)",
    "-moz-transform": "rotate(" + rotation + "deg)",
    "-ms-transform": "rotate(" + rotation + "deg)",
    transform: "rotate(" + rotation + "deg)",
  });
  var $width = $(window).width();
  if ($width <= 1000) {
    if (element == null) {
      closeNavbar();
    }
  }
});

$(".submenu-links").click(function () {
  var $width = $(window).width();

  if ($width <= 1000) {
    closeNavbar();
  }
});

function getRotationDegrees(obj) {
  var matrix =
    obj.css("-webkit-transform") ||
    obj.css("-moz-transform") ||
    obj.css("-ms-transform") ||
    obj.css("-o-transform") ||
    obj.css("transform");
  if (matrix !== "none") {
    var values = matrix.split("(")[1].split(")")[0].split(",");
    var a = values[0];
    var b = values[1];
    var angle = Math.round(Math.atan2(b, a) * (180 / Math.PI));
  } else {
    var angle = 0;
  }
  return angle < 0 ? angle + 360 : angle;
}

document.addEventListener("DOMContentLoaded", function () {
  document.querySelectorAll(".sidebar a").forEach(function (element) {
    element.addEventListener("click", function (e) {
      let nextEl = element.nextElementSibling;
      let parentEl = element.parentElement;

      if (nextEl) {
        e.preventDefault();
        let mycollapse = new bootstrap.Collapse(nextEl);

        if (nextEl.classList.contains("show")) {
          mycollapse.hide();
        } else {
          mycollapse.show();
          // find other submenus with class=show
          var opened_submenu =
            parentEl.parentElement.querySelector(".submenu.show");
          // if it exists, then close all of them
          if (opened_submenu) {
            new bootstrap.Collapse(opened_submenu);
          }
        }
      }
    }); // addEventListener
  }); // forEach
});
