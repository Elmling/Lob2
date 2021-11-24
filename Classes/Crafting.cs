//***** class description start *****
//		author: Elm
//
//		description - Crafting Class
//		
//		usage: 
//			  
//			  if(!isObject($class::crafting))
//			  {
//			  	<span style="padding-left:20px;">error("crafting_handle.cs -> Crafting class has not been executed, aborting.");</span>
//			  }
//			  else
//			  {
//			  	<span style="padding-left:20px;">%crafting = $class::crafting;</span>
//	
//			  	<span style="padding-left:20px;">%recipe = %crafting.newRecipe("handle");</span>
//			  	<span style="padding-left:20px;">%recipe.addItem("Wood",5);</span>
//			  	<span style="padding-left:20px;">%recipe.onComplete("handle",1);</span>
//			  }
//
//***** class description end ***** 

//***** Crafting class *****

//new scriptgroup
if(!isObject($class::crafting))
{
	$class::crafting = new scriptGroup(crafting);
}

//name: newRecipe
//description: attempts to create a new recipe object
function crafting::newRecipe(%this,%name)
{
	if(%this.hasRecipe(%name))
	{
		return false;
	}
	
	%recipe = new scriptGroup()
	{
		class = recipe;
		parent = %this;
		name = %name;
		//default
		requirementLevel = 0;
		onCompleteCount = -1;
		
	};
	
	%this.add(%recipe);
	
	return %recipe;
}

//name: hasRecipe
//description: checks the crafting class to see if
//%name string is already defined in the crafting class
function crafting::hasRecipe(%this,%name)
{
	%c = %this.getCount();
	for(%i=0;%i<%c;%i++)
	{
		%o = %this.getobject(%i);
		if(%o.name $= %name)
			return true;
	}
	
	return false;
}

//name: getRecipe
//description: attempts to get a recipe object by name
function crafting::getRecipe(%this,%name)
{
	%c = %this.getCount();
	for(%i=0;%i<%c;%i++)
	{
		%o = %this.getobject(%i);
		if(%o.name $= %name)
			return %o;
	}
	
	return false;
}

//***** Recipe class *****

//name: addItem
//description: attempts to add a new item into the recipe
//class. Item in this case would be one or many requirements to
//successfully craft the recipe.
function recipe::addItem(%this,%name,%amount)
{
	if(%this.hasItem(%name))
	{
		return false;
	}
	
	%item = new scriptObject()
	{
		parent = %this;
		name = %name;
		amount = %amount;
	};
	
	%this.add(%item);
}

//name: hasItem
//description: checks to see if an item already
//exists in the recipe class
function recipe::hasItem(%this,%name)
{
	%c = %this.getCount();
	for(%i=0;%i<%c;%i++)
	{
		%o = %this.getobject(%i);
		if(%o.name $= %name)
			return true;
	}
	return false;
}

//name: setRequirementLevel
//description: set the recipe's required crafting level
function recipe::setRequirementLevel(%this,%level)
{
	%this.requirementLevel = %level;
	return %level;
}

//name: getRequirementLevel
//description: get's the required crafting level to craft this recipe
function recipe::getRequirementLevel(%this)
{
	return %this.requirementLevel;
}

//name: setAnimationTime
//description: Sets the time the recipe animation plays for.
function recipe::setAnimationTime(%this,%time)
{
	//milliseconds
	%this.animationTime = %time;
}

//name: getAnimationTime
//description: Gets the time the recipe animation plays for.
function recipe::getAnimationTime(%this)
{
	//milliseconds
	return %this.animationTime;
}

//name: onComplete
//description:
function recipe::giveExp(%this,%type,%amount)
{
	%this.expType = %type;
	%this.exp = %amount;
}

//name: onComplete
//description: called when a recipe is completed to
//get the end-items the user crafted
function recipe::onComplete(%this,%itemName,%amount)
{
	//TODO
	//this needs to be more modular
	//has support for %this.onComplete[%this.onCompleteCount++];
	%this.onComplete["item"] = %itemName;
	%this.onComplete["amount"] = %amount;
}

//name: clientVerified
//description: verifies that the client
//has everything required to craft a recipe
function recipe::clientVerified(%this,%cl)
{
	if(%cl.profile.craftingLevel < %this.getRequirementLevel())
	{
		return false;
	}
	
	%c = %this.getCount();
	for(%i=0;%i<%c;%i++)
	{
		%o = %this.getObject(%o);
		%name = %o.name;
		%amount = %o.amount;
		
		if(%name $= "" || %amount $= "")
		{
			continue;
		}
		
		%item = %cl.inventory.getItem(%name);
		
		if(isObject(%item))
		{
			%itemAmount = %item.amount;
			if(%itemAmount >= %amount)
			{
				continue;
			}
			else
			{
				return false;
			}
		}
		else
		{
			return false;
		}
		
	}
	
	return true;
}

//***** client class *****

//name: isHoldingCraftingItem
//description: Checks to see if a player is holding an item used to craft things.
//In function see -> %craftItem[itemUIName]
function gameConnection::isHoldingCraftingItem(%this,%slot)
{
	if(%slot $= "")%slot = 0;
	%player = %this.player;
	if(isObject(%player))
	{
		%holding = %this.player.getMountedImage(%slot);
		if(isObject(%holding))
		{
			//modular list to check
			%craftItem["Carving Knife"] = true;
			//----------------------------------
			
			%dbName = %holding.item.uiName;
			
			if(%craftItem[%dbName] !$= "")
			{
				return true;
			}
		}
	}
	return false;
}

