if(isObject($class::npc::don_the_fisher)) {
	if(isObject($class::npc::don_the_fisher.bot)) {
		$class::npc::don_the_fisher.bot.delete();
	}
	$class::npc::don_the_fisher.delete();
}

$class::npc::don_the_fisher = new scriptGroup() {
	class = npc;
	name = "Don_The_Fisher";
	name_clean = "Don The Fisher";
	aggressive = false;
	command = "roam";
	roam = 5;
	attackable = false;
	type = "npc";
	avatar = "default";
	movespeed = "0.7";
};

function npc::on_click(%this, %client) {
	$class::chat.to(%client, "\c4" @ %this.name_clean @ " \c6is currently not \c5interactable\c6.", 5000);
}

// main spawn method. for all npc classes
function npc::spawn(%this,%pos) {
	if(%pos !$= "") %this.position = %pos;
	if(%this.position $= "") {
		$class::chat.to_all(%this.name @ " needs a position attribute, or the position passed through this npc::spawn(%this,%pos(optional) ) method.");
		return false;
	}
	if(%this.bot $= "" || !isObject(%this.bot)) {
		%this.bot = $class::bots.newBot(%this.name_clean, %this.position);
		if(isObject(%this.bot)) {
			%this.bot.schedule(100,setTransform,%this.position);
			// need to safe check that all attributes were declared...
			// this.roam, this.name, etc
			%this.bot.npc_handle = %this;
			//talk("we are here, setting " @ %this.bot @ " handle to " @ %this);
			%this.bot.schedule(1500,playthread,0,"root");
			if(%this.name_distance $= "")
				%this.bot.schedule(10,setshapenamedistance,25);
			else
				%this.bot.schedule(10,setshapenamedistance, %this.name_distance);
			
			if(%this.scale !$= "")
				%this.bot.schedule(10,setScale, %this.scale);
			if(%this.dataBlock !$= "")
				%this.bot.schedule(10,changeDatablock, %this.dataBlock);
			if(%this.command $= "roam") {
				%this.bot.newRoamObject(%this.position,%this.roam, true);
				%this.bot.schedule(1500,roam);
			} else if(%this.command $= "") {
				// do nothing..
			} else {
				eval("%this.bot." @ %this.command);
			}
			%this.bot.schedule(1,playthread,0,root);
			%onSpawn = strReplace(%this.onSpawn,"THIS",%this.bot);
			$class::chat.to_all(%this.name_clean @ " has spawned");
			if(%this.avatar $= "default")
				%this.bot.av_default();
			if(%this.movespeed !$= "")
				%this.bot.setmovespeed(%this.movespeed);
		} else 
			return false;
	}
	return %this;
}

