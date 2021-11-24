if(isObject($menu::brickAger))
	$menu::brickAger.delete();

$menu::brickAger = $class::menuSystem.newMenuObject("brickAger","Random Calc: \n<font:arial:17>\c2if \c0getRandom(0,randomcap) <= random \c2then \c0(change color)");

$menu::brickAger.setname("brickAgerMenu");
$menu::brickAger.class = "brickAgerMenu";

$menu::brickAger.addMenuItem("Set Random","$menu::brickAger.showInputMenu(#CLIENT);");
$menu::brickAger.addMenuItem("Age Selected Bricks","$class::brickAger.ageSelected(#CLIENT.ndSelection);");
$menu::brickAger.addMenuItem("Add Selected Colors","$class::brickAger.useColors(#CLIENT.ndSelection);");
$menu::brickAger.addMenuItem("Clear Colors","$class::brickAger.removeAllColors(#CLIENT.ndSelection);");

function serverCmdBrickAger(%client)
{
	if(!isobject(%client.ndselection))
	{
		messageclient(%client,'',"\c6You need a selection first ( /d )");
		return false;
	}
	
	$menu::brickAger.showMenu(%client);
}

function servercmdba(%client)
{
	servercmdbrickager(%client);
}

function serverCmdAgeBricks(%client)
{
	if(!isobject(%client.ndselection))
	{
		messageclient(%client,'',"\c6You need a selection first ( /d )");
		return false;
	}
	
	if($class::brickAger.getCount() <= 0)
	{
		messageclient(%client,'',"\c6You need a color list first ( /d )");
		return false;		
	}
	
	$class::brickAger.ageSelected(%client.ndselection);
}

package class_menu_brickAger
{
	function menuObject::onInputValueRecieved(%this,%client,%selectedTxt,%value)
	{
		if(%this.name $= "brickAgerMenu")
		{
			if(%value * 1 $= %value)
			{
				if(%selectedTxt $= "set random")
				{
					$class::brickAger.random = %value;
				}
				else
				if(%selectedTxt $= "set random cap")
				{
					$class::brickAger.randomcap = %value;
				}
			}
		}
		return parent::onInputValueRecieved(%this,%client,%selectedTxt,%value);
	}
};
activatePackage(class_menu_brickAger);