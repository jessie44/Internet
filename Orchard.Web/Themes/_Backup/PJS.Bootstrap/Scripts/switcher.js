$(function () {
    if (supports_html5_storage) {
        if (sessionStorage.PJSSwatch) {
            changeSwatch();
        }

        if (sessionStorage.PJSFluidLayout) {
            changeFluidLayout();
        }

        if (sessionStorage.PJSInverseNav) {
            changeInverseNav();
        }

        if (sessionStorage.PJSNavSearch) {
            changeNavSearch();
        }
        else {
            $('#navsearch').prop('checked', true);
        }

        if (sessionStorage.PJSHoverMenus) {
            changeHoverMenus();
        }
    }

    $('#style-switcher h2 a').click(function (e) {
        e.preventDefault();
        var div = $('#style-switcher');
        if (div.css('left') === '-206px') {
            $('#style-switcher').animate({
                left: '0px'
            });
        } else {
            $('#style-switcher').animate({
                left: '-206px'
            });
        }
    });

    $('#swatches').change(function (e) {
        if (supports_html5_storage) {
            sessionStorage.PJSSwatch = $(this).val();

            changeSwatch();
        }
        else {
            alert('Your browser does not support HTML5 Session Storage');
        }
    });

    $('#fluidlayout').change(function () {
        if (supports_html5_storage) {
            sessionStorage.PJSFluidLayout = $(this).is(':checked');

            changeFluidLayout();
        }
        else {
            alert('Your browser does not support HTML5 Session Storage');
        }
    });

    $('#inversenav').change(function () {
        if (supports_html5_storage) {
            sessionStorage.PJSInverseNav = $(this).is(':checked');

            changeInverseNav();
        }
        else {
            alert('Your browser does not support HTML5 Session Storage');
        }
    });

    $('#navsearch').change(function () {
        if (supports_html5_storage) {
            sessionStorage.PJSNavSearch = $(this).is(':checked');

            changeNavSearch();
        }
        else {
            alert('Your browser does not support HTML5 Session Storage');
        }
    });

    $('#hovermenus').change(function () {
        if (supports_html5_storage) {
            sessionStorage.PJSHoverMenus = $(this).is(':checked');

            changeHoverMenus();

            location.reload();
        }
        else {
            alert('Your browser does not support HTML5 Session Storage');
        }
    });
});

function supports_html5_storage() {
    try {
        return 'localStorage' in window && window['localStorage'] !== null;
    }
    catch (e) {
        return false;
    }
}

function changeSwatch() {
    setTimeout(function () {
        $('link[rel=stylesheet][href="/Themes/PJS.Bootstrap/Styles/bootstrap.min.css"]').remove();
        $('link[rel=stylesheet][href="/Themes/PJS.Bootstrap/Styles/amelia.min.css"]').remove();
        $('link[rel=stylesheet][href="/Themes/PJS.Bootstrap/Styles/cerulean.min.css"]').remove();
        $('link[rel=stylesheet][href="/Themes/PJS.Bootstrap/Styles/cosmo.min.css"]').remove();
        $('link[rel=stylesheet][href="/Themes/PJS.Bootstrap/Styles/cyborg.min.css"]').remove();
        $('link[rel=stylesheet][href="/Themes/PJS.Bootstrap/Styles/flatly.min.css"]').remove();
        $('link[rel=stylesheet][href="/Themes/PJS.Bootstrap/Styles/journal.min.css"]').remove();
        $('link[rel=stylesheet][href="/Themes/PJS.Bootstrap/Styles/readable.min.css"]').remove();
        $('link[rel=stylesheet][href="/Themes/PJS.Bootstrap/Styles/simplex.min.css"]').remove();
        $('link[rel=stylesheet][href="/Themes/PJS.Bootstrap/Styles/slate.min.css"]').remove();
        $('link[rel=stylesheet][href="/Themes/PJS.Bootstrap/Styles/spacelab.min.css"]').remove();
        $('link[rel=stylesheet][href="/Themes/PJS.Bootstrap/Styles/united.min.css"]').remove();
        $('link[rel=stylesheet][href="/Themes/PJS.Bootstrap/Styles/yeti.min.css"]').remove();

        $('html').removeClass('swatch-bootstrap');
        $('html').removeClass('swatch-amelia');
        $('html').removeClass('swatch-cerulean');
        $('html').removeClass('swatch-cosmo');
        $('html').removeClass('swatch-cyborg');
        $('html').removeClass('swatch-flatly');
        $('html').removeClass('swatch-journal');
        $('html').removeClass('swatch-readable');
        $('html').removeClass('swatch-simplex');
        $('html').removeClass('swatch-slate');
        $('html').removeClass('swatch-spacelab');
        $('html').removeClass('swatch-united');
        $('html').removeClass('swatch-yeti');

        var fileref = document.createElement("link");
        fileref.setAttribute("rel", "stylesheet");
        fileref.setAttribute("type", "text/css");
        fileref.setAttribute("href", "/Themes/PJS.Bootstrap/Styles/" + sessionStorage.PJSSwatch + ".min.css");
        document.getElementsByTagName("head")[0].appendChild(fileref);

        $('html').addClass('swatch-' + sessionStorage.PJSSwatch);

        $('#swatches').val(sessionStorage.PJSSwatch);
    }, 500);
}

function changeFluidLayout() {
    if (sessionStorage.PJSFluidLayout == 'true') {
        $('.container').removeClass('container').addClass('fluid-layout');
        $('#fluidlayout').prop('checked', true);
    }
    else {
        $('.fluid-layout').removeClass('fluid-layout').addClass('container');
        $('#fluidlayout').prop('checked', false);
    }
}

function changeInverseNav() {
    if (sessionStorage.PJSInverseNav == 'true') {
        $('.navbar-fixed-top').removeClass('navbar-default').addClass('navbar-inverse');
        $('#inversenav').prop('checked', true);
    }
    else {
        $('.navbar-fixed-top').removeClass('navbar-inverse').addClass('navbar-default');
        $('#inversenav').prop('checked', false);
    }
}

function changeNavSearch() {
    if (sessionStorage.PJSNavSearch == 'true') {
        $('.navbar-form').toggle(true);
        $('#navsearch').prop('checked', true);
    }
    else {
        $('.navbar-form').toggle(false);
        $('#navsearch').prop('checked', false);
    }
}

function changeHoverMenus() {
    if (sessionStorage.PJSHoverMenus == 'true') {
        var fileref = document.createElement('script')
        fileref.setAttribute("type", "text/javascript")
        fileref.setAttribute("src", "/Themes/PJS.Bootstrap/Scripts/hovermenus.js")
        document.getElementsByTagName("body")[0].appendChild(fileref)

        $('#hovermenus').prop('checked', true);
    }
    else {
        $("script[src='/Themes/PJS.Bootstrap/Scripts/hovermenus.js']").remove()

        $('#hovermenus').prop('checked', false);
    }

}