if(isObject($menu::CookingFishing)) $menu::CookingFishing.delete();
$menu::CookingFishing = $class::menuSystem.newMenuObject("CookingFishing","Choose a fish");
$menu::CookingFishing.addMenuItem("Raw Salmon","talk(\"Works\");");
$menu::CookingFishing.setName("CookingFishing"); $menu::CookingFishing.class = "Menu";
