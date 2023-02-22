package scripts_onClick
{
	function player::activateStuff(%a,%b,%c,%d,%e,%f)
	{
		%p = parent::activateStuff(%a,%b,%c,%d,%e,%f);
		
		%client = %a.client;
		%EyeVector = %a.getEyeVector();
		%EyePoint = %a.getEyePoint();
		%Range = 4;
		%RangeScale = VectorScale(%EyeVector, %Range);
		%RangeEnd = VectorAdd(%EyePoint, %RangeScale);
		%raycast = containerRayCast(%eyePoint,%RangeEnd,$TypeMasks::fxbrickobjecttype | $typemasks::playerobjecttype, %a);
		%o = getWord(%raycast,0);

		if(isObject(%o))
		{	
			if(%o.getclassname() $= "fxdtsBrick")
			{
				%client.lastBrickClicked = %o;
                if(stripos(%o.dataBlock,"brickfirepitdata") >= 0) {
                    %menu = $class::menusystem.getmenu("cooking");
					%menu.showmenu(%client,0);
                }
                if(%o.getName() $= "_explore") {
                    %client.player.setTransform(vectoradd(%client.player.position,"0 0 165"));
                }
				if(stripos(%o.dataBlock,"stage4data") >= 0) {
					if(!isObject(%o.seed)) {
						%seed_type = strReplace(%o.dataBlock,"_"," ");
						%seed_type = getWord(%seed_type,0);
						%seed_type = strReplace(%seed_type,"brick","");
						%o.seed = $class::farming.generate_seed_final(%seed_type, %o);
					}
				} else if(%o.dataBlock $= "brickTreasureChestData" && %o.opened $= "") {
					%items = $class::arrays.create("fancysworditem, bdartitem, coinstackgolditem, gemlowitem, gemhighitem, gemmediumitem, gemrareitem");
					%amount = getRandom(1,5);
					%amount_sub = 0;
					for(%i=0;%i<%amount;%i++) {
						if(getRandom(0,4) == 0) {
							%amount_sub++;
							continue;
						}
						%item = %items.getRandom();
						if(%item $= "fancysworditem") {
							%item_drop = $class::itemDrop.drop_at(vectorAdd(%o.position, getRandom(-2,2) SPC getRandom(-2,2) SPC 2),%item,true);
						} else {
							%item_drop = $class::itemDrop.drop_at(vectorAdd(%o.position, getRandom(-2,2) SPC getRandom(-2,2) SPC 2),%item);
						}
						if(isObject(%item_drop)) {
							%o.opened = true;
							%o.opener = %client.name;
							%o.open_amount = %amount - %amount_sub;
						}
					}
					$class::chat.to_all("\c5" @ %client.name @ " \c6has found a \c4Treasure Chest \c6containing \c2" @ %amount - %amount_sub @ " \c6items!",5000);
				} else if(%o.opened !$= "") {
					$class::chat.to(%client,"\c4" @ %o.opener @ " \c6has already opened this \c4Treasure Chest\c6 which contained \c2" @ %o.open_amount @ " \c4items.\c6",10000);
				}
				if(%o.dataBlock $= "brickfarming_tiledata") {
					$class::chat.c_print(%client,"Use a \c4Shovel \c6on this \c5farm tile",5);
				} else 
				if(%o.dataBlock $= "brickdungeondoorcwdata") {
					if(isObject(%client.dungeon) && %client.dungeon.floors.getCount() > 0) {
						%client.player.setTransform(vectorAdd(%client.dungeon.floors.getobject(0).position,"0 0 15"));
						// $class::chat.b_print(%client,"You \c4Teleport \c6to your \c5Dungeon\c6.", 5);
						$class::chat.to_all("\c4" @ %client.name @ "\c6 has entered his \c5Dungeon\c6 instance!", 60000);
						$class::chat.to(%client,"Commands: \c4/home\c6, \c4/invite \c5player\c6, \c4/dungeon \c5clear", 60000);
						%client.player.schedule(10,playsound,sound_teleport);
						%client.setMusic("musicData_music_dungeon1",0.7);
					} else {
						$class::chat.b_print(%client,"You do not have a \c5Dungeon\c6 yet. Type \c4/dungeon \c6to generate one.", 5);
					}
				} else
				if(%o.dataBlock $= "brickspinningwheeldata")
				{
					%menu = $class::menusystem.getmenu("onclickspinningwheel");
					%menu.showmenu(%client,0);
				}
				else if(%o.dataBlock $= "brickanvildata")
				{
					%menu = $class::menusystem.getmenu("onclickanvil");
					%menu.showMenu(%client,0);
				}
				else if(%o.dataBlock $= "brickfurnacedata")
				{
					%menu = $class::menusystem.getMenu("smelting");
					%menu.showMenu(%client,0);
				}
				else if(%o.dataBlock $= "brickCraftingStationData")
				{
					%menu = $class::menusystem.getMenu("craftingtable");
					%menu.showMenu(%client,0);
				}
				
				if(isObject(%o.item))
				{
					if(%o.item.dataBlock $= "WormsItem")
					{
						%o.setItem("");
						%amount = getRandom(1,3);
						%client.inventory.addItem("Worms",%amount);
						centerPrint(%client,"\c6You now have \c2" @ %client.inventory.getItem("worms").amount @ " \c4Worms\c6!",3);					
					}
				}
				if(isObject(%o.seed))
				{
					if(%o.seed.canHarvest)
					{
						%o.seed.harvest(%client);
					}
				}
				else
				if(%o.getName() $= "_table")
				{
					if(!isObject(%client.inventory))
						%knife = 0;
					else
						%knife = %client.inventory.getItem("Carving Knife");
					
					if(isObject(%knife))
					{
						%menu = $class::menuSystem.getMenu("crafting table");
						%menu.showMenu(%client,0);
					}
					else
					{
						%menu = $class::menuSystem.getMenu("ok");
						%menu.setTempBody("You need a \c3Carving Knife \c6to begin Crafting.");
						%menu.showMenu(%client,0);
						%menu.setDefaultBody();						
					}
				}
				else
				if(strPos(strLwr(%o.dataBlock.getName()),"door") >= 0)
				{
					
				}
				else
				{
					%menu = $class::menusystem.getMenu("brick");
					if(isObject(%menu))
					{
						//%client.lastClicked = %o;
						//%menu.showMenu(%client);
					}
				}
			}
			if(%o.getclassname() $= "aiPlayer") {
				if(isObject(%o.npc_handle)) {
					%client.lastClicked = %o;
					%o.npc_handle.on_click(%client);
				}
			}
			if(%o.getclassname() $= "player")
			{
				%menu = menusystem.getMenu("player");
				if(isObject(%menu))
				{
					%client.lastClicked = %o;
					%menu.showMenu(%client);
				}
			}

			if(%o.name !$= "")
			{
				%menuObject = menuSystem.getMenu(%o.name);
				if(isObject(%menuObject))
				{
					%client.lastClicked = %o;
					%menuObject.showmenu(%client);
				}
				else
				{
					%o.dopush(%client);
				}
			}
		}
		return %p;
	
	}
};
activatePackage(scripts_onClick);
deactivatePackage(TreasureChestPackage);