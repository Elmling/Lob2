//***** class description start *****
//		author: Elm
//
//		description - Menu Class
//		This class is used to present a user with an interface
//		they can interact with, without installing client sided files.
//		This class utilizes the centerprint to achieve this.
//		All menus should be created in /Menus/menu_menuName.cs
//
//		classes: menuSystem & menuObject
//
//***** class description end ***** 

//***** menuSystem Class ***** 

if(!isObject($class::menuSystem))
	$class::menuSystem = new scriptGroup(menuSystem);

//name: newMenuObject
//description: creates a new menu.
function menuSystem::newMenuObject(%this,%name,%bodyTxt)
{
	if(%name $= "")
	{
		talk("menuSystem::newMenuObject requires %name paramater");
		return false;
	}
	
	if(%name $= "")
	{
		talk("menuSystem::newMenuObject requires %bodytxt paramater");
		return false;
	}
	
	%menu = new scriptGroup()
	{
		class = menuObject;
		menu = %this;
		name = %name;
		bodyText = %bodyTxt;
	};
	
	%this.add(%menu);
	
	return %menu;
}

//name: getMenu
//description: attempts to get an existing menu from
//the menuSystem group
function menuSystem::getMenu(%this,%name)
{
	%c = menusystem.getcount();
	for(%i=0;%i<%c;%i++)
	{
		%menuObject = menusystem.getobject(%i);
		if(%menuObject.name $= %name)
		{
			return %menuObject;
		}
	}
	return false;
}

//name: getSelected
//description: gets the current selection in the menu
function menuObject::getSelected(%this,%client)
{
	%name = %this.name;
	if(%this.selected[%client] $= "") return false;
	return %this.getMenuItemByText(%this.selected[%client]);
}

//name: getMenuItemByText
//description: returns the actual menu item object by it's text value
function menuObject::getMenuItemByText(%this,%text)
{
	%c = %this.getCount();
	for(%i=0;%i<=%c;%i++)
	{
		%o = %this.getObject(%i);
		if(stripmlcontrolchars(%o.text) $= %text)
			return %o;
	}
	return false;
}

//name: onMenuSelect
//description: called when a user selects a menu item
function menuObject::onMenuSelect(%this,%client)
{
	%selected = %this.getSelected(%client);
	%this.hidemenu(%client);
	if(%selected.evalText !$= "")
		eval(stripmlcontrolchars(strReplace(%selected.evalText,"#CLIENT",%client)));
	
	//callback
	%this.onMenuSelectCallBack(%client,%selected);
}

//name: onMenuSelectCallBack
//description: empty callback function. Purpose is to be packaged
function menuObject::onMenuSelectCallBack(%this,%client,%selected)
{
	//empty, we should package this function to do stuff when this is called
}

//name: scrollDown
//description: scrolls down the interface list
function menuObject::scrollDown(%this,%client)
{
	if(%this.index[%client] $= "")
	{
		%this.index[%client] = -1;
	}
	%this.index[%client]++;
	if(%this.index[%client] >= %this.getCount())
		%this.index[%client] = 0;
	%this.showMenu(%client,%this.index[%client]);
}

//name: scrollUp
//description: scrolls up the interface list
function menuObject::scrollUp(%this,%client)
{
	if(%this.index[%client] $= "")
	{
		%this.index[%client] = %this.getCount();
	}
	%this.index[%client]--;
	if(%this.index[%client] < 0)
		%this.index[%client] = %this.getCount()-1;
	%this.showMenu(%client,%this.index[%client]);
}

//name: setTempBody
//description: all menuObjects have static body text
//defined in the /Menus/menu_menuName.cs file
//This method allows you to change that text
function menuObject::setTempBody(%this,%body)
{
	if(%this.defaultBody $= %body)
		return false;
	
	%this.defaultBody = %this.bodyText;
	%this.bodyText = %body;
	
	return true;
}

//name: setDefaultBody
//description: sets the body text of the menu interface back to
//whatever it was initially set to when created in
//the /Menus/menu_menuName.cs file
function menuObject::setDefaultBody(%this)
{
	%this.bodyText = %this.defaultBody;
	%this.defaultBody = "";
}

