
// 

//output
registerOutputEvent(gameConnection,lob2_showMenu,"list Menu 0 Input 1	string 100 100",1);
registerOutputEvent(gameConnection,lob2_showMenuByName,"string 100 100",1);
registerOutputEvent(gameConnection,lob2_CL_check,"string 100 100	list > 0 < 1 >= 2 <= 3 $= 4 !$= 5 == 6 != 7	string 100 100");
registerOutputEvent(gameConnection,lob2_CL_checkInventory,"string 100 100	list > 0 < 1 >= 2 <= 3 $= 4 !$= 5 == 6 != 7	int 1 1000 1");
registerOutputEvent(gameConnection,lob2_CL_set,"string 100 100	list = 0	string 100 100",1);
registerOutputEvent(gameConnection,lob2_CL_addItem,"string 100 100	int 1 1000 1",1);
registerOutputEvent(gameConnection,lob2_CL_removeItem,"string 100 100	int 1 1000 1",1);

registerOutputEvent(fxDTSBrick,lob2_BR_check,"string 100 100	list > 0 < 1 >= 2 <= 3 $= 4 !$= 5 == 6 != 7	string 100 100");
registerOutputEvent(fxDTSBrick,lob2_BR_set,"string 100 100	list = 0	string 100 100",1);
registerOutputEvent(fxDTSBrick,_setShapeName,"string 100 100	int 0 1000",1);

//output end


//input
registerInputEvent(fxDtsBrick,lob2_CL_onCheckTrue,"Self fxDTSBrick	Player Player	Client GameConnection	MiniGame MiniGame");
registerInputEvent(fxDtsBrick,lob2_CL_onCheckFalse,"Self fxDTSBrick	Player Player	Client GameConnection	MiniGame MiniGame");

registerInputEvent(fxDtsBrick,lob2_BR_onCheckTrue,"Self fxDTSBrick	Player Player	Client GameConnection	MiniGame MiniGame");
registerInputEvent(fxDtsBrick,lob2_BR_onCheckFalse,"Self fxDTSBrick	Player Player	Client GameConnection	MiniGame MiniGame");

registerInputEvent(fxDtsBrick,onBrickLoadPlant,"Self fxDTSBrick	Player Player	Client GameConnection	MiniGame MiniGame");
//input end

function gameConnection::lob2_CL_addItem(%this,%item,%amount,%cl)
{
	%inventory = %this.inventory;
	if(isObject(%inventory))
	{
		%inventory.addItem(%item,%amount);
	}
}

function gameConnection::lob2_CL_removeItem(%this,%item,%amount,%cl)
{
	%inventory = %this.inventory;
	if(isObject(%inventory))
	{
		%inventory.removeItem(%item,%amount);
	}
}

function gameConnection::lob2_CL_set(%this,%var,%operator,%val)
{
	if(strpos(%var,"(") >= 0 || strpos(%var,")") >= 0)
	{
		talk("invalid chars");
		return false;
	}
	
	eval("%this." @ %var @ " = %val;");
}

function gameConnection::lob2_showMenu(%this,%type,%str,%cl)
{
	if(%type == 0)
	{
		%menu = $menu::ok;
		%menu.setTempBody(%str);
		%menu.showMenu(%this);
		%menu.setDefaultBody();
	}
	else
	if(%type == 1)
	{
		%menu = $menu::input;
		%menu.setTempBody(%str);
		%menu.showInputMenu(%this);
	}
}

function gameConnection::lob2_showMenuByName(%this,%str,%cl)
{
    // %m=$class::menuSystem.getmenu(\"fishmenu\");%m.showmenu(#CLIENT,0);
    %m = $class::menuSystem.getmenu(%str);%m.showmenu(%cl,0);
}

function gameConnection::lob2_CL_checkInventory(%this,%item,%operator,%val)
{
	if(strpos(%var,"(") >= 0 || strpos(%var,")") >= 0)
	{
		talk("invalid chars");
		return false;
	}
	switch (%operator) 
	{
		case 0:
			%operator = ">";
		case 1:
			%operator = "<";
		case 2:
			%operator = ">=";
		case 3:
			%operator = "<=";
		case 4:
			%operator = "$=";
		case 5:
			%operator = "!$=";
		case 6:
			%operator = "==";
		case 7:
			%operator = "!=";
	}
	//talk("var: " @ %var @ " op: " @ %operator @ " val: " @ %val);
	
	%obj = %this.player.client.lastBrickClicked;
	$InputTarget_["Self"]   = %obj;
	$InputTarget_["Player"] = %this.player;
	$InputTarget_["Client"] = %this;
	
	%inventory = %this.inventory;
	
	if(isObject(%inventory))
	{
		%itemInfoObj = %inventory.getItem(%item);
		if(isObject(%itemInfoObj))
		{
			eval("%expression = %itemInfoObj.amount " @ %operator @ " %val;");
			if(%expression)
			{
				%obj.processInputEvent("lob2_CL_onCheckTrue", %this);
			}
			else
			{
				%obj.processInputEvent("lob2_CL_onCheckFalse", %this);	
			}
		}
		else
		{
			%amount = 0;
			eval("%expression = %amount " @ %operator @ " %val;");
			if(%expression)
				%obj.processInputEvent("lob2_CL_onCheckTrue", %this);
			else
				%obj.processInputEvent("lob2_CL_onCheckFalse", %this);
		}
	}
	else
		%obj.processInputEvent("lob2_CL_onCheckFalse", %this);
}


