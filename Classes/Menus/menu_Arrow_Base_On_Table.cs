//MENU Arrow Base On Table Generated On 2020-01-21

if(isObject($menu::Arrow_Base_On_Table))
	$menu::Arrow_Base_On_Table.delete();

$menu::Arrow_Base_On_Table = $class::menuSystem.newMenuObject("Arrow Base On Table","Place An Arrow Base Type");

$menu::Arrow_Base_On_Table.setName("Arrow Base On Table");
$menu::Arrow_Base_On_Table.class = "Menu";

//MENU INPUT $menu::Arrow Base On Table.addMenuItem("Menu Item","$menu::Arrow Base On Table.showInputMenu(#CLIENT);");

$menu::Arrow_Base_On_Table.addMenuItem("Pine Arrow Base","#CLIENT.crafting_arrow_base = \"pine\";$class::menuSystem.getMenu(\"arrowtiptype\").showMenu(#CLIENT);");
$menu::Arrow_Base_On_Table.addMenuItem("Oak Arrow Base","");
$menu::Arrow_Base_On_Table.addMenuItem("Willow Arrow Base","");
$menu::Arrow_Base_On_Table.addMenuItem("Yew Arrow Base","");


package class_menu_Arrow_Base_On_Table
{
	function _a(){}
};
activatePackage(class_menu_Arrow_Base_On_Table);