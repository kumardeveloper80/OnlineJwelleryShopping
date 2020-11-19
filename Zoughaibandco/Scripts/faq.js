$(document).ready(function(){
		
	$(".faqRightDiv h5").click(function(){
		if($(this).parent().hasClass("openedFAQ"))
		{
			$(this).parent().removeClass("openedFAQ")
			$(this).parent().find(".faqArrow").removeClass("faqArrowOpened");
			$(this).parent().find("article").stop().clearQueue().animate({"height":0+"px"})
		}
		else
		{
			$(this).parent().addClass("openedFAQ")
			$(this).parent().find(".faqArrow").addClass("faqArrowOpened");
			var neededHeight = $(this).parent().find("article").attr("height");
			$(this).parent().find("article").stop().clearQueue().animate({"height":neededHeight+"px"})
		}
	});
	
	$(".faqLeft h4").click(function(){
		$(".faqLeft h4").removeClass("activeFAQ")
		$(this).addClass("activeFAQ");
		var thisIndex = $(this).index();
		
		$(".allFAQWrapper>div").css({"display":"none"})
		
		setTimeout(function(){
			$(".faqRightDiv").removeClass("openedFAQ");
			$(".faqArrow").removeClass("faqArrowOpened");
			$(".faqRightDiv article").css({"height":"auto"});
			$(".allFAQWrapper>div").eq(thisIndex).css({"display":"block"})

			$(".faqRightDiv article").each(function(){
				var thisHeight = $(this).height();
				$(this).attr("height",thisHeight);
				$(this).css({"height":"0px"})
			});
		},400)
	})
	
	$(".faqLeft>h4:first-child").trigger("click")
	
});
