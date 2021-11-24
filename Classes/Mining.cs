//***** class description start *****
//		author: Elm
//
//		description - mining class
//		
//
//***** class description end ***** 

//***** mining class *****

if(!isObject($class::mining))
{
	$class::mining = new scriptGroup(mining)
	{
		levelRequirement["Copper"] = 0;
		levelRequirement["Zinc"] = 5;
		levelRequirement["Tin"] = 15;
		levelRequirement["Iron"] = 25;
		levelRequirement["coal"] = 25;
		levelRequirement["gold"] = 25;
		
		respawnTime["copper"] = 30000;
		respawnTime["zinc"] = 25000;
		respawnTime["tin"] = 35000;
		respawnTime["iron"] = 50000;
		respawnTime["coal"] = 50000;
		respawnTime["gold"] = 50000;
		
		ore["copper"] = 3;
		ore["zinc"] = 3;
		ore["tin"] = 3;
		ore["iron"] = 3;
		ore["coal"] = 3;
		ore["gold"] = 3;
		
		exp["copper"] = 25;
		exp["zinc"] = 25;
		exp["tin"] = 25;
		exp["iron"] = 40;
		exp["coal"] = 40;
		exp["gold"] = 40;
		
		//using these datablocks for rocks
		//--------------------------------
		usingDatablock["brickore_1data"] = true;
		usingDatablock["brickore_2data"] = true;
		usingDatablock["brickore_3data"] = true;
		usingDatablock["brickore_4data"] = true;
		usingDatablock["brickore_5data"] = true;
		usingDatablock["brickore_6data"] = true;
		usingDatablock["brickore_7data"] = true;
		usingDatablock["brickbase_oredata"] = true;
		//--------------------------------
		
		//typeFromDB["brickPineData"] = "Pine";
		//typeFromDB["brickWillowData"] = "Willow";
		//typeFromDB["brickMapleData"] = "Maple";
		//typeFromDB["brickOakData"] = "Oak";
		
		//colorid of brick
		typeFromColor[2] = "Copper";
		typeFromColor[50] = "Zinc";
		typeFromColor[48] = "Tin";
		typeFromColor[62] = "Iron";
		typeFromColor[33] = "Coal";
		typeFromColor[11] = "Gold";
	};
	
	$class::exp.addSkill("Mining");
}

function mining::onHitrock(%this,%client,%rock,%projectile)
{
	if(%rock.oreMax $= "")
	{
		%ran = getRandom(0,4);
		%rock.oreMax = %this.ore[%this.typeFromColor[%rock.colorId]] + %ran;
		%rock.oreCurrent = %rock.oreMax;
	}
	
	%profile = %client.profile;
	
	if(isObject(%profile))
	{
		if(%profile.skill["mining"] $= "")
		{
			%profile.skill["mining"] = 1;
		}
		
		%level = %profile.skill["mining"];
		%clamp = mclamp(%level,1,50);
		%ran = getRandom(0,(300 - %clamp));

		if(%ran <= 120)
		{
			%inventory = %client.inventory;
			if(isObject(%inventory))
			{
				%rockType = %this.typeFromColor[%rock.colorId];
				if(%rockType !$= "")
				{
					%item = %rockType @ " Ore";
					%inventory.addItem(%item,1);
					%exp = %this.exp[%rocktype];
					$class::exp.giveExp(%client,"mining",%exp);
					%expString = $class::exp.getExpString(%client,"mining","\c2","\c6","\c2");
					$class::chat.c_print(%client,"You have \c2" @ %inventory.getItem(%item).amount @ "\c4 " @ %item @ "\n\c6EXP: \c5" @ %expString ,4);
					%rock.oreCurrent--;
					
					%_item_pos = getWords(vectoradd(%client.player.getEyePoint(),%client.player.getForwardVector()),0,1) SPC getWord(%client.player.getEyePoint(),2);
					%_item_pos = vectoradd(%_item_pos,getRandom(-1,1) SPC getRandom(-1,1) SPC 0);
					
					%_item = new item(){
						dataBlock = %rockType @ "oreItem";
						scale = "0.7 0.7 0.7";
						position = %_item_pos;						
					};
					%_item.canpickup=false;
					%_item.setVelocity(vectorScale(%client.player.getForwardVector(),-5));
					%_item.schedule(1500,delete);
					
				}
			}
		}
	}
	
	if(%rock.oreCurrent <= 0)
	{
		%rockType = %this.typeFromColor[%rock.colorId];
		if(%rockType !$= "")
		{
			%rock.setColliding(0);
			%rock.setRaycasting(0);
			%rock.setRendering(0);
			%rock.schedule(%this.respawnTime[%rockType],respawnrock);
		}
	}
	
	$class::chat.b_print(%client,"\c5" @ %this.typeFromColor[%rock.colorId] @ " \c6Rock",4);
}

package class_mining
{
	function class_mining(){}
};

//FxDTS Brick Class Support
function fxDtsBrick::respawnrock(%this)
{
	%this.oreCurrent = %this.oreMax;
	%this.setColliding(1);
	%this.setRaycasting(1);
	%this.setRendering(1);
}


