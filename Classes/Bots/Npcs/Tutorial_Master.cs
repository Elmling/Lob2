if(isObject($class::npc::tutorial_master)) {
	if(isObject($class::npc::tutorial_master.bot)) {
		$class::npc::tutorial_master.bot.delete();
	}
	$class::npc::tutorial_master.delete();
}

$class::npc::tutorial_master = new scriptGroup(tutorial_master) {
	class = npc;
	name = "Tutorial_Master";
	name_clean = "Tutorial Master";
	name_distance = 100;
	aggressive = false;
	attackable = false;
	type = "npc";
	roam = 2;
	scale = "1.15 1.15 1.3";
};

function tutorial_master::on_click(%this,%client) {
	$class::menuSystem.getMenu("tutorial_master").showMenu(%client);
	%this.bot.setshapenamedistance("100");
}


//$class::npc::tutorial_master.spawn();