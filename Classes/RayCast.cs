if(isObject(raycast))
{
	raycast.delete();
}

$class::raycast = new scriptObject(raycast)
{
	
};


function raycast::search(%this,%point,%box, %ignore)
{
	%pos = %point;
	if(%box $= "")
		%box = "1 1 1";
	if(%ignore $= "water")
		%mask = $TypeMasks::FxBrickObjectType;
	else
		%mask = $TypeMasks::PhysicalZoneObjectType | $TypeMasks::FxBrickObjectType;
	initContainerBoxSearch(%pos, %box, %mask);
	%list = new simset(){};

	while(%searchObj = containerSearchNext())
	{	
		%list.add(%searchObj);
	}
	
	if(%list.getcount() > 0)
		return %list;
	else
	{
		%list.delete();
		return -1;
	}
}

function raycast::get(%this,%client,%range)
{
	%player = %client.player;
	%EyeVector = %player.getEyeVector();
	%EyePoint = %player.getEyePoint();
	if(%range $= "")
		%Range = 100;
	%RangeScale = VectorScale(%EyeVector, %Range);
	%RangeEnd = VectorAdd(%EyePoint, %RangeScale);
	%raycast = containerRayCast(%eyePoint,%RangeEnd,$TypeMasks::All, %player);
	%o = getWord(%raycast,0);
	
	if(isObject(%o))
	{
		//talk(%o.getclassname());
		
		if(%o.getClassName() $= "aiplayer")
		{
			%p = new projectile()
			{
				dataBlock = "color0paintprojectile";
				initialPosition = getWords(%raycast,1,3);
				initialVelocity = "0 -1 0";
				sourceObject = %player;
				client = %client;
				scale = "1 1 1";
				rotation = "1 0 0 0";
				sourceSlot = 0;
				fakePaint = true;
			};
			//talk(getWords(%raycast,1,3));
			//talk("projectile = " @ %p);
			
			//%p.explode();
		}
		else if(%o.getClassName() $= "player")
		{
			%p = new projectile()
			{
				dataBlock = "color0paintprojectile";
				initialPosition = getWords(%raycast,1,3);
				initialVelocity = "0 -1 0";
				sourceObject = %player;
				client = %client;
				scale = "1 1 1";
				rotation = "1 0 0 0";
				sourceSlot = 0;
				fakePaint = true;
			};			
		}
		
		return %o;
	}
}

function raycast::getCollidePoint(%this,%client,%range)
{
	%player = %client.player;
	%EyeVector = %player.getEyeVector();
	%EyePoint = %player.getEyePoint();
	if(%range $= "")
		%Range = 100;
	%RangeScale = VectorScale(%EyeVector, %Range);
	%RangeEnd = VectorAdd(%EyePoint, %RangeScale);
	%raycast = containerRayCast(%eyePoint,%RangeEnd,$TypeMasks::All, %player);
	%o = getWord(%raycast,0);
	
	if(isObject(%o))
	{
		return getwords(%raycast,1,3);
		//return getWords(%o,
	}
	
	return false;
}

function raycast::cameraGet(%this,%camera,%range) {
    %player = %camera;
	%EyeVector = %player.getEyeVector();
	%EyePoint = %player.getEyePoint();
	if(%range $= "")
		%Range = 100;
	%RangeScale = VectorScale(%EyeVector, %Range);
	%RangeEnd = VectorAdd(%EyePoint, %RangeScale);
	%raycast = containerRayCast(%eyePoint,%RangeEnd,$TypeMasks::All, %player);
	%o = getWord(%raycast,0);
	
	if(isObject(%o))
	{
		return %o;//getwords(%raycast,1,3);
		//return getWords(%o,
	}
	
	return false;
}

function raycast::cameraGetCollidePoint(%this,%camera,%range) {
    %player = %camera;
	%EyeVector = %player.getEyeVector();
	%EyePoint = %player.getEyePoint();
	if(%range $= "")
		%Range = 100;
	%RangeScale = VectorScale(%EyeVector, %Range);
	%RangeEnd = VectorAdd(%EyePoint, %RangeScale);
	%raycast = containerRayCast(%eyePoint,%RangeEnd,$TypeMasks::All, %player);
	%o = getWord(%raycast,0);
	
	if(isObject(%o))
	{
		return getwords(%raycast,1,3);
		//return getWords(%o,
	}
	
	return false;
}

package _raycast
{
	
	function player::setTempColor(%this,%a,%b,%c,%d,%e)
	{
		%p = parent::setTempColor(%this,%a,%b,%c,%d,%e);
		%pos = %c;
		%colscale = getWord(%this.getScale(), 2);
		%headShotLevel = getWord(%this.getWorldBoxCenter(), 2) - 3.3 * %colScale;
		%bodyShotLevel = getWord(%this.getWorldBoxCenter(), 2) - 3.5 * %colScale;
		%feetShotLevel = getWord(%this.getWorldBoxCenter(), 2) - 4.3 * %colScale;
		
		//talk(%this.getclassname());
		//talk(getWord(%pos, 2) @ " }{ FS: " @ %feetShotLevel);
		
				if(%this.blocking)
				{
					//talk("blocking");
					return %p;
				}
		
		if(getWord(%pos, 2) > %headShotLevel)
		{
			if(%this.getClassName() $= "aiplayer")
			{
				if(%this.attacker.getClassName() $= "AiPlayer")
				{
					//talk("An AI Player has headshot an AI Player");
					
				}
				else
				{
					// talk(%this.attacker.client.name @ " headshot an AI Player");
					%this.setdamagelevel(%this.getDamageLevel() + 20);
				}
			}
			else
			{
				if(%this.attacker.getClassName() $= "AiPlayer")
				{
					//talk("An AI Player has headshot " @ %this.client.name);
				}
				else
				{
					//talk(%this.attacker.client.name @ " headshot " @ %this.client.name);
					
				}
			}
			//talk("headshot");
			
		}
		else
		{
			if(getWord(%pos,2) > %feetShotLevel && getWord(%pos,2) < %bodyShotLevel)
			{
				if(%this.getClassName() $= "aiplayer")
				{
					if(%this.attacker.getClassName() $= "AiPlayer")
					{
						//talk("An AI Player has bodyshot an AI Player");
					}
					else
					{
						//talk(%this.attacker.client.name @ " bodyshot an AI Player");
						%this.setdamagelevel(%this.getDamageLevel() + 10);
						
					}
				}
				else
				{
					if(%this.attacker.getClassName() $= "AiPlayer")
					{
						//talk("An AI Player has bodyshot " @ %this.client.name);
					}
					else {
						//talk(%this.attacker.client.name @ " bodyshot " @ %this.client.name);
					}
				}
			}
			else
			{
				if(%this.getClassName() $= "aiplayer")
				{
					if(%this.attacker.getClassName() $= "AiPlayer")
					{
						//talk("An AI Player has footshot an AI Player");
					}
					else
					{
						//talk(%this.attacker.client.name @ " footshot an AI Player");
						%this.setdamagelevel(%this.getDamageLevel() + 5);
					}
				}
				else
				{
					if(%this.attacker.getClassName() $= "AiPlayer")
					{
						//talk("An AI Player has footshot " @ %this.client.name);
					}
					else {
						//talk(%this.attacker.client.name @ " footshot " @ %this.client.name);
					}
				}
			}
		}
		return %p;
	}
	
};
activatePackage(_raycast);