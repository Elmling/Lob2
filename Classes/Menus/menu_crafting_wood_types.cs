//MENU crafting_wood_types Generated On 2020-01-30

if(isObject($menu::crafting_wood_types))
	$menu::crafting_wood_types.delete();

$menu::crafting_wood_types = $class::menuSystem.newMenuObject("crafting_wood_types","Which type of Wood?");

$menu::crafting_wood_types.setName("crafting_wood_typesMenu");
$menu::crafting_wood_types.class = "Menu";

//MENU INPUT $menu::crafting_wood_types.addMenuItem("Menu Item","$menu::crafting_wood_types.showInputMenu(#CLIENT);");

$menu::crafting_wood_types.addMenuItem("Pine","#CLIENT.crafting_wood_type=\"Pine\";$class::menusystem.getMenu(\"crafting\").showmenu(#CLIENT,0);");
$menu::crafting_wood_types.addMenuItem("Oak","#CLIENT.crafting_wood_type=\"Oak\";");
$menu::crafting_wood_types.addMenuItem("Maple","#CLIENT.crafting_wood_type=\"Maple\";");
$menu::crafting_wood_types.addMenuItem("Willow","#CLIENT.crafting_wood_type=\"Willow\";");
$menu::crafting_wood_types.addMenuItem("Yew","#CLIENT.crafting_wood_type=\"Yew\";");


package class_menu_crafting_wood_types
{
	function _a(){}
};
activatePackage(class_menu_crafting_wood_types);