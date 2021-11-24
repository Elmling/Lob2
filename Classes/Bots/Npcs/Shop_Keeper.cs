if(isObject($class::npc::shop_keeper)) {
	if(isObject($class::npc::shop_keeper.bot)) {
		$class::npc::shop_keeper.bot.delete();
	}
	$class::npc::shop_keeper.delete();
}

$class::npc::shop_keeper = new scriptGroup(shop_keeper) {
	class = npc;
	name = "shop_keeper";
	name_clean = "Shop Keeper";
	position = "711 277 45";
	dataBlock = "playerStandardArmor";
};

function shop_keeper::on_click(%this,%client) {
	$class::chat.to(%client,"You fokin wot m8?",1000);
}

//$class::npc::shop_keeper.spawn();