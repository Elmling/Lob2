

while(isobject($class::smithing))
	$class::smithing.delete();

$class::smithing = new scriptGroup()
{
	class = smithing;
	bars_needed["Arrow Tips"] = 1;
	bars_converted["Arrow Tips"] = 5;
	
	bars_needed["Plate Helmet"] = 3;
	bars_converted["Plate Helmet"] = 1;
	
	bars_needed["Plate Armor"] = 5;
	bars_converted["Plate Armor"] = 1;
	
	bars_needed["Plate Legs"] = 3;
	bars_converted["Plate Legs"] = 1;
};

function smithing::smith(%this,%client,%item,%barType,%amount)
{
	if(%this.bars_needed[%item] $= "")
	{
		//talk(%item @ " isn't a registered item to be smithed.");
		return false;
	}
	if(%amount $= "")
		%amount = "ALL";
	
	%inv = %client.inventory;
	%_item = %inv.getItem(%barType @ " Bar");
	%bn = %this.bars_needed[%item];
	if(%_item.amount > 0)
	{
		%canmake = mfloor( %_item.amount / %bn);
		
		if(%canmake == 0)
		{
			$class::chat.to(%client,"You do not have enough bars to make this item: \c5" @ %bn @ " \c6bars required.");
			//talk("Does not have enough bars to make " @ %barType SPC %item);
			return false;
		}
		
		//talk("bars needed for " @ %item @ " -> " @ %bn @ " can make " @ %canmake);
		
		if(%bn > %_item.amount)
		{
			
		}
		
		if(%amount !$= "ALL" && %_item.amount < %amount) %amount = %_item.amount;
		
		if(%amount $= "ALL")
			%amount = %canmake;
		
		%result_item_amount = %this.bars_converted[%item] * %amount;
		
		%bar_subtract = %bn * %amount;
		
		%_item.amount -= %bar_subtract;
		
		%inv.addItem(%barType @ " " @ %item,%result_item_amount);

		%_str = %amount @ " bars and recieved " @ %result_item_amount @ " " @ %barType @ " " @ %item @ "";
		%client.player.start_smithing_animation(%amount * 2000,%_str);
		return true;
	}
	else
	{
		//talk(%client.name @ " doesn't have any " @ %barType @ " bars.");
		$class::chat.to(%client,"You do not have enough bars to make this item: \c5" @ %bn @ " \c6bars required.");
		return false;
	}
}

function player::start_smithing_animation(%this,%time,%str)
{
	%this.smithing_animation_start = getsimtime();
	$class::camera.orbitObj(%this.client,%this);
	%this.smithing_animation(%time,%str);
}

function player::smithing_animation(%this,%time,%str)
{
	cancel(%this.smithing_loop);
	%t = getsimtime();
	%time = mclamp(%time,0,20000);
	
	if(%t - %this.smithing_time > 2000)
	{
		%this.smithing_time = %t;
		%this.playthread(0,"smelting");
		
	}
	
	if(%t-%this.smithing_animation_start > %time)
	{
		cancel(%this.smithing_loop);
		%this.smithing_animation_start = "";
		$class::camera.root(%this.client);
		if(%str !$= "") {
			talk(%this.client.name @ " smithed " @ %str);
			$class::chat.to_all(%this.client.name @ " smithed " @ %str);
		}
		return;
	}
	
	
	//%this.smelt_time = %t;
	%this.smithing_loop = %this.schedule(1500,smithing_animation,%time,%str);
}


