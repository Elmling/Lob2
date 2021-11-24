//MENU tutorial_master Generated On 2021-04-28

if(isObject($menu::tutorial_master))
	$menu::tutorial_master.delete();

$menu::tutorial_master = $class::menuSystem.newMenuObject("tutorial_master","Hello, I am the Tutorial Master..");

$menu::tutorial_master.setName("tutorial_masterMenu");
$menu::tutorial_master.class = "Menu";

//MENU INPUT $menu::tutorial_master.addMenuItem("Menu Item","$menu::tutorial_master.showInputMenu(#CLIENT);");
//MENU REGULAR $menu::tutorial_master.addMenuItem("Menu Item","");

$menu::tutorial_master.addMenuItem("Get Task","talk(\"You fokin wot m8?\");");
$menu::tutorial_master.addMenuItem("Leave Tutorial","if(#CLIENT.profile.tutorial_complete)servercmdhome(#CLIENT);");


package class_menu_tutorial_master
{
	function _a(){
		
	}
};
activatePackage(class_menu_tutorial_master);