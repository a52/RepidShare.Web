$(document).ready(function(){
	
	//handle global nav
	 	
	var rotate = 0;
	$("#header_menu_link, .header_arrow a").click(function(){
		rotate += 180;
	    $(".header_arrow a").rotate(rotate);
		$(".header_full_menu").slideToggle();
	});
	
	var rotation = 0;

	jQuery.fn.rotate = function(degrees) {
	    $(this).css({'-webkit-transform' : 'rotate('+ degrees +'deg)',
	                 '-moz-transform' : 'rotate('+ degrees +'deg)',
	                 '-ms-transform' : 'rotate('+ degrees +'deg)',
	                 'transform' : 'rotate('+ degrees +'deg)'});
	    return $(this);
	};
	

	$("#header_search_link").click(function() {
		$(".header_search").slideToggle();
	});
	
	//mobile menu
	$("#nav_mobile_menu_trigger").click(function(){
		$(".header_full_menu").addClass("header_full_menu_show");
		$(".e_overlay").show();
	})
	
	$(".e_overlay").click(function(){
		$(".header_full_menu").removeClass("header_full_menu_show");
		setTimeout(function() {
			$(".e_overlay").hide();
		}, 300);
	});
	
	//swipey
	ele.touch.addSwipeListener(document.body, function(swipeDirection){
		if (swipeDirection == "right") {
			$("#nav_mobile_menu_trigger").click();
			
		} else if (swipeDirection == "left") {
			$(".e_overlay").click();
		} 
		
	});
	
	//search box

	$("#searchForm a.button").click(function() {
		$("#searchForm").submit();
	});

	$("#searchFormHeader a.button").click(function() {
		$("#searchFormHeader").submit();
	});
	
	
	//business/personal home accordion
	
	$(".accordion h2 a").on("click", function(){

		var topDocs = $(this).parent().siblings(".topdocs");
		topDocs.toggleClass("hidden");
		
		if ($(this).children(".fa-chevron-circle-down").length > 0) {
			$(this).children(".fa-chevron-circle-down").addClass("fa-chevron-circle-right").removeClass("fa-chevron-circle-down");
		} else {
			$(this).children(".fa-chevron-circle-right").addClass("fa-chevron-circle-down").removeClass("fa-chevron-circle-right");
		}
		
	});
	
		
	//lawguide moving stuff
	
	if ($("#e_page.ele-article").length > 0) {

		$("#e_relatedServices").prependTo("#relatedContainer");
		$(".toc").prependTo("#tocContainer");
		
	}
	
	
	//floating navigation on subsites
	var $sections = $('.e_body_subsite .e_productCategories');
	var $sectionNav = $('.e_body_subsite .ele-subsiteCats');
	$(window).scroll(function(){
		
		if ($( window ).width() >= 880) {
		
			//scroll the floating panels
			$(".e_body_subsite #jurisdictionSelectorWrapper").css({"margin-top": Math.max(0, $(window).scrollTop()-300) + "px"});
			$(".e_body_subsite .ele-subsiteCats").css({"margin-top": Math.max(0, $(window).scrollTop()-300) + "px"});
			
			// currentScroll is the number of pixels the window has been scrolled
			var currentScroll = $(this).scrollTop();
				    
			// $currentSection is somewhere to place the section we must be looking at
			var $currentSection;
				    
			// We check the position of each of the divs compared to the windows scroll positon
			$sections.each(function(){
				// divPosition is the position down the page in px of the current section we are testing      
				var divPosition = $(this).offset().top - 300;
			    
				// If the divPosition is less the the currentScroll position the div we are testing has moved above the window edge.
			    // the -1 is so that it includes the div 1px before the div leave the top of the window.
			    if (divPosition - 1 < currentScroll) {
			    	// We have either read the section or are currently reading the section so we'll call it our current section
			    	$currentSection = $(this);
			    }
			     
			    if ($currentSection) {
				    // Set active
				    var id = $currentSection.attr('id');
				    $sectionNav.find('a').removeClass('active');
				    $sectionNav.find("[href=#"+id+"]").addClass('active');
				}
			      
			});
		}
		
	});
	
	
	// same for lawguide
	//floating navigation on subsites
	var $sectionsLawguide = $('.ele-article h2, .ele-article h3');
	var $sectionNavLawguide = $('.ele-article .e_sidebar');
	//init
	$sectionNavLawguide.find("a").first().addClass("active");
	
	$(window).scroll(function(){
		
		if ($( window ).width() >= 880) {
			
			var scrollPos = $(this).scrollTop(),
				bodyHeight = $("body").height(),
				windowHeight = $(this).height();
			
			//there is a problem when this nav is too big. Disable scrolling when too big
	
			if ($sectionNavLawguide.height() < (windowHeight - 220)) {
					
				var lawguideSize = $sectionNavLawguide.height() - 220;
		
				//scroll the floating panels
				if (scrollPos >= (bodyHeight-windowHeight) - lawguideSize + 180) { //scrolling
					$sectionNavLawguide.css({
						"margin-top": (bodyHeight-windowHeight) - lawguideSize
					});
			    } else { //start of page - no margin-top
					$sectionNavLawguide.css({
						"margin-top": Math.max(0, scrollPos-180)
					});
			    }
						
				// currentScroll is the number of pixels the window has been scrolled
				var currentScroll = $(this).scrollTop();
					    
				// $currentSection is somewhere to place the section we must be looking at
				var $currentSection;
					    
				// We check the position of each of the divs compared to the windows scroll position
				$sectionsLawguide.each(function(){
					// divPosition is the position down the page in px of the current section we are testing      
					var divPosition = $(this).offset().top - 100;
		
					// If the divPosition is less the the currentScroll position the div we are testing has moved above the window edge.
				    // the -1 is so that it includes the div 1px before the div leave the top of the window.
				    if (divPosition - 1 < currentScroll) {
				    	// We have either read the section or are currently reading the section so we'll call it our current section
				    	$currentSection = $(this);
				    }
				     
				    if ($currentSection) {
					    // Set active
					    var id = $currentSection.attr('id');
					    $sectionNavLawguide.find('a').removeClass('active');
					    $sectionNavLawguide.find("[href=#"+id+"]").addClass('active');
					}
				      
				});
			}
		}
	});
	
	$(window).scroll();
});
