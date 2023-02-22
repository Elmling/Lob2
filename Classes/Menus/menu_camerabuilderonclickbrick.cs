if(isObject($menu::CameraBuilderOnClickBrick)) $menu::CameraBuilderOnClickBrick.delete();
$menu::CameraBuilderOnClickBrick = $class::menuSystem.newMenuObject("CameraBuilderOnClickBrick","appls");
$menu::CameraBuilderOnClickBrick.addMenuItem("Colors","serverCmdCB_onSelectBrick(#CLIENT);");
$menu::CameraBuilderOnClickBrick.addMenuItem("Delete","%m=$class::menuSystem.getmenu(\"camerabuilderdelete\");%m.showmenu(#CLIENT,0);");
$menu::CameraBuilderOnClickBrick.setName("CameraBuilderOnClickBrick"); $menu::CameraBuilderOnClickBrick.class = "Menu";
 

function serverCmdCB_onSelectBrick(%cl) {
	talk("worked");
}