//name: showInputMenu
//description: if the object is an input menu, it will be displayed
function menuObject::showInputMenu(%this,%client)
{
	cancel(%this.showInputMenuLoop[%client]);
	
	%this.inputMenuIndex[%client]++;
	if(%this.inputMenuIndex[%client] > 3)
	{
		%this.inputMenuIndex[%client] = 0;
		%this.periodAdder[%client] = "";
	}
	else
		%this.periodAdder[%client] = %this.periodAdder[%client] @ ".";
	
	if(%this.bodytext !$= %this.defaultBody)
	{
		%potential = " For: \c6 " @ %this.bodyText @ "\c3 ";
	}
	%display = "\c4Input Mode\c3:\n\c6Enter a value in the chat\n\n\c3Awaiting Value" @ %potential @ %this.periodAdder[%client];
	
	centerprint(%client,%display);
	
	%client.menu = %this;
	%this.inputMenuIsActive[%client] = true;
	%this.showInputMenuLoop[%client] = %this.schedule(500,showInputMenu,%client);
}

//name: hideInputMenu
//description: hides a currently displayed input menu
function menuObject::hideInputMenu(%this,%client)
{
	cancel(%this.showInputMenuLoop[%client]);
	//%selectedTxt = %client.menu.getSelected(%client).text;
	%this.inputMenuIsActive[%client] = false;
	centerprint(%client,"",1);
	if(%client.menu.inputMenuValue[%client] !$= "")
	{
		%this.onInputValueRecieved(%client,%client.menu.inputMenuValue[%client]);
		%client.menu.inputMenuValue[%client] = "";
	}
	
	if(%this.defaultBody !$= %this.bodyText)
	{
		%this.setDefaultBody();
	}
	%client.menu="";
}

//name: onInputValueRecieved
//description: callback for when a user gives the server input, from the input menu
function menuObject::onInputValueRecieved(%this,%client,%value)
{
	//empty, we should package this function to do stuff when this is called
}

//name: showMenu
//description: attempts to show a menu
function menuObject::showMenu(%this,%client,%indexHighlight)
{
	%c = %this.getcount();
	if(%c <= 0)
	{
		talk(%menu.name @ " is empty, use ::addMenuItem");
		return false;
	}
	
	if(!isObject(%client))
		return false;
	
	if(%client.menu $= "")
	{
		%indexHighlight = 0;
		%this.index[%client] = 0;
	}
	
	%client.menu = %this;
	%this.isActive[%client] = true;
	
	for(%i=0;%i<%c;%i++)
	{
		%menuItem = %this.getObject(%i);
		if(%menuItemDisplay $= "")
		{
			if(%i $= %indexHighlight)
			{
				%menuItemDisplay = "<div:1><font:arial:18>\c6" @ %menuItem.text @ "\n";
				%this.selected[%client] = %menuItem.text;
			}
			else
				%menuItemDisplay = "<font:arial:18>\c6" @ %menuItem.text @ "\n";
		}
		else
		{
			if(%i $= %indexHighlight)
			{
				%menuItemDisplay = %menuItemDisplay @ "<div:1><font:arial:18>\c6" @ %menuItem.text @ "\n";	
				%this.selected[%client] = %menuItem.text;
			}
			else
				%menuItemDisplay = %menuItemDisplay @ "<font:arial:18>\c6" @ %menuItem.text @ "\n";
		}
	}
	%bodyText = %this.bodytext;
	%menuItemDisplay = "\c6" @ %bodyText @ "\n\n" @ %menuItemDisplay @ "\n\n<just:right><font:arial:16>\c3Use Keypad To Nav<tab:10>";
	centerprint(%client,%menuItemDisplay);
}

//name: hideMenu
//description: attempts to hide a menu, if visible
function menuObject::hideMenu(%this,%client)
{
	centerprint(%client,"");
	%this.isActive[%client] = "";
	%client.menu = "";
}

