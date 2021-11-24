//MENU Banker Generated On 2021-05-01

if(isObject($menu::Banker))
	$menu::Banker.delete();

$menu::Banker = $class::menuSystem.newMenuObject("Banker","Hello, you can bank items here.");

$menu::Banker.setName("BankerMenu");
$menu::Banker.class = "Menu";

//MENU INPUT $menu::Banker.addMenuItem("Menu Item","$menu::Banker.showInputMenu(#CLIENT);");

//MENU REGULAR 
//$menu::Banker.addMenuItem("Bank All","servercmdDoBank(#CLIENT);");
$menu::Banker.addMenuItem("Enter Deposit Mode","servercmdEnterBankMode(#CLIENT);");
$menu::Banker.addMenuItem("Enter Withdraw Mode","servercmdEnterwithdrawMode(#CLIENT);");


package class_menu_Banker
{
	function _a(){}
};
activatePackage(class_menu_Banker);
