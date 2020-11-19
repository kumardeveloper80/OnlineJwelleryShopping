$(document).ready(function(){

	$(".closepopup").bind('touchstart click', function(){
		$(".login_popup").hide();
	});
	
	$(".search_icon").bind('touchstart click', function(){
		$(".search_popup").fadeIn(200);
	});

	$(".close_search").bind('touchstart click', function(){
		$(".search_popup").hide();
	});
	
	$(".burger_menu").bind('click', function(){
		$(".mobile_menu").show();
		$("html").css({"overflow":"hidden"});
	});	
	
	$(".mob_menu_close").bind('click', function(){
		$(".mobile_menu").hide();
		$("html").css({"overflow":"auto"});
	});	
	
	
	$(".mobile_menu_item").bind('click', function(){
		$( this ).toggleClass( "mbi_expanded" );
	});
	
	
	var TempInterval;
	//expandable submenu controls
	function ResetExpandableSubmenuPreviews(){
		
		// left first preview
	    var esm1_obj = $(".expandable_submenu").find(".expandable_submenu_preview1").children().length;
		var esm1_current = Math.floor(Math.random() * esm1_obj);
		for (var i=0;i<esm1_obj;i++)
		{
			if(i == esm1_current)
			    $(".expandable_submenu").find(".expandable_submenu_preview1 img").eq(i).css({ "opacity": "1" });
			else
			    $(".expandable_submenu").find(".expandable_submenu_preview1 img").eq(i).css({ "opacity": "0" });
		}
		
		
		//right second preview
		var esm2_obj = $(".expandable_submenu").find(".expandable_submenu_preview2").children().length;
		var esm2_current = Math.floor(Math.random() * esm2_obj);
		while( esm2_current == esm1_current){
			esm2_current = Math.floor(Math.random() * esm2_obj);
		}
		
		for (var i=0;i<esm2_obj;i++)
		{
			if(i == esm2_current)
			    $(".expandable_submenu").find(".expandable_submenu_preview2 img").eq(i).css({ "opacity": "1" });
			else
			    $(".expandable_submenu").find(".expandable_submenu_preview2 img").eq(i).css({ "opacity": "0" });
		}
		
		clearInterval(TempInterval);
		TempInterval = setInterval(function() {
			
			// left first preview
			for (var i=0;i<esm1_obj;i++)
			{
				if(i == esm1_current)
				    $(".expandable_submenu").find(".expandable_submenu_preview1 img").eq(i).css({ "opacity": "1" });
				else
				    $(".expandable_submenu").find(".expandable_submenu_preview1 img").eq(i).css({ "opacity": "0" });
			}
			esm1_current = Math.floor(Math.random() * esm1_obj);
			/*
			esm1_current++;
			if(esm1_current>=esm1_obj)
				esm1_current=0;
			*/
			
			//right second preview
			for (var i=0;i<esm2_obj;i++)
			{
				if(i == esm2_current)
				    $(".expandable_submenu").find(".expandable_submenu_preview2 img").eq(i).css({ "opacity": "1" });
				else
				    $(".expandable_submenu").find(".expandable_submenu_preview2 img").eq(i).css({ "opacity": "0" });
			}
			esm2_current = Math.floor(Math.random() * esm2_obj);
			while( esm2_current == esm1_current){
				esm2_current = Math.floor(Math.random() * esm2_obj);
			}
			/*
			esm2_current++;
			if(esm2_current>=esm2_obj)
				esm2_current=0;
			*/
			
		}, 4000); // TempInterval
	}//ResetExpandableSubmenuPreviews
	
	
	
	
	
	//open close exp submenu
	var ExpSBcurrent = -1;
	var old_ExpSBcurrent = -1;
	$(".nav_item").bind('hover', function(){
		if($(this).attr('exp_sb') != undefined)
		{
			if (ExpSBcurrent!= $(this).attr('exp_sb'))
			{
				ExpSBcurrent =  $(this).attr('exp_sb');
				$(".nav_item").removeClass('a_hover_active');
				$(this).addClass('a_hover_active');
			}
			//else
			//{
				//ExpSBcurrent = -1;
				//$(".nav_item").removeClass('a_active');
			//}
		}
	});
	
	$(".expandable_submenu").bind('mouseleave', function(){
		ExpSBcurrent = -1;
		$(".nav_item").removeClass('a_hover_active');
		clearInterval(TempInterval);
	});
	
	var TempInterval1 = setInterval(function() {
		
		if(old_ExpSBcurrent != ExpSBcurrent)
		{
			$(".expandable_submenu").css({"height":"0px"});
			if(old_ExpSBcurrent == -1)
			{
				if(ExpSBcurrent != -1)
				{	
				    //populate the menu here..
				    PopulateMenu(ExpSBcurrent);
					
					// animate the previews now
					ResetExpandableSubmenuPreviews();
					
					// expand the menu
					$(".expandable_submenu").css({"height":"240px"});
				}
			}//if(old_ExpSBcurrent == -1)
			else
			{
				// if an old menu was open, we need to wait 400ms for the old menu to close as set bel transition css 400ms
				setTimeout(function() {
					if(ExpSBcurrent != -1)
					{
						//populate the menu here..
					    PopulateMenu(ExpSBcurrent);
						
						// animate the previews now
						ResetExpandableSubmenuPreviews();
						
						// expand the menu
						$(".expandable_submenu").css({"height":"240px"});
					}
				}, 400);

			}
			
			old_ExpSBcurrent = ExpSBcurrent;
		}//if(old_ExpSBcurrent != ExpSBcurrent)
		
	}, 10); // TempInterval1
	
	/*
	$( ".homepage_venue_item" ).mouseenter(function() {
	});
	
	$( ".homepage_venue_item" ).mouseleave(function() {
		var tw = 1; // original value is 1
		var th = 1; // original value is 1
		$(this).find(".homepage_scalable_venue_image").css({
			   "-webkit-transform":"scale(" + tw + "," + th+ ")",
			   "transform":"scale(" + tw + "," + th+ ")",
			   "-ms-transform":"scale(" + tw + "," + th+ ")",
			   "-moz-transform":"scale(" + tw + "," + th+ ")",
			   "-o-transform":"scale(" + tw + "," + th+ ")"
		 });
	});
	
	
	// if they click anywhere else close month menu for homepage events
	$(document).click(function(e){
		if( $(e.target).attr('class') != "select_month" && month_menu)
		{
			$(".select_month").css({"display":"block"});
			$(".select_month_active").css({"display":"none"});
			month_menu = false;
		}
	});
	
	$( ".menu_ul li" ).mouseenter(function() {
		$(this).find(".blue_line_menu").css({"width":"100%"});
		
	});	
	$( ".menu_ul li" ).mouseleave(function() {
		$(this).find(".blue_line_menu").css({"width":"0%"});
	});
	
	
	var routeUrl = location.protocol + '//' + location.host;
	$(".about_menu").bind('touchstart click', function(){
		//window.location = routeUrl + "/index.html#about"
		$("html, body").animate({ scrollTop: $(".homepage_about_section").position().top }, 400);
		CloseFixedMenu();
	});
	
	setTimeout(function() {
		var hash = window.location.hash;
		hash = hash.substring(1, hash.length);
		
		if(hash == "about")
			$("html, body").animate({ scrollTop: $(".homepage_about_section").position().top }, 400);
	}, 200);
	
	$( window ).resize(function() {
	});
	
	var min = 0;
	var max = 15;
	var landing_count = 0;
	var TempInterval = setInterval(function() {
		var random = Math.floor(Math.random() * (max - min + 1)) + min;
		if( $(".landing_letter_item").eq(random).hasClass("yellow") )
			$(".landing_letter_item").eq(random).removeClass("yellow");
		else
			$(".landing_letter_item").eq(random).addClass("yellow");
			
		landing_count++;
		if(landing_count > 30)	
		{
			clearInterval(TempInterval);
			$(".landing_letter_item").removeClass("yellow");
			$(".landing_letter_item").eq(0).addClass("yellow");
			$(".landing_letter_item").eq(10).addClass("yellow");	
		}
		
	}, 150); // TempInterval
	
	
	
	
	
	*/
	
	$(window).scroll(function() {
		
		$(".anim_opacity0").each(function( index ) {
			if( $(window).scrollTop() > $(this).offset().top - ( $(window).height() - 300) || $(".safearea").width() <= 360)
            {
				$(this).removeClass('anim_opacity0');
			}
		});
			
	}); // window scroll	
	
	
    fbshareCurrentPage = function(location)
    {
      
		window.open("https://www.facebook.com/sharer/sharer.php?u="+escape(location));
		return false; 
	}
	
    TweetCurrentPage = function(tweet)
    {
		window.open("https://twitter.com/home?status="+escape(tweet));
		return false; 
	}
	
	LinkedInShareCurrentPage = function(location)
    {
		window.open("http://www.linkedin.com/shareArticle?mini=true&url="+escape(location));
		return false; 
	}
	
    PinterestShareCurrentPage = function(location)
    {
		window.open("http://pinterest.com/pin/create/button/?url="+escape(location));
		return false; 
	}
	
	WhatsappCurrentPage = function(Whatsapp)
    {
		window.open("whatsapp://send?text="+escape(Whatsapp));
		return false; 
	}
	
});//$(document).ready(function()

