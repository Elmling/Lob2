if(!isObject($class::crafting))
{
	error("crafting_handle.cs -> Crafting class has not been executed, aborting.");
}
else
{
	$class::crafting.newRecipe("Pine Plank");
	$class::crafting.getRecipe("Pine Plank").addItem("Pine Wood",4);
	$class::crafting.getRecipe("Pine Plank").onComplete("Pine Plank",1);
	$class::crafting.getRecipe("Pine Plank").setAnimationTime(10000);
	$class::crafting.getRecipe("Pine Plank").giveExp("Crafting",20);
	
	$class::crafting.newRecipe("Oak Plank");
	$class::crafting.getRecipe("Oak Plank").addItem("Oak Wood",4);
	$class::crafting.getRecipe("Oak Plank").onComplete("Oak Plank",1);
	$class::crafting.getRecipe("Oak Plank").setAnimationTime(12000);
	$class::crafting.getRecipe("Oak Plank").giveExp("Crafting",40);
	
	$class::crafting.newRecipe("Maple Plank");
	$class::crafting.getRecipe("Maple Plank").addItem("Maple Wood",4);
	$class::crafting.getRecipe("Maple Plank").onComplete("Maple Plank",1);
	$class::crafting.getRecipe("Maple Plank").setAnimationTime(14000);
	$class::crafting.getRecipe("Maple Plank").giveExp("Crafting",60);
	
	$class::crafting.newRecipe("Willow Plank");
	$class::crafting.getRecipe("Willow Plank").addItem("Willow Wood",4);
	$class::crafting.getRecipe("Willow Plank").onComplete("Willow Plank",1);
	$class::crafting.getRecipe("Willow Plank").setAnimationTime(16000);
	$class::crafting.getRecipe("Willow Plank").giveExp("Crafting",80);
}