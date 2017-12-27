//***** class description start *****
//		author: Elm
//
//		description - Artificial Intelligence Class
//		Bots are loaded from ./saves/bots/bots.cs
//
//***** class description end ***** 

//***** bots class *****

//initialize container/class/group
if(!isObject($class::bots))
	$class::bots = new scriptGroup(bots);

//name: onAdd
//description: when the class is first created
function bots::onAdd(%this)
{
	//check if this class exists already
	if(%this.getCount() > 0)
	{
		//It does, we need to remove it to avoid memory leaks
		if(%this.getId() !$= $class::bots.getid())
		{
			//cleanup
			$class::bots.clearbots();
			$class::bots.delete();
			$class::bots = %this;
		}
	}
}

//name: populate
//description: populates all bots in a given area
function bots::populate(%this,%area)
{
	if((%c=%this.getCount()) > 0)
	{
		for(%i=0;%i<%c;%i++)
		{
			%node = %this.getObject(%i);
			//make sure it's an actual node
			if(%node.position !$= "")
			{
				//intialize
				%areafinal = false;
				
				if(%area !$= "")
				{
					//Check if the node's area value matches
					if(%node.area $= %area)
					{
						//Declare new ai player
						%b = new aiplayer()
						{
							nodeMemory = %node;
							name = %node.name;
							dataBlock = playerStandardarmor;
							scale = "1 1 1";
							position = %node.position;
							randomTaskTimeout = getRandom(6000,1200);
							bots = %this;
							area = %area;
						};
						
						%node.bot = %b;
						%node.area = %area;
						
						%b.cleartask();							
					}
				}
				else
				{
					//Declare new ai player
					%b = new aiplayer()
					{
						nodeMemory = %node;
						name = %node.name;
						dataBlock = playerStandardarmor;
						scale = "1 1 1";
						position = %node.position;
						randomTaskTimeout = getRandom(6000,1200);
						bots = %this;
					};
					
					%node.bot = %b;
					
					%b.cleartask();
				}
			}
		}
	}
}

//name: clearBots
//description: Clears all bots in a given area
function bots::clearBots(%this,%area)
{
	%c = %this.getCount();
	for(%i=0;%i<%c;%i++)
	{
		%nm = %this.getObject(%i);
		%b = %nm.bot;
		if(isobject(%nm.bot))
			if(%area !$= "")
			{
				if(%nm.area $= %area)
					%nm.bot.delete();
			}
			else
			%nm.bot.delete();
		
	}
	
	for(%i=%this.getcount()-1;%i>=0;%i--)
	{
		if(%area !$= "")
		{
			if(%this.getobject(%i).area $= %area)
				%this.getObject(%i).delete();
		}
		else
		{
			%obj = %this.getObject(%i);
			if(isObject(%obj))
				%obj.delete();
		}
	}
}

//name: newBot
//description: Spawns a new bot with the name specified at the %player's position
function bots::newBot(%this,%name,%player)
{
	if(!isobject(%player))return false;
	if(%name $= "") return false;
	
	%b = new aiplayer()
	{
		name = %name;
		dataBlock = playerStandardarmor;
		scale = "1 1 1";
		position = %player.position;
		name = %name;
		randomTaskTimeout = getRandom(6000,1200);
		bots = %this;
	};
	
	%nodeMemory = new scriptGroup()
	{
		bot = %b;
		class = nodeMemory;
		cap = 8;
		name = %b.name;
		position = %b.position;
	};
	
	%b.nodeMemory = %nodeMemory;
	%b.clearTask();
	%this.add(%nodeMemory);
	return %b;		
}

//Deprecated
//*******************************
function new_bot(%name,%player)
{
	if(!isobject(%player))return false;
	if(%name $= "") return false;
	
	%nodeMemory = new scriptGroup()
	{
		class = nodeMemory;
		cap = 8;
	};
	%b = new aiplayer()
	{
		name = %name;
		dataBlock = playerStandardarmor;
		scale = "1 1 1";
		position = %player.position;
		name = %name;
		nodeMemory = %nodeMemory;
		randomTaskTimeout = getRandom(6000,1200);
	};
	%b.clearTask();
	missioncleanup.add(%nodeMemory);
	bots.add(%b);
	return %b;
}
//*********************************

