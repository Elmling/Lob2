if(isObject($menu::buildingTools))
	$menu::buildingTools.delete();

$menu::buildingTools = $class::menuSystem.newMenuObject("buildingTools","You can type /bcmds to see build commands.");

$menu::buildingTools.setname("buildingToolsMenu");
$menu::buildingTools.class = "buildingToolsMenu";

$menu::buildingTools.addMenuItem("Delete Bricks by Color","$menu::buildingTools.showInputMenu(#CLIENT);");

function serverCmdBT(%client)
{
	serverCmdBuildingTools(%client);
}

function serverCmdbcmds(%client)
{
	%m = "<div:1>\c6/getcolorid (color ID of brick you're looking at)";
	messageClient(%client,'',%m);
}

function serverCmdBuildingTools(%client)
{
	if(!isobject(%client.ndselection))
	{
		messageclient(%client,'',"\c6You need a selection first ( /d )");
		return false;
	}
	
	$menu::buildingTools.showMenu(%client);
}

function serverCmdGetColorId(%client)
{		
	%time = getSimTime();
	if(%time - %client.lastColorIdCheck > 500)
	{
		%a = %client.player;
		%EyeVector = %a.getEyeVector();
		%EyePoint = %a.getEyePoint();
		%Range = 4;
		%RangeScale = VectorScale(%EyeVector, %Range);
		%RangeEnd = VectorAdd(%EyePoint, %RangeScale);
		%raycast = containerRayCast(%eyePoint,%RangeEnd,$TypeMasks::fxbrickobjecttype, %a);
		%o = getWord(%raycast,0);
		
		if(isObject(%o))
		{
			%colorid = %o.colorId;
			messageClient(%client,'',"\c6ColorID: " @ %colorId);
			%client.lastColorIdCheck = %time;
		}
	}
}

package menu_buildingTools
{
	function menuObject::onInputValueRecieved(%this,%client,%value)
	{
		if(%this.name $= "buildingTools")
		{
			if(isNumber(%value))
			{
				$class::buildingTools.removeByColorID(%value,%client.ndSelection);
			}
		}
		return parent::onInputValueRecieved(%this,%client,%value);
	}		
		
};
activatePackage(menu_buildingTools);