function gameConnection::lob2_CL_check(%this,%var,%operator,%val)
{
	if(strpos(%var,"(") >= 0 || strpos(%var,")") >= 0)
	{
		talk("invalid chars");
		return false;
	}
	switch (%operator) 
	{
		case 0:
			%operator = ">";
		case 1:
			%operator = "<";
		case 2:
			%operator = ">=";
		case 3:
			%operator = "<=";
		case 4:
			%operator = "$=";
		case 5:
			%operator = "!$=";
		case 6:
			%operator = "==";
		case 7:
			%operator = "!=";
	}
	//talk("var: " @ %var @ " op: " @ %operator @ " val: " @ %val);
	eval("%expression = %this." @ %var @ ";");
	eval("%expression = %expression " @ %operator @ " %val;");
	
	%obj = %this.player.client.lastBrickClicked;
	$InputTarget_["Self"]   = %obj;
	$InputTarget_["Player"] = %this.player;
	$InputTarget_["Client"] = %this;
	if(%expression == 1)
		%obj.processInputEvent("lob2_CL_onCheckTrue", %this);
	else
		%obj.processInputEvent("lob2_CL_onCheckFalse", %this);
	return %expression;
}

//fxdtsbrick

function fxdtsBrick::lob2_BR_set(%this,%var,%operator,%val)
{
	if(strpos(%var,"(") >= 0 || strpos(%var,")") >= 0)
	{
		talk("invalid chars");
		return false;
	}
	
	eval("%this." @ %var @ " = %val;");
}

function fxDtsBrick::lob2_BR_check(%this,%var,%operator,%val)
{
	if(strpos(%var,"(") >= 0 || strpos(%var,")") >= 0)
	{
		talk("invalid chars");
		return false;
	}
	switch (%operator) 
	{
		case 0:
			%operator = ">";
		case 1:
			%operator = "<";
		case 2:
			%operator = ">=";
		case 3:
			%operator = "<=";
		case 4:
			%operator = "$=";
		case 5:
			%operator = "!$=";
		case 6:
			%operator = "==";
		case 7:
			%operator = "!=";
	}
	//talk("var: " @ %var @ " op: " @ %operator @ " val: " @ %val);
	eval("%expression = %this." @ %var @ ";");
	eval("%expression = %expression " @ %operator @ " %val;");
	
	%obj = %this;
	$InputTarget_["Self"]   = %obj;
	$InputTarget_["Player"] = %this.lastUserr;
	$InputTarget_["Client"] = %this.lastUserr.client;
	if(%expression == 1)
		%obj.processInputEvent("lob2_BR_onCheckTrue", %this);
	else
		%obj.processInputEvent("lob2_BR_onCheckFalse", %this);
	return %expression;
}

function fxDtsBrick::_setShapeName(%this,%name,%offset,%cl) {
	if(!isObject(%this.shapeName)) {
		%this.shapeName = new StaticShape()
		{
			datablock = BrickShapeName;
			position = vectorAdd(%this.getPosition(),"0 0" SPC %this.getDatablock().brickSizeZ/9 + (0.166 + %offset));
			scale = "0.1 0.1 0.1";
		};
	}
	if(%offset !$= "")
		%this.shapeName.setTransform(vectorAdd(%this.getPosition(),"0 0" SPC %this.getDatablock().brickSizeZ/9 + (0.166 + %offset)));
	%this.shapeName.setShapeName(%name);
	return %this.shapeName;
}

datablock StaticShapeData(BrickShapeName) {
        shapefile = "base/data/shapes/empty.dts";
};



function Player::loot_addItem(%player,%image)
{
	%client = %player.client;
	for(%i = 0; %i < %player.getDatablock().maxTools; %i++)
	{
		%tool = %player.tool[%i];
		if(%tool == 0)
		{
			%player.tool[%i] = %image;
			%player.weaponCount++;
			messageClient(%client,'MsgItemPickup','',%i,%image);
			return true;
		}
	}
}