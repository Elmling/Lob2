//***** class description start *****
//		author: Elm
//
//		description - nodeMemory Class
//		This class is the "brains" of the
//		AI class. It is used to manage nodes
//		that AI's will utilize.
//		See: AI.cs class.
//
//***** class description end ***** 

//name: addToMemory
//description: adds a node to the nodeMemory class
//of a specific AIPlayer utilizing this class
function nodeMemory::addToMemory(%this,%position,%brick)
{
	if(%this.isInMemory(%position))
		return false;
	
	if(%this.getCount() >= %this.cap)
	{
		
		%this.clearNodeBytime();
	}
	
	%node = new scriptObject()
	{
		position = %position;
		brick = %brick;
		time = getSimTime();
	};
	
	%this.add(%node);
}

//name: isInMemory
//description: checks to see if a position
//is actually a node and is in the nodeMemory's
//memory
function nodeMemory::isInMemory(%this,%position)
{
	%c = %this.getCount();
	for(%i=0;%i<%c;%i++)
	{
		%o = %this.getObject(%i);
		if(%o.position $= %position)
			return true;
	}
	
	return false;
}

//name: clearMemory
//description: clears all nodes from the nodeMemory
function nodeMemory::clearMemory(%this)
{
	if((%c=%this.getCount()) <= 0)
		return false;
	for(%i=%this.getCount();%i>=0;%i--)
	{

		%o = %this.getObject(%i);
		if(!isObject(%o))continue;
		if(isobject(%o.brick))
			%o.brick.setcolor(3);
		%this.remove(%o);
		if(isobject(%o))%o.delete();

	}
}

//name: clearNodeByTime
//description: Clears all nodes that the
//AIPlayer hasn't used in a while, to prevent
//potential memory hogging.
function nodeMemory::clearNodeByTime(%this)
{
	%c = %this.getCount();
	%time = getSimTime();
	for(%i=0;%i<%c;%i++)
	{
		%o = %this.getObject(%i);
		if(%lastHighestObj $= "")
		{
			%lastHighestObj = %o;
			%lastHighestTime = %o.time;
		}
		if(%time - %o.time > %time - %lastHighestTime)
		{
			%lastHighestObj = %o;
			%lastHighestTime = %o.time;				
		}
	}
	if(isObject(%lastHighestObj))
	{
		%lastHighestObj.brick.setColor(3);
		%lastHighestObj.delete();
	}
}