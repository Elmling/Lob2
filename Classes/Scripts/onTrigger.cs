package scripts_onTrigger
{
	function fancyswordimage::onPrefire(%a,%b,%c)
	{
		if(%b.blocking)return false;
		
		//$class::attackMode.setPrint("Attack: Single Slash");
		
		%p = parent::onPrefire(%a,%b,%c);
		
		return %p;
	}
	
	function fancyswordimage::onPrefire2(%a,%b,%c)
	{
		if(%b.blocking)return false;
		
		//$class::attackMode.setPrint("Attack: Double Slash");
		
		%p = parent::onPrefire2(%a,%b,%c);
		
		return %p;
	}
	
	function fancyswordimage::onPrefire3(%a,%b,%c)
	{
		if(%b.blocking)return false;
		
		//$class::attackMode.setPrint("Attack: Tripple Slash");
		
		%p = parent::onPrefire3(%a,%b,%c);
		
		return %p;
	}
	
	function weaponImage::onFire(%this,%a,%b,%c,%d)
	{
		$class::string.set(%this.getName());
		
		if($class::string.contains("longbow"))
		{
			if(%a.pulltime > 1300)
			{
				centerprint(%a.client,"<just:right>\c2<font:impact:30>\c0Arrow Accuracy \c2100%",2);
			}
			else if(%a.pulltime >= 1000 && %a.pulltime <= 1299)
			{
				centerprint(%a.client,"<just:right>\c2<font:impact:30>\c0Arrow Accuracy \c285%",2);
				%accuracy = 85;
			}
			else if(%a.pulltime >= 700 && %a.pulltime <= 999)
			{
				centerprint(%a.client,"<just:right>\c2<font:impact:30>\c0Arrow Accuracy \c275%",2);
				%accuracy = 75;
			}
			else if(%a.pulltime >= 400 && %a.pulltime <= 699)
			{
				centerprint(%a.client,"<just:right>\c2<font:impact:30>\c0Arrow Accuracy \c265%",2);
				%accuracy = 65;
			}
			else
			{
				centerprint(%a.client,"<just:right>\c2<font:impact:30>\c0Arrow Accuracy \c250%",2);
				%accuracy = 50;
			}
			
			%vel = vectorscale(%a.getmuzzleVector(0),80);
			
			if(%accuracy == 85)
			{
				%vel = vectoradd(%vel, getRandom(-2,2) SPC getrandom(-2,2) SPC 0);
				%vel = vectorscale(vectornormalize(%vel),90);
			}
			else if(%accuracy == 75)
			{
				%vel = vectoradd(%vel, getRandom(-5,5) SPC getrandom(-5,5) SPC 0);
				%vel = vectorscale(vectornormalize(%vel),30);
			}
			else if(%accuracy == 65)
			{
				%vel = vectoradd(%vel, getRandom(-5,5) SPC getrandom(-5,5) SPC 0);
				%vel = vectorscale(vectornormalize(%vel),25);
			}
			else if(%accuracy == 50)
			{
				%vel = vectoradd(%vel, getRandom(-8,8) SPC getrandom(-8,8) SPC 0);
				%vel = vectorscale(vectornormalize(%vel),15);
			}
			//spawn projectile
			%proj = new Projectile()
			{
				dataBlock = fancybowprojectile;
				initialPosition = %a.getmuzzlepoint(0);
				initialVelocity = %vel;
				client = %a.client;
				sourceObject = %a;
			};
			return;			
		}
		else
		if($class::string.contains("shortbow"))
		{
			if(%a.pulltime > 1000)
			{
				centerprint(%a.client,"<just:right>\c2<font:impact:30>\c0Arrow Accuracy \c2100%",2);
			}
			else if(%a.pulltime >= 700 && %a.pulltime <=  999)
			{
				centerprint(%a.client,"<just:right>\c2<font:impact:30>\c0Arrow Accuracy \c285%",2);
				%accuracy = 85;
			}
			else if(%a.pulltime >= 400 && %a.pulltime <= 699)
			{
				centerprint(%a.client,"<just:right>\c2<font:impact:30>\c0Arrow Accuracy \c275%",2);
				%accuracy = 75;
			}
			else if(%a.pulltime >= 100 && %a.pulltime <= 399)
			{
				centerprint(%a.client,"<just:right>\c2<font:impact:30>\c0Arrow Accuracy \c265%",2);
				%accuracy = 65;
			}
			else
			{
				centerprint(%a.client,"<just:right>\c2<font:impact:30>\c0Arrow Accuracy \c250%",2);
				%accuracy = 50;
			}
			
			%vel = vectorscale(%a.getmuzzleVector(0),80);
			
			if(%accuracy == 85)
			{
				%vel = vectoradd(%vel, getRandom(-2,2) SPC getrandom(-2,2) SPC 0);
				%vel = vectorscale(vectornormalize(%vel),55);
			}
			else if(%accuracy == 75)
			{
				%vel = vectoradd(%vel, getRandom(-5,5) SPC getrandom(-5,5) SPC 0);
				%vel = vectorscale(vectornormalize(%vel),15);
			}
			else if(%accuracy == 65)
			{
				%vel = vectoradd(%vel, getRandom(-5,5) SPC getrandom(-5,5) SPC 0);
				%vel = vectorscale(vectornormalize(%vel),10);
			}
			else if(%accuracy == 50)
			{
				%vel = vectoradd(%vel, getRandom(-8,8) SPC getrandom(-8,8) SPC 0);
				%vel = vectorscale(vectornormalize(%vel),8);
			}
			//spawn projectile
			%proj = new Projectile()
			{
				dataBlock = fancybowprojectile;
				initialPosition = %a.getmuzzlepoint(0);
				initialVelocity = %vel;
				client = %a.client;
				sourceObject = %a;
			};
			return;
			//talk("pull time - > " @ %a.pulltime);
		}
		
		if(%a.parry)
		{
			%a.playthread(0,"swordattack4");
		}
		else
		{
			if(%a.blocking)
			{
				//talk("blocking");
				%a.playthread(0,"root");
				return false;
			}
			else
			{
				%p = parent::onFire(%this,%a,%b,%c,%d);
			
				return %p;
			}
		}
	}
	
	function observer::onTrigger(%this,%obj,%trigger,%pressed)
	{	
		%p = parent::onTrigger(%this,%obj,%trigger,%pressed);
        
        %client = %obj.getControllingClient();
        
        if(%trigger == 0 && !%pressed) {
            // $class::menuSystem.show(%client,"CameraBuilderOnClickBrick");
        }
        
		//talk(%obj.getclassname());
		if(%trigger == 4 && %pressed)
		{
			if(%client.fishingSpot.active)
			{
				$class::fishing.endFishing(%client);
				//$class::fishing.fishgroup.cleanup();
			}
		}
		
		return %p;
	}
	
	function armor::onTrigger(%this,%obj,%trigger,%pressed)
	{
				
		%p = parent::onTrigger(%this,%obj,%trigger,%pressed);
		//talk("trigger: " @ %trigger);
		if(isObject(%obj))
		{
			%client = %obj.client;
			//climbing mechanics
			if(%trigger == 2)
			{
				if(%pressed == 1 && !isObject(%obj.climb_zone))
				{
					//charge
					%can_climb = $class::climbing.can_climb(%obj);
					if(%can_climb)
					{
						%obj.climb_charge_time = getSimtime();
						// start the thingy
						$class::climbing.bottom_print(%obj.client);
					}
				}
				else if(%pressed == 0 && %obj.climb_charge_time !$= "")
				{
					//release
					//end the thingy
					$class::climbing.bottom_print_stop(%obj.client);
					%obj_time_hold_brick = getSimTime() - %obj.climb_charge_time;
					%obj.climb_charge_time = "";
					//talk(%obj_time_hold_brick);
					if(%obj_time_hold_brick > 1600)
					{
						%charge = 8;
					}
					if(%obj_time_hold_brick > 1200 && %obj_time_hold_brick <= 1600)
					{
						%charge = 6;
					}
					else if(%obj_time_hold_brick > 800 && %obj_time_hold_brick <= 1200)
					{
						%charge = 4;
					}
					else if(%obj_time_hold_brick <= 800)
					{
						%charge = 2;
					}
					
					%launch = 14;
					
					%obj.climb_zone.delete();
					%vel_behind = vectorScale(%obj.getForwardVector(),-4);
					%obj.schedule(1,setVelocity,getWords(%vel_behind,0,1) SPC %launch + %charge);
					%obj.playthread(0,leap);
					%obj.climbing_last_leap_time = getSimTime();
					//%obj.schedule(1000,playthread,0,root);
					$class::climbing.schedule(1000,try_root,%obj);
					serverplay3d(lobjump,%obj.gethackposition());
					$class::chat.to(%obj.client, "You can \c2Leap-Dash\c6 by using the Jet Key after launching from a \c2Climb \c6( \c4Right Click \c6To Cast )", 60000 * 10);
					//$class::chat.b_print(%obj.client,"Launch Strength: " @ %launch + %charge, 4);
					//centerprint(%client,"<just:right>\c6Climb Launch: \c2" @ %launch + %charge,2);
				}
			}
            if(%trigger == 4 && %pressed == 0) {
                %wn = %client.player.getMountedImage(0).getname();
                if(%wn !$= "") {
                    $class::string.set(strLwr(%client.player.getMountedImage(0).getname()));
                    if($class::string.contains("bow")) {
                        $class::combat.specials_arrowRain(%client);
                    }
                }
            }
			if(%trigger == 4 && %pressed == 0 && getSimTime() -  %obj.climbing_last_leap_time <= 1500 && (getSimTime() - %obj.climbing_last_leapdash_time > 1000)) {
				$class::chat.c_print(%obj.client, "<font:impact:24>\c5Leap-Dash", 3);
				serverplay3d(sound_leap_dash, %obj.position);
				//serverplay3d(sound_leap_dash, %obj.position);
				//$class::chat.to(%obj.client, "You cast Leap Dash! Allows extra mobility after climbing ( Right Click To Cast )", 60000 * 10);
				%obj.addVelocity("0 0 16");
				%obj.schedule(100, setVelocity, vectorScale(%obj.getForwardVector(),16));
				%obj.climbing_last_leapdash_time = getSimTime();
			}
			//fishing mechanics
			if(isObject(%client.player.getmountedimage(0)))
			{
				
				if(%client.player.getmountedimage(0).getname() $= "bowImage")
				{
					%proj = missioncleanup.getobject(missioncleanup.getcount()-1);
					if(%proj.getclassname() $= "projectile" && %client.orbitLine != true)
					{
						%client.camera.schedule(10,setOrbitMode,%proj,%proj.getTransform(),5,10,5,0);
						%client.setcontrolobject(%client.camera);
						%client.orbitLine = true;
					}
				}
			}
			
			//grappler mechanics
			if(isEventPending(%client.player.grapplerReelLoop))
			{
				if(%trigger == 4 && %pressed == 1)
				{
					messageClient(%client,'',"\c6Grappler has been disconnected via right click.");
					cancel(%client.player.grapplerReelLoop);
					%client.player.ropeToolPosA = "";
					%client.ropeToolGhostClear();
				}
			}
			
			//blocking mechanics
			if(%obj.dataBlock.getName() $= "lobArmor")
			{

				%shield = %obj.getMountedImage(1);
				if(isObject(%shield))
				{
					if(%shield.getName() $= "woodenShieldImage")
					{
						if(%trigger == 4 && %pressed == 1)
						{
							%obj.playthread(0,"shieldBlock");
							%obj.blocking = true;
						}
						else
						if(%obj.blocking == true)
						{
							%obj.playThread(0,"root");
							%obj.blocking = false;
						}
					}
				}
				else
				{
					%sword = %obj.getMountedImage(0);
					if(isObject(%sword))
					{
						if(%sword.getName() $= "fancySwordImage")
						{
							
							if(%trigger == 0 && %pressed == 1 && %obj.parry)
							{
								%obj.playthread(0,root);
								%obj.doSwordBlockAttack();
								%obj.parry=false;
								return;
							}
							else
							if(%trigger == 4 && %pressed == 1)
							{
								if(%obj.blockTimeUp())
								{
									serverplay3d(sworddrawsound,%obj.position);
									%obj.playthread(0,"root");
									%obj.playthread(1,"swordparry");
									%obj.blocking = true;
									%obj.lastBlockTime = $sim::time;
									%obj.schedule(1500,unblock);
								}
							}
							else
							if(%obj.blocking == true)
							{
								//%obj.playThread(0,"root");
								//%obj.blocking = false;
							}
						}
					}	
				}					
			}
		}
				
		//talk(%client.name @ " triggere: " @ %trigger @ " pressed: " @ %pressed);
		
		return %p;
	}
	
	function player::blockTimeUp(%this)
	{
		if(%this.lastblockTime $= "")
			return true;
		
		%blocktimeout = 2.5;
		
		if($sim::time - %this.lastBlockTime >= %blockTimeout)
			return true;
		else
			return false;
	}
	
	function player::unBlock(%this)
	{
		%this.playthread(1,"Root");
		%this.blocking = false;
	}
};
activatePackage(scripts_onTrigger);