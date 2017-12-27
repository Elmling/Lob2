//***** class description start *****
//		author: Elm
//
//		description - Areas Class
//		A class to define areas and provide
//		extra functionality such as
//		requirements for entering areas,
//		call backs to package and a few
//		other aspects.
//
//***** class description end ***** 

//Initialize Areas class/group/container
if(!isObject($class::Areas))
	$class::Areas = new scriptGroup(Areas);

//name: getArea
//description: Returns an area object
function Areas::getArea(%this,%name)
{
	%c = %this.getCount();
	for(%i=0;%i<%c;%i++)
	{
		%area = %this.getObject(%i);
		if(%area.name $= %name)
			return %area;
	}
	return false;
}

//name: newArea
//description: Defines a new area -> object
//%vectorx and %vectory should create a box
function Areas::newArea(%this,%name,%vectorX,%vectorY)
{
	if(%name $= "")
	{
		talk("Areas::newArea(%this,%name,%vectorX,%vectorY)");
		return false;
	}
	if(%vectorX $= "")
		return false;
	if(%vectory $= "")
		return false;
	if(!%this.getArea(%name))
	{
		%clients = new simset();
		%area = new scriptGroup()
		{
			Areas = Areas;
			class = Area;
			name = %name;
			vectorX = %vectorX;
			vectorY = %vectory;
			trigger = null; 
			clients = %clients;
		};
		%area.setRespawnArea(%area);
		%area.createWorldTrigger();
		%this.add(%area);
			
		return %area;
	}
	return false;
}

//name: newArea_dupe
//description: utilize the duplicators selection box to easily make
//a new Area.
function Areas::newArea_dupe(%this,%name,%selection)
{
	if(!isObject(%selection))
	{
		talk("Error - > newArea_dupe(%name,%selection (.ndSelection));");
		return false;
	}
	
	%type = %selection.getName();
	if(%type $= "nd_selectionBox")
	{
		%vectorX = %selection.point1;
		%vectorY = %selection.point2;		
	}
	else
	{
		%vectorX = %selection.minx SPC %selection.miny SPC %selection.minZ;
		%vectorY = %selection.maxx SPC %selection.maxy SPC %selection.maxZ;
	}
	%area = %this.newArea(%name,%vectorX,%vectorY);
	return %area;
}

//name: registerBots
//description: Spawns all bots in all Areas
function Areas::registerBots(%this)
{
	%c = %this.getcount();
	for(%i=0;%i<%c;%i++)
	{
		%area = %this.getobject(%i);
		%x = %area.vectorx;
		%y = %area.vectory;
		%c2=$class::bots.getcount();
		
		for(%j=0;%j<%c2;%j++)
		{
			%node = $class::bots.getobject(%j);
			%bp = %node.bot.position;
			if(%area.vectorInArea(%bp))
			{
				%node.area = %area.name;
			}
		}
	}
	return true;
}

//***** Area Class *****

//name: onRemove
//description: Cleanup triggers and other objects
function area::onRemove(%this)
{
	if(isobject(%this.clients))
	{
		%this.trigger.delete();
		%this.clients.delete();
	}
}

//name: onEnter
//description: Called when a player enters an Area
function area::onEnter(%this,%client)
{
	if(%client.area $= %this)
		return false;
	%count = %this.getRequirementCount();
	if(%count > 0)
	{
		if(%this.meetsRequirements(%client))
		{
			//on enter
			%client.area = %this;
			%this.clients.add(%client);
			bottomprintall("\c6" @ %client.name @ "\c3 has entered the area  [\c6" @ %this.name @ "\c3] with \c4" @ %this.clients.getcount() @ "\c3 others.",3);
		}
		else
		{
			//dont meet requirements
			return false;
		}
	}
	else
	{
		//on enter
		%client.area = %this;
		bottomprintall("\c6" @ %client.name @ "\c3 has entered the area [\c6" @ %this.name @ "\c3] with \c4" @ %this.clients.getcount() @ "\c3 others.",3);
		%this.clients.add(%client);
	}
}

//name: onLeave
//description: Called when a player leaves an Area
function area::onLeave(%this,%client)
{
		%client.area = "";
		%this.clients.remove(%client);
		bottomprintall("\c6" @ %client.name @ " \c3has left the area [\c6" @ %this.name @ "\c3], \c4" @ %this.clients.getcount() @ " \c3players remain.",3);
}

//name: getPlayerCount
//description: Returns the amount of players in an Area
function area::getPlayerCount(%this)
{
	return %this.clients.getcount();
}

//name: newRequirement
//description: Defines a new requirement for entering an Area
//Example Syntax:
//area.newRequirement["isAdmin","==true;"]; <- use parentheses
//area.newRequirement["gold",">= 400;"]; <- use parentheses
//area.newRequirement["player.dataBlock","==playerStandardArmor;"]
function area::newRequirement(%this,%clientAttribute,%equalizer)
{
	if(%this.hasRequirement(%clientAttribute,%equalizer))
		return false;
	
	%requirement = new scriptObject()
	{
		class = requirement;
		Area = %this;
		Value = %clientAttribute;
		Equalizer = %equalizer;
		isRequirement = true;
	};
	
	%this.add(%requirement);
	return %requirement;
}

//name: hasRequirement
//description: Checks if an Area has a requirement already
function area::hasRequirement(%this,%clientAttribute,%equalizer)
{
	%c = %this.getcount();
	for(%i=0;%i<%c;%i++)
	{
		%requirement = %this.getObject(%i);
		if(%requirement.isRequirement)
			if(%requirement.value $= %clientAttribute && %requirement.equalizer $= %equalizer)
				return true;
	}
	return false;
}

