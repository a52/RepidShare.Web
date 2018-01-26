$(document).ready(function () {
    //Sidebar Nav
    $('.page-sidebar-menu li > a').click(function () {

        
        if ($(this).parent('li').hasClass('open')) {
            $(this).parent('li').removeClass('open');
            $(this).parent('li').find('ul').slideUp();
        }
        else {
            $(this).parent('li').addClass('open');
            //$(this).addClass('active');
            $(this).parent('li').children('ul').slideDown();
            $(this).parent('li').siblings('li').removeClass('open');
            $(this).parent('li').siblings('li').find('li').removeClass('open');
            $(this).parent('li').siblings('li').find('ul').slideUp();
        }
    });

    $(".table-selector tr:even").addClass("even");
    $("select, input[type='file'], input[type='checkbox'], input[type='radio']").uniform({
        fileButtonHtml: "Browse"
    });

    /*Responsive Menu*/
    $('#menu-toggle').click(function (e) {
        $('body').toggleClass('active');
        e.preventDefault();
    });


    //Mega Menu
    $('.mega-menu > ul > li').hover(function (e) {
        e.preventDefault();
        $(this).addClass('expand');
        $(' .nav_overlay').show();
        //$('.scrollBar').jScrollPane();
    }, function (e) {
        e.preventDefault();
        $(this).removeClass('expand');
        $('.nav_overlay').hide();
    });

    // Scrollbar
    $('.scrollBar').jScrollPane({
        autoReinitialise: true
    });

    $('.desktop-table-responsive').jScrollPane({
        autoReinitialise: true
    });


    //Greater than 900
    if ($(window).width() > 900) {
        // Sidebar Calculation 
        function calculateHeight() {
            var DocHeight = $('.page-content-body').outerHeight();
            var WinHeight = $(window).height() - 60;
            if (DocHeight > WinHeight) {
                $('.page-sidebar-wrapper').animate().css({ 'height': DocHeight });
            } else {
                $('.page-sidebar-wrapper').animate().css({ 'height': WinHeight });
            }
        }
        calculateHeight();
        $(window).on('resize', calculateHeight);
        $(window).on('scroll', calculateHeight);
    }


    //Less than 899
    if ($(window).width() < 899) {
        //Sidebar Scrollbar
        $('.page-sidebar').jScrollPane({
            autoReinitialise: true
        });
    }

});

function resetCSS() {

    //$(".scroll-cal-item").niceScroll().hide();

}

function initializeCSS() {
    $(".table-selector tr:even").addClass("even");

    $("select, input[type='file'], input[type='checkbox'], input[type='radio']").uniform({
        fileButtonHtml: "Browse"
    });

    $(".upl input[type='file']").uniform({
        fileButtonHtml: " "
    });



    $("[title]").tooltip();







    /*niceScroll*/




    
    
}
