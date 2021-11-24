//MENU smithingOptions Generated On 2020-01-25

if(isObject($menu::smithingOptions))
	$menu::smithingOptions.delete();

$menu::smithingOptions = $class::menuSystem.newMenuObject("smithingOptions","What would you like to Smith?");

$menu::smithingOptions.setName("smithingOptionsMenu");
$menu::smithingOptions.class = "Menu";

//MENU INPUT $menu::smithingOptions.addMenuItem("Menu Item","$menu::smithingOptions.showInputMenu(#CLIENT);");

$menu::smithingOptions.addMenuItem("Arrow Tips","#CLIENT.smithing_item=\"Arrow Tips\";%m=$class::menusystem.getmenu(\"smithingamount\");%m.showmenu(#CLIENT,0);");
$menu::smithingOptions.addMenuItem("Plate Helmet","#CLIENT.smithing_item=\"Plate Helmet\";%m=$class::menusystem.getmenu(\"smithingamount\");%m.showmenu(#CLIENT,0);");
$menu::smithingOptions.addMenuItem("Plate Armor","#CLIENT.smithing_item=\"Plate Armor\";%m=$class::menusystem.getmenu(\"smithingamount\");%m.showmenu(#CLIENT,0);");
$menu::smithingOptions.addMenuItem("Plate Legs","#CLIENT.smithing_item=\"Plate Legs\";%m=$class::menusystem.getmenu(\"smithingamount\");%m.showmenu(#CLIENT,0);");
//$menu::smithingOptions.addMenuItem("Boots","#CLIENT.smithing_item=\"Boots\";%m=$class::menusystem.getmenu(\"smithingamount\");%m.showmenu(#CLIENT,0);");


package class_menu_smithingOptions
{
	function _a(){}
};
activatePackage(class_menu_smithingOptions);