//name: getClosestPlayer
//description: Returns the closest player
function aiplayer::getClosestPlayer(%this)
{
	%bp = %this.position;
	%c = clientgroup.getCount();
	for(%i=0;%i<%c;%i++)
	{
		%cl = clientgroup.getobject(%i);
		if(stripos(%cl.name,$bob::playerIgnore) >= 0)
			continue;
		if(isObject(%cl.player))
		{
			if(%temp $= "")
			{
				%temp = %cl.player;
			}
			else
			if(vectorDist(%temp.position,%bp) > vectorDist(%cl.player.position,%bp))
			{
				%temp = %cl.player;
			}
		}
	}
	return %temp;
}

//nodeRunToPos
//description: Attempts to run to %pos by using existing nodes
//Method used is similiar to a*
function aiplayer::nodeRunToPos(%this,%pos)
{
	%this.setmovetolerance(0);
	%mask = $TypeMasks::FxBrickAlwaysObjectType;
	initContainerRadiusSearch(%this.position, "20", %mask);
	
	//Attempt to populate a node list in 20 studs near the aiPlayer
	while(isObject(%brick = containerSearchNext()))
	{
		if(%brick.getDataBlock().getName() $= "brick1x1fdata" && %brick.colorID == 3)
			%brickList = %brickList SPC %brick;
	}
	
	%c = getWordCount(%brickList);
	if(%c > 0)
	{
		//Loop through the node list that was built
		for(%i=0;%i<%c;%i++)
		{
			%b = getWord(%bricklist,%i);
			//Checks
			//-------------------------
			if(%this.nodeMemory.isInMemory(%b.position))
				continue;
			if(!%this.canSee(%b.position))
				continue;
			if(mabs(getWord(%this.position,2) - getWord(%b.position,2)) > 6)continue;
			//-------------------------
			
			if(%closest $= "")
			{
				%closest = %b;
			}
			else
			{
				//vector between aiposition and current brick position
				%vd1 = vectorDist(%this.position,%b.position);
				//vector between current brick position and destination pos
				%vd2 = vectorDist(%b.position,%pos);
				
				//vector between aiposition and closest brick position
				%cmpvd1 = vectorDist(%this.position,%closest.position);
				//vector between closest brick position and destination pos
				%cmpvd2 = vectorDist(%closest.position,%pos);
				
				if(%vd2 <= %cmpvd2)
				{
					%closest = %b;
				}
			}
		}
	}
	else
	{
		%this.clearTask();
		return false;
	}
	%this.nodeMemory.addToMemory(%closest.position,%closest);
	%this.currnode = %closest.position;
	%this.setMoveDestination(%closest.position);
	%closest.setcolor(2);
	%this.nodeMonitor(%pos);
}

//name: nodeMonitor
//description: Monitors where the aiPlayer is in conjunction with nodes
function aiplayer::nodeMonitor(%this,%pos)
{
	cancel(%this.nodeMonitorLoop);
	
	if(vectorDist(%this.currnode,%this.position) <= 1.8)
	{
		%zsub = getWord(%this.position,2) - getWord(%pos,2);
		%zsub = mabs(%zsub);
		if(vectorDist(%this.position,%pos) <= 10 && %zsub <= 3)
		{
			%this.currnode = "";
			%this.finalnode = "";
			%this.clearTask("movement");
			
			return true;
		}
		%this.currnode = "";
		%this.nodeAttempts=0;
		%this.setMoveDestination(%vecLoc);
		%this.nodeRunToPos(%pos);
		return true;
	}
	
	
	if(mfloor(mabs(getWord(%this.getVelocity(),0))) == 0 && mfloor(mabs(getWord(%this.getVelocity(),1))) == 0)
	{
		%this.nodeAttempts++;
	}
	
	if(%this.nodeAttempts >= 30)
	{
		%this.nodeAttempts=0;
		%this.nodeRunToPos(%pos);
		return true;
	}
	
	%this.nodeMonitorLoop = %this.schedule(10,nodeMonitor,%pos);
}

