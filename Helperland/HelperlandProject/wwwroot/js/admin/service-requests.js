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
  $(".sidebar button").hide();
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
  $(".sidebar button").show();
  $("#openbtn").hide();
}

$(document).ready(function () {
  findParameters();
  $(".sidebar ul")
    .children()
    .eq(1)
    .children()
    .eq(0)
    .addClass("active-sidebar-link");
});

$(document).on("click", ".serviceId", function () {
  if ($(this).text() != "Service ID") {
    $.ajax({
      url: "/admin/servicerequestdetail",
      type: "GET",
      data: {
        id: $(this).text(),
      },
      success: function (result) {
        $("#adminModal").html(result);
        $("#adminModal").modal("show");
      },
      error: function () {
        alert("error");
      },
    });
  }
});

function findParameters() {
  var table = $("#table_id").DataTable({
    info: false,
    processing: true,
    responsive: true,
    serverSide: true,
    ajax: {
      url: "/admin/filterdata",
      type: "POST",
      datatype: "json",
    },
    columnDefs: [
      {
        targets: 0,
        name: "ServiceRequestId",
        className: "serviceId",
        render: function (data, type, row, meta) {
          return row.serviceRequestId;
        },
      },
      {
        targets: 1,
        name: "ServiceStartDate",
        render: function (data, type, row, meta) {
          return getDateColumnData(row);
        },
      },
      {
        targets: 2,
        name: "CustomerName",
        render: function (data, type, row, meta) {
          return getCustomerColumnData(row);
        },
      },
      {
        targets: 3,
        name: "SPName",
        render: function (data, type, row, meta) {
          return getSPColumnData(row);
        },
        defaultContent: " ",
      },
      {
        targets: [4, 5],
        orderable: false,
        render: function (data, type, row, meta) {
          return row.totalAmount + "&euro;";
        },
      },
      {
        targets: 6,
        orderable: false,
        render: function (data, type, row, meta) {
          return " ";
        },
      },
      {
        targets: 7,
        orderable: false,
        render: function (data, type, row, meta) {
          return getStatusColumnData(row);
        },
      },
      {
        targets: 8,
        orderable: false,
        render: function (data, type, row, meta) {
          return '<span class="payment-status">Done</span>';
        },
      },
      {
        targets: 9,
        orderable: false,
        render: function (data, type, row, meta) {
          return getActionColumnData(row);
        },
      },
    ],
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
        next: '<img src="/images/polygon-2-copy-5.png" />',
        previous: '<img src="/images/polygon-1-copy-5.png" />',
      },
    },
  });

  $("#table_id_filter").hide();

  oTable = $("#table_id").DataTable();

  oTable.on("draw", function () {
    $(".rateit").rateit();
  });

  $.fn.dataTable.ext.errMode = "none";
  oTable.on("error.dt", function (e, settings, techNote, message) {
    console.log("An error occurred: ", message);
  });
  $("#searchBtn").click(function () {
    oTable.search([
      $("#serviceId").val(),
      $("#postalCode").val(),
      $("#email").val(),
      $("#customerName").val(),
      $("#spName").val(),
      $("#status").val(),
      $("#hasIssue").prop("checked"),
      $("#fromDate").val(),
      $("#toDate").val(),
    ]);
    oTable.draw();
  });

  $("#clearBtn").click(function () {
    $("input,#status").val("");
    $("#hasIssue").removeProp("checked");
    $("#searchBtn").trigger("click");
  });

  $("tbody tr").click(function () {
    alert("hello");
  });
}

function getDateColumnData(row) {
  return (
    '<img src="/images/calendar2.png" alt="" /><span id = "date">' +
    " " +
    row.serviceDate +
    '</span ><br /><img src="/images/layer-14.png" alt="" />' +
    " " +
    row.serviceTime
  );
}

function getCustomerColumnData(row) {
  return (
    '<div class="d-flex align-items-center"><img src="/images/layer-15.png"/><span>' +
    row.customerName +
    " <br/>" +
    row.customerAddress +
    "</span></div>"
  );
}

function getSPColumnData(row) {
  if (row.spAvtar == null) {
    return " ";
  } else {
    return (
      '<div class="d-flex align-items-center">' +
      '<img id="profileImg" src="' +
      row.spAvtar +
      '" />' +
      '<div class="d-flex flex-column users-rating">' +
      "<span>" +
      row.spName +
      "</span>" +
      '<div class="d-flex align-items-center">' +
      "<div class='rateit' data-rateit-mode='font' style='font-size:20px' data-rateit-readonly='true' data-rateit-value=" +
      row.spRating +
      "></div>" +
      "<span>" +
      row.spRating +
      "</span>"
    );
    "</div>" + "</div>" + "</div>";
  }
}

function getStatusColumnData(row) {
  return '<span class="' + row.status + '">' + row.status + "</span>";
}

function getActionColumnData(row) {
  var actionString = "";
  if (row.status == "New" || row.status == "Accepted") {
    actionString =
      '<li><a class="dropdown-item editLink" onclick="onEditLinkClick(' +
      row.serviceRequestId +
      ')">Edit & Reschedule</a></li> <li><a class="dropdown-item cancelLink" onclick="onCancelLinkClick(' +
      row.serviceRequestId +
      ')" >Cancel SR by Cust</a></li>';
  } else {
    actionString =
      '<li><a class="dropdown-item editLink disabled">Edit & Reschedule</a></li> <li><a class="dropdown-item cancelLink disabled">Cancel SR by Cust</a></li>';
  }
  return (
    '<div class="dropdown">' +
    '<a class="btn threedots" href="#" role="button" id="dropdownMenuLink" data-bs-toggle="dropdown" aria-expanded="false">' +
    '<img src="/images/group-38.png"/>' +
    "</a>" +
    '<ul class="dropdown-menu" aria-labelledby="dropdownMenuLink">' +
    actionString +
    "</ul>" +
    "</div>"
  );
}

function onEditLinkClick(id) {
  $.ajax({
    url: "/admin/editrequest",
    type: "GET",
    data: {
      id: id,
    },
    success: function (result) {
      $("#adminModal").html(result);
      $("#adminModal").modal("show");
    },
    error: function () {
      alert("error");
    },
  });
}

function onCancelLinkClick(id) {
  $.ajax({
    url: "/admin/cancelservicerequest",
    type: "GET",
    success: function (result) {
      $("#adminModal").html(result);
      $("#adminModal").modal("show");
      $("#cancelRequestBtn").attr("disabled", true);
      cancelDialogEvents();
      $("#cancelRequestBtn").click(function () {
        cancelRequestPost(id);
      });
    },
    error: function () {
      alert("error");
    },
  });
}

function cancelDialogEvents() {
  $(".cancel-request textarea").on("keyup", function () {
    var textarea_value = $(".cancel-request textarea").val();
    if (textarea_value != "") {
      $("#cancelRequestBtn").attr("disabled", false);
    } else {
      $("#cancelRequestBtn").attr("disabled", true);
    }
  });
}

function cancelRequestPost(serviceId) {
  $.ajax({
    url: "/admin/cancelservicerequest",
    type: "POST",
    data: {
      id: serviceId,
      comment: $(".cancel-request textarea").val(),
    },
    success: function (result) {
      $("#editModal").html(result);
      $("#editModal").modal("show");
    },
    error: function () {
      alert("error");
    },
  });
}
