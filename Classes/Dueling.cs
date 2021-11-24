if(isobject($class::dueling)) {
	$class::dueling.sessions.delete();
	$class::dueling.delete();
}
$class::dueling = new scriptGroup() {
	class = dueling;
	countDownTime = 6;
};

$class::dueling.sessions = new scriptGroup();

function dueling::new_duel(%this, %cl1, %cl2) {
	%session = new scriptGroup() {
		class = duelingSession;
		parent = %this;
	};
	%session.clients = new simset();
	
	%session.clients.add(%cl1);
	%session.clients.add(%cl2);
}

function duelingSession::start(%this) {
	if(%this.isValid()) {
		%this.start_time = getSimTime();
		$class::chat.to_all("\c4" @ %this.clients.getObject(0).name @ " \c6and \c4" @ %this.clients.getObject(1).name @ "\c6 have started a \c5Duel\c6.");
	}
}

function duelingSession::countDown(%this,%num,%player_index) {
	cancel(%this.countdownloop);
	if(!%num)
		%num = 1;
	if(!%player_index) {
		%player_index = 0;
		$class::camera.orbitObj(%this.clients.getObject(0),%this.clients.getObject(%player_index).player);
		$class::camera.orbitObj(%this.clients.getObject(1),%this.clients.getObject(%player_index).player);
	}
	%cl = %this.clients.getObject(%player_index);
	if(%num == mfloor(%this.parent.countDownTime / 2) && %player_index != %this.getCount() -1) {
		%player_index ++;
		$class::camera.orbitObj(%this.clients.getObject(0),%this.clients.getObject(%player_index).player);
		$class::camera.orbitObj(%this.clients.getObject(1),%this.clients.getObject(%player_index).player);
		$class::camera.schedule(mfloor(%this.parent.countDownTime /2),root,%this.clients.getobject(0));
		$class::camera.schedule(mfloor(%this.parent.countDownTime /2),root,%this.clients.getobject(1));
	}
	%this.countdownloop = %this.schedule(1000,countDown, %num, %player_index);
}

function duelingSession::end(%this) {
	if(isObject(%this.winner)) {
		$class::chat.to_all(%this.winner.name @ " has won the \c4Duel\c6 against player " @ %this.loser.name);
	} else {
		
	}
	%this.parent.remove(%this);
	%this.delete();
}

function duelingSession::setWinner(%this,%client) {
	if(isObject(%client)) {
		%this.winner = %client;
		%this.defineLoser();
	}
}

function duelingSession::defineLoser(%this) {
	if(!isObject(%this.winner))
		return false;
	for(%i=0;%i<%this.clients.getcount();%i++) {
		%cl = %this.clients.getObject(%i); 
		if(%cl != %this.winner) {
			%this.loser = %cl;
			return %this.loser;
		}
	}
}

function duelingSession::isValid(%this) {
	if(%this.clients.getCount() != 2) return false;
	for(%i=0;%i<%this.clients.getcount();%i++) {
		if(!isObject(%this.clients.getObject(%i).player)) return false;
	}
	return true;
}

// function duelingSession::