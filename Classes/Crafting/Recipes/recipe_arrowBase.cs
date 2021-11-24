if(!isObject($class::crafting))
{
	error("crafting_handle.cs -> Crafting class has not been executed, aborting.");
}
else
{
	%recipe = $class::crafting.newRecipe("Pine Arrow Base");
	%recipe.addItem("Pine Wood",10);
	%recipe.onComplete("Pine Arrow Base",10);
	%recipe.setAnimationTime(10000);
	%recipe.giveExp("Crafting","500");
}