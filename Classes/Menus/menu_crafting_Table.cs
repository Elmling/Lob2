if(isObject($menu::craftingtable))
	$menu::craftingtable.delete();

$menu::craftingtable = $class::menuSystem.newMenuObject("craftingtable","It's a Crafting Table.");

$menu::craftingtable.setname("craftingtable");
$menu::craftingtable.class = "craftingtable";

$menu::craftingtable.addMenuItem("Place Wood","$class::menusystem.getMenu(\"crafting_wood_types\").showmenu(#CLIENT,0);");//menusystem.getMenu(\"crafting\").showmenu(#CLIENT,0);");
$menu::craftingtable.addMenuItem("Place Bow Handle","$class::menusystem.getMenu(\"placebowhandle\").showmenu(#CLIENT,0);");
$menu::craftingtable.addMenuItem("Place Arrow Base","$class::menusystem.getMenu(\"Arrow Base On Table\").showmenu(#CLIENT,0);");
$menu::craftingtable.addMenuItem("Next","");
package class_menu_crafting_table
{
	function menuObject::onInputValueRecieved(%this,%client,%value)
	{
			//talk("name = " @ %this.name);
		if(%this.name $= "craftingtable")
		{
			%selected = %this.selected[%client];
			
			if(%value $= "pine")
			{				
				%client.crafting = "Pine";
				%this.hideMenu(%client);
				%menu = $class::menusystem.getMenu("crafting");
				%menu.schedule(2,showMenu,%client,0);
			}
			
		}
		
		return parent::onInputValueRecieved(%this,%client,%value);
	}
};
activatePackage(class_menu_crafting_table);
		