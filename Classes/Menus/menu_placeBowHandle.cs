//MENU placeBowHandle Generated On 2020-01-30

if(isObject($menu::placeBowHandle))
	$menu::placeBowHandle.delete();

$menu::placeBowHandle = $class::menuSystem.newMenuObject("placeBowHandle","Which Type Of Bow Handle?");

$menu::placeBowHandle.setName("placeBowHandleMenu");
$menu::placeBowHandle.class = "Menu";

//MENU INPUT $menu::placeBowHandle.addMenuItem("Menu Item","$menu::placeBowHandle.showInputMenu(#CLIENT);");

$menu::placeBowHandle.addMenuItem("Long Bow Handle","#CLIENT.crafting_bow_handle=\"Long\";$class::menuSystem.getmenu(\"crafting_handle_wood_type\").showmenu(#CLIENT);");
$menu::placeBowHandle.addMenuItem("Short Bow Handle","#CLIENT.crafting_bow_handle=\"Short\";$class::menuSystem.getmenu(\"crafting_handle_wood_type\").showmenu(#CLIENT);");//#CLIENT.craftingStart($class::crafting.getRecipe(\"\"),1);");


package class_menu_placeBowHandle
{
	function _a(){}
};
activatePackage(class_menu_placeBowHandle);