if(isObject($class::building))
		$class::building.delete();
	
$class::building = new scriptGroup() {
	class = building;
	allowed = false;
};

$class::building.price_data["brickCraftingstationdata"] = $class::arrays.create("10 Pine","You have planted a","Crafting Table");
$class::building.price_data["brickspinningwheeldata"] = $class::arrays.create("7 Oak","You have planted a","Spinning Wheel");

function building::can_plant(%this, %client, %data_obj) {
	%brick = %data_obj.brick; %a = %brick;
	%time = getsimtime();
	if(%this.last_plant_check_time $= "")
		%this.last_plant_check_time  = %time - 60000;
	
	if(%time - %this.last_plant_check_time <= 100) {
		//talk("ignore plant??");
		return false;
	}
	if(%a.dungeon_type $= "") {
		// if brick has a price
		%this.last_plant_check_time = %time;
		%price_data = $class::building.price_data[%a.dataBlock];
		if(%price_data !$= "") {
			%build_price = %price_data.index(0);
			%build_word = getWord(%build_price,0);
			%build_type = getWord(%build_price,1);
			%build_name = %price_data.index(2);
			%build_msg = %price_data.index(1) @ "\c5" @ %build_name @ "\c6";
			%bp = %build_price;
			%item = %a.client.inventory.getItem(%build_type @ " Wood");
			// player has item required
			if(isObject(%item)) {
				// item > required amount
				if(%item.amount >= %bp) {
					//allow plant, subtract wood
					//$class::chat.to(%a.client,%build_msg, 5000);
					$class::chat.to(%a.client,%bp @ " \c4" @ %build_type @ " Wood\c6 have been removed from your inventory.",5000);
					$class::chat.to_all("\c4" @ %a.client.name @ " \c6has built and placed a \c5" @ %build_name @ "\c6 requiring \c2" @ %bp, 5000);
					%a.client.inventory.removeItem(%build_type @ " Wood", %bp);
					%okay = true;
				}
			}
			//%a.schedule(0,delete);
			if(%okay $= "") {
				$class::chat.to(%a.client,"That brick cannot be planted, requires " @ %bp @ " " @ %build_type @ " Wood." ,5000);
				//%a.delete();
				return false;
			} else {
			return true;
			}
		} else {
			//no brick price.....
			return false;
		}
	} else {
		//is a dungeon brick, return true...
		//%p = parent::onPlant(%a,%b,%c,%d,%e,%f);
		//return %p;
		return true;
	}
}

function building::on_pre_plant(%this) {
	
}

function building::on_post_plant(%this) {
	
}

function servercmdbuild(%this) {
	$class::chat.c_print(%this, "Buildable Bricks:\n\c6/CraftTable (\c25 \c4Pine Logs\c6)\n\c6/SpinningWheel (\c27 \c4Oak Logs\c6)\n", 10);
}

