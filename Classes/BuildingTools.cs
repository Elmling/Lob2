//***** class description start *****
//		author: Elm
//
//		description - Some methods that work with Zeblotes
//		Duplicator to simplify building/editing builds
//		Requires Duplicator: https://forum.blockland.us/index.php?topic=288602.0
//
//***** class description end ***** 

//***** BuildingTools class *****

if(!isObject($class::buildingTools))
{
	$class::buildingTools = new scriptGroup(buildingTools)
	{
			
	};
}

function buildingTools::removeByColorID(%this,%id,%selection)
{
	if(%id $= "")
	{
		talk("buildingTools::removeByColorID(%id,%ndSelection)");
		return false;
	}
	%delete = 0;
	%c = %selection.brickcount;
	for(%i=0;%i<%c;%i++)
	{
		%b = $ns[%selection,"b",%i];
		if(%b.colorID == %id)
		{
			%b.delete();
			%delete++;
		}
	}
	
	if(%delete > 0)
	{
		messageClient(%selection.client, 'MsgUploadStart', "");
		talk(%selection.client.name @ " deleted " @ %delete @ " brick(s) by their colorID");
	}
}

function buildingTools::forceTrust(%this,%client1,%client2,%option)
{
	if(%client1 $= "")
	{
		talk("buildingTools::forceTrust(%client1,%client2,%trustLevel[optional]);");
	}
	
	if(%option $= "")
		%option = 2;
	
	if(isObject(%client1) && isObject(%client2))
	{
		%id1 = %client1.bl_id;
		%id2 = %client2.bl_id;
	
		messageAll('MsgUploadStart', "");
		talk("Trust forced between " @ %client1.name @ " | " @ %client2.name );
		setMutualBrickGroupTrust(%id1,%id2,%option);
		return true;
	}
	else
		return false;
}