
//override these from score pickup addon
function coinpileGoldItem::onpickup(%this,%a,%b,%c) {
	
}

function coinstackgolditem::onpickup(%this,%a,%b,%c) {
	
}

package scripts_onItemPickup {
	function shapeBase::pickUp(%player,%item) {
		if(isObject(%player.client) && !isObject(%item.spawnbrick)) {
			// talk("ui name " @ %item.dataBlock.uiName);
			if($class::inventoryinterface.command[%item.dataBlock.uiName] !$= "") {
				//$class::chat.to_all("picking up item, to inventory - > " @ %item.dataBlock.uiName);
				//$class::chat.to_all("Stats - > " @ %item.stats);
				if(isObject(%item.stats)) {
					%item_add = %player.client.inventory.addItem(%item.dataBlock.uiName,1, %item.stats, %item.rarity);
					if(%item.rarity $= "") {
						talk("error................");
					}
					// talk("stats set on inventory item : " @ %item_add.stats.getcount());
					// talk("rarity stat : " @ %item_add.rarity);
				} else {
					if(%item.amount !$= "")
						%amount = %item.amount;
					else
						%amount = 1;
					%player.client.inventory.addItem(%item.dataBlock.uiName,%amount);
				}
				serverplay3d(sound_pickup,%item.position);
				%item.delete();
			} else {
				%p = parent::pickUp(%player,%item);
				return %p;	
			}
		} else {
			%p = parent::pickUp(%player,%item);
			return %p;
		}
	}
};
activatePackage(scripts_onItemPickup);
//deactivatePackage(scripts_onItemPickup);




// since inventory items are objects, store stats to the object on pickup/drop
// roadblocks being fetching the inventory object after creation... also needing to store multiple instance types with diff stats (ie: 2 unique fancy swords)