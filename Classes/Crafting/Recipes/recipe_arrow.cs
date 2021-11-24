if(!isObject($class::crafting))
{
	error("crafting_handle.cs -> Crafting class has not been executed, aborting.");
}
else
{
	%recipe = $class::crafting.newRecipe("Bronze Pine Arrow");
	%recipe.addItem("Pine Arrow Base",10);
	%recipe.addItem("Bronze Arrow Tips",10);
	%recipe.onComplete("Bronze Pine Arrow",10);
	%recipe.setAnimationTime(10000);
	%recipe.giveExp("Crafting","500");
}