function PopulateMenu(menuID)
{
    if (menuID == "0") {
        $(".expandable_submenu").html($(".JewelleryContent").html());
    }
    else if (menuID == "1") {
        $(".expandable_submenu").html($(".BridalContent").html());
    }
    else if (menuID == "2") {
        $(".expandable_submenu").html($(".CollectionsContent").html());
    }
    else if (menuID == "3") {
        $(".expandable_submenu").html($(".WatchesContent").html());
    }
    else if (menuID == "4") {
        $(".expandable_submenu").html($(".LeatherGoodsContent").html());
    }
}





function checkLength(maxNumberOfLetters, idToCheck) {
    var fieldLength = document.getElementById(idToCheck).value.length;
    //Suppose u want 4 number of character
    if (fieldLength <= maxNumberOfLetters) {
        return true;
    }
    else {
        var str = document.getElementById(idToCheck).value;
        str = str.substring(0, str.length - 1);
        document.getElementById(idToCheck).value = str;
    }
}


//////////////////////////////////// Allow only numeric ///////////////////////////////////////////////
// Numeric only control handler
jQuery.fn.ForceNumericOnly =
function () {
    return this.each(function () {
        $(this).keydown(function (e) {
            var key = e.charCode || e.keyCode || 0;
            // allow backspace, tab, delete, arrows, numbers and keypad numbers ONLY
            return (
    key == 8 ||
    key == 9 ||
    key == 46 ||
    (key >= 37 && key <= 40) ||
    (key >= 48 && key <= 57) ||
    (key >= 96 && key <= 105));
        })
    })
};
$(document).ready(function () {
    //$("#number").ForceNumericOnly();
});
//////////////////////////////////// Allow only numeric ///////////////////////////////////////////////