//name: canSee
//description: Simple raycast from aiPlayer to %node.position
function aiPlayer::canSee(%this,%node)
{
		%a = %this;
		%eyepoint = %this.getEyePoint();
		%raycast = containerRayCast(%eyePoint,%node,$TypeMasks::all, %a);
		%o = getWord(%raycast,0);
		if(isObject(%o) && %o.position !$= %node)
		{	
			return false;
		}
		return true;
}

//name: roam
//description: Makes the aiPlayer roam
function aiplayer::roam(%this)
{
	if(!isObject(%this.roam))
	{
		%this.newRoamObject();
	}
	
	cancel(%this.roamloop);
	
	%roamObj = %this.roam;
	%homePos = %roamObj.homeposition;
	%radius = %roamObj.radius;
	%nradius = %radius*-1;
	if(%roamObj.destination $= "")
	{
		
		%walkPos = vectorAdd(%homePos,getRandom(%nradius,%radius) SPC getRandom(%nradius,%radius) SPC 0);
		%roamObj.destination = %walkPos;			
	}
	else
	if(vectorDist(%roamObj.destination,%this.position) <= 3)
	{
		
		%this.setMoveDestination(%this.position);
		if(%this.roam.lookAtPlayersBoolean)
		{
			%cp = %this.getClosestPlayer();
			if(isObject(%cp))
			{
				%vd = vectorDist(%cp.position,%this.position);
				if(%vd <= 8)
				{
					%this.setAimObject(%cp);
				}
			}
		}
		%ran = getRandom(1000,5000);
		%roamObj.destination = "";
		%this.schedule(%ran-200,clearAim);
		%this.roamLoop = %this.schedule(%ran,roam);
		return true;
	}
	
	%this.setMoveDestination(%roamObj.destination);
	
	
	%this.roamloop = %this.schedule(500,roam);
}

//name: newRoamObject
//description: Optional, but allows you to modify/declare the aiPlayer's roam properties.
//Alternatively, you can modify a roam object:
//aiPlayer.roamObject.properties...
function aiplayer::newRoamObject(%this,%homePosition,%radius,%lookAtPlayersBoolean)
{
	if(isObject(%this.roam))
		%this.roam.delete();
	if(%homePosition $= "")
		%homePosition = %this.position;
	if(%radius $= "")
		%radius = 15;
	if(%lookAtPlayersBoolean $= "")
		if(getRandom(0,1) == 1)
			%lookatplayersboolean = true;
		else
			%lookatplayersboolean = false;
	
	%roamObject = new scriptObject()
	{
		isRoamObject = true;
		bot = %this;
		homePosition = %homePosition;
		radius = %radius;
		lookAtPlayersBoolean = %lookatplayersboolean;
	};
	
	missionCleanup.add(%roamObject);
	
	%this.roam = %roamObject;
			
}

