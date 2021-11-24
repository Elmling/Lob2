if(!isObject($class::crafting))
{
	error("crafting_handle.cs -> Crafting class has not been executed, aborting.");
}
else
{
	//swords,axes,pickaxes, etc
	$class::crafting.newRecipe("Bow String");
	$class::crafting.getRecipe("Bow String").addItem("Flax",10);
	$class::crafting.getRecipe("Bow String").onComplete("Bow String",1);
	$class::crafting.getRecipe("Bow String").setAnimationTime(6000);
	$class::crafting.getRecipe("Bow String").giveExp("Crafting",60);
}