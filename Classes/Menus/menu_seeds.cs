if(isObject($menu::seeds))
	$menu::seeds.delete();

$menu::seeds = $class::menuSystem.newMenuObject("seeds","Choose What To Plant");
$menu::seeds.addMenuItem("Potato","$class::farming.plant(#CLIENT,\"Potato\");");
$menu::seeds.addMenuItem("Flax","$class::farming.plant(#CLIENT,\"Flax\");");