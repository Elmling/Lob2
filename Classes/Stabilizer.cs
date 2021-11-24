//***** class description start *****
//		author: Elm
//
//		description - Stabilizer Class
//		A centerprint that users need to keep within
//		lines. It will give an overall score on how the user
//		did and this can be used with many different skills
//		to determine whether or not a user successfully
//		did said skill.
//
//		Example: Crafting. User crafts a Pine handle, if he/she
//		does not score well enough, the Pine handle is not crafted
//		and fails. Can be used for many different skills.
//
//***** class description end ***** 

//***** stabilizer class *****

if(!isObject($class::stabilizer))
{
	$class::stabilizer = new scriptGroup(stabilizer)
	{
		lineCount = 5;
	};
}

//name: begin
//description: begins the stabilizer. %difficulty will be 
//the amount of lines. The more lines, the harder it is
//and the faster it goes off center. line formula is 
//%difficulty * $class::stabilizer.lineCount;. So a %difficulty of 1, will produce 5 lines.
function stabilizer::begin(%this,%client,%difficulty,%milliseconds,%identifier)
{
	%client.stabilizer["difficulty"] = %difficulty;
	%client.stabilizer["ms"] = %milliseconds;
	%client.stabilizer["startTime"] = getSimTime();
	%client.stabilizer["identifier"] = %identifier;
	
	%this.loop(%client);
}

function stabilizer::loop(%this,%client)
{
	cancel(%this.loop[%client]);
	
	%time = getSimTime();
	
	%ran = getRandom(0,1);
	
	%diff = %client.stabilizer["difficulty"];
	%ms = %client.stabilizer["ms"];
	
	if(%time - %client.stabilizer["startTime"] >= %ms)
	{
		%this.end(%client);
		return false;
	}
	
	%timeLeft = %ms - (%time - %client.stabilizer["startTime"]);
	%lines = %diff * $class::stabilizer.linecount;
	%mid = mfloor(%lines/2);
	
	if(%this.stabilizerIndex[%client] $= "")
	{
		%this.stabilizerIndex[%client] = %mid;
	}
	
	if(%ran)
	{
		%move = "left";
	}
	else
	{
		%move = "right";
	}
	
	
	if(%this.stabilizerIndex[%client] == %mid || %this.stabilizerIndex[%client] == (%mid-1) || %this.stabilizerIndex[%client] == (%mid+1))
	{
		%client.stabilizer["score"]+=3;
	}
	else
		%client.stabilizer["score"]-=1;
	

	if(%move $= "left")
	{
		%this.stabilizerIndex[%client]--;
		if(%this.stabilizerIndex[%client] < 0)
			%this.stabilizerIndex[%client] = %lines;
	}
	else
	{
		%this.stabilizerIndex[%client]++;
		if(%this.stabilizerIndex[%client] > %lines)
			%this.stabilizerIndex[%client] = 0;
	}
	
	for(%i=0;%i<%lines;%i++)
	{
		if(%i==%this.stabilizerIndex[%client])
			%l = %l @ "\c3|\c0";
		else
		if(%i==%mid)
			%l = %l @ "\c2|\c0";
		else
		if(%i==(%mid-1))
			%l = %l @ "\c2|\c0";
		else
		if(%i==(%mid+1))
			%l = %l @ "\c2|\c0";
		else
			%l = %l @ "|";
	}
	
	centerPrint(%client,"<font:impact:25>" @ %l @ "\n" @ "<font:impact:16>\c6Use numpad keys 4 and 6 to keep centered!",2);
	bottomPrint(%client,"\c6Score: " @ %client.stabilizer["Score"] @ " | Time Left (Milliseconds): " @ %timeLeft,2);
	%this.loop[%client] = %this.schedule(300,loop,%client);
}

function stabilizer::forceUpdate(%this,%client)
{
	%diff = %client.stabilizer["difficulty"];
	%ms = %client.stabilizer["ms"];
	
	%lines = %diff * $class::stabilizer.linecount;
	%mid = mfloor(%lines/2);	
	%time = getSimTime();
	%timeLeft = %ms - (%time - %client.stabilizer["startTime"]);
	
	if(%this.stabilizerIndex[%client] <= 0)
		%this.stabilizerIndex[%client] = %lines;
	else
	if(%this.stabilizerIndex[%client] > %lines)
		%this.stabilizerIndex[%client] = 0;
	
	for(%i=0;%i<%lines;%i++)
	{
		if(%i==%this.stabilizerIndex[%client])
			%l = %l @ "\c3|\c0";
		else
		if(%i==%mid)
			%l = %l @ "\c2|\c0";
		else
		if(%i==(%mid-1))
			%l = %l @ "\c2|\c0";
		else
		if(%i==(%mid+1))
			%l = %l @ "\c2|\c0";
		else
			%l = %l @ "|";
	}
	
	centerPrint(%client,"<font:impact:25>" @ %l @ "\n" @ "<font:impact:16>\c6Use numpad keys 4 and 6 to keep centered!",2);
	bottomPrint(%client,"\c6Score: " @ %client.stabilizer["Score"] @ " | Time Left (Milliseconds): " @ %timeLeft,2);
}

//name: begin
//description: Returns a score out of 100 on how the client
//performed.
function stabilizer::end(%this,%client)
{
	%client.stabilizer["difficulty"] = "";
	%client.stabilizer["ms"] = "";
	%client.stabilizer["startTime"] = "";
	
	%this.stabilizerIndex[%client] = "";
	$class::stabilizer.lastMove[%client] = "";
	
	%client.stabilizer["lastScore"] = %client.stabilizer["score"];
	%client.stabilizer["score"] = "";
	centerprint(%client,"\c6You scored: " @ %client.stabilizer["lastScore"],3);
	bottomprint(%client,"",2);
}

package class_stabilizer
{
	//name: servercmdsupershiftbrick
	//description: menu support for scrolling
	function servercmdsupershiftbrick(%client,%x,%y,%z)
	{
		if(isEventPending($class::stabilizer.loop[%client]))
		{
			%time = getSimTime();
			if(%time - $class::stabilizer.lastMove[%client] < 60)
				return false;
			if(%y == 1)
				$class::stabilizer.stabilizerIndex[%client]--;
			else
			if(%y == -1)
				$class::stabilizer.stabilizerIndex[%client]++;
			
			$class::stabilizer.lastMove[%client] = %time;
			$class::stabilizer.forceUpdate(%client);
			return false;
		}
		%p = parent::servercmdsupershiftbrick(%client,%x,%y,%z);	
		return %p;
	}
	

	//name: servercmdshiftbrick
	//description: menu support for scrolling
	function servercmdshiftbrick(%client,%x,%y,%z)
	{
		if(isEventPending($class::stabilizer.loop[%client]))
		{
			%time = getSimTime();
			if(%time - $class::stabilizer.lastMove[%client] < 60)
				return false;
			if(%y == 1)
				$class::stabilizer.stabilizerIndex[%client]--;
			else
			if(%y == -1)
				$class::stabilizer.stabilizerIndex[%client]++;
			
			$class::stabilizer.lastMove[%client] = %time;
			$class::stabilizer.forceupdate(%client);
			return false;
		}
		%p = parent::servercmdshiftbrick(%client,%x,%y,%z);	

		return %p;
	}	
};
activatePackage(class_stabilizer);