//name: meetsRequirement
//description: Checks if a client meets all requirements in Area
function area::meetsRequirements(%this,%client)
{
	%c = %this.getcount();
	for(%i=0;%i<%c;%i++)
	{
		%requirement = %this.getObject(%i);
		if(%requirement.isRequirement)
			if(!%requirement.meetsRequirement(%client))
				return false;
	}
	return true;
}

//name: getRequirementCount
//description: Returns amount of requirements in an Area
function area::getRequirementCount(%this)
{
	return %this.getcount();
}

//name: setRespawnArea
//description: By default, this is set to itself, so players that die in AreaA
//Will respawn in AreaA
//You can use this function to set AreaA to have players respawn in AreaB, for example:
//%area.setRespawnArea[%area2.name]; <- use parentheses
function area::setRespawnArea(%this,%name)
{
	%area = %this.areas.getArea(%name);
	if(!isObject(%area))
		return false;
	else
		%this.respawnArea = %area;
}

//name: getSpawnPoint
//description: Returns a valid spawn point (vector) in an area
function area::getSpawnPoint(%this)
{
	%x = %this.vectorx;
	%y = %this.vectory;
	%time = getsimtime();
	%pos = "null";
	while(%pos $= "null")
	{
		%pos = getRandom(getWord(%x,0),getWord(%y,0)) SPC getRandom(getWord(%x,1),getWord(%y,1)) SPC getRandom(getWord(%x,2),getWord(%y,2));			
		%mask = $TypeMasks::FxBrickAlwaysObjectType | $TypeMasks::PlayerObjectType;
		initContainerRadiusSearch(%pos, "0.5", %mask);
		while(isObject(%hit = containerSearchNext()))
		{
			%pos = "";
			break;
		}
		
		if(%pos !$= "null")
			break;
	}
	return %pos;
}

//name: vectorInArea
//description: Checks if a point is inside of another a 3d box
function area::vectorInArea(%this,%vector)
{
	%x = %this.vectorx;
	%y = %this.vectory;
	
	%z = %vector;
	
	%xx = getWord(%x,0);
	%xy = getWord(%x,1);
	
	%yx = getWord(%y,0);
	%yy = getWord(%y,1);
	
	%zx = getWord(%z,0);
	%zy = getWord(%z,1);
	
	if(%zx >= %xx && %zx <= %yx)
	{
		return true;
	}
	return false;
}



//***** Requirement Class *****

//name: meetsRequirement
//description: Checks if a client meets the requirement at hand
function requirement::meetsRequirement(%this,%client)
{
	%val = %this.value;
	%equalizer = %this.equalizer;
	
	eval("$eval = " @ %client @ "." @ %val @ %equalizer @ ";");
	%eval = $eval;
	$eval = "";
	
	return %eval;
}


//Vector & Trigger stuff

//Create a new trigger data object
if(!isObject(area_trigger))
{
	new triggerData(area_Trigger);
}

//name: vectorMid
//description: This is a default function but is named getBoxCenter();
//We'll use this instead though
function vectorMid(%v1,%v2)
{
	%sub = vectorSub(%v2,%v1);
	%sub = vectorScale(%sub,0.5);
	%final = vectorAdd(%v1,%sub);
	return %final;
}

//name: createWorldTrigger
//description: Creates a trigger using an Area's vectorx and vectory to form the box
function area::createWorldTrigger(%this)
{
	if(isobject(%this.trigger))return false;
	
	if(%this.vectorx $= "" || %this.vectorY $= "")
		return false;
	
	%trigger = new Trigger() 
	{
		scale = "1 1 1";
		dataBlock = "area_trigger";
		polyhedron =  "0 0 0 1 0 0 0 -1 0 0 0 1";
		area = %this;
	};
	
	%x = %this.vectorx;
	%y = %this.vectory;

	%diff = vectorSub(%x,%y);
	%diff = mabs(getWord(%diff,0)) SPC mabs(getWord(%diff,1)) SPC mabs(getWord(%diff,2));
	%scale = %diff;
	%trigger.setScale(%scale);

	%posa = vectorMid(%x,%y);
	%posb = %trigger.getWorldBoxCenter();
	%diff = vectorSub(%posa,%posb);

	%trigger.setScale(%scale);
	%trigger.setTransform(%diff);
	%this.trigger = %trigger;
	return %trigger;
}

//name: onEnterTrigger
//description: Callbacks
function area_trigger::onEnterTrigger(%this,%trigger,%obj)
{
	if(isobject(%obj.client) && %obj.dataBlock.getName() !$= "PhysicsRollVehicle")
	{
		//talk("ENTER:" @ %obj.dataBlock.getname());
		%trigger.area.onEnter(%obj.client);
	}
	else
	if(%obj.getMountedObjectCount() > 0)
	{
		%c = %obj.getMountedObjectCount();
		for(%i=0;%i<%c;%i++)
		{
			%player = %obj.getMountedObject(%i);
			if(isObject(%player))
			{
				%trigger.area.onEnter(%player.client);
			}
		}
	}
}

//name: onLeaveTrigger 
//description: Callbacks
function area_trigger::onLeaveTrigger(%this,%trigger,%obj)
{
	if(isobject(%obj.client) && %obj.dataBlock.getName() !$= "PhysicsRollVehicle")
	{
		//talk("LEAVE:" @ %obj.dataBlock.getname());
		%trigger.area.onLeave(%obj.client);
	}
	else
	if(%obj.getMountedObjectCount() > 0)
	{
		%c = %obj.getMountedObjectCount();
		for(%i=0;%i<%c;%i++)
		{
			%player = %obj.getMountedObject(%i);
			if(isObject(%player))
			{
				%trigger.area.onLeave(%player.client);
			}
		}
	}
	
}