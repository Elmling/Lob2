//MENU ArrowTipType Generated On 2020-01-21

if(isObject($menu::ArrowTipType))
	$menu::ArrowTipType.delete();

$menu::ArrowTipType = $class::menuSystem.newMenuObject("ArrowTipType","Choose An Arrow Tip Type");

$menu::ArrowTipType.setName("ArrowTipTypeMenu");
$menu::ArrowTipType.class = "Menu";

//MENU INPUT $menu::ArrowTipType.addMenuItem("Menu Item","$menu::ArrowTipType.showInputMenu(#CLIENT);");

$menu::ArrowTipType.addMenuItem("Bronze Arrow Tips","%ab = #CLIENT.crafting_arrow_base;#CLIENT.craftingStart($class::crafting.getRecipe(\"Bronze \" @ %ab @ \" Arrow\"),10);");


package class_menu_ArrowTipType
{
	function _a(){}
};
activatePackage(class_menu_ArrowTipType);