//***** class description start *****
//		author: Elm
//
//		description - InventoryInterface Class
//		This is the interface (centerprint)
//		used to work with and display inventory item
//
//***** class description end ***** 

//***** InventoryInterface class ***** 

if(!isObject($class::InventoryInterface))
	$class::InventoryInterface = new scriptGroup(InventoryInterface)
	{
		displayAmount = 8;
		command["Oak Wood"] = "if(#CLIENT.isHoldingCraftingItem()){$menu::crafting.setDefaultBody();$menu::crafting.showmenu(#CLIENT);}else{centerprint(#CLIENT,\"\c6You aren't wielding a crafting item!\",5);}";
		command["maple wood"] = "if(#CLIENT.isHoldingCraftingItem()){$menu::crafting.setDefaultBody();$menu::crafting.showmenu(#CLIENT);}else{centerprint(#CLIENT,\"\c6You aren't wielding a crafting item!\",5);}";
		command["willow wood"] = "if(#CLIENT.isHoldingCraftingItem()){$menu::crafting.setDefaultBody();$menu::crafting.showmenu(#CLIENT);}else{centerprint(#CLIENT,\"\c6You aren't wielding a crafting item!\",5);}";
		command["pine wood"] = "if(#CLIENT.isHoldingCraftingItem()){$menu::crafting.setDefaultBody();$menu::crafting.showmenu(#CLIENT);}else{centerprint(#CLIENT,\"\c6You aren't wielding a crafting item!\",5);}";
		command["glider"] = "if(!isObject(#CLIENT.glider)){#CLIENT.glider = new WheeledVehicle(){position = #CLIENT.player.position; datablock = gliderVehicle;};}#CLIENT.glider.setTransform(vectorAdd(#CLIENT.player.position,\"0 0 10\") SPC \"0 0\" SPC getWords(#CLIENT.player.rotation,1,2));#CLIENT.glider.mountObject(#CLIENT.player,0);#CLIENT.glider.setVelocity(\"0 0 15\");";
		command["Carving Knife"] = "if(#CLIENT.inventory.getItem(\"carving knife\").amount > 0){#CLIENT.player.mountImage(carvingKnifeImage,0);}";
	};

//name: display
//description: Displays the inventory to the client.
//Allows for cycling, notice: %index
function InventoryInterface::display(%this,%inventoryObject,%index)
{
	%io = %inventoryObject;
	if(!isObject(%io) || %io.getCount() <= 0)
	{
		error("InventoryInterface::display -> inventory non object or inventory count is 0");
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
	
	%head = "<font:impact:18>\c6Inventory | " @ %c @ " Items | Page " @ %this.page[%cl] + 1 @ "\n\n";	
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
	

	
	for(%i=%int;%i<%cap;%i++)
	{
		%item = %io.getObject(%i);
		if(!isObject(%item))continue;
		
		if(%i == %this.index[%cl])
			%build = %build @ "<div:1>" @ (%i + 1) @ ". \c2" @ %item.image @ " (\c0" @ %item.amount @ "\c2)\n";
		else
			%build = %build @ "" @ (%i + 1) @ ". \c2" @ %item.image @ " (\c0" @ %item.amount @ "\c2)\n";
		
		%this.indexValue[%cl,%i] = %item.amount SPC %item.image;
	}
	%cl.inventoryInterface = %this;
	centerprint(%cl,%head @ %build);
}

//name: hide
//description: Removes the interface from the client's screen 
function InventoryInterface::hide(%this,%inventoryObject)
{
	%io = %inventoryObject;
	%io.client.inventoryInteface = "";
	%cl = %io.client;
	%this.index[%cl] = "";
	%this.isActive[%cl] = "";
	%this.page[%cl] = "";
	centerprint(%cl,"");
}

//name: scrollDown
//description: scrolls down the inventory interface's list
function InventoryInterface::scrollDown(%this,%inventoryObject)
{
	%io = %inventoryObject;
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
function InventoryInterface::scrollUp(%this,%inventoryObject)
{
	%io = %inventoryObject;
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

function InventoryInterface::onMenuSelect(%this,%clientInventory)
{
	if(!isObject(%clientInventory))
		return false;
	
	%client = %clientInventory.client;
	%inventory = %clientInventory;
	%index = %this.index[%client];
	%value = %this.indexValue[%client,%index];
	
	%amount = firstWord(%value);
	%item = restWords(%value);
	
	if(%this.command[%item] !$= "")
	{
		%cmd = %this.command[%item];
		%cmd = strreplace(%cmd,"#CLIENT", %client);
		%client.lastInventoryItemSelected = %item;
		%this.hide(%inventory);
		eval(%cmd);
	}
}

package class_InventoryInterface
{
	
	//name: servercmdsupershiftbrick
	//description: <b>Packaged</b> Support for scrolling in the interface
	function servercmdsupershiftbrick(%client,%x,%y,%z)
	{
		%p = parent::servercmdsupershiftbrick(%client,%x,%y,%z);	
		if(InventoryInterface.isactive[%client])
		{
			if(%x == -1)
				InventoryInterface.scrollDown(%client.inventory);
			else
			if(%x == 1)
				InventoryInterface.scrollUp(%client.inventory);
			return false;
		}
		return %p;
	}
	
	//name: servercmdshiftbrick
	//description: <b>Packaged</b> Support for scrolling in the interface	
	function servercmdshiftbrick(%client,%x,%y,%z)
	{
		%p = parent::servercmdshiftbrick(%client,%x,%y,%z);	
		if(InventoryInterface.isactive[%client])
		{
			if(%x == -1)
				InventoryInterface.scrollDown(%client.inventory);
			else
			if(%x == 1)
				InventoryInterface.scrollUp(%client.inventory);
			if(%y == -1)
			{
				InventoryInterface.page[%client]++;
				InventoryInterface.display(%client.inventory);		
				//talk("next page");			
			}
			else //left
			if(%y == 1)
			{

				InventoryInterface.page[%client]--;
				if(InventoryInterface.page[%client] < 0)
					InventoryInterface.page[%client] = 0;
				InventoryInterface.display(%client.inventory);	
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
		if(InventoryInterface.isactive[%client])
		{
			InventoryInterface.onMenuSelect(%client.inventory);
			return false;
		}	
		return %p;
	}
	
	//name: serverCmdCancelBrick
	//description: <b>Packaged</b> Support for closing the interface	
	function serverCmdCancelBrick(%this,%a,%b,%c,%d,%e)
	{
		%p = parent::serverCmdCancelBrick(%this,%a,%b,%c,%d,%e);
		
		if(InventoryInterface.isActive[%this])
		{
			InventoryInterface.hide(%this.inventory);	
		}
		
		return %p;
	}
};
activatePackage(class_InventoryInterface);