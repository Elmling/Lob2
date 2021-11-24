//***** class description start *****
//		author: Elm
//
//		description - Experience/EXP class
//		
//
//***** class description end ***** 

//***** Exp class *****

if(!isObject($class::Exp))
{
	$class::Exp = new scriptGroup(Exp)
	{
		multiplier = 50;
		divider = 2;
	};
}

function Exp::getSkill(%this,%skill)
{
	%c = %this.getCount();
	for(%i=0;%i<%c;%i++)
	{
		%sk = %this.getObject(%i);
		if(%sk.name $= %skill)
		{
			return %sk;
		}
	}
	return false;
}

function Exp::nextLevelExp(%this,%client,%skill) {
	%level = %client.profile.skill[%skill];
	%expMax = mfloor( (msqrt(%level) * %this.multiplier) * (%level/%this.divider) );
	return %expMax;
}

function Exp::giveExp(%this,%client,%skill,%value)
{
	//expMax = msqrt(lvl)*(lvl/4)
	%profile = %client.profile;
	if(!isObject(%profile))
	{
		talk(%client.name @ " does not have a profile!");
		return false;
	}
	else
	{
		%level = %profile.skill[%skill];
		if(%level $= "")
		{
			//talk(%skill @ " has not been set on " @ %client.name @ "\'s profile! Setting to 1.");
			%profile.skill[%skill] = 1;
			%level = 1;
		}
		else
		if(%value $= "")
		{
			talk("Value needs to be supplied -> Exp::giveExp(%client,%skill,%value)!");
			return false;
		}
		
		%expMax = mfloor( (msqrt(%level) * %this.multiplier) * (%level/%this.divider) );
		%profile.exp[%skill] += mfloor(%value);
		if(%profile.exp[%skill] >= %expMax)
		{
			%profile.skill[%skill]++;
			%leftover = %profile.exp[%skill] - %expMax;
			%profile.exp[%skill] = 0;
			
			if(%leftover > 0)
			{
				if(%leftover > %expMax)
				{
					%div = mfloor(%leftover / %expMax);
					%leftover = %leftOver - %expMax;
					%profile.skill[%skill] += %div;
				}
				%profile.exp[%skill] = mfloor(%leftover); 
			}
			
			//talk("[SYSTEM] " @ %client.name @ " leveled his " @ %skill @ " to " @ %profile.skill[%skill]);
			$class::chat.to_all("\c5" @ %client.name @ " \c6leveled his \c4" @ %skill @ "\c6 to \c2" @ %profile.skill[%skill]);
			%client.player.playsound(sound_levelup);
		}
		//talk("EXP Cap at level " @ %level @ " = " @ %expMax);
	}
	return true;
}

function Exp::addSkill(%this,%skillName)
{
	%skill = %this.getSkill(%skillName);
	if(!isObject(%skill))
	{
		%skill = new scriptGroup()
		{
			//class = Skill;
			name = %skillName;
			cap = 99;
			levelUpSound = "N/A";
		};
		%this.add(%skill);
		return %skill;
	}
	else
		return false;
}

function Exp::getExpString(%this,%client,%skillName,%currExpStyler,%splitterStyler,%maxExpStyler)
{
	if(!isObject(%client))
		return false;
	if(isObject(%skill = %this.getSkill(%skillName)))
	{
		
	}
	
	%profile = %client.profile;
	
	if(!isObject(%profile))
	{
		talk(%client.name @ " does not have a profile!");
		return false;
	}
	
	%level = %profile.skill[%skillName];
	
	if(%level $= "")
	{
		talk(%skillName @ " has not been set on " @ %client.name @ "\'s profile!");
		return false;
	}
	
	%expMax = mfloor( (msqrt(%level) * %this.multiplier) * (%level/%this.divider) );
	
	return %currExpStyler @ %profile.exp[%skillName] @ %splitterStyler @ "/" @ %maxExpStyler @ %expMax;
	
}

function Exp::getPercentToNextLevel(%this,%client,%skillName)
{
	if(!isObject(%client))
		return false;
	if(isObject(%skill = %this.getSkill(%skillName)))
	{
		
	}
}