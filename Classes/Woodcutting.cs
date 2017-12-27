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
		levelRequirement["Willow"] = 5;
		levelRequirement["Maple"] = 15;
		levelRequirement["Oak"] = 25;
		
		respawnTime["Pine"] = 30000;
		respawnTime["Willow"] = 25000;
		respawnTime["Maple"] = 35000;
		respawnTime["Oak"] = 50000;
		
		wood["Pine"] = 3;
		wood["Maple"] = 2;
		wood["Oak"] = 6;
		wood["Willow"] = 4;
		
		exp["Pine"] = 2;
		exp["Willow"] = 4;
		exp["Maple"] = 7;
		exp["Oak"] = 10;
		
		//using these datablocks for trees
		//--------------------------------
		usingDatablock["brickPineData"] = true;
		usingDatablock["brickWillowData"] = true;
		usingDatablock["brickMapleData"] = true;
		usingDatablock["brickOakData"] = true;
		//--------------------------------
		
		typeFromDB["brickPineData"] = "Pine";
		typeFromDB["brickWillowData"] = "Willow";
		typeFromDB["brickMapleData"] = "Maple";
		typeFromDB["brickOakData"] = "Oak";
	};
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

		if(%ran <= 40)
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
					centerprint(%client,"<font:impact:25><color:gggfff>You have \c6" @ %inventory.getItem(%item).amount @ " <color:gggfff>" @ %item @ "\nEXP: " @ %expString ,4);
					%tree.woodCurrent--;
				}
			}
		}
	}
	
	if(%tree.woodCurrent <= 0)
	{
		%treeType = %this.typeFromDB[%tree.dataBlock];
		if(%treeType !$= "")
		{
			%tree.setColliding(0);
			%tree.setRaycasting(0);
			%tree.setRendering(0);
			%tree.schedule(%this.respawnTime[%treeType],respawnTree);
		}
	}
}

package class_woodCutting
{
	function class_woodCutting(){}
};

//FxDTS Brick Class Support
function fxDtsBrick::respawnTree(%this)
{
	%this.woodCurrent = %this.woodMax;
	%this.setColliding(1);
	%this.setRaycasting(1);
	%this.setRendering(1);
}


