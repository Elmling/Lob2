//***** class description start *****
//		author: Elm
//
//		description - Woodcutting class
//		
//
//***** class description end ***** 

//***** Woodcutting class *****

if(!isObject($class::woodCutting))
{
	$class::woodCutting = new scriptGroup(woodCutting)
	{
		levelRequirement["Pine"] = 0;
		levelRequirement["Pine Tree New 2"] = 0;
		levelRequirement["Willow"] = 5;
		levelRequirement["Maple"] = 15;
		levelRequirement["Oak"] = 25;
		levelRequirement["Yew"] = 35;
		
		respawnTime["Pine"] = 30000;
		respawnTime["Pine Tree New 2"] = 30000;
		respawnTime["Willow"] = 25000;
		respawnTime["Maple"] = 35000;
		respawnTime["Oak"] = 50000;
		respawnTime["Yew"] = 60000;
		
		wood["Pine"] = 3;
		wood["Pine Tree New 2"] = 3;
		wood["Maple"] = 2;
		wood["Oak"] = 6;
		wood["Willow"] = 4;
		wood["Yew"] = 3;
		
		exp["Pine"] = 25;
		exp["Pine Tree New 2"] = 25;
		exp["Willow"] = 25;
		exp["Maple"] = 25;
		exp["Oak"] = 25;
		exp["Yew"] = 50;
		
		//using these datablocks for trees
		//--------------------------------
		usingDatablock["brickPineData"] = true;
		usingDatablock["brickWillowData"] = true;
		usingDatablock["brickMapleData"] = true;
		usingDatablock["brickOakData"] = true;
		usingDatablock["brickYewData"] = true;
		usingDatablock["brickpinetreenew2data"] = true;
		//--------------------------------
		
		stump["brickPineData"] = "brickPineStumpData";
		stump["brickWillowData"] = "brickWillowStumpData";
		stump["brickMapleData"] = "brickMapleStumpData";
		stump["brickOakData"] = "brickOakStumpData";
		stump["brickYewData"] = "brickYewStumpData";
		stump["brickpinetreenew2data"] = "brickPineStumpData";
		
		typeFromDB["brickPineData"] = "Pine";
		typeFromDB["brickWillowData"] = "Willow";
		typeFromDB["brickMapleData"] = "Maple";
		typeFromDB["brickOakData"] = "Oak";
		typeFromDB["brickYewData"] = "Yew";
		typeFromDB["brickpinetreenew2data"] = "Pine";
	};
	
	$class::exp.addSkill("WoodCutting");
}

function woodCutting::onHitTree(%this,%client,%tree,%projectile)
{
	if(%tree.woodMax $= "")
	{
		%ran = getRandom(0,4);
		%tree.woodMax = %this.wood[%this.typeFromDB[%tree.dataBlock.getName()]] + %ran;
		%tree.woodCurrent = %tree.woodMax;
	}
	
	%profile = %client.profile;
	
	if(isObject(%profile))
	{
		if(%profile.skill["woodcutting"] $= "")
		{
			%profile.skill["woodcutting"] = 1;
		}
		
		%level = %profile.skill["woodcutting"];
		%clamp = mclamp(%level,1,50);
		%ran = getRandom(0,(300 - %clamp));

		if(%ran <= 120)
		{
			%inventory = %client.inventory;
			if(isObject(%inventory))
			{
				%treeType = %this.typeFromDB[%tree.dataBlock];
				if(%treeType !$= "")
				{
					%item = %treeType @ " Wood";
					%inventory.addItem(%item,1);
					%exp = %this.exp[%treetype];
					$class::exp.giveExp(%client,"woodcutting",%exp);
					%expString = $class::exp.getExpString(%client,"woodcutting","\c2","\c6","\c2");
					$class::chat.c_print(%client,"You have \c2" @ %inventory.getItem(%item).amount @ "\c4 " @ %item @ "\n\c6EXP: \c5" @ %expString ,4);
					%tree.woodCurrent--;
					
					%_item_pos = getWords(vectoradd(%client.player.getEyePoint(),%client.player.getForwardVector()),0,1) SPC getWord(%client.player.getEyePoint(),2);
					%_item_pos = vectoradd(%_item_pos,getRandom(-1,1) SPC getRandom(-1,1) SPC 0);
					
					%_item = new item(){
						dataBlock = %treeType @ "logItem";
						scale = "0.7 0.7 0.7";
						position = %_item_pos;						
					};
					
					%_item.canPickup = false;
					
					%_item.setVelocity(vectorScale(%client.player.getForwardVector(),-5));
					%_item.schedule(1500,delete);
				}
			}
		}
	}
	
	if(%tree.woodCurrent <= 0)
	{
		%treeType = %this.typeFromDB[%tree.dataBlock];
		if(%treeType !$= "")
		{
			//%tree.setColliding(0);
			//%tree.setRaycasting(0);
			//%tree.setRendering(0);
			if(!%tree.revertDataBlock)
			{
				%tree.revertDatablock = %tree.dataBlock;
				
			}
			//talk("set tree to " @ %this.stump[%tree.dataBlock]);
			%tree.setDataBlock(%this.stump[%tree.dataBlock]);
			serverplay3d(sound_treeFall,%tree.position);
			%tree.schedule(%this.respawnTime[%treeType],respawnTree);
		}
	}
	
	$class::chat.b_print(%client,"\c5" @ %this.typeFromDB[%tree.dataBlock] @ " \c6Tree",4);
}

package class_woodCutting
{
	function class_woodCutting(){}
};

//FxDTS Brick Class Support
function fxDtsBrick::respawnTree(%this)
{
	%this.woodCurrent = %this.woodMax;
	//%this.setColliding(1);
	//%this.setRaycasting(1);
	//%this.setRendering(1);
	%this.setDataBlock(%this.revertDataBlock);
}


