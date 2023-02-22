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
        // removed profile creation here..
		return %p;
	}
};
activatePackage(scripts_onSpawn);