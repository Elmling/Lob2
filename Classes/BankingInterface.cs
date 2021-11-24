//***** class description start *****
//		author: Elm
//
//		description - BankingInterface Class
//		This is the interface (centerprint)
//		used to work with and display inventory item
//
//***** class description end ***** 

//***** BankingInterface class ***** 

// ENTER INVENTORY BINDINGS HERE
if(!isObject($class::BankingInterface))
	$class::BankingInterface = new scriptGroup(BankingInterface)
	{
		displayAmount = 6;
		command["Oak Wood"] = "if(#CLIENT.isHoldingCraftingItem()){$menu::crafting.setDefaultBody();$menu::crafting.showmenu(#CLIENT);}else{centerprint(#CLIENT,\"\c6You aren't wielding a crafting item!\",5);}";
		command["maple wood"] = "#CLIENT.craftingStart($class::crafting.getRecipe(\"Wooden Blow Dart\"), 1);";
		command["willow wood"] = "if(#CLIENT.isHoldingCraftingItem()){$menu::crafting.setDefaultBody();$menu::crafting.showmenu(#CLIENT);}else{centerprint(#CLIENT,\"\c6You aren't wielding a crafting item!\",5);}";
		command["pine wood"] = "if(#CLIENT.isHoldingCraftingItem()){$menu::crafting.setDefaultBody();$menu::crafting.showmenu(#CLIENT);}else{centerprint(#CLIENT,\"\c6You aren't wielding a crafting item!\",5);}";
		command["glider"] = "if(!isObject(#CLIENT.glider)){#CLIENT.glider = new WheeledVehicle(){position = #CLIENT.player.position; datablock = gliderVehicle;};}#CLIENT.glider.setTransform(vectorAdd(#CLIENT.player.position,\"0 0 10\") SPC \"0 0\" SPC getWords(#CLIENT.player.rotation,1,2));#CLIENT.glider.mountObject(#CLIENT.player,0);#CLIENT.glider.setVelocity(\"0 0 15\");";
		command["Carving Knife"] = "if(#CLIENT.bank.getItem(\"carving knife\").amount > 0){#CLIENT.player.mountImage(carvingKnifeImage,0);}";
		command["Bronze Plate Helmet"] = "if(!#CLIENT.player.wearing[\"Helmet\"]){ #CLIENT.player.unhidenode(\"platehelmet\");#CLIENT.player.wearing[\"Helmet\"] = true;}else{#CLIENT.player.hideNode(\"platehelmet\");#CLIENT.player.wearing[\"Helmet\"] = false;}";
		command["Bronze Plate Armor"] = "if(!#CLIENT.player.wearing[\"Armor\"]){#CLIENT.player.hideNode(\"larm\");#CLIENT.player.hideNode(\"rarm\");#CLIENT.player.unhideNode(\"platelarm\");#CLIENT.player.unhideNode(\"platerarm\");#CLIENT.player.unhideNode(\"platerglove\");#CLIENT.player.unhideNode(\"platelglove\");#CLIENT.player.hideNode(\"lhand\");#CLIENT.player.hideNode(\"rhand\");#CLIENT.player.hideNode(\"chest\"); #CLIENT.player.unhidenode(\"platearmor\");#CLIENT.player.wearing[\"Armor\"] = true;}else{#CLIENT.player.hidenode(\"platerarm\");#CLIENT.player.hidenode(\"platelarm\");#CLIENT.player.hidenode(\"platelglove\");#CLIENT.player.hidenode(\"platerglove\");#CLIENT.player.hideNode(\"platearmor\"); #CLIENT.player.unhidenode(\"lhand\");#CLIENT.player.unhidenode(\"rhand\");#CLIENT.player.unhidenode(\"larm\");#CLIENT.player.unhidenode(\"rarm\");#CLIENT.player.unhidenode(\"chest\");#CLIENT.player.wearing[\"Armor\"] = false;}";
		command["Bronze Plate Legs"] = "if(!#CLIENT.player.wearing[\"Legs\"]){#CLIENT.player.hideNode(\"lshoe\"); #CLIENT.player.hideNode(\"rshoe\");#CLIENT.player.unhidenode(\"platelshoe\");#CLIENT.player.unhidenode(\"platershoe\");#CLIENT.player.unhidenode(\"platepants\");#CLIENT.player.wearing[\"Legs\"] = true;}else{#CLIENT.player.hideNode(\"platelshoe\");#CLIENT.player.hideNode(\"platershoe\"); #CLIENT.player.unhidenode(\"lshoe\");#CLIENT.player.unhidenode(\"rshoe\");#CLIENT.player.hidenode(\"platepants\");#CLIENT.player.wearing[\"Legs\"] = false;}";
		command["Pine Long Bow"] = "#CLIENT.player.mountimage(pinelongbowimage,0);";
		command["Pine Short Bow"] = "#CLIENT.player.mountimage(pineshortbowimage,0);";
		command["Fancy Sword"] = "#CLIENT.player.mountImage(fancySwordImage,0);$class::combat.show_weapon_bonus(#CLIENT); ";
		command["Blowdart"] = "#CLIENT.player.mountImage(bdartimage,0);$class::combat.show_weapon_bonus(#CLIENT);";
		command["Hatchet"] = "#CLIENT.player.mountImage(hatchetimage,0);$class::combat.show_weapon_bonus(#CLIENT);";
		command["Pickaxe"] = "#CLIENT.player.mountImage(pickaxeimage,0);$class::combat.show_weapon_bonus(#CLIENT);";
	};

