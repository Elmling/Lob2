//***** class description start *****
//		author: Elm
//
//		description - Brick Ager Class
//		requires: Zeblote's Duplicator
//		https://forum.blockland.us/index.php?topic=288602.0
//		
//		Attempts to make a stack of bricks look
//		'aged' as in, different colored stones.
//		This class utilizes the menu class to present
//		users with a simple interface to work with
//
//		step 1: plant 4 or more different colored bricks and select them with the dupe
//		step 2: highlight the stack - press enter
//		step 3: user types /brickAger and is presented with a menu
//		step 4: user selects 'add selected colors' option on the menu
//		step 5: make a new selection on another stack of bricks you wish to 'age' - press enter 
//		step 6: type /brickAger again, and select 'age select bricks'
//
//***** class description end ***** 

//***** class brickAger ***** 

if(!isobject($class::brickAger))
	$class::brickAger = new scriptGroup(brickAger)
	{
		random = 60;
		randomCap = 100;
	};

//name: useColors
//description: Will attempt to age a selection
function brickAger::useColors(%this,%selection)
{
	%c = %selection.brickcount;
	
	for(%i=0;%i<%c;%i++)
	{
		%brick = $ns[%selection,"b",%i];
		
		%obj = new scriptObject()
		{
			brick = %brick;
			colorId = %brick.colorid;
		};
		
		%this.add(%obj);
	}
	messageClient(%selection.client, 'MsgUploadStart', "");
	%client = %selection.client;
	talk(%client.name @ " added " @ %c @ " different colors (" @ %this.getcount() @ " total).");
}

//name: removeAllColors
//description: Removes all colors from memory
function brickAger::removeAllColors(%this,%selection)
{
	%c=%this.getcount()+1;
	for(%i=%c;%i>0;%i--)
	{
		%this.getobject(%i).delete();
	}
	%this.getobject(0).delete();
	%client = %selection.client;
	messageClient(%selection.client, 'MsgUploadStart', "");
	talk(%client.name @ " removed " @ %c-1 @ " colors.");
}

//name: ageSelected
//description: ages the selected bricks
function brickAger::ageSelected(%this,%selection)
{
	if(%this.getcount() <= 0)
		return false;
	%c = %selection.brickcount;
	
	for(%i=0;%i<%c;%i++)
	{
		%brick = $ns[%selection,"b",%i];
		if(%this.shouldAge())
		{
			%color = %this.getRandomColor();
			%brick.setColor(%color);
		}
	}
	
	messageClient(%selection.client, 'MsgUploadStart', "");
	talk(%selection.client.name @ " aged " @ %c @ " bricks.");
}

//name: setRandom
//description: This is calculated in the shouldAge method
function brickAger::setRandom(%this,%int)
{
	%this.random = %int;
}

//name: shouldAge
//description: Performs some logic to determine if a brick
//should be aged or not.
function brickAger::shouldAge(%this)
{
	if(getRandom(0,100) <= %this.random)
	{
		return true;
	}
	return false;
}

//name: getRandomColor
//description: gets a random color from
//the current list of colors specified
//by the user
function brickAger::getRandomColor(%this)
{
	if(%this.getcount() > 0)
	{
		%color = %this.getobject(getRandom(0,%this.getcount()-1)).colorID;
		return %color;
	}
	
	return false;
}