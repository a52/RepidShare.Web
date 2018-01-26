// Remove HasError on Focus

//****** DOCUMENT READY FUNCTION ******//
$(document).ready(function(){

	loginDropdown();
	MyStandingTabs();
});

$(document).mouseup(function(){
	$('.user-menu').fadeOut();
	$('.icon-arrow').removeClass('selected');
	}); 
$('.user-menu').mouseup(function(){
		return false;
	}); 
	
function loginDropdown(){
$('.loginDropdown').bind('click',function(){
	if($('.user-menu').is(':visible')){
		$('.user-menu').fadeOut();
		$(this).removeClass('selected');
		}
	else{
		$('.user-menu').fadeIn();
		$(this).addClass('selected');
		}	
	});
} 



//*END*//
function MyStandingTabs() {
	$('.tab-view > a').click(function () {
		var contentRel = $(this).attr('data-rel');
		if(!$('#' + contentRel).is(':visible')){
			$('.tab-content').hide();
			$('.tab-view > a').removeClass('active');
			$(this).addClass('active');
			$('#' + contentRel).fadeIn();
			$(this).parent().prev('.tab-text').text($(this).text());
		}
	});
	$('.tab-view > a').each(function() {
        if($(this).hasClass('active')){
			$(this).parent().prev('.tab-text').text($(this).text());
		}
    });
}


// For Date Picker
function DatePickerFn(){
	$(".date").datepicker({
		changeMonth: true,
		changeYear: true
	});	
	$(document).on("click", function(e) {
    var elem = $(e.target);
    if(!elem.hasClass("hasDatepicker") && 
        !elem.hasClass("ui-datepicker") && 
        !elem.hasClass("ui-icon") && 
        !elem.hasClass("ui-datepicker-next") && 
        !elem.hasClass("ui-datepicker-prev") && 
        !$(elem).parents(".ui-datepicker").length){
            $('.hasDatepicker').datepicker('hide');
    }
});
}
