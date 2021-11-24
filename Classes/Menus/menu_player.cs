
if(isObject($menu::player))
	$menu::player.delete();

$menu::player = $class::menuSystem.newMenuObject("Player","This is a Player:");
$menu::player.addMenuItem("Burn Player","#CLIENT.lastClicked.burn(7);");
$menu::player.addMenuItem("Okay.","%a=1;");

if(isObject($menu::bob))
	$menu::bob.delete();

$menu::bob = $class::menuSystem.newMenuObject("Bob","This is Bob");
$menu::bob.addMenuItem("Tasks","$menu::bobTasks.showMenu(\"bobTasks\",#CLIENT,0);");
$menu::bob.addMenuItem("Calculate","$menu::bob.showInputMenu(#CLIENT);");
$menu::bob.addMenuItem("Avatar","$menu::bobAvatar.showMenu(\"bobAvatar\",#CLIENT,0);");
$menu::bob.addMenuItem("Push","findBotByName(\"Bob\").doPush(#CLIENT);");
$menu::bob.addMenuItem("Okay.","%a=1;");

if(isObject($menu::bobTasks))
	$menu::bobTasks.delete();

$menu::bobTasks = $class::menuSystem.newMenuObject("BobTasks","This is Bob's Tasks");
$menu::bobTasks.addMenuItem("Roam","findbotbyname(\"bob\").clearTask();findbotbyname(\"bob\").newTask(\"roam\");");
$menu::bobTasks.addMenuItem("Follow","findbotbyname(\"bob\").clearTask();findbotbyname(\"bob\").newTask(\"follow\",findbotbyname(\"bob\").getClosestPlayer(#CLIENT));");
$menu::bobTasks.addMenuItem("Chase","findbotbyname(\"bob\").clearTask();findbotbyname(\"bob\").newTask(\"chase\");");
$menu::bobTasks.addMenuItem("Idle","findbotbyname(\"bob\").clearTask();");

if(isObject($menu::bobAvatar))
	$menu::bobAvatar.delete();

$menu::bobAvatar = $class::menuSystem.newMenuObject("BobAvatar","Choose Bob's Avatar");
$menu::bobAvatar.addMenuItem("Swordsman","findbotbyname(\"Bob\").setCustomAvatar(\"swordsman\");");
$menu::bobAvatar.addMenuItem("Spearman","findbotbyname(\"Bob\").setCustomAvatar(\"spearman\");");
$menu::bobAvatar.addMenuItem("Dueler","findbotbyname(\"Bob\").setCustomAvatar(\"ai_dueler\");");

if(isObject($menu::brick))
	$menu::brick.delete();

$menu::brick = $class::menuSystem.newMenuObject("Brick","A brick you can build with.");
$menu::brick.addMenuItem("Select Color","servercmdgetcolor(#CLIENT);");
$menu::brick.addMenuItem("Select Brick","servercmdgetbrick(#CLIENT);");
$menu::brick.addMenuItem("Equip Duplicator","servercmdd(#CLIENT);");
$menu::brick.addMenuItem("Okay","%a=1;");
