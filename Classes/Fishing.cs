if(!isobject($class::Fishing))
	$class::Fishing = new scriptGroup(Fishing)
	{
		
	};
	
if(!isObject($class::Fishing.fishGroup))
{
	$class::Fishing.fishGroup = new scriptGroup()
	{
		class = fishGroup;
	};
}

if(!isObject($class::Fishing.fishingSpot))
{
	$class::Fishing.fishingSpot = new scriptGroup()
	{
		class = fishingSpot;
	};
}

function fishing::point_in_water(%this,%vector)
{
	%list = raycast.search(%vector,"0.5 0.5 0.5");
	
	if(isObject(%list))
	{
		%c = %list.getCount();
		
		for(%i=0;%i<%c;%i++)
		{
			%so = %list.getobject(%i);
			
			if(%so.isWater)
			{
				%list.delete();
				return true;
			}
		}
		
		%list.delete();
	}
	
	return false;
}

function fishing::endFishing(%this,%client)
{
	if(%client.fishingSpot.active)
	{
		%client.fishingSpot.active = 0;
		cancel(%client.fishingSpot.bitten.fish_reelInLoop);
		clearRopes(%client.bl_id);
		cancel(%this.fish_reelInLoop);
		$class::camera.root(%client);
		$class::fishing.fishgroup.cleanup(%client.fishingSpot);
		%client.player.playthread(0,root);
		clearbottomprint(%client);
		%client.player.playthread(2,root);
		$class::fishing.fishingSpot.deleteHook(%client);
        %client.player.setImageTrigger(0,0);
	}
	cancel(%client.fishingSpot.bitten.fish_reelInLoop);
}

function fishingSpot::spawnHook(%this,%client)
{
	%fp = %this.getSpot(%client);
	
	%hook = new item(){
		dataBlock = hookItem;
		scale = "1 1 1";
		position = vectorAdd(%fp.location,"0 0 0.1");
		static = 1;
		canpickup = false;
	};
	%hook.canPickup=false;
	%hook.setVelocity("0 0 -5");
	
	%fp.hook = %hook;
}

function fishingSpot::deleteHook(%this,%client)
{
	%fp = %this.getSpot(%client);
	if(isObject(%fp.hook))
		%fp.hook.delete();
}

function fishingSpot::newSpot(%this,%client,%location)
{		
	if(isObject(%this.getSpot(%client)))
	{
		$class::fishing.fishingSpot.deleteHook(%client);
		%fp = %this.getSpot(%client);
		%fp.location = %location;
		%fp.active = true;
		
		%client.fishingSpot = %fp;
		$class::fishing.fishingSpot.spawnHook(%client);
	}
	else
	{

		%fp = new scriptObject()
		{
			location = %location;
			owner = %client;
			active = true;
		};
			
		%this.add(%fp);
		%client.fishingSpot = %fp;
		//talk("New Fishing Spot created for " @ %client.name);
	}
    
    $class::camera.orbitPos(%client,vectorAdd(%location,"0 0 8"));
	
	%fp.time_created = getSimTime();


	%yds = mfloor($class::fishing.getLineDist(%client));
	if(%yds >= 70)
		talk("\c6" @ %client.name @ "\c3 threw a cast out \c2" @ %yds @ " \c3yards!");

	
	bottomprint(%client,"\c6Your fishing spot is \c2Active");
}

function fishingSpot::getSpot(%this,%client)
{
	%c = %this.getCount();
	for(%i=0;%i<%c;%i++)
	{
		%fp = %this.getObject(%i);
		if(%fp.owner == %client)
			return %fp;
	}
	return false;
}

function fishingSpot::getByLocation(%this,%location)
{
	%c = %this.getCount();
	for(%i=0;%i<%c;%i++)
	{
		%fp = %this.getObject(%i);
		if(%fp.location $= %location)
			return %fp;
	}
	return false;
}
function fishGroup::newFish(%this,%location)
{
	
	%amount = getRandom(1,3);
	
	%fishingSpot = $class::fishing.fishingSpot.getByLocation(%location);
	//talk("found fishing spot -> " @ %fishingspot @ " at: " @ %location);
	
	for(%i=0;%i<%amount;%i++)
	{
		%location = getWord(%location,0) + getRandom(-10,10) SPC getWord(%location,1) + getRandom(-10,10) SPC getWord(%location,2);
		%type = "Salmon";
		
		%fish = new aiplayer()
		{
			dataBlock = "fisharmor";
			scale = "1." @ getRandom(0,3) SPC "1." @ getRandom(0,3) SPC "1." @ getRandom(0,3);
			position = %location;
			initialFishingSpot = %fishingSpot;
			type = %type;
			rarity = "Normal";
		};
        
        %weight = getRandom(1,2);
		
		if(getRandom(0,3) == 1)
		{
			%fish.setScale("1.7 1.7 1.7");
			%fish.rarity = "Big";
            %weight += getRandom(3,6);
		}
		
		if(getRandom(0,10) <= 2)
		{
			%fish.setScale("2 2 2");
			%fish.rarity = "Huge";
            %weight += getRandom(6,12);
		}
		
		%fish.weight = %weight;
		%this.add(%fish);
		%fish.roam();
		%fish.fish_checkBait();
	}
}

