
if(!isObject($class::display))
	$class::display = new scriptGroup(display);

function display::initialize(%this)
{
		for(%i=%this.getCount()-1;%i>=0;%i--)
		{
			%this.getObject(%i).delete();
		}
		%this.display["hunger"] = new scriptObject()
		{
			font = "<font:impact:22>";
			text["bottom"] = "You are kind of hungry.";
			time = 3;
		};
		
		%this.add(%this.display["hunger"]);
}

function display::show(%this,%cl,%type)
{
	if(!isObject(%this.display[%type]))
	{
		return false;
	}
	if(!isObject(%cl))
	{
		return false;
	}
	
	%display = %this.display[%type];
	
	if(%display.text["bottom"] !$= "")
	{
		commandToClient(%cl,'bottomprint',%display.text["bottom"],%display.time);
	}
	
}

display.initialize();