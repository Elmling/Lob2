if(isObject($class::npc::dungeon_master)) {
	if(isObject($class::npc::dungeon_master.bot)) {
		$class::npc::dungeon_master.bot.delete();
	}
	$class::npc::dungeon_master.delete();
}

$class::npc::dungeon_master = new scriptGroup(dungeon_master) {
	class = npc;
	name = "Dungeon_Master";
	name_clean = "Dungeon Master";
	name_distance = 100;
	aggressive = false;
	attackable = false;
	type = "npc";
	roam = 2;
	scale = "1.15 1.15 1.3";
};

function dungeon_master::on_click(%this,%client) {
	$class::menuSystem.showMenu(%client,"dungeon_master");
	%this.bot.setshapenamedistance("100");
}


//$class::npc::dungeon_master.spawn();