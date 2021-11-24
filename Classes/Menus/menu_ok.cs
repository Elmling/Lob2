if(isObject($menu::ok))
	$menu::ok.delete();

$menu::ok = $class::menuSystem.newMenuObject("Ok","Menu Ok");
$menu::ok.addMenuItem("Ok","%a=1");