//name: display
//description: Displays the inventory to the client.
//Allows for cycling, notice: %index
function BankingInterface::display(%this,%bankingObject,%index)
{
	%io = %bankingObject;
	if(!isObject(%io) || %io.getCount() <= 0)
	{
		error("BankingInterface::display -> inventory non object or inventory count is 0");
		return false;
	}

	%cl = %io.client;
	%c = %io.getCount();

	if(%this.index[%cl] $= "")
	{
		%this.index[%cl] = -1;
	}
	else
	if(%this.index[%cl] >= %c)
	{
		%this.index[%cl] = -1;
		//SHOW NEXT PAGE IF AVAILABLE
	}
	
	if(%index $= "")
		%this.index[%cl]++;
	else
		%this.index[%cl] = %index;
	
	if(%this.page[%cl] $= "")
		%this.page[%cl] = 0;
	
	%this.isActive[%cl] = true;
	
	%head = "<font:impact:18>\c3Bank | " @ %c @ " Items | Page " @ %this.page[%cl] + 1 @ "\n";	
	%int = %this.page[%CL] * %this.displayAmount;
	%cap = %int + %this.displayAmount;

	if(%cap > %c)
		%cap = %c;

	if(%this.index[%cl] <= -1)
	{
		%this.index[%cl] = %cap-1;
	}
	
	if(%this.index[%cl] >= %cap)
	{
		%this.index[%cl] = %int;
	}
	
	if(%this.index[%cl] < %int)
		%this.index[%cl] = %cap-1;
	

	%item_count = %int;
	%color["rare"] = "\c0";
	%color["unique"] = "\c1";
	%color["elite"]="\c5";
	%color["legendary"] = "\c3";
	for(%i=%int;%i<%cap;%i++)
	{
		%item = %io.getObject(%i);
		if(%cap > %c){%cap = %c;break;}
		if(!isObject(%item)){continue;}
		if(%item.amount <= 0){%item.delete();%cap++;continue;}
		%item_count++;
		if(%item.rarity !$= "") {
			%itemRarity = %color[%item.rarity] @ "" @ %item.rarity @ " \c6( \c2" @ %item.stats.getCount() @ "\c6 )";
		} else {
			%itemRarity = "";
		}
		if(%i == %this.index[%cl])
			%build = %build @ "<div:1>\c4" @ (%item_count) @ ". \c5" @  %itemrarity @ "\c6" @ %item.image @ " (\c2" @ %item.amount @ "\c6)\n";
		else
			%build = %build @ "\c4" @ (%item_count) @ ". \c5" @  %itemrarity @ "\c6" @ %item.image @ " (\c2" @ %item.amount @ "\c6)\n";
		
		%this.indexValue[%cl,%i] = %item.amount SPC %item.image;
	}
	%cl.BankingInterface = %this;

	centerprint(%cl,%head @ %build);
}

//name: hide
//description: Removes the interface from the client's screen 
function BankingInterface::hide(%this,%bankingObject)
{
	%io = %bankingObject;
	%io.client.bankingInterface = "";
	%cl = %io.client;
	%this.index[%cl] = "";
	%this.isActive[%cl] = "";
	%this.page[%cl] = "";
	centerprint(%cl,"");
}

//name: scrollDown
//description: scrolls down the inventory interface's list
function BankingInterface::scrollDown(%this,%bankingObject)
{
	%io = %bankingObject;
	if(isobject(%io))
	{
		%cl = %io.client;
		%this.index[%cl]++;
		
		//if(%this.index[%cl] >= %io.getcount())
		//{
		//	%this.index[%cl] = 0;
		//}
		%this.display(%io,%this.index[%cl]);
	}
}