//name: addMenuItem
//description: adds an item into the menu that is selectable and scrollable
function menuObject::addMenuItem(%this,%text,%evalText)
{
	%menuItem = new scriptObject()
	{
		class = menuItem;
		parent = %this;
		text = %text;
		evalText = %evalText;
	};
	
	%this.add(%menuItem);
}

//name: isMenu
//description: simple menu check
function menuObject::isMenu(%this,%name)
{
	if(isObject(%this.getMenu(%name)))
		return true;
	else
		return false;
}

//name: getMenu
//description: attempts to return a menu object, by name
function menuObject::getMenu(%this,%name)
{
	%c = %this.menu.getCount();
	for(%i=0;%i<%c;%i++)
	{
		%o = %this.menu.getObject(%i);
		if(%o.name $= %name)
		{
			return %o;
		}
	}
	return false;
}

//end menu system

package class_menu
{
	
	//name: serverCmdMessageSent
	//description: <b>Packaged</b> overwrite - input menu support
	function serverCmdMessageSent(%client,%message)
	{
		%m = %message;
		
		if(%client.menu.inputMenuIsActive[%client])
		{
			%value = stripmlcontrolchars(%m);
			%client.menu.inputMenuValue[%client] = %value;
			%client.menu.hideInputMenu(%client);
			return false;
		}
		
		%p = parent::serverCmdMessageSent(%client,%message);
		
		
		return %p;
		
	}
	
	//name: 
	//description: <b>Packaged</b> Example on displaying menus based on
	//certain things being clicked by a user
	function menuObject::onInputValueRecieved(%this,%client,%value)
	{
		%p = parent::onInputValueRecieved(%this,%client,%value);
		if(%client.menu.name $= "bob")
		{
			%remove = "abcdefghijklmnopqrstuvwxyz\\<>!@#$%^&_=;";
			%newval = %value;
			for(%i=0;%i<=strLen(%remove);%i++)
			{
				%letter = getSubStr(%remove,%i,1);
				%newval = strReplace(%newVal,%letter,"");
			}
			eval("$val = " @ %newval @ ";");
			if($val !$= "")
			{
				messageAll('',"\c6Bob says, \c3" @ %newval @ " \c6equates to \c4" @ $val);
				$val = "";
			}
		}
		return %p;
	}

	//name: servercmdsupershiftbrick
	//description: menu support for scrolling
	function servercmdsupershiftbrick(%client,%x,%y,%z)
	{
		if(%client.menu.isactive[%client])
		{
			%apples=true;
			if(%x == -1)
				%client.menu.scrollDown(%client);
			else
			if(%x == 1)
				%client.menu.scrollUp(%client);
			return false;
		}
		%p = parent::servercmdsupershiftbrick(%client,%x,%y,%z);	

		return %p;
	}
	

	//name: servercmdshiftbrick
	//description: menu support for scrolling
	function servercmdshiftbrick(%client,%x,%y,%z)
	{
		if(%client.menu.isactive[%client])
		{
			%apples=true;
			if(%x == -1)
				%client.menu.scrollDown(%client);
			else
			if(%x == 1)
				%client.menu.scrollUp(%client);
			return false;
		}
		%p = parent::servercmdshiftbrick(%client,%x,%y,%z);	

		return %p;
	}

	//name: serverCmdPlantBrick
	//description: menu support for selecting an item, in a menu	
	function serverCmdPlantBrick(%client,%a,%b,%c,%d)
	{
		if(%client.menu.isactive[%client])
		{
			%client.menu.onMenuSelect(%client);
			return false;
		}	
		%p = parent::serverCmdPlantBrick(%client,%a,%b,%c,%d);		

		return %p;
	}

	//name: serverCmdCancelBrick
	//description: support for closing a menu, if visible
	function serverCmdCancelBrick(%this,%a,%b,%c,%d,%e)
	{
		if(%this.menu.isActive[%this])
		{
			%this.menu.hidemenu(%this);	
			return false;
		}
		
		%p = parent::serverCmdCancelBrick(%this,%a,%b,%c,%d,%e);
		
		return %p;
	}
};
activatePackage(class_menu);