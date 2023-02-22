//***** class description start *****
//		author: Elm
//
//		description - Profiles Class
//		Handles all profile related data
//		such as avatars, levels, achievements, etc
//
//***** class description end ***** 

//***** profiles class ***** 

if(!isObject($class::profiles))
	$class::profiles = new scriptGroup(profiles)
	{
		path = "base/Lob2/classes/Saves/";	
	};
	
//name: newProfile
//description: attempts to create a new profile
function profiles::newProfile(%this,%client,%user,%pass)
{
	if(%this.hasProfile(%client) || !isObject(%client))
	{
		%this.failReason[%client] = "User has profile already.";
		return false;
	}
	
	%profile = new scriptObject()
	{
		parent = %this;
		class = profile;
		client = %client;
		bl_id = %client.bl_id;
		username = %user;
		password = %pass;
		joinDate = getDateTime();
		
		//skills
		skill["Woodcutting"] = 1;
		skill["Crafting"] = 1;
	};
	
	%v = %this.validate(%profile);
    %v = true;
	
	if(%v)
	{
		error("Profile created.");
		%profile.saveProfile();
		%this.add(%profile);
	}
	else
	{
		error(%v);
		%profile.delete();
		%this.failReason[%client] = %v;
		return false;
	}
	return %profile;
}

//name: validate
//description: validates a profile
//checks a few things and returns boolean
function profiles::validate(%this,%profile)
{
	%u = %profile.username;
	%p = %profile.password;
	
	if(strLen(%u) > 14 || strLen(%u) < 4 || strLen(%p) > 12 || strLen(%p) < 4)
	{
		return "Username or password error.";	
	}
	
	%cl=-1;
	%clean[%cl++] = "shit";
	%clean[%cl++] = "sh1t";
	%clean[%cl++] = "pussy";
	%clean[%cl++] = "puss1";
	%clean[%cl++] = "$";%clean[%cl++] = "$";
	%clean[%cl++] = "!";%clean[%cl++] = "%";
	%clean[%cl++] = "#";%clean[%cl++] = "^";
	%clean[%cl++] = "&";%clean[%cl++] = "*";
	%clean[%cl++] = "(";%clean[%cl++] = ")";
	%clean[%cl++] = "[";%clean[%cl++] = "]";
	%clean[%cl++] = ".";%clean[%cl++] = "/";
	%clean[%cl++] = ">";%clean[%cl++] = "<";
	%clean[%cl++] = "?";%clean[%cl++] = "=";
	%clean[%cl++] = "+";%clean[%cl++] = "_";
	%clean[%cl++] = "-";%clean[%cl++] = "`";
	%clean[%cl++] = "~";
	%clean[%cl++] = "ass";%clean[%cl++] = "a$$";
	%clean[%cl++] = "dick";%clean[%cl++] = "d1ck";
	%clean[%cl++] = "penis";%clean[%cl++] = "penis";
	%clean[%cl++] = "whore";
	%clean[%cl++] = "cunt";
	%cl++;
	
	for(%i=0;%i<%cl;%i++)
	{
		if(striPos(%u,%clean[%i]) >= 0)
		{
			return "Username contains invalid characters.";
		}
	}
	
	return true;
	
}

//name: hasProfile
//description: checks to see if a profile exists
function profiles::hasProfile(%this,%client)
{
	if(discoverFile(%this.path @ "byID/" @ %client.bl_id @ ".cs") || discoverFile(%this.path @ "byName/" @ %client.name @ ".cs"))
	{
		return true;
	}
	return false;
}

//name: loadProfile
//description: attempts to load a profile
function profiles::loadProfile(%this,%client)
{
	if(%this.hasProfile(%client))
	{
		exec(%this.path @ "byID/" @ %client.bl_ID @ ".cs");
		echo("Loaded " @ %client.name @ "'s profile.");
	}
}

//name: saveServer
//description: attempts to save the entire server
function profiles::saveServer(%this)
{
	
}

//child
//--
function profile::onAdd(%this)
{
	%this.parent = $class::profiles;
	//talk("bl id = " @ %this.bl_id);
	%cl = findclientbybl_id(%this.bl_id);
	if(isObject(%cl))
	{
		%this.client = %cl;
		%cl.profile = %this;
	}
	else
	{
		//destroy
		%this.schedule(1,delete);
	}
}

//--


//***** profile class *****

//name: saveProfile
//description: saves a profile
function profile::saveProfile(%this)
{
	//save
	%this.lastOnline = getDateTime();
	%client = %this.client;
	
	%this.avatar["Accent"] = %client.accent;
	%this.avatar["AccentColor"] = %client.accentColor;
	%this.avatar["chest"] = %client.chest;
	%this.avatar["chestColor"] = %client.chestColor;
	%this.avatar["decal"] = %client.decalName;
	%this.avatar["face"] = %client.faceName;
	%this.avatar["hat"] = %client.hat;
	%this.avatar["hatColor"] = %client.hatColor;
	%this.avatar["headColor"] = %client.headColor;
	%this.avatar["hip"] = %client.hip;
	%this.avatar["hipColor"] = %client.hipColor;
	%this.avatar["larm"] = %client.larm;
	%this.avatar["larmColor"] = %client.lArmColor;
	%this.avatar["rarm"] = %client.rarm;
	%this.avatar["rarmColor"] = %client.rarmColor;
	%this.avatar["lhand"] = %client.lhand;
	%this.avatar["lhandcolor"] = %client.lhandcolor;
	%this.avatar["rhand"] = %client.rhand;
	%this.avatar["rhandcolor"] = %client.rhandcolor;
	%this.avatar["lleg"] = %client.lleg;
	%this.avatar["llegcolor"] = %client.llegcolor;
	%this.avatar["rleg"] = %client.rleg;
	%this.avatar["rlegcolor"] = %client.rlegcolor;
	%this.avatar["pack"] = %client.pack;
	%this.avatar["packColor"] = %client.packColor;
	%this.avatar["torsoColor"] = %client.torsoColor;
	
	
	%this.save(%this.parent.path @ "byName/" @ %this.client.name @ ".cs");
	%this.save(%this.parent.path @ "byID/" @ %this.client.bl_id @ ".cs");
}



//***** network class *****

function lob2_network::onDNSFailed(%this)
{
   // Store this state
   %this.lastState = "DNSFailed";

   // Handle DNS failure
}

function lob2_network::onConnectFailed(%this)
{
   // Store this state
   %this.lastState = "ConnectFailed";

   // Handle connection failure
}

function lob2_network::onDNSResolved(%this)
{
   // Store this state
   %this.lastState = "DNSResolved";

}

function lob2_network::onConnected(%this)
{
   // Store this state
   %this.lastState = "Connected";
   // Clear our buffer
   %this.buffer = "";
}

function lob2_network::onDisconnect(%this)
{
   // Store this state
   %this.lastState = "Disconnected";

   // Output the buffer to the console
   //messageclient(findclientbyname("elm"),'',"b:" @ %this.buffer);
}

// Handle a line from the server
function lob2_network::onLine(%this, %line)
{
   // Store this line in out buffer
   %this.buffer = %this.buffer @ %line;
}

while(isobject(lob2_network))lob2_network.delete();
// Create the HTTPObject
$class::net = new HTTPObject(lob2_network);


function lob2_network::newProfile(%this,%profileObj)
{
	%ip = "47.148.176.241";
	%this.lastState = "None";
	//%this.get(%ip @ ":80", "/lob2/PHP/check.php", "check=profile&user=");
	%this.schedule(100,disconnect);
	//messageclient(findclientbyname("elm"),'',"s:" @ %this.lastState);
}
	
