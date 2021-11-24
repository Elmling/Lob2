

while(isobject($class::smelting))
	$class::smelting.delete();

$class::smelting = new scriptGroup()
{
	class = smelting;
	ores_required["bronze"] = 3;
	ores_needed["bronze"] = "Copper Tin";
	smelting_time["bronze"] = 1500;
};


function smelting::smelt(%this,%bar,%player,%amount)
{
	%c = %player.client;
	%i_c = %c.inventory.getCount();
	
	%amount = %this.max_can_smelt(%c,%bar);
	
	//talk("amount set to " @ %amount);
	
	if(%this.smelt_check(%bar,%player))
	{
		%count = getwordcount(%this.ores_needed[%bar]);
		for(%i=0;%i<%count;%i++)
		{
			%ores_needed = getWord(%this.ores_needed[%bar],%i);
			
			%_remove = %this.ores_required[%bar];
			
			%c.inventory.removeitem(%ores_needed @ " Ore", %_remove * %amount);
			//talk("removed " @ %_remove * %amount @ " " @ %ores_needed);
		}
		
		%c.inventory.addItem(%bar @ " Bar",1 * %amount);		
		%player.start_smelt_animation(%amount * %this.smelting_time[%bar],%amount @ " " @ %bar @ " Bars");
	}
	else
		return false;
}

function player::start_smelt_animation(%this,%time,%str)
{
	%this.smelt_animation_start = getsimtime();
	$class::camera.orbitObj(%this.client,%this);
	%this.smelt_animation(%time,%str);
}

function player::smelt_animation(%this,%time,%str)
{
	cancel(%this.smelt_loop);
	%t = getsimtime();
	%time = mclamp(%time,0,20000);
	
	if(%t - %this.smelt_time > 2000)
	{
		%this.smelt_time = %t;
		%this.playthread(0,"smelting");
		%this.playsound(sound_smelt);
	}
	
	if(%t-%this.smelt_animation_start > %time)
	{
		cancel(%this.smelt_loop);
		%this.smelt_animation_start = "";
		$class::camera.root(%this.client);
		if(%str !$= "")
			$class::chat.to_all(%this.client.name @ " has \c4smelted \c6" @ %str);
		return;
	}
	
	
	//%this.smelt_time = %t;
	%this.smelt_loop = %this.schedule(1500,smelt_animation,%time,%str);
}

function smelting::max_can_smelt(%this,%client,%bar)
{
	%needed = %this.ores_needed[%bar];
	%required = %this.ores_required[%bar];
	
	if(%_count = getWordCount(%needed) > 1)
	{
		%lowest = "";
		for(%i=0;%i<=%_count;%i++)
		{
			%val[%i] = %client.inventory.getItem(getWord(%this.ores_needed[%bar],%i) @ " Ore");
			if(%lowest $= "")
			{
				%lowest = %val[%i].amount;
				%_lowest = %val[%i];
			}
			else if(%val[%i].amount < %lowest)
			{
				%lowest = %val[%i].amount;
				%_lowest = %val[%i];
			}
		}
		
		//talk("Lowest = " @ %lowest @ " " @ %_lowest);
		
		%result = mfloor(%lowest / %required);
		
	}
	else
	{
		%item = %client.inventory.getItem(%needed @ " Ore");
		%result = mfloor(%item.amount / %required);
		
		//talk("@ Lowest = " @ %lowest @ " " @ %_lowest);
	}
	
	
	return %result;
}

function smelting::smelt_check(%this,%bar,%player,%amount)
{
	%c = %player.client;
	%ores_needed = %this.ores_needed[%bar];
	%_on = getWordCount(%ores_needed);
	%found = 0;
	%i_c = %c.inventory.getCount();
	
	for(%j=0;%j<%_on;%j++)
	{
		%current_item = getWord(%ores_needed,%j) @ " Ore";
		//talk("current " @ %current_item);
		for(%i=0;%i<%i_c;%i++)
		{
			%item = %c.inventory.getobject(%i);
			//talk(%item.image);
			if(%item.image $= %current_item)
			{
				//talk("found " @ %item.image);
				if(%this.ores_required[%bar] * %amount <= %item.amount)
				{
					%found[%current_item] = true;
					%found++;
					break;
				}
			}
		}	
	}
	
	if(%found == %_on)
	{
		//talk("Can smelt");
		return true;
	}
	else
	{
		$class::chat.to(%player.client,"You don't have the required ores :  ( \c4" @ %_on @ "\c6 each ) [ \c5" @ strreplace(%ores_needed," "," , ") @ "\c6 ]", 2500);
		return false;
	}
}
