//***** class description start *****
//		author: Elm
//
//		description - Avatars Class
//		requires: <b>https://github.com/Elmling/Avatar_API<b>
//		Allows for users to define simple avatars
//		to be used with the Avatar API to present
//		and easy to use avatar system - instead
//		of trying to work with the default system.
//
//***** class description end ***** 

//This class requires the Avatar API to be installed to work correctly

//***** Avatars Class ******

if(!isObject($class::avatars))
	$class::avatars = new scriptGroup(avatars)
	{
		//change path upon release
		path = "base/avatar api/avatars/";
	};
	
	
//name: update
//description: Updates all avatars to be current in the system
function avatars::update(%this)
{
	%count = getFileCount(%this.path @ "*.cs");
	%this.clear();
	for(%i=0;%i<%count;%i++)
	{
		if(%i == 0)
			%f = findFirstFile(%this.path @ "*.cs");
		else
			%f = findNextFile(%this.path @ "*.cs");
		
		%n = filename(%f);
		
		if(isFile(%f))
		{
			%avatar = new scriptObject()
			{
				path = %f;
				name = %n;
				nameRaw = strReplace(%n,".cs","");
				avatars = %this;
			};
			
			%this.add(%avatar);
		}
	}
}
	
//name: displayBegin
//description: using the avatar class
//we start to display the avatars to a client
//allowing them to cycle through available
//avatars, allowing them to choose
//Uses <b>Menu.cs</b>
function avatars::displayBegin(%this,%client)
{
	%client.displayAvatarMode = true;
	%this.displayIndex[%client] = -1;
	
	//set client into orbit mode
	%point = %client.player.getEyePoint();
	%client.camera.setOrbitPointMode(%point,5);
	%client.setControlObject(%client.camera);
}

//name: displayEnd
//description: stops displaying the avatar menu
//and the cycling
function avatars::displayEnd(%this,%client)
{
	%client.displayAvatarMode = true;
	%client.setControlObject(%client.player);
	//remove client from orbit mode
}

//name: displayNext
//description: displays the next available avatar to
//the client
function avatars::displayNext(%this,%client)
{
	if(%this.displayIndex[%client] >= %this.getCount()-1)
	{
		%this.displayIndex[%client] = 0;
	}
	else
		%this.displayIndex[%client]++;
	
	%index = %this.displayIndex[%client];
	//avatar api method
	%client.setCustomAvatar(%this.getAvatarAPIName(%this.getObject(%index).nameraw));
}

//name: displayPrevious
//description: displays the previous available avatar to the client
function avatars::displayPrevious(%this,%client)
{
	if(%this.displayIndex[%client] <= 0)
	{
		%this.displayIndex[%client] = %this.getCount()-1;
	}
	else
		%this.displayIndex[%client]--;
	
	%index = %this.displayIndex[%client];
	//avatar api method
	%client.setCustomAvatar(%this.getAvatarAPIName(%this.getObject(%index).nameraw));
}

//name: setAvatar
//description:avatar api defines a client attribute so we dont need to do anything.
//Ex: %client.customAvatar => current avatar 
function avatars::setAvatar(%this,%client,%avatar)
{
	//empty
}

//name: getAvatarAPIName
//description: support for AvatarAPI
function avatars::getAvatarAPIName(%this,%name)
{
	%name = strReplace(%name,"Avatar_","");
	%name = strReplace(%name,"avatar_","");
	return %name;
}

//put this in the avatar api add-on
//smmdeadbodies support
package class_Avatars
{
	//name: onDeath
	//description: <b>Packaged</b> - support for smmdeadbodies when using AvatarAPI
	function GameConnection::onDeath(%this, %sourcePlayer, %sourceClient, %damageType, %damageArea)
	{
		%p = parent::onDeath(%this, %sourcePlayer, %sourceClient, %damageType, %damageArea);
		if(%this.isAvatarLocked())
			%this.corpse.setCustomAvatar(%this.customAvatar);
		return %p;
	}
};
activatePackage(class_Avatars);