function fishGroup::cleanUp(%this,%initialFishingSpot)
{
	%c = %this.getcount()-1;
	
	for(%i=%c;%i>=0;%i--)
	{
		%fish = %this.getObject(%i);
		
		if(isObject(%fish.biting))
		{
			continue;
		}
		
		if(isObject(%initialFishingSpot))
		{
			if(%fish.initialFishingSpot == %initialFishingSpot)
				%fish.delete();
		}
		else
			%fish.delete();
	}
}
	
//not used
function Fishing::spawnProjectile(%this,%player)
{
	if(isObject(%player))
	{
		%client = %player.client;
		
		%p = new projectile()
		{
			dataBlock = "fishingpoleprojectile";
			initialVelocity = "0 0 30";
			initialPosition = vectorScale(%player.getEyePoint(),1);
			sourceObject = %player;
			sourceClient = %client;
			client = %client;
			lifetime=60000;
			controllingClient = %client;
		};
		
		$class::camera.schedule(10,orbitobj,%client,%p);
	}
}

function Fishing::getLineDist(%this,%clientOrFishingSpot)
{
	%cofs = %clientorfishingspot;
	
	if(%cof.active)
	{
		return vectorDist(%cofs.owner.location,%cofs.location);
	}
	else
	{
		return vectorDist(%cofs.fishingSpot.location,%cofs.player.position);
	}
}

function Fishing::getUserHook(%this,%client)
{
	%mc = missioncleanup;
	%mcc = missioncleanup.getcount()-1;
	%projectile = "null";
	
	for(%i=%mcc;%i>=0;%i--)
	{
		%mco = %mc.getobject(%i);
		
		if(isObject(%mco))
		{
			if(%mco.getclassname() $= "projectile")
			{
				//talk("found a projectile with db of " @ %mco.dataBlock @ " owner = " @ %mco.sourceObject.getclassname());
				%mcoso = %mco.sourceObject;
				if(isObject(%mcoso))
				{
					%mcosc = %mcoso.client;
					if(%mcosc == %client)
						return %mco;
				}
			}
		}
	}
	return %projectile;
}

function Fishing::hookHelper(%this,%client)
{
	%hook = $class::fishing.getUserHook(%client);
	
	//talk("found hook ( " @ %hook @ " ) for " @ %client.name);
	
	if(%hook !$= "null")
	{
		$class::camera.orbitobj(%client,%hook);
	}
}

//aiplayer functions

function aiplayer::fish_checkBait(%this)
{
	cancel(%this.checkBaitLoop);
	
	if(!isObject(%this.initialFishingSpot))
	{
		%this.schedule(1,delete);
	}
	
	//make dynamic this.type
	%type = %this.type;
	
	//size type randommodifier
	%rm = getRandom(0,5);
	%size = getword(%this.getScale(),2);
	
	if(striPos(%size,".") != -1)
	{
		%sizeMod = getRandom(0, strReplace(strchr(%size,"."),".",""));
		%size = strReplace(%size,strchr(%size,"."),"");
	}
	else
		%sizeMod = 0;
	
	
	
	%difficulty["salmon"] = getRandom(1,5);
	
	if(getRandom(0,10) <= 3)
		%difficulty = %difficulty[%type] + getRandom(0,3);
	else
		%difficulty = %difficulty[%type];
	
	%total = %rm + %size + %difficulty + %sizeMod;
	
	
	//%random = getRandom(
	
	//talk("( " @ %sizemod @ " ) Fish size " @ %size @ " check " @ %total @ " <= \c63 ?");
	
	if(%total <= (5 + getRandom(0,4)))
	{
		if(getRandom(0,1) == 1)
		{
			%bait = %this.fish_getClosestBait();
			if(%bait !$= "")
			{
				//talk("Fish interested in bait of " @ %bait.owner.name);
				if(getSimTime() - %this.initialFishingSpot.time_created > 2500)
					%this.fish_biteBait(%bait);
				
				
			}
			//commandtoclient(%
		}
		else
		{
			//talk("Fish was interested but changed mind");
		}
	}
	else
	{
		
	}
	
	%this.checkBaitLoop = %this.schedule(7000,fish_checkBait);
}

