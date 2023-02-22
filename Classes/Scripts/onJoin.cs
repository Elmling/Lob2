package scripts_onJoin
{
	function gameConnection::autoAdminCheck(%client,%a,%b,%c,%d,%e,%f)
	{
		%p = parent::autoAdminCheck(%client,%a,%b,%c,%d,%e,%f);
		
		if($class::profiles.hasProfile(%client))
		{
			//load
			talk(%client.name @ "\'s profile has been loaded.");
			$class::profiles.loadProfile(%client);
		}
		else
		{
			//create new
			// %client.profilePending = true;
            $class::profiles.newProfile(%client,%client.BL_ID,"na");
            echo("created profile for: " @ %client.name);
		}
		
		%inv = $class::inventorys.newInventory(%client);
		
		if($class::banking.hasBank(%client, true)) {
			$class::banking.loadBank(%client);
			echo("Loaded " @ %client.name @ "\'s bank");
		} else {
			$class::banking.newBank(%client);
			echo("Created new bank for " @ %client.name);
		}
		
		return %p;
	}
	
	function gameConnection::onDrop(%client,%a,%b,%c,%d,%e,%f)
	{
		if(isObject(%client.dungeon)) {
			if(%client.dungeon.floors.getCount() > 0) {
				servercmddungeon(%client, "clear");
			}
		}
		if(isObject(%client.profile))
		{
			talk(%client.name @ "\'s profile has been saved.");
			%client.profile.saveProfile();
		}
		if(isObject(%client.inventory))
		{
			echo("Cleaning up profile object for: " @ %client.name @ "");
			%client.inventory.saveInventory();
			%client.inventory.delete();
		}
		if(isObject(%client.bank)) {
			$class::banking.saveBank(%client);
			echo("Saved " @ %client.name @ "\'s Bank.");
		}
		%p = parent::onDrop(%client,%a,%b,%c,%d,%e,%f);
		
		return %p;
	}
};

activatePackage(scripts_onJoin);