//name: follow
//description: Follow a playerobject
function aiplayer::follow(%this,%cp)
{
	cancel(%this.followLoop);
	
	if(!isObject(%cp))return false;
	
	%vd = vectorDist(%this.position,%cp.position);
	if(%vd > 120)
	{
		%this.setVelocity("0 0 0");
		%this.setTransform(vectorAdd(%cp.position,"0 0 10"));
	}
	else
	if(%vd > 6)
	{
		%ran = getRandom(0,10);
		if(%ran > 4 && %ran <= 6)
			%this.spamHands();
			
		%pz = getWord(%cp.position,2);
		%bz = getWord(%this.position,2);
		if((%pz-%bz) > 2)
		{
			%this.doJump();
		}
		%this.setMoveDestination(%cp.position);
		%this.setDest = "";
		%this.setAimobject(%cp);
		
	}	
	else
	{
		if(%this.isFighting)
		{
			%this.clearmovex();
			%this.clearmovey();
			if(%vd > 1)
			{
				%ran = getRandom(0,10);
				if(%ran > 4 && %ran <= 6)
					%this.spamHands();
					
				%pz = getWord(%cp.position,2);
				%bz = getWord(%this.position,2);
				if((%pz-%bz) > 2)
				{
					%this.doJump();
				}
				%this.setMoveDestination(%cp.position);
				%this.setDest = "";
				%this.setAimobject(%cp);					
			}
			else
			{

				if(%this.setDest $= "")
				{
					%this.setdest=true;
					%this.setMoveDestination(%this.position);
					%this.setAimobject(%cp);
				}
			}
		}
		else
		{
				if(%this.setDest $= "")
				{
					%this.setdest=true;
					%this.setMoveDestination(%this.position);
					%this.setAimobject(%cp);
				}				
		}
	}
	%this.followLoop = %this.schedule(400,follow,%cp);
}

//name: chase
//description: Chase a player
function aiplayer::chase(%this)
{
	cancel(%this.chaseloop);
	%cp = %this.getClosestPlayer();
	
	if(!isObject(%cp))return false;
	
	%vd = vectorDist(%this.position,%cp.position);
	if(%vd > 120)
	{
		%this.setVelocity("0 0 0");
		%this.setTransform(vectorAdd(%cp.position,"0 0 10"));
	}
	else
	if(%vd > 6)
	{
		%ran = getRandom(0,10);
		if(%ran > 4 && %ran <= 6)
			%this.spamHands();
			
		%pz = getWord(%cp.position,2);
		%bz = getWord(%this.position,2);
		if(%pz > %bz)
		{
			%this.doJump();
		}
		%this.setMoveDestination(%cp.position);
		%this.setDest = "";
		%this.setAimobject(%cp);
		
	}
	else
	{

		if(%this.setDest $= "")
		{
			%this.setdest=true;
			%this.setMoveDestination(%this.position);
			%this.setAimobject(%cp);
		}
	}
	
	%this.chaseloop = %this.schedule(500,chase);
}

//name: jump
//description: Makes the aiPlayer jump
function aiplayer::doJump(%this)
{
	%time = getsimtime();
	if(%time - %this.lastJumpTime > 2500)
	{
		if(mfloor(getword(%this.getVelocity(),2)) == 0)
		{
			%this.addVelocity("0 0 13");
			%this.lastJumpTime = %time;
		}
	}
}

//name: findBotByName
//description: Finds a bot by name
function findBotByName(%name)
{
	%c = bots.getcount();
	for(%i=0;%i<%c;%i++)
	{
		%o = bots.getobject(%i);
		if(isobject(%o))
			if(%o.getclassname() $= "aiplayer")
				if(%o.name $= %name)
					return %o;
	}
	return false;
}

//name: spamHands
//description: Simulates spam clicking the mouse
function aiplayer::spamHands(%this)
{
	%time = getsimtime();
	if(%time - %this.lastSpamTime > 1500)
	{		
		%this.lastspamtime = %time;
		%this.playthread(0,activate2);
		%this.schedule(300,playthread,0,activate2);
		%this.schedule(600,playthread,0,activate2);
		%this.schedule(900,playthread,0,activate2);
	}
}

//name: setShName
//description: Set's the aiPlayer's shape name.
function aiplayer::setshname(%this,%n)
{
	//This requires a password that
	//has been removed since it is private.
	%this.setshapename(%n,8564862);
}

