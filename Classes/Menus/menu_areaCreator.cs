if(isObject($menu::areaCreator))
	$menu::areaCreator.delete();

$menu::areaCreator = $class::menuSystem.newMenuObject("areaCreator","You are about to define a new area.");

$menu::areaCreator.setname("areaCreatorMenu");
$menu::areaCreator.class = "areaCreatorMenu";

$menu::areaCreator.addMenuItem("Enter Area Name","$menu::areaCreator.showInputMenu(#CLIENT);");

function serverCmdArea(%client)
{
	if(!isobject(%client.ndselection))
	{
		messageclient(%client,'',"\c6You need a selection first ( /d )");
		return false;
	}
	
	$menu::areaCreator.showMenu(%client);
}

package class_menu_areaCreator
{
	function menuObject::onInputValueRecieved(%this,%client,%selectedTxt,%value)
	{
		if(%value !$= "" && %this.name $= "areaCreator")
		{
			$class::areas.newArea_dupe(%value,%client.ndSelection);
			messageClient(%client, 'MsgUploadStart', "");
			talk(%client.name @ " defined a new area ( " @ %value @ " )");
		}
	}
};
activatePackage(class_menu_areaCreator);