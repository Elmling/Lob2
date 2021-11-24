if(isObject($menu::YesNo))
	$menu::YesNo.delete();

$menu::YesNo = $class::menuSystem.newMenuObject("YesNo","Menu YesNo");
$menu::YesNo.addMenuItem("Yes","%a=1");
$menu::YesNo.addMenuItem("No","%a=1");