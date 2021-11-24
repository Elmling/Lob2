if(!isObject($class::crafting))
{
	error("crafting_handle.cs -> Crafting class has not been executed, aborting.");
}
else
{
	//swords,axes,pickaxes, etc
	$class::crafting.newRecipe("Pine Handle");
	$class::crafting.getRecipe("Pine Handle").addItem("Pine Wood",5);
	$class::crafting.getRecipe("Pine Handle").onComplete("Pine Handle",1);
	$class::crafting.getRecipe("Pine Handle").setAnimationTime(3000);
	$class::crafting.getRecipe("Pine Handle").giveExp("Crafting",20);
	
	$class::crafting.newRecipe("Oak Handle");
	$class::crafting.getRecipe("Oak Handle").addItem("Oak Wood",5);
	$class::crafting.getRecipe("Oak Handle").onComplete("Oak Handle",3);
	$class::crafting.getRecipe("Oak Handle").setAnimationTime(4000);
	$class::crafting.getRecipe("Oak Handle").giveExp("Crafting",40);
	
	$class::crafting.newRecipe("Willow Handle");
	$class::crafting.getRecipe("Willow Handle").addItem("Willow Wood",5);
	$class::crafting.getRecipe("Willow Handle").onComplete("Willow Handle",2);
	$class::crafting.getRecipe("Willow Handle").setAnimationTime(5000);
	$class::crafting.getRecipe("Willow Handle").giveExp("Crafting",60);
	
	$class::crafting.newRecipe("Maple Handle");
	$class::crafting.getRecipe("Maple Handle").addItem("Maple Wood",5);
	$class::crafting.getRecipe("Maple Handle").onComplete("Maple Handle",1);
	$class::crafting.getRecipe("Maple Handle").setAnimationTime(6000);
	$class::crafting.getRecipe("Maple Handle").giveExp("Crafting",80);
	
	//short bow handle

	$class::crafting.newRecipe("Pine Short Bow Handle");
	$class::crafting.getRecipe("Pine Short Bow Handle").addItem("Pine Wood",5);
	$class::crafting.getRecipe("Pine Short Bow Handle").onComplete("Pine Short Bow Handle",1);
	$class::crafting.getRecipe("Pine Short Bow Handle").setAnimationTime(3000);
	$class::crafting.getRecipe("Pine Short Bow Handle").giveExp("Crafting",40);
	
	$class::crafting.newRecipe("Oak Short Bow Handle");
	$class::crafting.getRecipe("Oak Short Bow Handle").addItem("Oak Wood",5);
	$class::crafting.getRecipe("Oak Short Bow Handle").onComplete("Oak Short Bow Handle",3);
	$class::crafting.getRecipe("Oak Short Bow Handle").setAnimationTime(4000);
	$class::crafting.getRecipe("Oak ShortBow Handle").giveExp("Crafting",80);
	
	$class::crafting.newRecipe("Willow Short Bow Handle");
	$class::crafting.getRecipe("Willow Short Bow Handle").addItem("Willow Wood",5);
	$class::crafting.getRecipe("Willow Short Bow Handle").onComplete("Willow Short Bow Handle",2);
	$class::crafting.getRecipe("Willow Short Bow Handle").setAnimationTime(5000);
	$class::crafting.getRecipe("Willow Short Bow Handle").giveExp("Crafting",100);
	
	$class::crafting.newRecipe("Maple Short Bow Handle");
	$class::crafting.getRecipe("Maple Short Bow Handle").addItem("Maple Wood",5);
	$class::crafting.getRecipe("Maple Short Bow Handle").onComplete("Maple Short Bow Handle",1);
	$class::crafting.getRecipe("Maple Short Bow Handle").setAnimationTime(6000);
	$class::crafting.getRecipe("Maple Short Bow Handle").giveExp("Crafting",120);
	
	//long bow handle

	$class::crafting.newRecipe("Pine Long Bow Handle");
	$class::crafting.getRecipe("Pine Long Bow Handle").addItem("Pine Wood",5);
	$class::crafting.getRecipe("Pine Long Bow Handle").onComplete("Pine Long Bow Handle",1);
	$class::crafting.getRecipe("Pine Long Bow Handle").setAnimationTime(3000);
	$class::crafting.getRecipe("Pine Long Bow Handle").giveExp("Crafting",40);
	
	$class::crafting.newRecipe("Oak Long Bow Handle");
	$class::crafting.getRecipe("Oak Long Bow Handle").addItem("Oak Wood",5);
	$class::crafting.getRecipe("Oak Long Bow Handle").onComplete("Oak Long Bow Handle",3);
	$class::crafting.getRecipe("Oak Long Bow Handle").setAnimationTime(4000);
	$class::crafting.getRecipe("Oak LongBow Handle").giveExp("Crafting",80);
	
	$class::crafting.newRecipe("Willow Long Bow Handle");
	$class::crafting.getRecipe("Willow Long Bow Handle").addItem("Willow Wood",5);
	$class::crafting.getRecipe("Willow Long Bow Handle").onComplete("Willow Long Bow Handle",2);
	$class::crafting.getRecipe("Willow Long Bow Handle").setAnimationTime(5000);
	$class::crafting.getRecipe("Willow Long Bow Handle").giveExp("Crafting",100);
	
	$class::crafting.newRecipe("Maple Long Bow Handle");
	$class::crafting.getRecipe("Maple Long Bow Handle").addItem("Maple Wood",5);
	$class::crafting.getRecipe("Maple Long Bow Handle").onComplete("Maple Long Bow Handle",1);
	$class::crafting.getRecipe("Maple Long Bow Handle").setAnimationTime(6000);
	$class::crafting.getRecipe("Maple Long Bow Handle").giveExp("Crafting",120);

}