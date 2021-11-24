//MENU onClickSpinningWheel Generated On 2020-01-30

if(isObject($menu::onClickSpinningWheel))
	$menu::onClickSpinningWheel.delete();

$menu::onClickSpinningWheel = $class::menuSystem.newMenuObject("onClickSpinningWheel","It's a Spinning Wheel.. I can place Flax here");

$menu::onClickSpinningWheel.setName("onClickSpinningWheelMenu");
$menu::onClickSpinningWheel.class = "Menu";

//MENU INPUT $menu::onClickSpinningWheel.addMenuItem("Menu Item","$menu::onClickSpinningWheel.showInputMenu(#CLIENT);");

$menu::onClickSpinningWheel.addMenuItem("Place Flax","#CLIENT.craftingStart($class::crafting.getRecipe(\"Bow String\"),1);");


package class_menu_onClickSpinningWheel
{
	function _a(){}
};
activatePackage(class_menu_onClickSpinningWheel);