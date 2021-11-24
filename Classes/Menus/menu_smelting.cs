if(isObject($menu::smelting))
	$menu::smelting.delete();

$menu::smelting = $class::menuSystem.newMenuObject("smelting","What would you like to smelt?");

$menu::smelting.setname("smelting");
$menu::smelting.class = "smelting";

$menu::smelting.addMenuItem("Bronze Bar","$class::smelting.smelt(\"bronze\",#CLIENT.player,10);");