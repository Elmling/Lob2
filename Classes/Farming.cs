if(!isobject($class::Farming))
	$class::Farming = new scriptGroup(Farming)
	{
		
	};
	
if(!isObject($class::Farming.seedGroup))
	$class::Farming.seedGroup = new scriptGroup()
	{
		class = seedGroup;
		dataB["potatoes"] = "BrickPotatoes_";
		phaseTime["potatoes"] = 60000 * 1;
		generate["potatoes"] = 5;
		exp["potatoes"] = 100;
		
		dataB["Flax"] = "BrickFlax_";
		phaseTime["Flax"] = 60000 * 1;
		generate["Flax"] = 5;
		exp["flax"] = 125;
		
		soilBrick["datablock"] = "brickFarming_Tiledata";
		soilBrick["color","dry"] = 47;
		soilBrick["color","wet"] = 45;
		
		//amount of stages for all crops
		phaseMax = 5;
	};	
	
	
function farming::water(%this,%client,%seed)
{
	%phase = %seed.phase;
	if(!%seed.watered[%phase])
	{
		%soilBrick = %seed.getSoil();
		
		if(isObject(%soilBrick))
		{
			//talk("found soil brick");
			%seedGroup = $class::farming.seedGroup;
			
			if(%soilBrick.dataBlock $= %seedGroup.soilBrick["datablock"])
			{
				%soilBrick.setColor(%seedGroup.soilBrick["color","wet"]);
				%nextLevelExp = $class::exp.nextLevelExp(%client,"Farming");
				$class::exp.giveExp(%client,"Farming", mfloor($class::farming.seedGroup.exp[%seed.type] * 0.85) + getRandom(-20,20));
				$class::chat.b_print(%client,"Farming Exp: \c5" @ %client.profile.exp["farming"] @ " \c6 / \c4" @ %nextLevelExp,10);
			}
			else
			{
				talk("Not a valid soil brick below the seed trying to be watered.");
			}
		}
		
		//talk(%client.name @ " has watered a plant");
		$class::chat.c_print(%client,"\c6You've watered the \c2" @ %seed.type @ " \c4Plant!",3);
		%seed.watered[%phase] = true;
	}
	else
	{
		$class::chat.c_print(%client,"\c6The \c2" @ %seed.type @ " \c4Plant \c6looks to be watered already!",3);
	}
}

function Farming::canPlant(%this,%position)
{
	%searchPos = vectorSub(%position,"0 0 0");
	%box = "0.5 0.5 2";
	%mask = $TypeMasks::FxBrickAlwaysObjectType;

	initContainerBoxSearch(%searchPos, %box, %mask);

	while(%searchObj = containerSearchNext())
	{	
		if(stripos(%searchObj.dataBlock,"stage") != -1 || striPos(%searchObj.dataBlock,"mound") != -1)
		{
			return false;
		}
	}
	
	return true;
}

function Farming::plant(%this,%client,%type)
{
	if(!%this.canPlant(%client.plantPosition))
	{
		talk("already a plant or seed at plant location!");
		return false;	
	}
	%sg = %this.seedGroup;
	
	if(isObject(%client) && %type !$= "")
	{
		if(%type $= "potato")%type = "potatoes";
		%seed = new scriptObject()
		{
			class = seed;
			owner = %client;
			parent = %this;
			type = %type;
			phase = -1;
			canHarvest = false;
		};
		
		%this.seedGroup.add(%seed);
		%seed.startPhase();
		%give_exp = ($class::farming.seedGroup.exp[%seed.type] * 2) + getRandom(-20,20);
		%nextLevelExp = $class::exp.nextLevelExp(%client,"Farming");
		$class::exp.giveExp(%client,"Farming", %give_exp);
		$class::chat.b_print(%client,"Farming Exp: \c5" @ %client.profile.exp["farming"] @ " \c6 / \c4" @ %nextLevelExp,10);
	}
}

function Farming::generate_seed_final(%this,%type,%brick) {
		if(%type $= "potato")%type = "potatoes";
		%seed = new scriptObject()
		{
			class = seed;
			owner = -1;
			parent = %this;
			type = %type;
			phase = %this.phaseMax;
			canHarvest = true;
			brick = %brick;
		};
		%seed.generate();
		return %seed;
}

