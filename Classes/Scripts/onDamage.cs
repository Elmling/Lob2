package script_onDamage
{
	function armor::onDamage(%a,%b,%c,%d,%e,%f,%g)
	{
		if(isObject(%b.npc_handle)) {
			if(%b.npc_handle.type $= "npc")
				return false;
		}
		
		
		// %p = parent::onDamage(%a,%b,%c,%d,%e,%f,%g);
		
		//talk(%a SPC %b.getclassname() SPC %c @ " | " @ %d SPC %e SPC %f);
		if(%b.getclassname() $= "aiplayer")
		{
			%cp = %b.getClosestPlayer();
			if(vectorDist(%b.position,%cp.position) <= 70) {
				cancel(%b.roamloop);
				%b.fight(%cp);
				%b.fighting = %cp;
			}
			//talk(%b.getdamageLevel());
			if(%b.getdamageLevel() >= 100 && !%b.handled_respawn)
			{
				%b.handled_respawn = true;
				if(!isObject(%b.dungeon)) {
					$class::bots.respawn(%b,%b.name,$class::bots.get_handle(%b).position,6000, %b.dataBlock);
				} else {
					$class::chat.schedule(100,b_print,%b.fighting.client,"\c4" @ %b.dungeon.enemies.getcount() @ " \c6Dungeon Enemies left.",5);
				}
			} else {
				//talk("in fighting - > " @ %b.fighting.getclassname());
				bottomprint(%b.fighting.client,"\c4" @ %b.name @ " \c3HP: \c4" @ 100-%b.getdamageLevel(),5);
				cancel(%b.roamloop);
			}
		} else {
			//%dmg = $class::combat.damage(%client,%dmg,%dmger) 
		}
		// return %p;
	}
	
	function armor::damage(%a,%b,%c,%d,%e,%f,%g) {
		%p = parent::damage(%a,%b,%c,%d,%e,%f,%g);
		//talk("in dmgggggggggggggggggg");
		return %p;
	}
	
	function player::Damage(%a,%b,%c,%d,%e)
	{	
		//talk(%c @ " , " @ %d);
		if(%a.blocking)
		{
			//talk("\c5 Blocked");
			serverplay3d(lobswordclashsound,%a.position);
			%a.doSwordJumpBack();
			return false;
		}
		
		%p = parent::damage(%a,%b,%c,%d,%e);
		
		
		//talk(%a.client.name SPC %b.client.name SPC %c SPC %d);
		
		return %p;
	}
};
activatePackage(script_onDamage);