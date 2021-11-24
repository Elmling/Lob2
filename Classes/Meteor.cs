//***** class description start *****
//		author: Elm
//
//		description - Meteor Class
//		
//
//***** class description end ***** 

//***** Meteor class *****

while(isObject($class::meteor))
{

	talk("deleting old meteor class before creating new one.");
	$class::meteor.delete();
}


$class::meteor = new scriptGroup()
{
	class = meteor;
	projectile = "meteorProjectile";
};

$class::meteor.meteorShowers = new scriptGroup();

function meteor::startShower(%this,%xy1,%xy2)
{
	if(%xy1 $= "" || %xy2 $= "")
	{
		if(isObject(_meteorxy1) && isobject(_meteorxy2))
		{
			%xy1 = _meteorxy1.position;
			%xy2 = _meteorxy2.position;
		}
		else
		{
			talk("Correct usage: meteor.startShower(pos1,pos2);");
			return false;	
		}
	}
	
	%time = 60000;
	
	%meteorShower = new scriptObject()
	{
		class = meteorShower;
		time = %time;
		projectile = %this.projectile;
		xy1 = getWords(%xy1,0,1);
		xy2 = getWords(%xy2,0,1);
	};
	
	%this.meteorShowers.add(%meteorShower);
	%meteorShower.begin();
}

//--		meteorShower			--

function meteorShower::begin(%this)
{
	talk("Starting Meteor Shower for " @ (%this.time/60000) @ " minute(s)");
	%xy1 = %this.xy1;
	%xy2 = %this.xy2;
	
	//%this.emitter = new particleEmitterNode()
	//{
	//	dataBlock = "genericEmitterNode";
	//	emitter = "playerburnemitter";
		//position =  ( ( getWord(%xy2,0) + getWord(%xy1,0) ) / 2 ) SPC ( ( getWord(%xy2,1) + getword(%xy1,1) / 2 ) ) SPC 20;
	//	position = getBoxCenter(%this.xy1 SPC "10" SPC %this.xy2 SPC "10");
	//	scale = "50 50 50";
	//};
	//findclientbyname("elm").player.position = %this.emitter.position;
	//talk("emitter pos = " @ %this.emitter.position);
	
	%this.tick();
	%this.schedule(%this.time,end);
}

function meteorShower::tick(%this)
{
	cancel(%this.tick);
	
	%x1 = getWord(%this.xy1,0);
	%y1 = getWord(%this.xy1,1);
	
	%x2 = getWord(%this.xy2,0);
	%y2 = getWord(%this.xy2,1);
	
	%ranPos = getRandom(%x1,%x2) SPC getRandom(%y1,%y2) SPC "200";
	
	%scale = getRandom(3,7) SPC getRandom(3,7) SPC getRandom(3,7);
	
	%projectile = new projectile()
	{
		dataBlock = %this.projectile;
		scale = %scale;
		initialPosition = %ranPos;
		initialVelocity = "0 0 "  @ getRandom(-75,-55);
	};
	
	%this.tick = %this.schedule(1500,tick);
}

function meteorShower::end(%this)
{
	talk("Meteor Shower is now over.");
	//%this.emitter.delete();
	%this.schedule(5,delete);
}


package class_Meteor
{
	function class_Meteor(){}
};


