if(isObject($menu::world_exchanger))
	$menu::world_exchanger.delete();

$menu::world_exchanger = $class::menuSystem.newMenuObject("World Exchanger","Hello there, welcome to the World Exchange.");
$menu::world_exchanger.addMenuItem("Exchange","%a=1;");
$menu::world_exchanger.addMenuItem("Help","%a=1;");
$menu::world_exchanger.addMenuItem("Okay","%a=1;");