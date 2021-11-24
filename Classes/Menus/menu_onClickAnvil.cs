//MENU onClickAnvil Generated On 2020-01-25

if(isObject($menu::onClickAnvil))
	$menu::onClickAnvil.delete();

$menu::onClickAnvil = $class::menuSystem.newMenuObject("onClickAnvil","It's an Anvil.. I can smith items.");

$menu::onClickAnvil.setName("onClickAnvilMenu");
$menu::onClickAnvil.class = "Menu";

//MENU INPUT $menu::onClickAnvil.addMenuItem("Place Bronze Bar","$menu::onClickAnvil.showInputMenu(#CLIENT);");

$menu::onClickAnvil.addMenuItem("Place Bronze Bar","%m = $class::menuSystem.getmenu(\"smithingoptions\");%m.showmenu(#CLIENT,0);#CLIENT.smithing_bar_type=\"Bronze\";");
$menu::onClickAnvil.addMenuItem("Place Iron Bar","%m = $class::menuSystem.getmenu(\"smithingoptions\");%m.showmenu(#CLIENT,0);#CLIENT.smithing_bar_type=\"Iron\";");
//$menu::onClickAnvil.addMenuItem("Place Bronze Bar","%m = $class::menuSystem.getmenu(\"smithingoptions\");%m.showmenu(#CLIENT,0);#CLIENT.smithing_bar_type=\"Bronze\";");


package class_menu_onClickAnvil
{
	function _a(){}
};
activatePackage(class_menu_onClickAnvil);