//name: clearTask
//description: Clears the aiPlayers current task, and sets them to Idle.
function aiplayer::clearTask(%this,%keep)
{
	cancel(%this.chaseloop);
	cancel(%this.roamloop);
	cancel(%this.nodeMonitorLoop);
	cancel(%this.followloop);
	%this.clearaim();
	if(%keep !$= "movement")
		%this.setMoveDestination(%this.position);
	%this.roamHomePos="";
	cancel(%this.fightloop);
	%this.firstfight="";
	%this.timenearenemy="";
	%this.isFighting="";
	%this.currnode = "";
	%this.finalnode = "";
	%this.roam.destination="";
	%this.setshname(%this.name @ " - (IDLE)");
	%this.playthread(0,root);
	%this.setImageTrigger(0,0);
	%this.nodeAttempts=0;
	%this.setmovetolerance(0);
	
}

//name: newTask
//description: If the tasks exists, the aiplayer will perform it
function aiPlayer::newTask(%this,%task,%arg)
{
	if(isfunction("aiplayer",%task))
	{
		if(getWordCount(%arg) > 1)
			%arg = "\"" @ %arg @ "\"";
		eval("%this." @ %task @ "(" @ %arg @ ");");
		%this.setshname(%this.name @ " - (" @ strUpr(%task) @ ")");
	}
}

//name: startMapping
//description: mapper - used by a player to build a node list in a map.
function player::startMapping(%this)
{
	cancel(%this.startMappingloop);
	
	if(%this.lastNodePlant $= "")
	{
		%mask = $TypeMasks::FxBrickAlwaysObjectType;
		initContainerRadiusSearch(%this.position, "8", %mask);
		
		while(isObject(%brick = containerSearchNext()))
		{
			if(%brick.getDataBlock().getName() $= "brick1x1fdata" && %brick.colorID == 3)
			{
				%noplant=true;
				break;
			}
		}
		if(!%noplant)
			%this.lastNodePlant = %this.plantNode();
	}
	else
	{
		if(vectorDist(%this.position,%this.lastNodePlant.position) > 5)
		{
			%this.lastNodePlant = "";
		}
	}
	%this.startmappingloop = %this.schedule(100,startmapping);
}

//name: stopMapping
//description: Stops the loop and stops mapping.
function player::stopMapping(%this)
{
	cancel(%this.startmappingloop);
	%this.lastPlantnode="";
}

//name: plantNode
//description: This is utilized in the player::startMapping function.
function player::plantNode(%this)
{
	%b = new fxDtsBrick()
	{
		datablock = "brick1x1fdata";
		scale = "1 1 1";
		position = %this.position;
		client = %this.client;
		angleid = 2;
		isplanted=true;
		printid=0;
		colorid=3;
		rotation="0 0 1 180";
		stackBl_id=%this.client.bl_id;
	};
	%error = %b.plant();
	%this.client.brickgroup.add(%b);
	return %b;
}
//mapper end

//bot randomizer

//name:doRandomTask
//description: attempts to perform a random task
//specified inside of this method
function aiplayer::doRandomTask(%this)
{
	%this.clearTask();
	%task[0] = "nodeRunToPos_position";
	%task[1] = "roam";
	%task[2] = "follow_closestplayer";
	%task[3] = "chase";
	
	%val = %task[getRandom(0,3)];
	if(%val == %task[0])
		%arg = vectorAdd(%this.position,getRandom(-20,20) SPC getRandom(-20,20) SPC 0);
	if(%val == %task[2])
		%arg = %this.getClosestPlayer();
	if(stripos(%val,"_") >= 0)
	{
		%newVal = strReplace(%val,"_"," ");
		if(getWordCount(%newVal) == 2)
		{
			%hasarg=true;
			%newVal = getWord(%newVal,0);
		}
		if(%hasarg)
			%eval = "%this.newTask(" @ %newVal @ "," @ %arg @ ");";
		else
			%eval = "%this.newTask(" @ %newVal @ ");";
		
		eval(%eval);
	}
	else
		eval("%this.newTask(" @ %val @ ");");
}

//name: startRandomTask
//description: Random task initialize
function aiplayer::startRandomTask(%this)
{
	cancel(%this.startRandomTaskLoop);
	
	if(%this.isRandomTasking $= "")
	{
		%this.isRandomTasking = true;
	}
	
	%this.nodememory.clearMemory();
	%this.doRandomTask();
	
	%this.startRandomTaskLoop = %this.schedule(%this.randomTaskTimeout,startRandomTask);
}

