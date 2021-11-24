
if(isObject($class::banking)) {
	$class::banking.delete();
}

$class::banking = new scriptGroup() {
	class = banking;
	save_path = "config/client/banks/";
};

function banking::newBank(%this,%client) {
	if(isObject(%client)) {
		%bank = new scriptGroup() {
			class = bank;
			bl_id = %client.bl_id;
			client = %client;
		};
		%client.bank = %bank;
		
		return %bank;
	}
	return false;
}

function banking::loadBank(%this,%client,%force) {
	if(%force $= "") %force = false;
	if(!isObject(%client.bank) || %force) {
		exec($class::banking.save_path @ %client.bl_id @ "_" @ %client.name @  ".cs");
	}
}

function banking::saveBank(%this,%client) {
	if(isobject(%client.bank)) {
		%client.bank.save($class::banking.save_path @ %client.bl_id @ "_" @ %client.name @ ".cs");
	}
	return %client.bank;
}

function banking::hasBank(%this,%client, %refresh_folders) {
	if(%refresh_folders $= "")
		%refresh_folders = false;
	else
		%refresh_folders = true;
	if(%refresh_folders)
		setmodpaths(getmodpaths());
	if(isFile($class::banking.save_path @ %client.bl_id @ "_" @ %client.name @ ".cs")) {
		if(isObject(%client.bank)) {
		} else {
			
		}
		return true;
	}
	return false;
}

//deposit mode
function banking::enterBankMode(%this,%client) {
	if(isObject(%client.bank)) {
		%client.bankMode = true;
		$class::camera.orbitPos(%client,%client.player.position);
		servercmdlight(%client);
		$class::chat.b_print(%client,"You are in \c4Deposit Mode\c6. Type \c5/end\c6 to exit \c4DepositMode\c6.");
	} else {
		
	}
}

function banking::exitBankMode(%this,%client) {
	if(isObject(%client.bank)) {
		%client.bankMode = false;
		$class::chat.b_print(%client,"",1);
	} else {
		
	}
}

function banking::enterWithdrawMode(%this,%client) {
	if(isObject(%client.bank)) {
		%client.bankMode = true;
		$class::camera.orbitPos(%client,%client.player.position);
		//servercmdlight(%client);
		$class::bankingInterface.display(%client.bank);
		$class::chat.b_print(%client,"You are in \c4Withdraw Mode\c6. Type \c5/end\c6 to exit \c4Withdraw Mode\c6.");
	} else {
		
	}
}

function bank::onAdd(%this)
{
	%c = clientgroup.getcount();
	for(%i=0;%i<=%c;%i++)
	{
		%cl = clientgroup.getobject(%i);
		if(%cl.bl_id $= %this.bl_id)
		{
			if(isObject(%cl.bank && %cl.bank != %this))
				%cl.bank.delete();
			%cl.bank = %this;
			%this.client = %cl;
			return true;
		}
	}
	return false;
}

function bank::deposit(%this,%itemName, %amount, %rarity, %stats) {
	if(isObject(%this.client.inventory.getItem(%itemName))) {
		if(%rarity $= "") {
			%item = %this.bank.getItem(%itemName);
		}
		if(isObject(%item)) {
			%item.amount += %amount;
		} else {
			%item = new scriptObject() {
				item = %itemName;
				image = %itemName;
				amount = %amount;
				stats = %stats;
				rarity = %rarity;
			};
		}
		%this.client.inventory.removeItem(%itemName,%amount);
		%this.add(%item);
		return %item;
	}
	return false;
}

function bank::getItem(%this,%item, %stats, %rarity) {
	for(%i=0;%i<%this.getcount();%i++) {
		%_item = %this.getObject(%i);
		
		if(%_item.item $= %item) {
			return %_item;
		}
	}
	return false;
}
function bank::hasItem(%this,%item, %stats, %rarity) {
	for(%i=0;%i<%this.getcount();%i++) {
		%_item = %this.getObject(%i);
		
		if(%_item.item $= %item) {
			return true;
		}
	}
	return false;
}

function bank::withdraw(%this,%item, %amount, %rarity, %stats) {
	if(%this.hasItem(%item)) {
		%_item = %item;
		%amount = 1;
		if(%rarity $= "") {
			%item = %this.client.inventory.getItem(%item);
			if(isObject(%item)) {
				%item.amount += %amount;
			} else {
				%item = new scriptObject() {
					class = itemInfo;
					inventory = %this.client.inventory;
					amount = %amount;
					image = %item;
				};
			}
		}
		
		//talk(%_item SPC %item SPC %amount);
		%this.client.inventory.addItem(%_item,%amount);
		
		return %item;
	}
	return false;
}

function servercmdend(%this) {
	if(isObject(%this.bank) && %this.bankMode) {
		$class::camera.root(%this);
		$class::inventoryInterface.hide(%this.inventory);
		$class::bankingInterface.hide(%this.bank);
		$class::banking.exitBankMode(%this);
		//$class::banking.exitWithdrawMode(%this);
		$class::chat.b_print(%this,"",1);
	}
}

function servercmdenterbankmode(%this) {
	$class::banking.schedule(10,enterBankMode,%this);
}

function servercmdenterwithdrawmode(%this) {
	$class::banking.schedule(10,enterWithdrawMode,%this);
}
