var totalServieTime = 0;
var perCleaningPayment = 60;
var totalPayment = 60;

$(document).ready(function () {
   
    totalServieTime = 3;

    $(".newslater-area").hide();

    var url = "/bookservice/yourdetails";
    $.get(url, function (data) {
        $("#details-div").html(data);
        findYourDetailsElements();
    });

    $('#terms-conditions').change(function () {
        $('#paymentSubmit').prop("disabled", !this.checked);
    }).change()

   /* $("#schedule-div").change(function () {
        findServiceScheduleElements();
    });

    $("#details-div").change(function () {
        findYourDetailsElements();
    });*/

    $("#ServiceDate").change(function () {
        $("#service-date").html($("#ServiceDate").val());
        $("#per-cleaning-payment").html(perCleaningPayment+",00&euro;");
        $("#total-payment-count").html(totalPayment+",00&euro;");
    });

    $("#ServiceTime").change(function () {
        $("#service-time").html($("#ServiceTime").val());
    });
    findServiceScheduleElements();
});

function makeActive(link) {
    link.addClass("nav-item-active");
    var img = link.children().eq(0);
    var imgPath = img.attr("src").split(".");
    img.attr("src", imgPath[0] + "-white.png");
}

function hideModal() {
    $("#resultModal").modal("hide");
}

function findServiceScheduleElements() {
    $(".round-border").click(function () {
        var service = $(this).parent().children().eq(1).text();
        var children;
        switch (service) {
            case "Inside cabinets": children = 0;
                break;
            case "Inside fridge": children = 1;
                break;
            case "Inside oven": children = 2;
                break;
            case "laundry wash & dry": children = 3;
                break;
            case "Interior windows": children = 4;
                break;
        }
        
        var img = $(this).children().eq(0);
        if ($(this).hasClass("active-service")) {
            $(this).removeClass("active-service");
            var imgPath = img.attr("src").split("-green.png");
            img.attr("src", imgPath[0] + ".png");
            RemoveService(children);
        }
        else {
            $(this).addClass("active-service");
            var imgPath = img.attr("src").split(".");
            img.attr("src", imgPath[0] + "-green.png");
            AddService(service,children);
        }
        
    });
}

function AddService(service, children) {
    var html = '<span></span>' + service + '<span class="ms-auto">30 Min.</span>';
    $("#card-extra-services").children().eq(children).html(html);
    totalServieTime += 0.5;
    totalPayment += 10;
    $("#total-service-time").html(totalServieTime);
    $("#total-payment-count").html(totalPayment + ",00&euro;");
}

function RemoveService(children) {
    $("#card-extra-services").children().eq(children).empty();
    totalServieTime -= 0.5;
    totalPayment -= 10;
    $("#total-service-time").html(totalServieTime);
    $("#total-payment-count").html(totalPayment + ",00&euro;");
}


function findYourDetailsElements() {
    $("#newAddressForm").hide();
    $("#add-new-address").click(function () {
        $(this).hide();
        $("#newAddressForm").show();
    });
}

function ServiceRequestResult(isError,Message,ServiceRequestId)
{
    if (isError=="True") {
        $("#resultImage").attr("src", "/images/big-error.jpg");
    }
    else {
        $("#resultImage").attr("src", "/images/big-right.png");
        $("#serviceRequestId").html("Service Request Id: "+ServiceRequestId);
    }
    $("#resultMessage").html(Message);
    $(window).scrollTop(0);
    $("#resultModal").modal("show");
}

function addNewAddress() {
    var postalCode;
    var data = {
        AddressLine1: $("#AddressLine1").val(),
        AddressLine2: $("#AddressLine2").val(),
        PostalCode: $("#PostalCode").val(),
        City:$("#City").val(),
        Mobile:$("#Mobile").val()
    }
    $.ajax({
        type: 'POST',
        url: '/BookService/AddNewAddress',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(data),
        success: function (result) {
            var url = "/bookservice/yourdetails";
            $.get(url, function (data) {
                $("#details-div").html(data);
                findYourDetailsElements();
            });
        },
        error: function () {
            alert('Failed to receive the Data');
            console.log('Failed ');
        }
    })
}

function hideNewAddressDiv() {
    $("#newAddressForm").hide();
    $("#add-new-address").show()
}

function submitPaymentInfo() {
    var id = 0;
    var list = [];
    $('#card-extra-services > div').each(function (i) {
        id += 1;
        if ($(this).children().length > 0) {
            list.push({"ServiceExtraId":id});
        }
    });

    var data = {
        ServiceHourlyRate: 20,
        ExtraHours: totalServieTime - 3,
        SubTotal: totalPayment,
        TotalCost: totalPayment,
        PaymentDue: false,
        PaymentDone: true,
        ServiceRequestExtras: list
    }

    $.ajax({
        type: 'POST',
        url: '/bookservice/makepayment',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(data),
        success: function (result) {
            $("#payment-div").html(result);
            ServiceRequestResult(isError, resultMessage, serviceRequestId);
        },
        error: function () {
            alert('Failed to receive the Data');
            console.log('Failed ');
        }
    })
}
