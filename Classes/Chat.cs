
if(isObject($class::chat)) {
	$class::chat.delete();
}

$class::chat = new scriptGroup() {
	class = "chat";
};

$class::chat.memory = new simSet();

function chat::to_all(%this, %msg, %delay, %ignore_sim_set) {
	if(isObject(%ignore_sim_set)) {
		
	}
	
	for(%i=0;%i<clientGroup.getCount();%i++) {
		%cl  = clientGroup.getObject(%i);
		%this.to(%cl, "\c6( \c2Global \c6) \c6" @ %msg, %delay, true);
	}
	return %this;
}

function chat::to(%this, %client, %msg, %delay, %global_msg) {
	if(%delay $= "")
		%delay = -1;
	if (%this.last_message_time[%client.bl_id, %msg] $= "") {
		%this.last_message_time[%client.bl_id, %msg] = 0;
	}
	if(getSimTime() - %this.last_message_time[%client.bl_id, %msg] > %delay) {
		if(%global_msg !$= "")
			messageClient(%client,'',%msg);
		else
			messageClient(%client,'', "\c6( \c3Direct \c6) \c6" @ %msg);
		%this.last_message_time[%client.bl_id, %msg] = getSimTime();
	}
	return %this;
}

function chat::b_printAll(%this, %msg, %time, %delay) {
	%global_msg = true;
	if(%delay $= "")
		%delay = -1;
	for(%i=0;%i<clientGroup.getCount();%i++) {
		%cl  = clientGroup.getObject(%i);
		%this.b_print(%cl,%msg,%time, %delay, true);
	}
}

function chat::b_print(%this, %client, %msg, %time, %delay, %global_msg) {
	if(%global_msg $= "")
		%msg = "\c6( \c3Direct \c6) \c6" @ %msg;
	else
		%msg = "\c6( \c2Global \c6) \c6" @ %msg;
	
	if(getSimTime() - %this.last_bottomprint_time[%client.bl_id] > %delay) {
		%this.last_bottomprint_time[%client.bl_id] = getSimTime();
		
	if(%time $= "")
		bottomPrint(%client,  %msg);
	else
		bottomPrint(%client, %msg, %time);
	}
}

function chat::c_printAll(%client, %msg, %delay) {
	
}

function chat::c_print(%this, %client, %msg,%time,%delay) {
	%msg ="<just:right>\c6" @ %msg @  "\c6 ( \c3Direct \c6)";
	if(%time $= "") {
		%time = 5;
	}
	centerprint(%client,%msg,%time);
}

function chat::area_to(%this,%pos,%radius,%msg,%timeout) {
	if(%timeout $= "") %timeout = 5000;
	for(%i=0;%i<clientGroup.getCount();%i++) {
		%cl = clientGroup.getObject(%i);
		if(isobject(%cl.player)) {
			if(vectorDist(%cl.player.position,%pos) <= %radius) {
				$class::chat.to(%cl,%msg,%timeout);
			}
		}
	}
}

function chat::area_cprint(%this,%pos,%radius,%msg,%time,%timeout) {
	if(%time $= "") %time = 5;
	if(%timeout $= "") %timeout = 5000;
	for(%i=0;%i<clientGroup.getCount();%i++) {
		%cl = clientGroup.getObject(%i);
		if(isobject(%cl.player)) {
			if(vectorDist(%cl.player.position,%pos) <= %radius) {
				$class::chat.c_print(%cl,%msg,%time,%timeout);
			}
		}
	}
}
