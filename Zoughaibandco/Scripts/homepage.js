$(document).ready(function(){
	
	$( window ).resize(function() {
		ResizeMyHomepage();
	});
	
	ResizeMyHomepage = function(){
		//if($(window).width() > 760)
		//	$(".homepage_banner").css({"height": ( $(window).height() - $("header").height() ) +"px"});
		//else
  //          $(".homepage_banner").css({ "height": "auto", "background-image": "none", "background-color": "#f4efec" });

		var MargBottom = Math.floor($(".safearea").width()*0.02);
		//var NewHeight = ($(window).width() * 487 )/1400;
		
		$(".hbox1,.hbox2,.hbox3,.hbox4,.hbox5,.hbox6,.fragrance_collection,.hbox7,.hbox8,.hbox9,.hbox10,.hbox11").css({"margin-bottom": MargBottom + "px"});
		//$(".hbox1,.hbox2,.hbox3,.hbox4").css({"height": NewHeight + "px"});
		var watches_collection_height = $(".hbox5").height() - $(".fragrance_collection").height() - MargBottom;
		$(".watches_collection").css({"height": watches_collection_height + "px"});
	}
	
	
	ResizeMyHomepage();
	var type_text = '“ Zoughaib &amp; Co’s master pieces are deﬁned by brilliant design and unparalleled craftsmanship ”';
	var written = false;
	
	var youtubePlayed = false;
	
	$(window).scroll(function() {
	
		
		$(".animatedItem").each(function( index ) {
			if( $(window).scrollTop() > $(this).offset().top - ( $(window).height() - 300) || $(".safearea").width() <= 360)
			{
				$(this).removeClass('animatedItem');
			}
		});
		
		$(".youtubeIframe").each(function( index ) {
			if( $(window).scrollTop() > $(this).offset().top - ( $(window).height() - 300) || $(".safearea").width() <= 360)
			{
				if(!youtubePlayed)
				{
					$(".youtubeIframe")[0].src += "&autoplay=1";
					youtubePlayed = true;
				}
			}
        });
        if ($(".hbox11_quote").length > 0) {
            if ($(window).scrollTop() > $(".hbox11_quote").offset().top - ($(window).height() - 300) && !written) {
                $(".hbox11_quote").typed({
                    strings: [type_text],
                    typeSpeed: 10
                });

                var TypeTempInterval = setInterval(function () {
                    $(".hbox11_quote_blurred").text($(".hbox11_quote").text());
                }, 150); // TempInterval

                $(".doumit_signature").removeClass('animatedSignature');

                setTimeout(function () {
                    clearInterval(TypeTempInterval);
                }, 8000);

                written = true;
            }// end if 
        }
		
		//if( $(window).scrollTop() > $(".hbox11_quote").offset().top - ( $(window).height() - 300) && !written)
		//{
		//	$(".hbox11_quote").typed({
		//				strings: [type_text],
		//				typeSpeed: 10
		//	});
			
		//	var TypeTempInterval = setInterval(function() {
		//		$(".hbox11_quote_blurred").text($(".hbox11_quote").text());
		//	}, 150); // TempInterval
			
		//	$(".doumit_signature").removeClass('animatedSignature');
			
		//	setTimeout(function() {
		//			clearInterval(TypeTempInterval);
		//	}, 8000);
					
		//	written = true;	
		//}// end if 
			
	}); // window scroll
	
	
});//$(document).ready(function()

