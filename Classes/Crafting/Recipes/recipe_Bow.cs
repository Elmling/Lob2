if(!isObject($class::crafting))
{
	error("crafting_handle.cs -> Crafting class has not been executed, aborting.");
}
else
{
	//long bow
	$class::crafting.newRecipe("Pine Long Bow");
	$class::crafting.getRecipe("Pine Long Bow").addItem("Pine Long Bow Handle",1);
	$class::crafting.getRecipe("Pine Long Bow").addItem("Bow String",2);
	$class::crafting.getRecipe("Pine Long Bow").onComplete("Pine Long Bow",1);
	$class::crafting.getRecipe("Pine Long Bow").setAnimationTime(7500);
	$class::crafting.getRecipe("Pine Long Bow").giveExp("Crafting",500);
	
	//short bow
	$class::crafting.newRecipe("Pine Short Bow");
	$class::crafting.getRecipe("Pine Short Bow").addItem("Pine Short Bow Handle",1);
	$class::crafting.getRecipe("Pine Short Bow").addItem("Bow String",2);
	$class::crafting.getRecipe("Pine Short Bow").onComplete("Pine Short Bow",1);
	$class::crafting.getRecipe("Pine Short Bow").setAnimationTime(7500);
	$class::crafting.getRecipe("Pine Short Bow").giveExp("Crafting",500);
}