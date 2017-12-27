//***** class description start *****
//		author: Elm
//
//		description - Sounds Class
//		This class handles all sounds, music, ambience, etc.
//
//		classes: Sounds
//
//***** class description end ***** 

//***** Sounds Class ***** 

if(!isObject($class::Sounds))
	$class::Sounds = new scriptGroup(Sounds);

//name: play2D
//description: plays a sound 2D.
function sounds::play2D(%this,%clientOrPlayer,%sound)
{
	if(!isObject(%sound))
		%sound = %this.getSound(%sound);
	if(!isObject(%sound) || !isObject(%clientOrPlayer))
		return false;
	
	//playsound
	%soundDB = %sound.object;
	talk(%soundDB);
	if(%clientOrPlayer.getclassName() $= "player")
		%clientOrPlayer.client.play2d(%soundDB);
	else
		%clientOrPlayer.play2d(%soundDB);
}

//name: play3D
//description: plays a sound 3D. 
function sounds::play3D(%this,%posOrObj,%sound)
{
	if(!isObject(%sound))
		%sound = %this.getSound(%sound);
	if(!isObject(%sound))
		return false;
	
	%soundDB = %sound.object;
}

//name: register
//description: registers a sound into the sounds class
function sounds::register(%this,%name,%obj)
{
	%sound = %this.getSound(%name);
	if(isObject(%sound) || %name $= "")
		return false;
	
	if(!isObject(%obj))
		return false;
	
	%sound = new scriptObject()
	{
		object = %obj;
		name = %name;
	};
	
	%this.add(%sound);
}

//name: getSound
//description: Attempts to get an object from the sounds class
//containing data about the sound.
function sounds::getSound(%this,%sound)
{
	%c = %this.getCount();
	for(%i=0;%i<%c;%i++)
	{
		%s = %this.getObject(%i);
		if(%s.name $= %sound)
			return %s;
	}
	return false;
}

//name: display
//description: writes to the console the current sounds registered.
function sounds::display(%this,%sound)
{
	%c = %this.getCount();
	echo("\n======= SOUNDS REGISTERED START =======\n");
	for(%i=0;%i<%c;%i++)
	{
		%s = %this.getObject(%i);
		echo(%s.name SPC "->" SPC %s);
	}
	echo("\n======= SOUNDS REGISTERED END =======\n");
}