if(isObject($menu::Crafting))
	$menu::Crafting.delete();

$menu::Crafting = $class::menuSystem.newMenuObject("Crafting","What would you like to do to the Wood?");

$menu::Crafting.setname("CraftingMenu");
$menu::Crafting.class = "CraftingMenu";

$menu::Crafting.addMenuItem("Craft Long Bow Handle","$menu::crafting.setTempBody(\"Enter an Amount or type ALL\");$menu::crafting.showInputMenu(#CLIENT);");//"$menu::Crafting.showInputMenu(#CLIENT);");
$menu::Crafting.addMenuItem("Craft Short Bow Handle","$menu::crafting.setTempBody(\"Enter an Amount or type ALL\");$menu::crafting.showInputMenu(#CLIENT);");//"$menu::Crafting.showInputMenu(#CLIENT);");
$menu::Crafting.addMenuItem("Craft Handle (for weapons/tools)","$menu::crafting.setTempBody(\"Enter an Amount or type ALL\");$menu::crafting.showInputMenu(#CLIENT);");//"$class::Crafting.ageSelected(#CLIENT.ndSelection);");
$menu::Crafting.addMenuItem("Craft Arrow Base","$menu::crafting.setTempBody(\"Enter an Amount or type ALL\");$menu::crafting.showInputMenu(#CLIENT);");//"$class::Crafting.useColors(#CLIENT.ndSelection);");

package class_menu_crafting_wood
{
	function menuObject::onInputValueRecieved(%this,%client,%value)
	{
			//talk("name = " @ %this.name);
		if(%this.name $= "crafting")
		{
			%selected = %this.selected[%client];
			if(%selected $= "craft arrow Base")
			{
				%type = "Arrow Base";
			}
			else
			if(%selected $= "craft handle (for weapons/tools)")
			{
				%type = "Handle";
			}
			else
			if(%selected $= "Craft Long Bow handle")
			{
				%type = "Long Bow Handle";
			}
			else
			if(%selected $= "Craft Short Bow Handle")
			{
				%type = "Short Bow Handle";
			}
			
			%recipe = $class::crafting.getRecipe(%client.crafting_wood_type @ " " @ %type);
			
			if(!isObject(%recipe))
			{
				talk("The wood classes are: Pine, Oak, Willow, Maple");
				return parent::onInputValueRecieved(%this,%client,%value);
			}
			//talk("value to make = " @ %value);
			%client.craftingStart(%recipe,%value);
		}
		return parent::onInputValueRecieved(%this,%client,%value);
	}
};
activatePackage(class_menu_crafting_wood);