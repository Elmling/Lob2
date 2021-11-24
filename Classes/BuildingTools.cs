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
		props_save_path = "base/lob2/classes/Saves/Props/";
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

function buildingTools::props_load(%this,%client,%name)
{
	%start_position = mfloor(getWord(%client.player.position,0)) SPC mfloor(getWord(%client.player.position,1)) SPC mfloor(getWord(%client.player.position,2));
	%start_position = vectorAdd(%start_position,"0 0 3");
	%f = new fileObject();
	%f.openForRead(%this.props_save_path @ %name @ ".txt");
	
	%index = -1;
	%last_pos = "";
	while(!%f.iseof())
	{
		%index++;
		%brick = "";
		%line = %f.readLine();
		%b_p = getWords(%line,1,3);
		talk("bp " @ %b_p);
		%d_b = getWord(%line,0);
		%b_c = getWord(%line,4);
		if(%index == 0)
		{
			%b_p_final = %start_position;
		}
		else
		{
			%b_p_final = vectorSub(%start_position,vectorSub(%last_pos,%b_p));
			//%diff = vectorSub(%last_pos,%b_p);
		}

		
		talk(%b_p_final);
		%last_pos = %b_p;
		
		%brick = new fxDtsBrick()
		{
			dataBlock = %d_b;
			position = %b_p_final;
			isPlanted = true;
			client = %client;
			rotation = "0 0 1 180";
			angleId = "1";
			colorid = %b_c;
			bl_id = %client.bl_id;
			scale = "1 1 1";
		};
		
		%brick.plant();
		%client.brickgroup.add(%brick);
		talk("planting " @ %d_b);
	}
	
	%f.close();
	%f.delete();
}

function buildingTools::props_save(%this,%client,%name)
{
	if(!isObject(%client.ndSelection.ghostGroup) || %name $= "")
	{
		talk("Usage: buildingTools::props_save(%this,%client,%name)");
		return false;
	}
	
	
	%brick_group = %client.ndSelection.ghostGroup;
	%count = %brick_group.getCount();
	
	
	talk(%count @ " bricks propified");
	talk(isfile(%this.props_save_path));
	
	%f = new fileObject();
	%f.openForWrite(%this.props_save_path @ %name @ ".txt");
	//%f.openForRead(%this.props_save_path @ %name @ ".txt");
	
	//while(!%f.iseof())
	//{
	//	talk(%f.readLine());
	//}
	for(%i=0; %i<%count; %i++)
	{
		%brick = %brick_group.getObject(%i);
		%f.writeLine(%brick.dataBlock @ " " @ %brick.position @ " " @ %brick.colorID);
	}
	
	
	%f.close();
	%f.delete();
}