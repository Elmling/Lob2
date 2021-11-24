package scripts_onSpawn
{
	function gameConnection::spawnPlayer(%client,%a,%b,%c,%d,%e,%f)
	{
		if(!%client.hasSpawnedOnce) {
			if(!isObject(%client.profile) && %client.profilePending)
			{
					$class::profiles.newProfile(%client,"Forced","Created");
					$class::chat.to_all(%client.name @ " has created a Profile.");
					// talk("New profile created for " @ %client.name);
			} else {
				if(!isObject(%client.profile)) {
					if($class::profiles.hasProfile(%client)) {
						//talk("Loading profile for " @ %client.name);
						$class::profiles.loadProfile(%client);
						$class::chat.to(%client,"Your Profile has been loaded.");
					}
				}
			}
			$class::chat.to(%client,"Talk to the \c4Dungeon Master \c6to get \c5tools/weapons\c6. Head to the \c5Dungeon \c6to get resources and \c4Items\c6.");
		}
		%p = parent::spawnPlayer(%client,%a,%b,%c,%d,%e,%f);
		%client.player.schedule(1,changeDatablock,lobarmor);
		%spawn_pos = "706 346 30";
		%client.player.setTransform(vectorAdd(%spawn_pos,getRandom(-5,5) SPC getRandom(-5,5) SPC 0));
		%client.schedule(50,setMusic,"musicData_music_home1",0.7);
		return %p;
	}
	
	function menuObject::onInputValueRecieved(%this,%client,%value)
	{			
		%p = parent::onInputValueRecieved(%this,%client,%value);
		if(%client.profilePending == true && %client.profilePending2 $= "")
		{
			%client.userName = %value;
			%client.profilePending2 = true;
			$class::menuSystem.getMenu("input").showInputMenu(%client);
			bottomPrint(%client,"<font:impact:25>\c6Step 1: Enter Username\n<div:1>\c6Step 2: Enter a password.\n\c3Use the Chat.");
		}
		else
		if(%client.profilePending == true && %client.profilePending2 == true)
		{
			%client.profilePending = "";
			%client.profilePending2 = "";
			%client.password = %value;
			%val = $class::profiles.newProfile(%client,%client.username,%client.password);
			if(isObject(%val))
			{
				%client.username = "";
				%client.password = "";
				talk(%client.name @ " created a profile.");
				schedule(500,0,bottomprint,%client,"",1);
			}
			else
			{
				%client.profilePending = true;
				%client.username = "";
				%client.password = "";
				talk(%client.name @ " failed to create a profile: " @ $class::profiles.failReason[%client]);
			}
		}
		return %p;
	}
};
activatePackage(scripts_onSpawn);