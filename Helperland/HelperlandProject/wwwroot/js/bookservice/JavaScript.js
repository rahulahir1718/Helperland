$(".round-border").click(function () {
    alert("click");
    $(this).addClass("active-service");
    var img = $(this).children().eq(0);
    var imgPath = img.attr("src").split(".");
    img.attr("src", imgPath[0] + "-green.png");
});