
if(isObject($class::itemDrop))
	$class::itemDrop.delete();


$class::itemDrop = new scriptGroup() {
	class = itemDrop;
};


function itemDrop::from_eye(%this,%aiOrPlayer, %db) {
	%pos = vectorscale(%aiOrPlayer.getForwardVector(),2);
	%pos = vectorAdd(%aiOrPlayer.getEyePoint(),%pos);
	%this.drop_at(%pos,%db);
}

function itemDrop::from_body(%this,%aiorplayer, %db) {
	
}

function itemDrop::drop_at(%this,%pos, %db, %do_rarity) {
	%item = new item() {
		dataBlock = %db;
		scale = "1 1 1";
		position = %pos;
	};
	%item.canpickup = true;
	if(%do_rarity $= "")
		%do_rarity = false;
	if(%do_rarity) {
		%rarity = %this.generate_rarity();
		//prob dont need the below..
		%this.rarity = %rarity;
		%prefix = "";
		if(%rarity $= "Normal") {
			//nothing..
		} else if(%rarity $= "unique") {
			%item.setShapeNameColor("0 0 1");
			%prefix = "( Unique ) - ";
			%stats = %this.generate_stats("unique");
		} else if(%rarity $= "elite") {
			%item.setShapeNameColor("1 0 0.5");
			%prefix = "( Elite ) - ";
			%stats = %this.generate_stats("elite");
		} else if(%rarity $= "Rare") {
			%item.setShapeNameColor("1 0 0");
			%prefix = "( Rare ) - ";
			%stats = %this.generate_stats("rare");
		} else if(%rarity $= "Legendary") {
			%item.setShapeNameColor("1 1 0");
			%prefix = "( Legendary ) - ";
			%stats = %this.generate_stats("legendary");
		}
		%item.stats = %stats;
		%item.rarity = %rarity;
	}
	%item.schedule(500, setShapeName, %prefix @ %item.dataBlock.uiName);
	%item.setVelocity("0 0 10");
	%item.schedule(60000 * 3, delete);
	return %item;
}

function itemDrop::generate_rarity(%this) {
	if(getRandom(0,100) <= 40) { //unique
	//talk("unique");
		return "Unique";
	} else if(getRandom(0,100) <= 30) { //rare
		return "Rare";
	} else if(getRandom(0,100) <= 20) { //elite
		return "Elite";
	} else if(getRandom(0,100) <= 10) { //elite
		return "Legendary";
	} else { // normal
		return "Normal";
	}
}

function itemDrop::generate_stats(%this,%rarity) {
	//MODIFIERS
	// move speed
	// jump height
	// melee
	// range
	// mage
	// armor
	// health regen
	// life steal
	// item drop luck chance
	// crit chance
	%max["unique"] = 5;
	%max["rare"] = 10;
	%max["elite"] = 15;
	%max["legendary"] = 20;
	
	if(%rarity $= "" || %max[%rarity] $= "")
		%max = 5;
	else
		%max = %max[%rarity];
	%a = -1;
	%modifier[%a++] = "Jump Height";
	%modifier[%a++] = "Move Speed";
	%modifier[%a++] = "Melee";
	%modifier[%a++] = "Range";
	%modifier[%a++] = "Mage";
	%modifier[%a++] = "Armor";
	%modifier[%a++] = "Health Regen";
	%modifier[%a++] = "Life Steal";
	%modifier[%a++] = "Drop Chance";
	%modifier[%a++] = "Crit Chance";
	%amount = getRandom(2,6);
	%arr =  $class::arrays.create();
	$class::chat.to_all("A \c5" @ %this.rarity @ " \c4Item\c6 has been \c2dropped\c6 somewhere!");
	for(%i=0;%i<%amount;%i++) {
		%num = getRandom(1,%max);
		%arr.push_last(%modifier[getRandom(0,%a)] @ " : + " @ %num);
	}
	$class::chat.to_all("\c4Item\c5 Stats: \c6" @ %arr.toString(	));
	if(%arr.getCount() > 0)
		return %arr;
	else
		return false;
}

// using $class::arrays.create method..
function itemStats::create(%this,%array_of_modifiers) {
	for(%i=0;%i<%array_of_modifiers.getCount();%i++) {
		//talk(%array_of_modifiers.index(%i));
	}
	//%this.
}