//MENU smithingAmount Generated On 2020-01-29

if(isObject($menu::smithingAmount))
	$menu::smithingAmount.delete();

$menu::smithingAmount = $class::menuSystem.newMenuObject("smithingAmount","Amount to Smith?");

$menu::smithingAmount.setName("smithingAmountMenu");
$menu::smithingAmount.class = "Menu";

//MENU INPUT $menu::smithingAmount.addMenuItem("Menu Item","$menu::smithingAmount.showInputMenu(#CLIENT);");

$menu::smithingAmount.addMenuItem("1","$class::smithing.smith(#CLIENT,#CLIENT.smithing_item,#CLIENT.smithing_bar_type,1);");
$menu::smithingAmount.addMenuItem("2","");
$menu::smithingAmount.addMenuItem("3","");
$menu::smithingAmount.addMenuItem("All","");

package class_menu_smithingAmount
{
	function _a(){}
};
activatePackage(class_menu_smithingAmount);