if(isObject($menu::CraftingTable))
	$menu::CraftingTable.delete();

if(isObject($menu::CraftingTable["Plank"]))
	$menu::CraftingTable["Plank"].delete();

$menu::CraftingTable = $class::menuSystem.newMenuObject("Crafting Table","What would you like to Craft?");

$menu::CraftingTable.setname("Crafting Table");
$menu::CraftingTable.class = "Crafting Table";

$menu::CraftingTable.addMenuItem("Craft Plank","$menu::craftingTable[\"plank\"].showMenu(#CLIENT,0);");//"$menu::Crafting.showInputMenu(#CLIENT);");
//$menu::CraftingTable.addMenuItem("Craft Handle (for weapons/tools)","");//"$class::Crafting.ageSelected(#CLIENT.ndSelection);");
//$menu::CraftingTable.addMenuItem("Craft Brick","");//"$class::Crafting.useColors(#CLIENT.ndSelection);");


$menu::craftingTable["Plank"] = $class::menuSystem.newMenuObject("Crafting Table2","Plank - used for props and buildings");
$menu::CraftingTable["Plank"].setname("Crafting Table2");
$menu::CraftingTable["Plank"].class = "Crafting Table2";
$menu::CraftingTable["Plank"].addMenuItem("Pine (2 Plank)","");
$menu::CraftingTable["Plank"].addMenuItem("Oak (4 Plank)","");
$menu::CraftingTable["Plank"].addMenuItem("Maple (2 Plank)","");
$menu::CraftingTable["Plank"].addMenuItem("Willow (3 Plank)","");

package menu_crafting_plank
{
	function menuObject::onMenuSelectCallBack(%this,%client,%selected)
	{
		%menu = %selected.parent;
		if(%menu.name $= "Crafting Table")
		{
			//%client.crafting["plankType"] = %selected;
		}
		else
		if(%menu.name $= "Crafting Table2")
		{
			%input = $class::menuSystem.getmenu("input");
			%input.setTempBody("Enter an amount, or type All to begin crafting.");
			%input.showInputMenu(%client);
			%input.setdefaultbody();
			%client.crafting["plank"] = true;
			%client.crafting["plankType"] = %selected;
		}
		else
		{
			%p = parent::onMenuSelectCallBack(%this,%client,%selected);
			return %p;
		}
	}
	
	function menuObject::onInputValueRecieved(%this,%client,%value)
	{			
		%p = parent::onInputValueRecieved(%this,%client,%value);
		if(%client.crafting["plank"])
		{
			if( %value $= "all" || ( (%value * 1) == %value && %value > 0 ) )
			{
				if(%value $= "all")%value = 10000000;
				
				%type = %client.crafting["planktype"].text;
				%type = getSubStr(%type,0,striPos(%type,"(")-1);
				
				//talk(%client.name @ " to craft " @ %value @ " " @ %type @ " planks");
				
				%client.crafting["plank"] = "";
				%client.crafting["plankType"] = "";
				
				%recipe = %type @ " Plank";
				%recipe = $class::crafting.getRecipe(%recipe);
				
				if(isObject(%recipe))
				{
					if(!%client.isHoldingCraftingItem())
						if(isObject(%client.inventory.getItem("Carving Knife")))
						{
							%client.player.mountImage(carvingKnifeImage,0);
						}
					%client.craftingStart(%recipe,%value);
				}
			}
		}
		return %p;
	}
};
activatePackage(menu_crafting_plank);