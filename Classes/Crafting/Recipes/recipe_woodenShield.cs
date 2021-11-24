if(!isObject($class::crafting))
{
	error("crafting_handle.cs -> Crafting class has not been executed, aborting.");
}
else
{
	$class::crafting.newRecipe("Pine Shield");
	$class::crafting.getRecipe("Pine Shield").addItem("Pine Wood",10);
	$class::crafting.getRecipe("Pine Shield").onComplete("Pine Shield",1);
	$class::crafting.getRecipe("Pine Shield").setAnimationTime(10000);
	$class::crafting.getRecipe("Pine Shield").giveExp("Crafting","500");
	
	$class::crafting.newRecipe("Oak Shield");
	$class::crafting.getRecipe("Oak Shield").addItem("Oak Wood",10);
	$class::crafting.getRecipe("Oak Shield").onComplete("Oak Shield",1);
	$class::crafting.getRecipe("Oak Shield").setAnimationTime(12000);
	$class::crafting.getRecipe("Oak Shield").giveExp("Crafting","1000");
	
	$class::crafting.newRecipe("Willow Shield");
	$class::crafting.getRecipe("Willow Shield").addItem("Willow Wood",10);
	$class::crafting.getRecipe("Willow Shield").onComplete("Willow Shield",1);
	$class::crafting.getRecipe("Willow Shield").setAnimationTime(14000);
	$class::crafting.getRecipe("Willow Shield").giveExp("Crafting","1500");
	
	$class::crafting.newRecipe("Maple Shield");
	$class::crafting.getRecipe("Maple Shield").addItem("Maple Wood",10);
	$class::crafting.getRecipe("Maple Shield").onComplete("Maple Shield",1);
	$class::crafting.getRecipe("Maple Shield").setAnimationTime(16000);
	$class::crafting.getRecipe("Maple Shield").giveExp("Crafting","2000");
}