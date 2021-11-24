if(isObject($menu::input))
	$menu::input.delete();

$menu::input = $class::menuSystem.newMenuObject("input","Menu input");
$menu::input.addMenuItem("Enter Input","$menu::input.showInputMenu(#CLIENT);");