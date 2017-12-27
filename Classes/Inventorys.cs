//***** class description start *****
//		author: Elm
//
//		description - Inventorys Class
//		This class is used to handle all inventory related
//		tasks. There are two classes in this file.
//		notice: Inventorys (parent) and Inventory (child)
//
//***** class description end ***** 

//***** Inventorys class ***** 

if(!isObject($class::Inventorys))
	$class::Inventorys = new scriptGroup(Inventorys)
	{
			savePath = "config/Client/Inventorys/";
	};

//name: newFakeInventory
//description: For debugging purposes
function Inventorys::newFakeInventory(%this,%client)
{
	if(!isObject(%client))
	{
		talk("Inventorys::newFakeInventory - requires a Client Object.");
		return false;
	}
	
	if(isObject(%client.inventory))
	{
		talk("Inventorys::newFakeInventory - Client already has an Inventory Object.");
		return false;
	}
	
	%inventory = new scriptGroup()
	{
		class = Inventory;
		client = %client;
		bl_id = %client.bl_id;
		isFake = true;
	};
	
	%client.inventory = %inventory;
	%this.add(%inventory);
	%inventory.addItem("Gold","2000");
	%inventory.addItem("Sword","3");
	%inventory.addItem("Berries","30");
	%inventory.addItem("Armor","1");
	%inventory.addItem("Helmet","1");
	%inventory.addItem("Boots","2");
	%inventory.addItem("Ores","2000");
	%inventory.addItem("Arrows","200");
	return %inventory;	
}

//name: newInventory
//description: Attempts to give %client a new inventory object
function Inventorys::newInventory(%this,%client)
{
	if(!isObject(%client))
	{
		talk("Inventorys::newInventory - requires a Client Object.");
		return false;
	}
	
	if(isObject(%client.inventory))
	{
		talk("Inventorys::newInventory - Client already has an Inventory Object.");
		return false;
	}
	
	%inventory = new scriptGroup()
	{
		class = Inventory;
		client = %client;
		bl_id = %client.bl_id;
	};
	
	%test = %inventory.loadInventory();
	
	if(%test)
	{
		%this.add(%client.inventory);
	}
	else
	{
		%client.inventory = %inventory;
		%this.add(%inventory);
	}
	return %inventory;
}

//***** Inventory Class *****

//name: addItem
//description: adds an item to an Inventory
function Inventory::addItem(%this,%image,%amount)
{
	%c = %this.getCount();

	if(%c > 0)
	{
		%itemInfoSet = false;
		for(%i=0;%i<%c;%i++)
		{
			%iteminfo = %this.getObject(%i);
			if(%iteminfo.image $= %image)
			{
				%itemInfoSet = true;
				break;
			}
		}
	}
	
	if(!%itemInfoSet)
	{
		%iteminfo = new scriptObject()
		{
			inventory = %this;
			class = ItemInfo;
			image = %image;
			amount = %amount;
		};
		
		%this.add(%itemInfo);
	}
	else
	{
		%iteminfo.amount += %amount;

	}
	return true;
}

//name: removeItem
//description: attempts to remove an item from an inventory
function Inventory::removeItem(%this,%image,%amount)
{
	%c = %this.getCount();

	if(%c > 0)
	{
		%itemInfoSet = false;
		for(%i=0;%i<%c;%i++)
		{
			%iteminfo = %this.getObject(%i);
			if(%iteminfo.image $= %image)
			{
				%itemInfoSet = true;
				break;
			}
		}
	}
	
	if(!%itemInfoSet)
	{
		//do nothing, they dont have the object
		return false;
	}
	else
	{
		
		%iteminfo.amount -= %amount;
		if(%itemInfo.amount <= 0)
		{
			%itemInfo.delete();
			return true;
		}
	}
}

//name: getItem
//description: attempts to return an item Object from an inventory object
function Inventory::getItem(%this,%image)
{
	%c = %this.getCount();

	if(%c > 0)
	{
		for(%i=0;%i<%c;%i++)
		{
			%iteminfo = %this.getObject(%i);
			if(%iteminfo.image $= %image)
			{
				return %itemInfo;
			}
		}
	}
	return false;
}

//name: drop
//description: attempts to drop an item from an inventory
function Inventory::drop(%this,%image,%amount)
{
	if(isObject(%item = %this.getItem(%image)))
	{
		%am = %item.amount;
		if(%amount > %am)
			%amount = %am;
		
		%this.removeItem(%image,%amount);
		%db = strReplace(strLwr(%image),"image","item");
		%name = %db.uiname;
		%item = new item()
		{
			datablock = %db;
			canPickup = true;
			scale = "1 1 1";
			specifiedClient = %this.client;
			name = %name;
			amount = %amount;
		};
		
		%item.setCollisionTimeout(%this.client.player);
		%item.position = %this.client.player.getEyePoint();
		%item.setVelocity(vectorScale(%this.client.player.getEyeVector(),getRandom(2,8)));
		%item.setShapeName(%name SPC " (" @ %amount @ ")");
		%item.schedule(120000,delete);
		return %item;
	}
}

//name: reset
//description: resets an inventory
function Inventory::reset(%this)
{
	%this.client.inventory = Inventorys.newInventory(%this.client);
	%this.schedule(1,delete);
	return true;
}

//name: saveInventory
//description: saves an inventory to: Inventorys.savePath
function Inventory::saveInventory(%this)
{
	%f = Inventorys.savepath @ %this.client.bl_id @ "_" @ %this.client.name @ ".cs";
	%this.save(%f);
}

//name: loadInventory
//description: attempts to load an inventory
function Inventory::loadInventory(%this)
{
	%f = Inventorys.savePath @ %this.client.bl_id @ "_" @ %this.client.name @ ".cs";
	if(isfile(%f))
	{
		exec(%f);
		return true;
	}
	
	return false;
}

//name: onAdd
//description: <b>overwrite</b> called when a new inventory object is instantiated.
function Inventory::onAdd(%this)
{
	%c = clientgroup.getcount();
	for(%i=0;%i<=%c;%i++)
	{
		%cl = clientgroup.getobject(%i);
		if(%cl.bl_id $= %this.bl_id)
		{
			if(isObject(%cl.inventory && %cl.inventory != %this))
				%cl.inventory.delete();
			%cl.inventory = %this;
			%this.client = %cl;
			return true;
		}
	}
	return false;
}

//ItemInfo Class
	