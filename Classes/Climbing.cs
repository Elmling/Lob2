if(isObject($class::climbing))
{
	$class::climbing.delete();
}

$class::climbing = new scriptObject()
{
	class = climbing;
	time_out = 850;
};

function climbing::try_root(%this,%player)
{
	if(!isObject(%player.climb_zone))
		%player.playthread(0,root);
}

function climbing::bottom_print(%this, %client, %str_add) {
	cancel(%this.bpLoop[%client]);
	if(%str_add $= "") {
		%str_add = "-";
	} else {
		%str_add = %str_add @ "-";
	}
	%len = strLen(%str_add);
	if(%len > 25) {
		%color = "\c2";
		$class::chat.b_print(%client,"Launch Strength: <font:impact:22>" @ %color @ %str_add);
		return false;
	}
	
	if(%len >= 18) {
		%color = "\c2";
	} else if(%len >= 9 && %len < 18 ) {
		%color = "\c4";
	} else if(%len < 9) {
		%color = "\c0";
	}
	//$class::chat.c_print(%client,%len);
	$class::chat.b_print(%client,"Launch Strength: <font:impact:22>" @ %color @ %str_add);
	
	%this.bpLoop[%client] = %this.schedule(100,bottom_print,%client,%str_add);
}

function climbing::bottom_print_stop(%this,%client) {
	cancel(%this.bpLoop[%client]);
	schedule(2500,0,bottomprint,%client,"",1);
}

function climbing::can_climb(%this,%playerObject)
{
	%po = %playerObject;
	
	if(getSimTime() - $class::climbing.last_climb[%playerObject.client] <= $class::climbing.time_out) {
		$class::chat.to(%playerObject.client, "Climbing timeout, take your time and hold spacebar to stay stationary.",60000 * 10);
		return false;
	}
	
	if(isObject(%po))
	{
		%client = %po.client;
		%obj = raycast.get(%client,2.2);
		
		if(isObject(%obj))
		{
			
			if(%obj.getClassname() $= "fxdtsbrick")
			{
				%po.setVelocity("0 0 0");
				//%zonePos = %po.gethackposition();
				//from jetz ClimbingPick add-on
				//------------------------------
				
				%zonePos = %po.getWorldBoxCenter();
				%zonePos = vectorAdd(%zonePos, "0 0" SPC (getWord(%po.getObjectBox(), 5) / -2 - 0.09));
	
				%po.climb_zone = new PhysicalZone() //A physicalzone is employed to prevent movement. It is kept small to hopefully prevent any other players intersecting it, but I can't confirm this will work for all sizes.
				{
					position = %zonePos;
					scale = "0.1 0.1 0.1";
					velocityMod = 0;
					gravityMod = 0;
					extraDrag = 31; //Prevents almost all player movement for the default datablock.
					polyhedron = "0 0 0 1 0 0 0 -1 0 0 0 1";
					//imageSlot = %slot; //For detaching later.
					//leapVector = %leapVector;
					//targetBrick = %col;
					//soundPos = %pos; //For playing the disconnect sound.
				};
				serverplay3d(defbowFireSound,vectorAdd(%po.getEyePoint(),%po.getForwardVector()));
				%po.climb_zone.activate(); 
				%po.playthread(0,grab);
				$class::climbing.last_climb[%playerObject.client] = getSimTime();
				return true;
			}
		}
	}
	return false;
}