function aiPlayer::fish_biteBait(%this,%bait)
{
	if(isObject(%bait.bitten))
	{
		return false;
	}
	
	cancel(%this.roamLoop);
	
	talk("A \c2" @ %this.rarity @ " " @ %this.type @ " \c6Fish is biting \c2" @ %bait.owner.name @ "'\c6s Line!");
	
	bottomPrint(%bait.owner,"<font:impact:22>\c2Use the numberpad keys 4 and 6 to stay inside of the green!\c2 Starting in \c16 \c2seconds!",5);
	
	%bait.bitten = %this;
	%this.biting = %bait;
	
	%this.setMoveDestination(%bait.location);
		//$class::camera.orbitobj(%bait.owner.player);
		

	%mod["normal","time"] = 5000;
	%mod["normal","difficulty"] = 3;
	
	%mod["big","time"] = 7500;
	%mod["big","difficulty"] = 6;
	
	%mod["huge","time"] = 9000;
	%mod["huge","difficulty"] = 9;
	
	%bait.owner.player.playthread(2,root);
	$class::camera.schedule(4000,orbitObj,%bait.owner,%bait.owner.player);
	%bait.owner.player.schedule(4000,adjustRodFor,%mod[%this.rarity,"time"]);
	
	%difficulty = %mod[%this.rarity,"difficulty"];
	%time = %mod[%this.rarity,"time"];
	
	$class::stabilizer.schedule(6000,begin,%bait.owner,%difficulty,%time,"fishing");
}

function aiPlayer::fish_getClosestBait(%this)
{
	%p = %this.position;
	
	%fp = $class::fishing.fishingSpot;
	%bait = "";
	
	for(%i=0;%i<%fp.getCount();%i++)
	{
		if(%fp.getObject(%i).active != 1)
			continue;
		if(%bait $= "")
			%bait = %fp.getObject(%i);
		else
		if(vectorDist(%p,%fp.getObject(%i).location) < vectorDist(%bait.location,%p))
		{
			%bait = %fp.getObject(%i);
		}
	}
	return %bait;
}

//_aimRope( %player.ropeToolGhost[%i], %subPosA, %subPosB ); dynamic rope?
function aiplayer::fish_reelIn(%this,%player)
{
	cancel(%this.fish_reelInLoop);
	
	%pp = %player.position;
	%fp = %this.position;
	%this.playerFishing = %player;
	
	if(!isObject(%player))
		return false;
	
    // Caught fish, give player rewards
	if(vectorDist(%pp,%fp) <= 6)
	{
		%player.playthread(0,root);
		%player.playthread(2,root);
		$class::camera.root(%player.client);
		$class::fishing.endFishing(%player.client);
		%fishItem = %player.client.inventory.addItem("Raw Salmon","1");
        if(isObject(%fishItem)) {
            if(%this.rarity $= "big" || %this.rarity $= "huge") {
                $class::chat.to_all(%player.client.name @ " has caught a fish weighing in at: \c4" @ %this.weight @ " \c6lbs!");
            }
        }
		%this.schedule(1,delete);
		return false;
	}
	
	if(getsimtime() - %player.lastReelTime >= 600)
	{
		%player.playThread(0,rodreel);
		%player.lastReelTime = getSimTime();
	}
	
	%this.setVelocity(vectorAdd(vectorScale(vectorSub(%pp,%fp),0.6),"0 0 2"));
	
	%this.fish_reelInLoop = %this.schedule(100,fish_reelIn,%player);
}

function player::adjustRod(%this)
{
	%ran = getRandom(1,2);
	if(%ran == 0)
		%this.playthread(0,swordparry);
	else
	if(%ran == 1)
		%this.playthread(0,fishfight1);
	else
	if(%ran == 2)
		%this.playthread(0,fishfight2);
	
	%this.schedule(getRandom(300,600),playthread,0,root);
}

function player::adjustRodFor(%this,%time)
{
	cancel(%this.adjustRodLoop);
	
	%this.adjustRod();
	
	if(%this.rodAdjustTime $= "")
		%this.rodAdjustTime = getSimTime();
	
	//talk("time = " @ %this.rodAdjustTime @ " - " @ getSimTime() @ " = " @ getSimTime() - %player.rodAdjustTime);
	
	if(getSimTime() - %this.rodAdjustTime >= %time)
	{
		%this.rodAdjustTime = "";
		return false;
	}
	
	%this.adjustRodLoop = %this.schedule(getRandom(600,1000),adjustRodFor,%time);
}

package class_Fishing
{
	function fishingPoleImage::onRelease(%this,%a,%b,%c,%d,%e)
	{
			%r = parent::onRelease(%this,%a,%b,%c,%d,%e);
			$class::Fishing.schedule(500,hookHelper,%a.client);
			return %r;
	}
	
	function stabilizer::end(%this,%client)
	{
		%p = parent::end(%this,%client);
		
		if(isObject(%client.fishingSpot.bitten))
		{
			if(%client.stabilizer["identifier"] $= "fishing")
			{
				if(%client.stabilizer["lastscore"] >= 20)
				{
					$class::camera.orbitobj(%client,%client.player);
					clearRopes(%client.bl_id);
					%client.fishingSpot.bitten.fish_reelIn(%client.player);

				}
				else
				{
					centerPrint(%client,"\c6The Fish got away!",5);
					%client.player.playthread(0,rodroot);
					$class::camera.orbitpos(%client,%client.fishingSpot.location);
					bottomprint(%client,"\c6Your fishing spot is \c2Active");
					%f = %client.fishingSpot.bitten;
					%client.fishingSpot.bitten = "";
					%f.biting = "";
					%f.roam();
				}
			}
		}
		
		return %p;
	}
};
activatePackage(class_Fishing);