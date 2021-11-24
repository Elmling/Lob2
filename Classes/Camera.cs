if(!isobject($class::Camera))
	$class::Camera = new scriptGroup(_camera)
	{
		
	};
	
function _camera::orbitObj(%this,%client,%obj)
{
	%camera = %client.camera;
	if(isObject(%obj))
	{
		%ppos = %obj.getTransform();
		
		%camera.setOrbitMode(%obj,%ppos,2,10,5,0);
		%client.setControlObject(%client.camera);
	}
}

function _camera::orbitPos(%this,%client,%pos)
{
	%camera = %client.camera;
	if(%pos !$= "")
	{
		%camera.setOrbitPointMode(%pos,"8");
		%client.setControlObject(%client.camera);
	}
}

function _camera::setRotation(%this,%client,%rotation,%do_orbit) {
	if(%do_orbit $= "")
		%do_orbit = false;
	if(getWordcount(%rotation) == 7) {
		%pos = getWords(%rotation,0,2);
		%rotation = getWords(%rotation,3,6);
	} else 
		%pos = %client.camera.position;
	if(%do_orbit)
		%this.schedule(5,orbitpos,%client,%pos);
	%client.camera.schedule(10,setTransform,%pos SPC %rotation);
	//talk("set camera rot to " @ %pos @ " - " @ %rotation);
}

function _camera::root(%this,%client)
{
	%client.setControlObject(%client.player);
}

