//***** class description start *****
//		author: Elm
//
//		description - Chat System Class
//		Logic for everything related to chat
//		is in this file.
//
//***** class description end ***** 

//***** chatSystem class *****

if(!isObject($class::chatSystem))
{
	$class::chatSystem = new scriptGroup(chatSystem)
	{
		localRange = 20;
		Active = false;
	};
}

package class_chatSystem
{
	//name: serverCmdMessageSent
	//description: <b>packaged</b> overwrite
	function serverCmdMessageSent(%cl,%m)
	{
		if(!$class::chatSystem.active)
		{
			%p = parent::serverCmdMessageSent(%cl,%m);
			
			return %p;
		}
		else
		{
			//global	
		}
	}
	//name: serverCmdTeamMessageSent
	//description: <b>packaged</b> overwrite	
	function serverCmdTeamMessageSent(%cl,%m)
	{
		if(!$class::chatSystem.active)
		{
			%p = parent::serverCmdTeamMessageSent(%cl,%m);
			
			return %p;
		}
		else
		{
			//local
		}
	}
};
activatePackage(class_chatSystem);

//name: getUsersInRange
//description: gets all available players in range
//range is defined in $class::chatSystem
//RETURNS AN OBJECT WITH PLAYERS IN RANGE.
//BE SURE TO DELETE THE OBJECT AFTER USING
//TO AVOID MEMORY LEAKS
//Alternatively, objects are stored in chatSystem
//just incase.
function chatSystem::getUsersInRange(%this,%client)
{
	%p = %client.player;
	if(!%p)
		return false;
	%c = clientGroup.getCount();
	%count = 0;
	for(%i=0;%i<%c;%i++)
	{
		%cl = clientGroup.getObject(%i);
		if(!isObject(%cl.player) || %cl.player == %client.player)
			continue;
		%p = %cl.player;
		if(vectorDist(%client.player.position,%p.position) <= %this.localRange)
		{
			if(!isObject(%group))
			{
				%group = new simSet()
				{
					timeCreated = getSimTime();
				};
			}
			
			%group.add(%p);
		}
	}
	if(isObject(%group))
	{
		%this.add(%group);
		return %group;
	}
	else
		return false;
}