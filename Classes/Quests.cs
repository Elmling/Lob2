if(!isObject($class::Quests))
	$class::quests = new scriptGroup(Quests);

//Quests class

function Quests::newQuest(%this,%name)
{
	if(%name $= "")
	{
		talk("Quests::newQuest requires a %name parameter");
		return false;
	}
	
	%quest = new scriptGroup()
	{
		class = quest;
		Quests = Quests;
		name = %name;
		path = "config/client/Quests/";
	};
	
	%loadedClients = new simSet()
	{
			quests = %this;
			quest = %quest;
	};
	
	%quest.loadedClients = %loadedClients;
	return %quest;
}

function Quests::getQuest(%this,%name)
{
	%c = %this.getCount();
	for(%i=0;%i<%c;%i++)
	{
		%quest = %this.getObject(%i);
		if(%quest.name $= %name)
			return %quest;
	}
	return false;
}

function Quests::newQuestBook(%this,%client)
{
	if(isObject(%client.questBook))
		%client.questBook.delete();
	
	%qb = new scriptGroup()
	{
		class = questBook;
		client = %client;
		Quests = Quests;
		bl_id = %client.bl_id;
	};
	
	%client.questBook = %qb;
	
	return %qb;
}

//Quests class End

//Quest class

function quest::addStep(%this,%clientAttribute,%value,%menuOnTrue,%menuOnFalse)
{
	if(%clientAttribute $= "" || %value $= "")
	{
		talk("quest::addStep requires %clientAttribute, %value,%menuObjectOnTrue (optional),%menuObjectOnFalse (optional)");
		return false;
	}
	
	if(!isObject(%menuOnTrue))
		%menuOnTrue = "0";
	if(!isObject(%menuOnFalse))
		%menuOnFalse = "0";
	
	%step = new scriptObject()
	{
		class = questStep;
		quest = %this;
		menuOnTrue = %menuOnTrue;
		menuOnFalse = %menuOnFalse;
		clientAttribute = %clientAttribute;
		value = %value;
	};
	
	%this.add(%step);
	
	return %step;
}

function quest::checkStep(%this,%client)
{
	%qb = %client.questbook;
	if(%qb.complete[%this.name])
		return false;
	%step = mfloor(%qb.currentStep[%this.name]);
	%attribute = %this.getObject(%step).clientAttribute;
	%value = %this.getObject(%step).value;
	eval("%final = " @ %client @ "." @ %attribute @ %value);
	
	if(%attribute $= "" && %value $= "")
	{
		if(%step > 0)
		{
			%this.onFinish(%client);
			return true;
		}
		return false;
	}
	
	if(%final)
	{
		%this.onStep(%client);
		return true;
	}
	else
	{
		return false;
	}
}

function quest::getStep(%this,%client)
{
	%step = false;
	
	if(!%this.isLoaded(%client))
	{
		%this.loadClient(%client);
	}
	
	%qb = %client.questBook;
	
	if(isObject(%qb))
	{
		%step = mfloor(%qb.currentStep[%this.name]);
	}
	
	return %step;
}

function quest::onStep(%this,%client)
{
	%client.questBook.currentStep[%this.name]++;
}

function quest::onStart(%this,%client)
{
	%client.questBook.currentStep[%this.name] = 0;
}

function quest::onFinish(%this,%client)
{
	%client.questBook.complete[%this.name] = true;
	%client.questBook.currentStep[%this.name] = -1;
}

function quest::isLoaded(%this,%client)
{
	if(isObject(%this.loadedClients))
	{
		%lc = %this.loadedClients;
		%c = %lc.getcount();
		for(%i=0;%i<%c;%i++)
		{
			%cl = %lc.getObject(%i);
			if(%cl == %client)
				return true;
		}
		return false;
	}
	return false;
}

function quest::loadClient(%this,%client)
{
	if(%this.isLoaded(%client))
	{
		return false;
	}
	
	%path = %this.quests.path;
	%f = %path @ %client.bl_ID @ "_" @ %client.name @ ".cs";
	if(discoverFile(%f))
	{
		%this.loadedClients.add(%client);
		exec(%this.quests.path @ %client.bl_ID @ "_" @ %client.name @ ".cs");
	}
	else
		return false;
}

function quest::saveClient(%this,%client)
{
	%path = %this.quests.path;
	%f = %path @ %client.bl_ID @ "_" @ %client.name @ ".cs";
	%client.questBook.save(%f);
}

function quest::cleanup(%this,%client)
{
	%this.loadedClients.remove(%client);
}

function quest::onRemove(%this)
{
	%this.loadedClients.delete();
}

//Quest class end

//QuestBook class

function questBook::onAdd(%this)
{
	%blid = %this.bl_id;
	%c = clientgroup.getcount();
	for(%i=0;%i<%c;%i++)
	{
		%cl = clientgroup.getobject(%i);
		if(%cl.bl_id $= %this.bl_id)
		{
			if(%cl.questBook != %this)
			{
				%cl.questbook = %this;
			}
			return %this;
		}
	}
	return false;
}

//QuestBook class end

//QuestStep class

//QuestStep class end
