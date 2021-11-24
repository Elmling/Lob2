if(!isObject($class::crafting))
{
	error("crafting_handle.cs -> Crafting class has not been executed, aborting.");
}
else
{
	//swords,axes,pickaxes, etc
	
	$class::crafting.newRecipe("Wooden Blow Dart");
	$class::crafting.getRecipe("Wooden Blow Dart").addItem("Maple Wood",1);
	$class::crafting.getRecipe("Wooden Blow Dart").onComplete("Wooden Blow Dart", 25);
	$class::crafting.getRecipe("Wooden Blow Dart").setAnimationTime(10000);
	$class::crafting.getRecipe("Wooden Blow Dart").giveExp("Crafting",500);
}