function seed::harvest(%this,%client)
{
	%time = getSimTime();
	
	if(%time - %client.lastHarvestTime >= 2500)
	{
		%amount = getRandom(1,3);
		if(%amount > %this.generate)
			%amount = %this.generate;
		
		%client.player.playthread(0,"smelting");
		%client.player.schedule(2000,playthread,0,root);
		%client.inventory.addItem(%this.type,%amount);
		%cl_amount = %client.inventory.getItem(%this.type).amount;
		%client.lastHarvestTime = %time;
		//talk("sub " @ %amount @ " from "  @ %this.generate);
		%this.generate = %this.generate - %amount;
		serverPlay3d(lobLeavesRustling,%this.brick.position);
	
		//centerPrint(%client,"\c6You have \c2" @ %cl_amount @ " \c3" @ %this.type @ "\c6 now.",3);
		$class::chat.c_print(%client,"\c6You have \c2" @ %cl_amount @ " \c3" @ %this.type @ "\c6 now.",3);
		%nextLevelExp = $class::exp.nextLevelExp(%client,"Farming");
		$class::exp.giveExp(%client,"Farming", $class::farming.seedGroup.exp[%this.type] + getRandom(-20,20));
		$class::chat.b_print(%client,"Farming Exp: \c5" @ %client.profile.exp["farming"] @ " \c6 / \c4" @ %nextLevelExp,10);
	}
	
	if(%this.generate <= 0 || !isObject(%this.brick))
	{		
		%wormChance = getRandom(0,10);
		
		if(%wormChance <= 2)
		{
			%soil = %this.getSoil();
			if(isObject(%soil))
			{
				talk("Worms have surfaced from a plant Harvest.");
				$class::chat.to(%client,"\c5Worms\c6 have surfaced, quickly capture them.");
				%soil.setItem("WormsItem");
				%soil.schedule(5000,setitem,"");
				%nextLevelExp = $class::exp.nextLevelExp(%client,"Farming");
				$class::exp.giveExp(%client,"Farming", mfloor($class::farming.seedGroup.exp[%this.type] * 0.75) +  getRandom(-20,20));
				$class::chat.b_print(%client,"Farming Exp: \c5" @ %client.profile.exp["farming"] @ " \c6 / \c4" @ %nextLevelExp,10);
			}
		}
		//talk("Plant fully harvested.");
		$class::chat.to(%client,"The \c4" @ %this.type @ " \c6has been fully harvested.");
		%this.brick.delete();
		%this.schedule(1,delete);
	}
	
}

function seed::generate(%this)
{
	%waterPhase = 0;
	
	for(%i=0;%i<%this.parent.seedGroup.phaseMax;%i++)
	{
		if(%this.watered[%i] !$= "")
		{
			%waterPhase++;
		}
	}
	
	%bonus = getRandom(0,%waterPhase);
	%total = %this.parent.seedGroup.generate[%this.type] + %bonus;
	%this.generate = %total;
	//talk("Total water phase " @ %waterPhase @ " / " @ %this.parent.seedGroup.phaseMax);
	
	//talk("Generated " @ %total @ " " @ %this.type @ " from 1 seed.");
}

function seed::getSoil(%this)
{
	if(isObject(%this.brick))
	{
		%searchPos = vectorSub(%this.brick.position,"0 0 1");
		%box = "2 2 2";
		%mask = $TypeMasks::FxBrickAlwaysObjectType;

		initContainerBoxSearch(%searchPos, %box, %mask);

		while(%searchObj = containerSearchNext())
		{	
			if(%searchObj.dataBlock $= $class::farming.seedGroup.soilBrick["dataBlock"])
			{
				return %searchObj;
			}
		}
	}
	return false;
}

function seed::startPhase(%this)
{
	//plant brick
	%position = "";
	
	if(%this.owner.plantPosition !$= "")
	{
		%position = %this.owner.plantPosition;
		%this.owner.plantPosition = "";
	}
	else
	{
		%ray = raycast.getCollidePoint(%this.owner);
		
		if(%ray == false)
			%position = vectorAdd(%this.owner.player.position,"0 0 0.2");
		else
			%position = %ray;
	}
	
	%ranRotation = getRandom(0,3);
	%ran[0] = "1 0 0 0";
	%ran[1] = "0 0 1 90.0002";
	%ran[2] = "0 0 -1 90.0002";
	%ran[3] = "0 0 1 180";
	%rotation = %ran[%ranRotation];
	
	%brick = new fxDtsBrick()
	{
		client = %this.owner;
		dataBlock = "brickDirtMoundData";
		scale = "1 1 1";
		colorID = 0;
		position = vectorAdd(%position,"0 0 0.2");
		rotation = %rotation;
		isPlanted = 1;
	};
	
	%this.owner.brickGroup.add(%brick);
	%brick.setTrusted(1);
	%brick.plant();
	%brick.seed = %this;
	//talk("seed set to " @ %brick.seed);
	
	%this.brick = %brick;
	
	%this.schedule( ($class::farming.seedGroup.phaseTime[%this.type]),nextPhase);
	
	
}

function seed::nextPhase(%this)
{
	cancel(%this.nextPhaseLoop);
	%this.phase++;
	%db = %this.parent.seedGroup.dataB[%this.type];
	%db =  %db @ "Stage" @ %this.phase @ "Data";
	
	if(!isObject(%db) && %this.phase <= 1)
	{
		%this.phase=0;
		talk("cannont find | " @ %db @ " | for seed");
		return false;
	}
	else
	{
		
		%soilBrick = %this.getSoil();
		
		if(isObject(%soilBrick))
		{
			%soilBrick.setColor(%this.parent.seedGroup.soilBrick["color","dry"]);
		}
		
		if(%this.phase == %this.parent.seedGroup.phaseMax-1)
		{
			%this.canHarvest = true;
			%this.generate();
			cancel(%this.nextPhaseLoop);
			//talk("return false");
			%this.brick.setDatablock(%db);
			return false;
		}
		
		//talk("Changing " @ %this.brick.dataBlock @ " to " @ %db);
		
		%this.brick.setDatablock(%db);
		
		%this.nextPhaseLoop = %this.schedule( ($class::farming.seedGroup.phaseTime[%this.type]),nextPhase);
	}
}