if(isObject($menu::Cooking)) $menu::Cooking.delete();
$menu::Cooking = $class::menuSystem.newMenuObject("Cooking","What would you like to cook?");
$menu::Cooking.addMenuItem("Fish","%m=$class::menuSystem.getmenu(\"cookingFishing\");%m.showmenu(#CLIENT,0);");
$menu::Cooking.setName("Cooking"); $menu::Cooking.class = "Menu";
