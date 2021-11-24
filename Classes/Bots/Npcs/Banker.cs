if(isObject($class::npc::banker)) {
	if(isObject($class::npc::banker.bot)) {
		$class::npc::banker.bot.delete();
	}
	$class::npc::banker.delete();
}

$class::npc::banker = new scriptGroup(banker) {
	class = npc;
	name = "banker";
	name_clean = "Banker";
	name_distance = 100;
	aggressive = false;
	attackable = false;
	type = "npc";
	roam = 2;
	scale = "1.15 1.15 1.3";
	position = "692 318 28 0 0 0.999941 0.0106531";
};

function banker::on_click(%this,%client) {
	$class::menuSystem.showMenu(%client,"banker");
	%this.bot.setshapenamedistance("100");
}

$class::npc::banker.spawn();