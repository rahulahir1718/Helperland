$('#myCheckbox').change(function () {
    $('.btn-primary').prop("disabled", !this.checked);
}).change()