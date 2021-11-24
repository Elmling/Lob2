package scripts_onProjectileHit
{	
	function fxDtsBrick::onProjectileHit(%this,%a,%b)
	{
		
		%p = parent::onProjectileHit(%this,%a,%b);
		%bn = %this.dataBlock.getname();
		%pn = %a.dataBlock.getName();
		//talk(%pn);
		if(%pn $= "shovelProjectile")
		{
			if(%bn $= $class::farming.seedGroup.soilBrick["dataBlock"])
			{
				%pos = %this.position;
				%collide = raycast.getCollidePoint(%a.sourceObject.client);
				%finalpos = getWords(%pos,0,1) SPC getWord(%collide,2);
				
				$class::menuSystem.getMenu("seeds").showMenu(%a.sourceObject.client);
				%a.sourceObject.client.plantPosition = %finalPos;
			}
		}
		else
		if(%pn $= "wateringCanProjectile")
		{
			//talk("bn = " @ %bn);
			if(striPos(%bn,"mound") != -1 || striPos(%bn,"stage") != -1)
			{
				%seed = %this.seed;
				if(isObject(%seed))
				{
					$class::farming.water(%a.sourceObject.client,%seed);
				} else {
					talk("cant water, no seed obj");
				}
			}
		}
		else
		if(%pn $= "defarrowprojectile")
		{
			if(%a.client.name $= "luie" || %a.client.name $= "elm" || %a.client.name $= "boba fett" || %a.client.name $= "jyvot" || %a.client.name $= "owl" || %a.client.name $= "rallyblock" || %a.client.name $= "DragonoidSlayer" || %a.client.name $= "khaz")
			{
				messageclient(%a.client,'',"\c2Test Grappler Connected To Location.");
				
				%a.client.player.grapplerReel(%this.position);
			}
		}
		else
		if(%pn $= "pickaxeprojectile")
		{
			if($class::mining.usingDataBlock[%bn])
			{
				$class::Mining.onHitRock(%b,%this,%a);
				//serverplay3d(swordhitsound,%this.position);
			}			
		}
		else
		if(%pn $= "hatchetProjectile")
		{
			if($class::woodCutting.usingDataBlock[%bn])
			{
				$class::woodCutting.onHitTree(%b,%this,%a);
				//serverplay3d(swordhitsound,%this.position);
			}
		}
		

		return %p;
	}
	
	function projectile::onAdd(%this,%a,%b,%c,%d,%e)
	{
		%pn = %this.dataBlock.getName();
		if(%pn $= "blowdartprojectile") {
			if(isObject(%this.sourceObject)) {
				if(%this.sourceObject.getClassName() $= "player") {
					%cl = %this.sourceObject.client;
					if(%cl.inventory.getItem("Wooden Blow Dart").amount > 0) {
						%cl.inventory.removeItem("Wooden Blow Dart", 1);
						$class::chat.c_print(%cl,"\c4" @ %cl.inventory.getItem("Wooden Blow Dart").amount @ " \c6Wooden Blow Darts",10);
					} else {
						$class::chat.c_print(%cl,"You need \c5Blow Darts\c6, craftable from \c5Maple\c6 Logs.");
						%this.delete();
						return false;
					}
				}
			}
			// %this.delete();
		}
		%p = parent::onAdd(%this,%a,%b,%c,%d,%e);
		
		%pn = %this.dataBlock.getName();
		//%p.dump();
		if(%pn $= "blowdartprojectile") {
			%this.sourceObject.client.blowdart_shoot_position = %this.sourceObject.position;
		} else if(%pn $= "fancybowprojectile") {
			%obj = rayCast.get(%a.client,100);
			%a.last_hit = %obj;
			%obj.last_hitter = %a;
		} else if(%pn $= "FancySwordProjectile") {
			if(%a.sourceObject.getClassName() $= "aiplayer") {
			} else {
				//player 
				%obj = rayCast.get(%a.client,6);
				%a.last_hit = %obj;
				%obj.last_hitter = %a;
				// check if object hit is a friendly npc
				if(isObject(%obj.npc_handle)) {
					if(%obj.npc_handle.type $= "npc") {
						return false;
					}
				}
				// object being hit is blocking?
				if(%obj.blocking) {
					// type of ai player?
					if(%obj.getClassName() $= "aiplayer") {
						//talk("An AI Player has blocked " @ %a.client.name @ "'s slash");
						%obj.fight(%a.client.player);
						%obj.parry=true;
						return false;				
					} else {
						//talk(%obj.client.name @ " blocked " @ %a.client.name @ "'s slash");
						%obj.parry=true;
						return false;
					}
				} else { //obj not blocking
					if(isObject(%obj))
						if(%obj.getclassname() $= "aiplayer")
						{
							%dmg = $class::damage.calc(%a);
							$class::combat.damage_ai(%obj, %dmg, %a.client.player);
							//%obj.setDamageLevel(%obj.getDamageLevel() + %dmg);
							%obj.fight(%a.client.player);
							$class::chat.b_print(%a.client,"You hit a \c2" @ %dmg @ " \c4" @ %obj.name @ " \c3HP: \c4" @ 100-%obj.getdamageLevel(),5);
							$class::exp.giveexp(%a.client,"Melee",%dmg*5);
							if(100-%obj.getdamageLevel() <= 0) {
								%obj.kill();
							}
							return false;
						} else if(%obj.getclassname() $= "player") {
							if(%obj.client.minigame !$= "") {
								if(%obj.client.minigame == %a.client.minigame) {
									%dmg = $class::damage.calc(%a);
									%dmg = mclamp(%dmg,1,45 - getRandom(0,15));
									$class::combat.damage(%obj.client, %dmg, %a.client.player);
									//%obj.setDamageLevel(%obj.getDamageLevel() + %dmg);
									// %obj.fight(%a.client.player);
									$class::chat.b_print(%a.client,"You hit a \c2" @ %dmg @ " \c4" @ %obj.name @ " \c3HP: \c4" @ 100-%obj.getdamageLevel(),5);
									$class::exp.giveexp(%a.client,"Melee",%dmg*5);
									if(100-%obj.getdamageLevel() <= 0) {
										$class::camera.orbitPos(%obj.client,%obj.position);
										//%obj.kill();
									}
									return false;
								}
							}
						}
				}
			}
			if(isObject(%a.client.player))
				if(%a.client.player.getclassname() $= "player")
					%this.delete();
		}
		
		return %p;
	}
	
	function blowdartprojectile::onCollision(%a,%b,%c,%d,%e,%f,%g) {
		//talk(%b SPC %c SPC %d);
		if(%b.sourceObject.getClassName() $= "player") {
		%attacking_object = %b.sourceObject;
		//talk("pos? " @ %e);
		if(isObject(%c))
		{
			if(%c.getClassname() $= "aiplayer")
			{
				if(isObject(%c.npc_handle)) {
					if(%c.npc_handle.type $= "npc") {
						return false;
					}
				}
				//talk("doing dmg here " @ %c.getDamageLevel());
				if(isObject(%attacking_object)) {
					
					%shoot_dist = mfloor(vectorDist(%c.position,%attacking_object.client.blowdart_shoot_position));
					if(%shoot_dist > 65) {
						$class::chat.to_all("\c5" @ %attacking_object.client.name @ " \c6has hit a \c4" @ %c.dataBlock @ " \c6from a distance of \c5" @ %shoot_dist @ " \c6yards!",10000);
					}
					%dmg = $class::damage.calc(%attacking_object);
					%dmg  = %dmg + getRandom(0, %dmg * 2);
					%c.setDamageLevel(%c.getDamageLevel() + %dmg);
					$class::chat.b_print(%attacking_object.client,"You hit a \c2" @ %dmg @ " \c4" @ %c.name @ " \c3HP: \c4" @ 100-%c.getdamageLevel(),5);
					$class::exp.giveexp(%attacking_object.client,"Range",%dmg*5);
					%text = $class::worldText.show_at(vectorAdd(%c.getEyePoint(),getRandom(-2,2) SPC getRandom(-2,2) SPC 0),%dmg @ " Damage",2500);
					%text.setShapeNameDistance(300);
					if(100-%c.getdamageLevel() <= 0) {
						%c.kill();
					}
				}
			}
		}
		} else if(%b.sourceObject.getClassName() $= "aiPlayer") {
			%ai = %b.sourceObject;
			%hitting_obj = %c;
			%hitting_cl = %c.client;
			if(%hitting_obj.blocking) {
				return false;
			}
			if(%ai.level $= "") {
				if($class::bots.level[%ai.dataBlock] $= "") {
					talk("No level set for db " @ %ai.dataBlock);
				} else {
					%lo = $class::bots.level[%ai.dataBlock];
					%ai.level = getRandom(%lo.index(0), %lo.index(1));
				}
			}
			%dmg = mfloor(%ai.level/3) + getRandom(0,4);
			$class::combat.damage(%hitting_cl, %dmg);
			//talk("dmg = " @ %dmg);
		}
		%p = parent::onCollision(%a,%b,%c,%d,%e,%f,%g);
		return %p;
	}
	
	function fancySwordProjectile::onCollision(%a,%b,%c,%d,%e,%f,%g) {
		if(%b.sourceObject.getClassName() $= "aiPlayer") {
			%ai = %b.sourceObject;
			%hitting_obj = %c;
			%hitting_cl = %c.client;
			if(%hitting_obj.blocking) {
				return false;
			}
			if(%ai.level $= "") {
				if($class::bots.level[%ai.dataBlock] $= "") {
					talk("No level set for db " @ %ai.dataBlock);
				} else {
					%lo = $class::bots.level[%ai.dataBlock];
					%ai.level = getRandom(%lo.index(0), %lo.index(1));
				}
			}
			%dmg = mfloor(%ai.level/3) + getRandom(0,4);
			$class::combat.damage(%hitting_cl, %dmg);
			//talk("dmg = " @ %dmg);
		}
		%p = parent::onCollision(%a,%b,%c,%d,%e,%f,%g);

		//talk("b class "  @ %b.getclassname());
		
		//if(

		return %p;

	}
	
	function fancybowprojectile::onCollision(%a,%b,%c,%d,%e,%f)
	{
		// %p = parent::onCollision(%a,%b,%c,%d,%e,%f);
		
		// talk("coll = " @ %b.sourceObject);
		%attacking_object = %b.sourceObject;
		//talk("pos? " @ %e);
		if(isObject(%c))
		{
			if(%c.getClassname() $= "aiplayer")
			{
				if(isObject(%c.npc_handle)) {
					if(%c.npc_handle.type $= "npc") {
						return false;
					}
				}
				//talk("doing dmg here " @ %c.getDamageLevel());
				if(isObject(%attacking_object)) {
					%dmg = $class::damage.calc(%attacking_object);
					%c.setDamageLevel(%c.getDamageLevel() + %dmg);
					$class::chat.b_print(%attacking_object.client,"You hit a \c2" @ %dmg @ " \c4" @ %c.name @ " \c3HP: \c4" @ 100-%c.getdamageLevel(),5);
					$class::exp.giveexp(%attacking_object.client,"Range",%dmg*5);
					if(100-%c.getdamageLevel() <= 0) {
						%c.kill();
					}
				}
			}
		}
	}
	
	function fishingPoleProjectile::onCollision(%a,%b,%c,%d,%e,%f)
	{
		%p = parent::onCollision(%a,%b,%c,%d,%e,%f);
		
		//talk("coll = " @ %b.sourceObject);
		//talk("pos? " @ %e);
		if(isObject(%c))
		{
			//talk("point = " @ %e);
			if(!$class::fishing.point_in_water(%e))
			{
				//talk("point not in water source object cl = " @ %b.sourceObject.client);
				%client = %b.sourceObject.client;
				%client.setControlObject(%client.player);
				%client.schedule(1000,setcontrolobject,%client.player);
				%client.player.schedule(10,playthread,0,root);
				//$class::fishing.schedule(10,endfishing,%client);
				return false;
			}
			 
			%group = _getRopeGroup(getSimTime(), %b.sourceObject.client.getBLID(), "");
			
			if( ( %mi = %b.sourceObject.getMountedImage(0) ) != 0)
			{
				%playerPoint = %b.sourceObject.getMuzzlePoint(0);
			}
			else
				%playerPoint = %b.sourceObject.getEyePoint();
			
			createRope(%playerPoint, %e, "1 1 1 1", "0.05", "2.5", %group);
			$class::fishing.fishingSpot.newSpot(%b.sourceObject.client,%e);
			$class::fishing.fishGroup.newFish(%e);
		}
		return %p;
	}
	function color0paintprojectile::onCollision(%a,%b,%c,%d,%e,%f)
	{
		return false;
		//if(%c.getClassName() !$= "aiplayer" && %c.getclassname() !$= "player")
		if(isObject(%c.npc_handle)) {
			if(%c.npc_handle.type $= "npc") {
				return false;
			}
		}
		
		%p = parent::onCollision(%a,%b,%c,%d,%e,%f);
		
		//talk("coll = " @ %b.sourceObject);
		
		if(isObject(%c))
		{
			if(%b.fakePaint)
			{
				if(%c.getClassName() $= "aiPlayer")
				{
					%c.attacker = %b.sourceObject;
				}
				else
				if(%c.getClassName() $= "player")
				{
					%c.attacker = %b.sourceObject;
				}
			}
		}
		
		return %p;
	}
};
activatePackage(scripts_onProjectileHit);