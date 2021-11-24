if(isObject($class::tutorial)) {
	$class::tutorial.destroy();
}


$class::tutorial = new scriptGroup() {
	class = tutorial;
	time_transition = 3800;
};

$class::tutorial.steps = new simset();

$class::tutorial.newstep("717 181 41 0.33 -0.16 0.92 0.99","Mining and Woodcutting", "Mine ores to smelt useful metals, and chop trees to build, craft, and more");
$class::tutorial.newstep("736 163 32 0.2 -0.29 0.90 1.67","Crafting","Crafting: Make useful items and tools.");
$class::tutorial.newstep("738.9 153.43 31.67 0.2 -0.29 0.90 1.67","Smelting and Smithing","Smelt ores into bars or buildable metals. Use the anvil to construct gear!");
$class::tutorial.newstep("735.0 149.266 32.13 -0.089 -0.302 0.948 3.688","Farming","Farming various types of plants to be used in various situations.");
$class::tutorial.newstep("716.703 145.43 31.82 0.00122175 -0.405 0.9139 3.136","Fishing","Advanced fishing mechanics. Good for hunger, and Trophy catches.");


function tutorial::newStep(%this, %cameraTransform, %centerPrint, %bottomPrint) {
	%step = new scriptGroup() {
		transform = %cameraTransform;
		centerPrint = %centerPrint;
		bottomPrint = %bottomPrint;
	};
	%this.steps.add(%step);
}

function tutorial::destroy(%this) {
	%this.steps.clear();
	%this.steps.delete();
	%this.delete();
	%this.step = "";
}

function tutorial::play(%this,%client, %index) {
	cancel(%this.playLoop[%client]);
	if(%this.steps.getCount() > 0) {
		if(%index $= "")
			%index = -1;
		%index++;
		
		if(%index >= %this.steps.getcount()) {
			//talk("breaking");
			$class::camera.root(%client);
			return false;
		}
		
		%step = %this.steps.getObject(%index);
		$class::camera.orbitPos(%client,%step.transform);
		$class::camera.setRotation(%client,getWords(%step.transform,3,6));
		if(%step.centerPrint !$= "")
			$class::chat.c_print(%client,%step.centerPrint,10);
		if(%step.bottomPrint !$= "")
			$class::chat.b_print(%client,%step.bottomPrint,10);
		//talk("Step " @ %index SPC %step.transform);
	}
	
	%this.playLoop[%client] = %this.schedule(%this.time_transition,play,%client,%index);
}