//name:endRandomTask
//description: End random task.
function aiplayer::endRandomTask(%this)
{
	cancel(%this.startRandomTaskLoop);
	%this.clearTask();
}

//end bot randomizer

//ai combat

//cancel(%this.fightloop);
//%this.firstfight="";
//%this.timenearenemy="";
//%this.isFighting="";

//name: fight
//description: attempts to fight %enemy
function aiplayer::fight(%this,%enemy)
{
	cancel(%this.fightloop);
	if(%enemy $= "")
	{
		//change
		%enemy = findclientbyname("elm").player;
		%this.follow(%enemy);
	}
	if(%this.firstFight $= "")
	{
		%this.isFighting = true;
		%this.firstFight = true;
		if(isObject(%enemy))
			%this.follow(%enemy);
	}
	
	
	if(isobject(%enemy))
	{
		//dmg percent: the higher (100 max) the percent value the lower hp the bot is
		%health = %this.getDamagePercent();
		if(%health == 100)
			return false;
		
		%style = %this.getmountedimage(0).melee;
		
		if(%style)
			%style="melee";
		else
			%style="range";
		if(%health == 0)
		{
			//aggro
			if(%style $= "melee")
			{
				%dist = vectorDist(%this.position,%enemy.position);
				if(%dist <= 3)
				{
					%this.timeNearEnemy++;
					%this.setImageTrigger(0,1);
					//put below in weapon initialization function
					%this.playthread(0,armreadyright);
					//--
					
				}
				else
				{
					%this.setImageTrigger(0,0);	
					%this.playthread(0,root);
				}
			}
			else
			{
				if(%dist <= 3)
				{
					%this.timeNearEnemy++;
					%this.setImageTrigger(0,1);
					//put below in weapon initialization function
					%this.playthread(0,armreadyright);
					//--
					
				}
				else
				{
					%this.setImageTrigger(0,0);	
					%this.playthread(0,root);
				}					
			}
		}
		else
		if(%health > 0.2 && %health < 0.45)
		{
				//semi-aggro
			if(%style $= "melee")
			{%dist = vectorDist(%this.position,%enemy.position);
				if(%dist <= 3)
				{
					%this.timeNearEnemy++;
					%this.setImageTrigger(0,1);
					//put below in weapon initialization function
					%this.playthread(0,armreadyright);
					//--
				}
				else
				if(%dist > 3 && %dist < 15)
				{
					%this.setImageTrigger(0,0);	
					%this.playthread(0,root);
				
					if(getRandom(0,200) < 100)
					{
						%this.clearMoveDestination();
						%rand = getrandom(0,1);
						if(%rand==0)%rand=-1;
						%this.setMoveX(%rand);
						cancel(%this.followpend);
						%this.followpend=%this.schedule(getRandom(200,600),follow,%enemy);
						messageclient(findlocalclient(),'',"strafe");
					}
				}	
				else
				{
					%this.setImageTrigger(0,0);	
					%this.playthread(0,root);
				}					
			}
			else
			{
					
			}
		}
		else
		if(%health >= 0.45 && %health <= 0.75)
		{
			//defensive aggro
			if(%style $= "melee")
			{
					
			}
			else
			{
					
			}
		}
		else
		{
			//defensive
			if(%style $= "melee")
			{
					
			}
			else
			{
					
			}
		}
	}
	else
	{
			echo("no enemy found to fight with");
			return false;
	}
	%this.fightloop = %this.schedule(500,fight,%enemy);
}

//end aicombat

//name: doPush
//description: Empty method for the moment, does nothing.
function aiplayer::doPush(%this,%client)
{
	//%this.setVelocity(getrandom(-2,2) SPC getRandom(-2,2) SPC getRandom(10,25));
	//tumble(%this);
	//commandtoclient(%client,'centerprint',"\c6You've pushed \c2" @ %this.name @ "\c6!",3);
}