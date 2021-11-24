if(isobject($class::damage)) {
	$class::damage.delete();
}


$class::damage = new scriptGroup() {
	class = damage;
};

function damage::calc(%this,%player_obj1,%player_obj2) {
		%dmg = 0;
	
	%base = 1 + getRandom(0,%this.bonus_total(%player_obj1.client));
	%base += getRandom(0,4);
	
	%dmg = %base;
	//talk("dmg calc " @ %dmg);
	return %dmg;
}

function damage::on_attack(%this,%player_obj1, %player_obj2) {

}

// get atk bonus
function damage::bonus_atk(%this,%client) {
	return %client.profile.skill["melee"]*1;
}
//get range bonus
function damage::bonus_range(%this, %client) {
	return %client.profile.skill["range"]*1;
}
//get mage bonus
function damage::bonus_mage(%this, %client) {
	return %client.profile.skill["mage"]*1;
}

function damage::bonus_total(%this,%client) {
	return %this.bonus_atk(%client) + %this.bonus_range(%client) + %this.bonus_mage(%client);
}
// get same for defense