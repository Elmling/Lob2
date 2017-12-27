//***** class description start *****
//		author: Elm
//
//		description - Trading Class
//		Handles trading items between two players
//
//***** class description end ***** 

//***** trading class *****

if(!isObject($class::trading))
{
	$class::trading = new scriptGroup(trading)
	{
		
	};
}

//name: newTrade
//description: attempts to create a new trade session
//between %client1 and %client2
function trading::newTrade(%this,%client1,%client2)
{
	if(isObject(%client1) && isObject(%client2))
	{
		if(isObject(%client1.tradeSession) || isObject(%client2.tradeSession))
		{
			talk("Trading -> one of the two users is already in a trade.");
			return false;
		}
		
		%inv1 = %client1.inventory;
		%inv2 = %client2.inventory;
		
		if(isObject(%inv1) && isObject(%inv2))
		{
			%session = new scriptGroup()
			{
				client1 = %client1;
				client2 = %client2;
				class = tradeSession;
			};
			
			%client1.tradeSession = %session;
			%client2.tradeSession = %session;
			
			%this.add(%session);
			
			return true;
		}
	}
	
	return false;
}

//name: addItem
//description: attempts to add an item into the tradeSession object
function tradeSession::addItem(%this,%client,%item,%amount)
{
	if(%amount $= "" || %amount <= 0)
	{
		return false;
	}
	
	%inv = %client.inventory;
	
	if(!isObject(%inv))
	{
		return false;
	}

	if(isObject(%item))
	{
		%item = %item.image;
	}
	
	%itemCheck = %inv.getItem(%item);
	
	if(isObject(%this.getItem(%item)))
	{
		talk("(Existing) - will add item: " @ %item @ " | amount: " @ %amount);
		if(%amount <= %itemCheck.amount)
		{
			%this.getItem(%item).amount += %amount;
			%inv.getItem(%item).amount -= %amount;
			%this.resetAccepted();
			return true;
		}
	}
	else
	{
		if(%amount <= %itemCheck.amount)
		{
			talk("(New Instance) will add item: " @ %item @ " | amount: " @ %amount);

			%tradeSessionItem = new scriptObject()
			{
				inventory = $class::inventory;
				class = ItemInfo;
				image = %itemCheck.image;
				amount = %amount;
				owner = %client;
			};
			
			%itemCheck.amount-=%amount;
			%this.add(%tradeSessionItem);
			%this.resetAccepted();
			return true;
		}
	}
	
	return false;
}

//name: getItem
//description: returns an item from the trade session
//argument should be a string
function tradeSession::getItem(%this,%item)
{
	%c = %this.getCount();
	for(%i=0;%i<%c;%i++)
	{
		%sessionItem = %this.getObject(%i);
		if(%sessionItem.image $= %item)
		{
			return %sessionItem;
		}
	}
	return false;
}

//name: removeItem
//description: attempts to remove an item from the tradeSession object
function tradeSession::removeItem(%this,%client,%item,%amount)
{
	%itemCheck = %this.getItem(%item);
	if(isObject(%itemCheck))
	{
		if(%amount $= "" || %amount <= 5)
			return false;
		
		if(%amount > %itemCheck.amount)
		{
			%amount = %itemCheck.amount;
		}
		
		%leftOver = %itemCheck.amount - %amount;
		
		if(%leftOver > 0)
		{
			//dont remove the item
			//talk("Keeping instance since amount is less than session amount.");
			%itemCheck.amount-=%amount;
			%client.inventory.addItem(%item,%amount);
			%this.resetAccepted();
			return true;
		}
		else
		{
			//we can remove the item
			//talk("Removing instance.");
			%client.inventory.addItem(%item,%amount);
			%itemCheck.delete();
			%this.resetAccepted();
			return true;
		}
	}
	
	return false;
}

//name: accept
//description: %client accepts the trade.
//once both clients accept, we can call the ::exchange function
function tradeSession::accept(%this,%client)
{
	if(%this.tradeAccepted[%client] $= "")
	{
		%this.tradeAccepted[%client] = true;
	}
	
	%cl1 = %this.client1;
	%cl2 = %this.client2;
	
	if(%this.tradeAccepted[%cl1] && %this.tradeAccepted[%cl2])
	{
		//call exchange
		//%this.resetAccepted();
		%this.exchange();
	}
}

//name: resetAccepted
//description: resets the accepted value
//between two clients.
function tradeSession::resetAccepted(%this)
{
	%this.tradeAccepted[%this.client1] = "";
	%this.tradeAccepted[%this.client2] = "";
}

//name: cancel
//description: will cancel the trade session
//between two users 
function tradeSession::cancel(%this,%client)
{
	%c = %this.getCount();
	for(%i=0;%i<%c;%i++)
	{
		%item = %this.getObject(%i);
		%owner = %item.owner;
		
		if(%owner $= "") 
			continue;
		
		%owner.inventory.addItem(%item.image,%item.amount);
	}
	
	//remove objects
	%this.deleteAll();
	%this.resetAccepted();
	%this.endTrade();
}

//name: exchange
//description: exchanges the current items in the session
//between the two users trading.
function tradeSession::exchange(%this)
{
	%cl1 = %this.client1;
	%cl2 = %this.client2;
	%c = %this.getCount();
	for(%i=0;%i<%c;%i++)
	{
		%item = %this.getObject(%i);
		if(%item.owner == %cl1)
		{
			%cl2.inventory.addItem(%item.image,%item.amount);
		}
		else
		{
			%cl1.inventory.addItem(%item.image,%item.amount);	
		}
	}
	%this.deleteAll();
	%this.endTrade();
	return true;
}

//name: endTrade
//description: ends the trade session
function tradeSession::endTrade(%this)
{
	%this.client1.tradeSession = "";
	%this.client2.tradeSession = "";
	%this.schedule(1,delete);
}
