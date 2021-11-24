//MENU crafting_handle_wood_type Generated On 2020-01-30

if(isObject($menu::crafting_handle_wood_type))
	$menu::crafting_handle_wood_type.delete();

$menu::crafting_handle_wood_type = $class::menuSystem.newMenuObject("crafting_handle_wood_type","A menu entry");

$menu::crafting_handle_wood_type.setName("crafting_handle_wood_typeMenu");
$menu::crafting_handle_wood_type.class = "Menu";

//MENU INPUT $menu::crafting_handle_wood_type.addMenuItem("Menu Item","$menu::crafting_handle_wood_type.showInputMenu(#CLIENT);");

//MENU REGULAR $menu::crafting_handle_wood_type.addMenuItem("Menu Item","");
$menu::crafting_handle_wood_type.addMenuItem("Pine","#CLIENT.crafting_wood_handle_type=\"Pine\";#CLIENT.craftingStart($class::crafting.getRecipe(\"Pine \" @ #CLIENT.crafting_bow_handle @ \" Bow\"),1);");
$menu::crafting_handle_wood_type.addMenuItem("Oak","#CLIENT.crafting_wood_handle_type=\"Oak\";");
$menu::crafting_handle_wood_type.addMenuItem("Maple","#CLIENT.crafting_wood_handle_type=\"Maple\";");
$menu::crafting_handle_wood_type.addMenuItem("Willow","#CLIENT.crafting_wood_handle_type=\"Willow\";");
$menu::crafting_handle_wood_type.addMenuItem("Yew","#CLIENT.crafting_wood_handle_type=\"Yew\";");


package class_menu_crafting_handle_wood_type
{
	function _a(){}
};
activatePackage(class_menu_crafting_handle_wood_type);