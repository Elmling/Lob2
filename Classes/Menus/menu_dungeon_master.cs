//MENU dungeon_master Generated On 2021-04-07

if(isObject($menu::dungeon_master))
	$menu::dungeon_master.delete();

$menu::dungeon_master = $class::menuSystem.newMenuObject("dungeon_master","I am the Dungeon Master..");

$menu::dungeon_master.setName("dungeon_masterMenu");
$menu::dungeon_master.class = "Menu";

//MENU INPUT $menu::dungeon_master.addMenuItem("Menu Item","$menu::dungeon_master.showInputMenu(#CLIENT);");

//MENU REGULAR $menu::dungeon_master.addMenuItem("Menu Item","");


$menu::dungeon_master.addMenuItem("Help","servercmdhelp(#CLIENT);");
$menu::dungeon_master.addMenuItem("Starter Pack","servercmdstarterpack(#CLIENT);");
$menu::dungeon_master.addMenuItem("Joining Dungeons","servercmdDungeonHowToJoin(#CLIENT);");


package class_menu_dungeon_master
{
	function _a(){}
};
activatePackage(class_menu_dungeon_master);