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
  $("#openbtn").show();
  $(".sidebar button").hide();
  $(".sidebar").hide();
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
    .eq(2)
    .children()
    .eq(0)
    .addClass("active-sidebar-link");
});

function findParameters() {
  var table = $("#table_id").DataTable({
    info: false,
    buttons: ["excelHtml5"],
    processing: true,
    responsive: true,
    serverSide: true,
    ajax: {
      url: "/admin/usermanagementdata",
      type: "POST",
      datatype: "json",
    },
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
    columnDefs: [
      {
        targets: 0,
        name: "User Name",
        render: function (data, type, row, meta) {
          return row.firstName + " " + row.lastName;
        },
      },
      {
        targets: 1,
        orderable: false,
      },
      {
        targets: 2,
        name: "Registration Date",
        className: "createdDate",
        render: function (data, type, row, meta) {
          return getDateColumnData(row);
        },
      },
      {
        targets: 3,
        orderable: false,
        render: function (data, type, row, meta) {
          return getUserTypeColumnData(row);
        },
      },
      {
        targets: 4,
        name: "Phone",
        render: function (data, type, row, meta) {
          return row.mobile;
        },
      },
      {
        targets: 5,
        orderable: false,
        render: function (data, type, row, meta) {
          return row.zipCode;
        },
      },
      {
        targets: 6,
        orderable: false,
        render: function (data, type, row, meta) {
          return getStatusColumnData(row);
        },
      },
      {
        targets: 7,
        orderable: false,
        render: function (data, type, row, meta) {
          return getActionColumnData(row);
        },
      },
    ],
  });
  $(".buttons-excel").hide();
  $("#table_id_filter").hide();
  $.fn.dataTable.ext.errMode = "none";
  $("#table_id").on("error.dt", function (e, settings, techNote, message) {
    console.log("An error occurred: ", message);
  });
  oTable = $("#table_id").DataTable();
  $("#searchBtn").click(function () {
    oTable.search([
      $("#userName").val(),
      $("#userType").val(),
      $("#phoneNumber").val(),
      $("#postalCode").val(),
      $("#email").val(),
      $("#fromDate").val(),
      $("#toDate").val(),
    ]);
    oTable.draw();
  });

  $("#clearBtn").click(function () {
    $("input,#userType").val("");
    $("#searchBtn").trigger("click");
  });

  $("#export").on("click", function () {
    $(".buttons-excel").trigger("click");
  });
}

function getDateColumnData(row) {
  var date = row.createdDate.split("T");
  var array = date[0].split("-");
  return (
    '<img src="/images/calendar2.png"/>' +
    " " +
    array[2] +
    "/" +
    array[1] +
    "/" +
    array[0]
  );
}

function getUserTypeColumnData(row) {
  var userType = "";
  if (row.userTypeId == 1) {
    userType = "Customer";
  } else if (row.userTypeId == 2) {
    userType = "Service Provider";
  } else if (row.userTypeId == 3) {
    userType = "Admin";
  }
  return "<span>" + userType + "</span>";
}

function getStatusColumnData(row) {
  if (row.isActive) {
    return '<span class="active">Active</span>';
  } else {
    return '<span class="inactive">Inactive</span>';
  }
}

function getActionColumnData(row) {
  var deactiveOprion = "";
  if (row.isActive) {
    deactiveOprion =
      '<a class="dropdown-item" onclick="onDeactivateLinkClick(' +
      row.userId +
      ')">Deactivate</a>';
  } else {
    deactiveOprion =
      '<a class="dropdown-item" onclick="onActivateLinkClick(' +
      row.userId +
      ')">Activate</a>';
  }

  return (
    '<div class="dropdown">' +
    '<a class="btn threedots" role="button" id="dropdownMenuLink" data-bs-toggle="dropdown" aria-expanded="false">' +
    '<img src="/images/group-38.png"/>' +
    "</a>" +
    '<ul class="dropdown-menu" aria-labelledby="dropdownMenuLink">' +
    '<li><a class="dropdown-item" onclick="onDeleteLinkClick(' +
    row.userId +
    ')">Delete</a></li>' +
    deactiveOprion +
    "</ul >" +
    "</div >"
  );
}

function onDeactivateLinkClick(userId) {
  $.ajax({
    url: "/admin/deactivate",
    type: "GET",
    data: {
      id: userId,
    },
    success: function (result) {
      window.location.href = "/admin/usermanagement";
    },
    error: function () {
      alert("error");
    },
  });
}

function onActivateLinkClick(userId) {
  $.ajax({
    url: "/admin/activate",
    type: "GET",
    data: {
      id: userId,
    },
    success: function (result) {
      window.location.href = "/admin/usermanagement";
    },
    error: function () {
      alert("error");
    },
  });
}

function onDeleteLinkClick(userId) {
  $.ajax({
    url: "/admin/delete",
    type: "GET",
    data: {
      id: userId,
    },
    success: function (result) {
      window.location.href = "/admin/usermanagement";
    },
    error: function () {
      alert("error");
    },
  });
}
