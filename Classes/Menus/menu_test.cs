if(isObject($menu::test)) $menu::test.delete(); if(isObject($menu::test2)) $menu::test2.delete();
$menu::test = $class::menuSystem.newMenuObject("test","this is a test menu");
$menu::test.addMenuItem("toast","%m=$class::menuSystem.getmenu(\"test2\");%m.showmenu(#CLIENT,0);");
$menu::test2 = $class::menuSystem.newMenuObject("test2","does this work?");
$menu::test2.addMenuItem("yes","talk(\"xD\");");
$menu::test.setName("test"); $menu::test.class = "Menu"; $menu::test2.setName("test2"); $menu::test2.class = "Menu";
 