//name: canCraft
//description: Checks to see if a client can craft
//a recipe by name or by obj is supported in this function
function gameConnection::canCraft(%this,%nameOrObj)
{
	if(%this.profile.skill["Crafting"] $= "")
	{
		%this.profile.skill["Crafting"] = 1;
	}
	if(isObject(%nameOrObj))
	{
		%obj = %nameOrObj;
		%name = %obj.name;
	}
	else
	{
		%name = %nameOrObj;
		%obj = $class::crafting.getRecipe(%nameOrObj);
	}
	
	if(!isObject(%obj))
	{
		//uh oh
	}
	else
	{
		%recipe = %obj;
		if(isObject(%recipe))
		{
			%requirement = %recipe.getRequirementLevel();
			
			//TODO
			//check player inventory item against required
			//amount provided by the recipe's individual item
			//check players crafting level and requirement
			
			if(%recipe.clientVerified(%this))
			{
				return true;
			}
		}
		else
		{
			error("crafting_handle.cs -> Error, no such recipe: " @ %name);
		}
	}
	return false;
}

//name: craftingStart
//description: attempts to craft a recipe
function gameConnection::craftingStart(%this,%recipe,%amount)
{
	//TODO
	//talk("r = " @ %recipe);
	if(!isObject(%recipe))
		return false;
	//talk("past");

	%c = $class::crafting;
	
	if(%this.canCraft(%recipe))
	{
		
		if(%this.craftWarn $= "")
		{
			%this.craftWarn = true;
			centerPrint(%this,"\c6Get ready to keep the stabilizer centered by using \c4Numpad \c34 \c6and \c36\c6.",5);
			%this.schedule(4500,craftingStart,%recipe,%amount);
			return;
		}
		%time = %recipe.getAnimationTime();
		
		if(%amount !$= "")
		{
			if(%amount $= "all")
				%amount = 10000;
			if(%amount < 1)
			{
				centerPrint(%this,"\c6Crafting canceled -> amount entered needs to be more than 0.",3);
				return false;
			}
			%c = %recipe.getCount();
			for(%i=0;%i<%c;%i++)
			{
				%item = %recipe.getobject(%i);
				if(%item.amount * %amount > %this.inventory.getItem(%item.name).amount)
				{
					%amount = mfloor(%this.inventory.getItem(%item.name).amount / %item.amount);
				}
			}
			
			//%newTime = mfloor(%time/2);
			%newAmount = mceil(%amount/2);
			if(%newAmount == 0)
				%newAmount = 1;
			//talk("new amount = " @ %newAmount);
			%newTime = %time * %newAmount;
			%time = mfloor(%newTime);
		}
		
		%time = mclamp(%time,2000,60000);
		%stabilizer = $class::stabilizer;
		%stabilizer.begin(%this,10,%time);
		//%this.player.playthread(0,smelting);
		%this.crafting_animate();
		%this.player.schedule(%time + 500,playthread,0,root);
		//talk("time = " @ %time);
		%this.schedule(%time + 500,craftingFinish,%recipe,%amount);
		
	}
	else
	{
		%c = %recipe.getCount();
		for(%i=0;%i<%c;%i++)
		{
			%item = %recipe.getobject(%i);
			%build = %build @ "\c3(" @ %item.amount @ ") " @ %item.name @ "\n";
		}
		centerPrint(%this,"\c6Crafting canceled -> You are missing the requirements to craft that item.\n" @ %build,3);
	}
	
	%this.craftWarn = "";
}

function gameconnection::crafting_animate(%this)
{
	cancel(%this.crafting_animate_loop);
	
	%this.player.playthread(0,smelting);
	
	%this.crafting_animate_loop = %this.schedule(2500,crafting_animate);
}

//name: craftingFinish
//description: cleanup give items, remove items, etc
function gameConnection::craftingFinish(%this,%recipe,%amountt)
{
	//TODO
	cancel(%this.crafting_animate_loop);
	%score = %this.stabilizer["lastScore"];
	if(%score > 10 + getRandom(-2,6))
	{
		%inventory = %this.inventory;
		if(isObject(%inventory))
		{
			if(%amountt $= "")
				%amountt = 1;
			if(%amountt <= 1)
				%s = "s";
			%inventory.addItem(%recipe.onComplete["item"],%recipe.onComplete["amount"] * %amountt);
			centerPrint(%this,"\c4You crafted " @ %amountt @ " set of \c6" @ %recipe.onComplete["item"] @ %s @ " \c4- \c6" @ %inventory.getItem(%recipe.onComplete["item"]).amount @ "\c4 in total.",4);
			$class::worldText.show_at(vectorAdd(%this.player.getEyePoint(),"0 0 1"),"Crafted " @ %amountt @ " set of " @ %recipe.onComplete["item"] @ %s, 5000);
			%e = $class::exp;
			%e.giveExp(%this,%recipe.expType,%recipe.exp * %amountt);
			
			%count = %recipe.getCount();
			for(%i=0;%i<%count;%i++)
			{
				%item2Remove = %recipe.getObject(%i).name;
				%amount = %recipe.getObject(%i).amount;
				%inventory.removeItem(%item2Remove,%amount * %amountt);
			}			
		}
	}
	else
	{
		%inventory = %this.inventory;
		centerprint(%this,"\c6You failed to craft the " @ %recipe.name @ " and lost the materials in the process.",4);
		%count = %recipe.getCount();
		for(%i=0;%i<%count;%i++)
		{
			%item2Remove = %recipe.getObject(%i).name;
			%amount = %recipe.getObject(%i).amount;
			%inventory.removeItem(%item2Remove,%amount);
		}			
	}
}