
if(isObject($class::worldText)) {
	$class::worldText.delete();
}
	
$class::worldText = new scriptGroup() {
	class = worldText;
};

function worldText::show_at(%this,%pos, %text, %time) {
	if(%text $= "")
		return false;
	
	%obj = new scriptObject() {
		class = worldTextObject;
		text = %text;
		time = %time;
	};
	%obj.shapeNameObject = new StaticShape() {
		dataBlock = emptyShape; //or name of preexisting one, if there is one
		data = %obj;
	};
	%obj.shapeNameObject.setShapeName(%text);
	%obj.shapeNameObject.setShapeNameDistance(60);
	%obj.shapeNameObject.setTransform(%pos);
	if(%time !$= "" && %time > 0) {
		%obj.shapeNameObject.schedule(%time,delete);
		%obj.schedule(%time+100,delete);
	}
	%this.add(%obj);
	return %obj;
}

function worldText::byText(%this,%text) {
	for(%i=0;%i<%this.getcount();%i++) {
		%o = %this.getobject(%i);
		if(stripos(%o.text,%text) > -1) {
			return %o;
		}
	}
	return false;
}

function worldTextObject::onAdd(%this) {
	//talk("we added");
}

datablock StaticShapeData(emptyShape) //assuming there isnt one already
{
    shapeFile = "base/data/shapes/empty.dts";
};

function fxDTSBrick::setText(%this, %name, %dist)
{
    //stuff
	if(!isObject(%this.shapeNameObject)) {
		%this.shapeNameObject = new StaticShape() {
			dataBlock = emptyShape; //or name of preexisting one, if there is one
			brick = %this;
		};
	}
	if(%dist !$= "")
		%this.setTextDistance(%dist);
	%this.shapeNameObject.setTransform(vectorAdd(%this.position,"0 0 2"));
	%this.shapeNameObject.setShapeName(%name);
	//%this.shapeNameObject.dump();	
    //stuff
}

function fxDTSBrick::setTextDistance(%this,%distance) {
	if(isObject(%this.shapeNameObject)) {
		%this.shapeNameObject.setShapeNameDistance(%distance);
	}
	return false;
}



// function aiPlayer::buildWorldText(%this,%text,%pos,%timer)
// {
	// if(%text $= "" || %pos $= "")
		// return "0 MISSING FIELD";
		
	// if(%timer $= "")
		// %timer = 3000;
	
	// %shape = new staticShape(worldTextClass)
	// {
		// datablock = worldText;
		// position = %pos;
	// };
	
	// %so = new scriptObject()
	// {
		// class = worldTextClass;
		// shape = %shape;
		// timer = %timer;
	// };
	
	// %shape.setShapeName(%text);
	// %shape.setShapeNameDistance(100);
	// %shape.setShapeNameColor("1 1 0 1");
	
	// return %so;
// }

// function worldTextClass::animate(%this)
// {
	// if(%this.killAnimation)
		// return true;
		
	// cancel(%this.animateLoop);
	
	// if(!%this.destroy)
	// {
		// %this.destroy = true;
		// schedule(700,0,eval,%this@ ".killAnimation = true;");
		// %this.shape.schedule(%this.timer,delete);
		// %this.schedule(%this.timer + 100,delete);
	// }
	
	// %this.shape.setTransform(vectorAdd(%this.shape.position,"0 0 0.01"));
	
	// %this.animateLoop = %this.schedule(1,animate);
// }