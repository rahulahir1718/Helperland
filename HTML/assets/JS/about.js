alert(
  "header and footer html files will be loaded using jquery..so make sure that this page is loaded using web server like Apache or VS Code plugin Live Server because header and footer will not be shown directly on Google Chrome or any other browser due to file access constraint."
);
$("#header").load("header.html");
$("#footer").load("footer.html");
$(window).scroll(function () {
  var sticky = $("#header"),
    scroll = $(window).scrollTop();

  if (scroll > 64) sticky.addClass("fixed");
  else sticky.removeClass("fixed");
});
