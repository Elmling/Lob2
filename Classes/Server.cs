//client
exec("./client/interface/bot_menu.cs");
// client end

//main classes
exec("./arrays.cs");
exec("./dictionary.cs");
exec("./misc.cs");
exec("./areas.cs");
exec("./nodeMemory.cs");
exec("./menu.cs");
exec("./ai.cs");
exec("./inventorys.cs");
exec("./inventoryInterface.cs");
exec("./brickager.cs");
exec("./avatars.cs");
exec("./Crafting.cs");
exec("./Profiles.cs");
exec("./Trading.cs");
exec("./Stabilizer.cs");
exec("./buildingTools.cs");
exec("./Sounds.cs");
exec("./Sounds/soundDatablocks.cs");
exec("./ExcelReader.cs");
exec("./Raycast.cs");
exec("./display.cs");
exec("./buildingTools.cs");
exec("./exp.cs");
exec("./quests.cs");
exec("./meteor.cs");
exec("./fishing.cs");
exec("./camera.cs");
exec("./farming.cs");
exec("./Mining.cs");
exec("./smelting.cs");
exec("./smithing.cs");
exec("./String.cs");
exec("./Climbing.cs");
exec("./Chat.cs");
exec("./Building.cs");
exec("./damage.cs");
exec("./itemDrop.cs");
exec("./worldText.cs");
exec("./combat.cs");
exec("./tutorial.cs");
exec("./banking.cs");
exec("./bankingInterface.cs");
exec("./dueling.cs");

//skills
exec("./Exp.cs");
exec("./Woodcutting.cs");
exec("./cooking.cs");

//menus
exec("./menus/menu_player.cs");
exec("./menus/menu_ok.cs");
exec("./menus/menu_yesno.cs");
exec("./menus/menu_input.cs");
exec("./menus/menu_brickager.cs");
exec("./menus/menu_crafting_wood.cs");
exec("./menus/menu_crafting_plank.cs");
exec("./menus/menu_seeds.cs");
exec("./menus/menu_smelting.cs");
exec("./menus/menu_crafting_table.cs");
exec("./menus/menu_crafting_wood_types.cs");
exec("./menus/menu_Arrow_Base_On_Table.cs");
exec("./menus/menu_ArrowTipType.cs");
exec("./menus/menu_onClickAnvil.cs");
exec("./menus/menu_smithingOptions.cs");
exec("./menus/menu_smithingAmount.cs");
exec("./menus/menu_onClickSpinningwheel.cs");
exec("./menus/menu_placeBowHandle.cs");
exec("./menus/menu_crafting_handle_wood_type.cs");
exec("./menus/menu_tutorial_master.cs");
exec("./menus/menu_banker.cs");
exec("./menus/menu_cooking.cs");
exec("./menus/menu_cookingFishing.cs");


//crafting recipes
exec("./crafting/recipes/recipe_handle.cs");
exec("./crafting/recipes/recipe_plank.cs");
exec("./crafting/recipes/recipe_woodenShield.cs");
exec("./crafting/recipes/recipe_arrowbase.cs");
exec("./crafting/recipes/recipe_arrow.cs");
exec("./crafting/recipes/recipe_bowstring.cs");
exec("./crafting/recipes/recipe_Bow.cs");
exec("./crafting/recipes/recipe_woodenBlowDart.cs");



//custom scripts by users
exec("./scripts/onClick.cs");
exec("./scripts/onJoin.cs");
exec("./scripts/onSpawn.cs");
exec("./scripts/onProjectileHit.cs");
exec("./scripts/onUseLight.cs");
exec("./scripts/onPlantBrick.cs");
exec("./scripts/onSwordBlock.cs");
exec("./scripts/onTrigger.cs");
exec("./scripts/onDamage.cs");
exec("./scripts/onGrapple.cs");
exec("./scripts/onSendWrenchData.cs");
exec("./scripts/onMount.cs");
exec("./scripts/onRemoveBrick.cs");
exec("./scripts/onItemPickup.cs");
exec("./scripts/onCollision.cs");

//bots
exec("base/lob2/classes/bots/npcs/don_the_fisher.cs");
exec("base/lob2/classes/bots/npcs/dungeon_master.cs");
exec("base/lob2/classes/bots/npcs/shop_keeper.cs");
exec("base/lob2/classes/bots/npcs/Banker.cs");

//datablocks
exec("./datablocks/weapons/weapon_boulderfist.cs");
exec("./datablocks/armor/armor_shields.cs");

//events
exec("./events/events.cs");

//dungeon
exec("./dungeon.cs");

//eval
exec("base/lob2/eval/eval.cs");

exec("base/lob2/classes/menusml/menusml.cs");

//glitched menus that have to be executed last... wtf?
exec("./menus/menu_dungeon_master.cs");

exec("base/lob2/eval/eval.cs");

// servercmds

function servercmdinit(%this) {
	if(%this.isSuperAdmin) {
		exec("base/lob2/classes/server.cs");
		servercmdenvgui_setvar(%this,"wateridx",3);
		servercmdenvgui_setvar(%this,"waterheight","18");
        findlocalclient().evalmode=true;
	}
}