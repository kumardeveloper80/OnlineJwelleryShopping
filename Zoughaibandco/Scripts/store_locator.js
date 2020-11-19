$(document).ready(function(){
	
	var country = 0;
	var store_current = 0;
	$(".storeRegionLoc").eq(country).show(300);
	
	$(".StoreRegion li").bind('touchstart click', function(){
		
		if( country != $(this).index())
		{
			country = $(this).index();
			store_current = 0;
			$(".StoreRegion li").removeClass('activeLoc');
			$(this).addClass('activeLoc');
			$(".storeRegionLoc li").removeClass('activeLoc');
			
			$(".storeRegionLoc").eq(country).find("li").eq(store_current).addClass('activeLoc');
			$(".storeRegionLoc").hide(300);
			setTimeout(function() {
				$(".storeRegionLoc").eq(country).show();
				$(".storeRegionLoc").eq(country).css({"display":"table-cell"});
			}, 300);
		}//country != $(this).index()
	});
	
	$(".storeRegionLoc li").bind('touchstart click', function(){
		
		if( store_current != $(this).index())
		{
			store_current = $(this).index();
			$(".storeRegionLoc li").removeClass('activeLoc');
			$(this).addClass('activeLoc');
		}//store_current != $(this).index()
	});
	
	
	var old_country = -1;
	var old_store_current = -1;
	
	var TempInterval = setInterval(function() {
		if( old_country != country || old_store_current != store_current )
		{
			
			$(".Country_Stores").hide(300);
			$(".City_Stores").hide(300);
			$(".Country_Stores").eq(country).delay(300).show(300);
			$(".Country_Stores").eq(country).find(".City_Stores").eq(store_current).delay(600).show(400);
			
			var new_map = $(".Country_Stores").eq(country).find(".City_Stores").eq(store_current).attr('map_rel');
			$(".map_iframe").attr('src',new_map);
			
			old_country = country;
			old_store_current = store_current;
		}
		
	}, 100); // TempInterval
	
	
});//$(document).ready(function()

