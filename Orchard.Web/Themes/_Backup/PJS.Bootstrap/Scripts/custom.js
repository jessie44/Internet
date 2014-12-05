$(function () {
    // TO TOP BUTTON
    $(window).scroll(function () {
        if ($(this).scrollTop() != 0) {
            $("#toTop").fadeIn()
        }
        else {
            $("#toTop").fadeOut()
        }
    });

    $("#toTop").click(function () {
        $("body,html").animate({ scrollTop: 0 }, 2e3)
    })

    // FORM VALIDATION CLASSES
    $('.input-validation-error').parent().addClass('has-error');
    $('.validation-summary-errors').addClass('has-error');
    $('.validation-summary-errors span').addClass('control-label');
    $('.validation-summary-errors li').addClass('control-label');
    $('.field-validation-error').addClass('control-label');

    // PAGINATION
    $('#pagination ul').removeClass('pager').addClass('pagination');
});