//name: scrollUp
//description: scrolls up the inventory interface's list
function BankingInterface::scrollUp(%this,%bankingObject)
{
	%io = %bankingObject;
	if(isobject(%io))
	{
		%cl = %io.client;
		%this.index[%cl]--;

		//if(%this.index[%cl] <= -1)
		//{
		//	%this.index[%cl] = %io.getcount()-1;
		//}
		%this.display(%io,%this.index[%cl]);
	}
}

function BankingInterface::onMenuSelect(%this,%clientBank)
{
	if(!isObject(%clientBank))
		return false;
	
	%client = %clientBank.client;
	%bank = %clientBank;
	%index = %this.index[%client];
	%value = %this.indexValue[%client,%index];
	
	%amount = firstWord(%value);
	%amount = 1;
	%item = restWords(%value);
	if(isObject(%client.bank) && %client.bankMode) {
		if(%client.bank.getObject(%index).amount <= 0) {
			%client.bank.getObject(%index).delete();
		} else
			%client.bank.getObject(%index).amount--;
		%rarity = %client.bank.getObject(%index).rarity;
		%stats = %client.bank.getObject(%index).stats;
		%client.bank.withdraw(%item,%amount, %rarity, %stats);

		//talk("deposit " @ %item @ " - " @ %amount @ " - " @ %rarity @ " - " @ %stats);
		//servercmdlight(%client);
		//servercmdlight(%client);
		$class::bankingInterface.schedule(10,display,%client.bank,0);
		//
	} else	if(%this.command[%item] !$= "") {
		talk("just remove this");
		%cmd = %this.command[%item];
		%cmd = strreplace(%cmd,"#CLIENT", %client);
		%client.lastInventoryItemSelected = %item;
		%client.lastInventoryItemSelectedIndex = %index;
		%this.hide(%bank);
		eval(%cmd);
	}
}

package class_BankingInterface
{
	//name: servercmdsupershiftbrick
	//description: <b>Packaged</b> Support for scrolling in the interface
	function servercmdsupershiftbrick(%client,%x,%y,%z)
	{
		%p = parent::servercmdsupershiftbrick(%client,%x,%y,%z);	
		
		if(BankingInterface.isactive[%client])
		{
			if(%x == -1)
				BankingInterface.scrollDown(%client.bank);
			else
			if(%x == 1)
				BankingInterface.scrollUp(%client.bank);
			if(%y == -1)
			{
				BankingInterface.page[%client]++;
				BankingInterface.display(%client.bank);		
				//talk("next page");			
			}
			else //left
			if(%y == 1)
			{

				BankingInterface.page[%client]--;
				if(BankingInterface.page[%client] < 0)
					BankingInterface.page[%client] = 0;
				BankingInterface.display(%client.bank);	
				//talk("prev page");	
			}
			return false;
		}
		return %p;
	}
	
	//name: servercmdshiftbrick
	//description: <b>Packaged</b> Support for scrolling in the interface	
	function servercmdshiftbrick(%client,%x,%y,%z)
	{
		%p = parent::servercmdshiftbrick(%client,%x,%y,%z);	
		if(BankingInterface.isactive[%client])
		{
			if(%x == -1)
				BankingInterface.scrollDown(%client.bank);
			else
			if(%x == 1)
				BankingInterface.scrollUp(%client.bank);
			if(%y == -1)
			{
				BankingInterface.page[%client]++;
				BankingInterface.display(%client.bank);		
				//talk("next page");			
			}
			else //left
			if(%y == 1)
			{

				BankingInterface.page[%client]--;
				if(BankingInterface.page[%client] < 0)
					BankingInterface.page[%client] = 0;
				BankingInterface.display(%client.bank);	
				//talk("prev page");	
			}
			return false;
		}
		return %p;
	}
	
	//name: serverCmdPlantBrick
	//description: <b>Packaged</b> Support for selecting an item, using the interface	
	function serverCmdPlantBrick(%client,%a,%b,%c,%d)
	{
		%p = parent::serverCmdPlantBrick(%client,%a,%b,%c,%d);		
		if(BankingInterface.isactive[%client])
		{
			BankingInterface.onMenuSelect(%client.bank);
			return false;
		}	
		return %p;
	}
	
	//name: serverCmdCancelBrick
	//description: <b>Packaged</b> Support for closing the interface	
	function serverCmdCancelBrick(%this,%a,%b,%c,%d,%e)
	{
		%p = parent::serverCmdCancelBrick(%this,%a,%b,%c,%d,%e);
		
		if(BankingInterface.isActive[%this])
		{
			BankingInterface.hide(%this.bank);	
		}
		
		return %p;
	}
};
activatePackage(class_BankingInterface);