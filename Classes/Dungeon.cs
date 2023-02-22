if(isObject($class::dungeon))
	$class::dungeon.delete();

$class::dungeon = new scriptGroup(dungeon) {
};
$class::dungeon.waterBricksDisabled = false;
$class::dungeon.spectate_generation_z = 600; 
$class::dungeon.spectate_build_speed = 44;
$class::dungeon.generation_default_time = 3500;
$class::dungeon.sessions = new simSet();
$class::dungeon.spam_dungeons = new simSet();
$class::dungeon.water_height_set = false;
$class::dungeon.water_height = 17;
$class::dungeon.spectate_rotation = "-0.0198516 0.706969 -0.706966 3.18129";

function test(){
	$d = $class::dungeon.create(findlocalclient());
}

function spam_dungeon() {
	$class::dungeon.spam_dungeons.add(findlocalclient().dungeon);
	findlocalclient().dungeon = "";
	%dg = $class::dungeon.create(findlocalclient());
	%dg.generate();
	
}

function dungeon::get_start_position(%this) {
	%sp = getWords(vectorAdd(%this.owner.player.position,getRandom(-10000,10000) SPC getRandom(-10000,10000) SPC 0), 0, 1) SPC 4;//300 + getRandom(0,800));
	%sp = getWords(%sp, 0, 1) SPC 18;
	%st = getsimTime();
	//talk("begin");
	while(isObject($class::raycast.search(%sp,"500 500  10","water"))) {
		%sp = getWords(vectorAdd(%this.owner.player.position,getRandom(-10000,10000) SPC getRandom(-10000,10000) SPC 0), 0, 1) SPC 4;
		%sp = getWords(%sp, 0, 1) SPC 18;

		if(getSimTime() - %st > 1500) {
			$class::chat.to_all("Broke out of a potential infinite loop in dungeon::get_start_position method.");
			break;
		}
	}
	
	return %sp;
}

function dungeon::create(%this, %owner) {
	if(!%this.water_height_set) {
		%this.water_height_set = true;
		if(%this.isAdmin)
			%is_admin = true;
		else
			%is_admin = false;
		%this.isAdmin=true;
		servercmdenvgui_setvar(%owner,"waterheight", %this.water_height);
		if(!%is_admin)
			%this.isAdmin = false;
		$class::chat.to_all("\c5Water height adjusted for Dungeons ( " @ %this.water_height @ " )");
	}
	
	%instance = new scriptGroup() {
		class = dungeonInstance;
		parent = %this;
		owner = %owner;
	};
	%instance.rocks = new simSet();
	%instance.trees = new simSet();
	%instance.floors = new simSet();
	%instance.walls = new simSet();
	%instance.bricks = new simSet();
	%instance.enemies = new simSet();
	%instance.scenery = new simSet();
	%instance.chests = new simSet();
	%instance.waterBricks = new simSet();
	%instance.dragons = new simSet();
	%instance.farmtiles = new simSet();
	%instance.islands = new simSet();
	%this.sessions.add(%instance);
	echo("new dungeon: " @ %instance);
	return %instance;
}

function dungeonInstance::generate(%this, %sp, %time) {
	
	cancel(%this.genLoop);
	if(%time $= "")
		%time = $class::dungeon.generation_default_time ;
	if(%sp $= "") {
		%sp = %this.parent.get_start_position();
	}
	
	
	if(%this.start_time $= "") {
		$class::chat.to_all(%this.owner.name @ " is generating a Dungeon!");
		$class::chat.b_print(%this.owner, "Look \c4DOWN\c6. Your \c5Dungeon \c6is being \c4generated\c6.",60);
		%this.start_time  = getSimTime();
		%this.owner.player.setTransform(vectorAdd(%sp,"0 0 80"));
		$class::dungeon.sessions.add(%this);
	}
	
	%start_time = %this.start_time;
		
	if(%this.bricks.getcount() <= 0)
		%this.place_floor(%sp);
	
	%np = %this.getNextFreeSpaceFrom(%this.bricks.getObject(%this.bricks.getCount()-1).position);
	if(%this.posIsTrapped(%np)) {
		%np =  %this.getNextFreeSpaceFrom(%this.bricks.getObject(%this.bricks.getCount()-2).position);
		if(%this.posIsTrapped(%np)) {
			%np =  %this.getNextFreeSpaceFrom(%this.bricks.getObject(%this.bricks.getCount()-3).position);
			if(%this.posIsTrapped(%np)) {
				%np =  %this.getNextFreeSpaceFrom(%this.bricks.getObject(%this.bricks.getCount()-4).position);
				if(%this.posIsTrapped(%np)) {
					%np =  %this.getNextFreeSpaceFrom(%this.bricks.getObject(%this.bricks.getCount()-5).position);
					if(%this.posIsTrapped(%np)) {
						%np =  %this.getNextFreeSpaceFrom(%this.bricks.getObject(%this.bricks.getCount()-6).position);
					}
				}
			}
		}
	}
	//%this.owner.player.setTransform(vectorAdd(%np,"0 0 10"));
	if(%np != -1) {
		%this.place_floor(%np);
	} else {
		error("Generate error");
	}
	
	if(getSimTime() - %start_time > %time) {
		$class::chat.to(%this.owner, "Floor Generation completed ( " @ %this.floors.getcount() @ " Floor Tiles )");
		%this.start_time = "";
		%this.place_walls();
		return false;
	}
	
	%this.genLoop = %this.schedule(%this.parent.spectate_build_speed,generate,%sp,%time);
}

function dungeon::clear_idle_dungeons(%this) {
	$class::chat.to_all("Cleaning up idle Dungeons.");
	for(%i=0;%i<$class::dungeon.sessions.getcount();%i++) {
		%sess = $class::dungeon.sessions.getObject(%i);
		%sess.owner = "";
		if(!isObject(%sess.owner) || %sess.owner $= "" || (isObject(%sess.owner) && %sess.owner.dungeon $= "")) {
			%sess.cleanup();
		}
	}
}

function dungeon::clear_dungeons(%this) {
	$class::chat.to_all("Cleaning up idle Dungeons.");
	for(%i=0;%i<$class::dungeon.sessions.getcount();%i++) {
		%sess = $class::dungeon.sessions.getObject(%i);
		%sess.cleanup();
	}
}

function dungeonInstance::cleanup(%this) {
	cancel(%this.cleanupLoop);
	if(%this.bricks.getCount() <= 0) {
		if(isObject(%this.owner)) {
			$class::chat.to_all(%this.owner.name @ " has cleared their Dungeon!",15000);
		}
		else {
			$class::chat.to_all("Dungeon Cleared : " @ %this @ " ( Idle )");
		}
		
		%this.parent.sessions.remove(%this);
		for(%i=%this.enemies.getCount()-1; %i>=0;%i--) {
			%this.enemies.getObject(%i).delete();
		}
		for(%i=%this.dragons.getCount(); %i>=0;%i--) {
			%this.dragons.getObject(%i).delete();
		}
		return false;
	}
	%this.bricks.getObject(%this.bricks.getCount()-1).delete();
	%this.cleanupLoop = %this.schedule(1,cleanup);
}

function dungeonInstance::place_trees(%this, %curr_index) {
	cancel(%this.place_treesLoop);
	if(%curr_index $= "") {
		%curr_index = -1;
	}
	if(%curr_index >= %this.floors.getCount()) {
		$class::chat.to(%this.owner, "Tree Generation completed ( " @ %this.trees.getCount() @ " Trees )");
		%this.place_rocks();
		return;
	}
	%brick = %this.floors.getObject(%curr_index++);
	if(%brick.dungeon_type $= "floor") {
		%trees = "pine willow maple oak";
		%colors = "5 4 20 23 42";
		%z["pine"] = 8.8;
		%z["maple"] = 5.85;
		%z["willow"] = 6.5;
		%z["oak"] = 7.2;
		%owner = %this.owner;
		%amount = getRandom(2,5);
		for(%i=0;%i<%amount; %i++) {
			%tree = getWord(%trees,getRandom(0,getWordCount(%trees)-1));
			%color = getWord(%colors,getRandom(0,getWordCount(%colors)-1));
			if(%z[%tree] !$= "") {
				%z = %z[%tree];
			} else {
				%z = 2;
			}
			(%b = new fxDtsBrick() {
				client = %this.owner;
				position = vectorAdd(%brick.position,getRandom(-16,16) SPC getRandom(-16,16) SPC %z);
				dataBlock = "brick" @ %tree @ "Data";
				scale = "1 1 1";
				rotation = %this.random_rotation();
				colorID = %color;
				isPlanted = "1";
				dungeon_type = "tree";
			}).angleID="0";
			%error = %b.plant();
			%this.bricks.add(%b);
			%this.trees.add(%b);
			$class::camera.setRotation(%this.owner,vectorAdd(%b.position,"0 0 " @ $class::dungeon.spectate_generation_z) SPC $class::dungeon.spectate_rotation,1);
		} 
	} 
	%this.place_treesLoop = %this.schedule(%this.parent.spectate_build_speed, place_trees, %curr_index);
}

function dungeonInstance::place_walls(%this, %size, %curr_index) {
	cancel(%this.place_wallsLoop);
	if(%size $= "")
		%size = 64;
	if(%curr_index $= "")
		%curr_index = -1;
	if(%curr_index >= %this.floors.getCount()) {
		$class::chat.to(%this.owner, "Wall Generation completed ( " @ %this.walls.getCount() @ " Walls )");
		//talk("waterbrick count " @ %this.waterBricks.getcount());
		for(%i=%this.waterBricks.getCount();%i>=0;%i--) {
			%br = %this.waterBricks.getObject(%i);
			//talk("in at index " @ %i);
			if(isObject(%br.floor_brick)) {
				//talk("in2");
				%fbp = %br.floor_brick.position;
				%br.floor_brick.delete();
			}
			%br.delete();
			if(%fbp !$= "")
			{
				%this.place_lilies_at(%fbp);
				%fbp = "";
			}
		}
		%this.place_islands();
		return;
	}
	%brick = %this.floors.getObject(%curr_index++);
	if(%brick.dungeon_type $= "floor") {
		if(%this.getNextFreeSpaceFrom(%brick.position) != -1) {
			(%b = new fxDtsBrick() {
				client = %this.owner;
				position = %this.getNextFreeSpaceFrom(%brick.position);
				dataBlock = "brick" @ %size @ "xCubeData";
				scale = "1 1 1";
				rotation = "0 0 1 90.0002";
				colorID = "20";//"55";
				isPlanted = "1";
				dungeon_type = "wall";
			}).angleID="0";
			%curr_index--;
			%error = %b.plant();
			%this.bricks.add(%b);
			%this.walls.add(%b);
			$class::camera.setRotation(%this.owner,vectorAdd(%b.position,"0 0 " @ $class::dungeon.spectate_generation_z) SPC $class::dungeon.spectate_rotation,1);
		}
	}
	%this.place_wallsLoop = %this.schedule(20,place_walls, %size, %curr_index); //%this.parent.spectate_build_speed
}

function dungeonInstance::place_lilies_at(%this,%position) {
	%amount = getRandom(3,7);
	for(%i=0;%i<%amount;%i++) {
		%p = vectorAdd(%position,getRandom(-10,10) SPC getRandom(-10,10) SPC 0.2);
		(%b = new fxDtsBrick() {
			client = %this.owner;
			position = %p;
			dataBlock = "brickSwamplily" @ getRandom(1,4) @ "data";
			scale = "1 1 1";
			rotation = %this.random_rotation();
			colorID = "20";//"55";
			isPlanted = "1";
			dungeon_type = "scenery";
		}).angleID="0";
		%curr_index--;
		%error = %b.plant();
		%this.bricks.add(%b);
		%this.scenery.add(%b);
	}
}

function dungeonInstance::place_rocks(%this, %curr_index) {
	cancel(%this.place_rocksLoop);
	if(%curr_index $= "") {
		%curr_index = -1;
	}
	if(%curr_index >= %this.floors.getCount()) {
		$class::chat.to(%this.owner, "Rocks Generation completed ( " @ %this.rocks.getCount() @ " Rocks )");
		%this.place_scenery();
		return;
	}
	%brick = %this.floors.getObject(%curr_index++);
	if(%brick.dungeon_type $= "floor") {
		%colors = "11 2 33 48 50 62";
		%amount = getRandom(0,2);
		for(%i=0;%i<%amount; %i++) {
			%color = getWord(%colors,getRandom(0,getWordCount(%colors)-1));
			(%b = new fxDtsBrick() {
				client = %this.owner;
				position = vectorAdd(%brick.position,getRandom(-16,16) SPC getRandom(-16,16) SPC 0.65);
				dataBlock = "brickore_" @ getRandom(0,6) + (1) @ "Data";
				scale = "1 1 1";
				rotation = %this.random_rotation();
				colorID = %color;
				isPlanted = "1";
				dungeon_type = "rock";
			}).angleID="0";
			%error = %b.plant();
			%this.bricks.add(%b);
			%this.rocks.add(%b);
			%b.setText($class::mining.typeFromColor[%color] @ " Rock", 15);
			$class::camera.setRotation(%this.owner,vectorAdd(%b.position,"0 0 " @ $class::dungeon.spectate_generation_z) SPC $class::dungeon.spectate_rotation,1);
		}
	}
	
	%this.place_rocksLoop = %this.schedule(%this.parent.spectate_build_speed,place_rocks,%curr_index);
}

function dungeonInstance::place_scenery(%this, %curr_index) {
	cancel(%this.place_sceneryLoop);
	if(%curr_index $= "") {
		%curr_index = -1;
	}
	if(%curr_index >= %this.floors.getCount()) {
		$class::chat.to(%this.owner, "Scenery Generation completed ( " @ %this.scenery.getCount() @ " Scene Items )");
		%this.place_enemies();
		return;
	}
	%brick = %this.floors.getObject(%curr_index++);
	if(%brick.dungeon_type $= "floor") {
		%objs = "brickhugebush3data brickhugebush1data brickhugebush2data ";
		%objs = %objs @ "bricklargeflatbush2data bricklargefern1Data bricktallgrass1Data bricktallgrass1SpreadData brickpalmgrass3data brickpalmgrass1data ";
		%objs = %objs @ "brickrainforestplant3data brickpinetreenew2data brickswamptree2data brickswamptree1data brickrainforestkapok2data brickrainforestkapok1data brickdeserttree1data";
		%colors = "5 4 20 23 42";
		%z["brickhugebush2data"] = 1.2;
		%z["bricklargeflatbush2data"] = 1;
		%z["bricktallgrass1data"] = 0;
		%z["bricklargefern1data"] = 0;
		%z["bricktallgrass1spreaddata"] = 0;
		%z["brickpalmGrass3Data"] = 1.2;
		%z["brickpalmgrass1data"] = 1.2;
		%z["brickrainforestplant3data"] = 0.6;
		%z["Brickhugebush1data"] = 1.2;
		%z["brickfarming_tileData"] = 0.4;
		%z["brickhugebush3data"] = 2.2;
		%amount = getRandom(4,10);
		if(getRandom(0,1) == 1) {
			%no_farm_tile = true;
		}
		for(%i=0;%i<%amount; %i++) {
			%ob = getWord(%objs,getRandom(0,getWordCount(%objs)-1));
			if(%z[%ob] !$= "") {
				%p = %z[%ob];
			} else {
				%p = 3.4;
			}
			%color = getWord(%colors, getRandom(0,getWordCount(%colors) -1));
			(%b = new fxDtsBrick() {
				client = %this.owner;
				position = vectorAdd(%brick.position,getRandom(-16,16) SPC getRandom(-16,16) SPC %p);
				dataBlock = %ob;
				scale = "1 1 1";
				rotation = %this.random_rotation();
				colorID = %color;
				isPlanted = "1";
				dungeon_type = "scenery";
			}).angleID="0";
			%error = %b.plant();
			%this.bricks.add(%b);
			%this.scenery.add(%b);
			$class::camera.setRotation(%this.owner,vectorAdd(%b.position,"0 0 " @ $class::dungeon.spectate_generation_z) SPC $class::dungeon.spectate_rotation,1);
			
			if(%no_farm_tile $= "")
				if(getRandom(0,2) == 1)
					%this.place_farmtile(vectorAdd(%brick.position,getRandom(-14, 14) SPC getRandom(-14, 14) SPC 0));
		}
	}
	
	%this.place_sceneryLoop = %this.schedule(%this.parent.spectate_build_speed,place_scenery,%curr_index);
}

function dungeonInstance::place_enemies(%this, %curr_index) {
	cancel(%this.place_enemiesLoop);
	if(%curr_index $= "") {
		%curr_index = -1;
	}
	if(%curr_index >= %this.floors.getCount()) {
		$class::chat.to(%this.owner, "Enemy Generation completed ( " @ %this.enemies.getCount() @ " Enemies )");
		%this.place_dragons();
		%this.place_chests();
		$class::chat.to_all(%this.owner.name @ "\'s Dungeon is completed ( " @ %this.floors.getcount() @ " Floor Tiles, " @ %this.bricks.getCount()  + %this.enemies.getCount() @ " Total Objects )");
		$class::chat.c_print(%this.owner,"Your \c5Dungeon\c6 is ready. Click the \c4door\c6 to \c4Teleport \c6to your \c5Dungeon\c6.", 6);
		$class::camera.root(%this.owner);
		return;
	}
	%brick = %this.floors.getObject(%curr_index++);
	if(getRandom(0,4) <= 1) {
		%amount = getRandom(0,4);
		%db = "lobarmor BlockoParrotHoleBot ghoulArmor horseArmor";
		%dict = $class::dictionaries.create("lobarmor=Warrior,BlockoParrotHoleBot=Parrot,ghoularmor=Skeleton,horseArmor=Horse");
		%dict_armor = $class::dictionaries.create("lobarmor=0.5 0.3 0.1 1");
        %dict_scale = $class::dictionaries.create("ghoularmor=2.2 2.2 2.2");
		for(%i=0;%i<%amount;%i++) {
			%db_choice = getWord(%db,getRandom(0,getWordCount(%db)-1));
			%b = $class::bots.newBot(%dict.index(%db_choice), vectorAdd(%brick.position,getRandom(-5,5) SPC getRandom(-5,5) SPC "10"));
			if(%dict_armor.index(%db_choice) !$= "0") {
				%b.setNodeColor("ALL",%dict_armor.index(%db_choice));
			}
            if(%dict_scale.index(%db_choice) !$= "0") {
                %b.setScale(%dict_scale.index(%db_choice));
            }
			%name = %dict.index[%db_choice];
			%b.changeDatablock(%db_choice);
			%this.enemies.add(%b);
			%b.schedule(1500,roam);
			%b.dungeon = %this;
			$class::camera.setRotation(%this.owner,vectorAdd(%b.position,"0 0 " @ $class::dungeon.spectate_generation_z) SPC $class::dungeon.spectate_rotation,1);
		}
	}
	%this.place_enemiesLoop = %this.schedule(%this.parent.spectate_build_speed, place_enemies, %curr_index);
}

function dungeonInstance::place_dragons(%this) {
	%r = getRandom(1,5);
	for(%i=0;%i<%r;%i++) {
		%b = %this.floors.getObject(getRandom(mfloor(%this.floors.getCount() / 2),%this.floors.getCount()-1));
		%d = $class::bots.newBot("Dungeon Enemy " @ getRandom(1000,10000),%b.position);
		%d.changeDataBlock("airDragonArmor");
		%d.setScale("2 2 2");
		%d.schedule(10,setTransform,vectorAdd(%d.position,"0 0 200"));
		%d.schedule(1500,roam);
		%this.dragons.add(%d);
	}
}

function dungeonInstance::place_floor(%this, %pos, %size) {
	if(%size $= "")
		%size = 64;
	
	(%b = new fxDtsBrick() {
		client = %this.owner;
		position = %pos;
		dataBlock = "brick" @ %size @ "x" @ %size @ "fData";
		scale = "1 1 1";
		rotation = "0 0 1 90.0002";
		colorID = "21";
		isPlanted = "1";
		dungeon_type = "floor";
	}).angleID="0";
	%error = %b.plant();
	%this.bricks.add(%b);
	%this.floors.add(%b);
	if(getRandom(0,10) <= 1 && !%this.parent.waterBricksDisabled && %this.floors.getCount() > 1) {
			%water_pos = vectorAdd(%pos,"0 0 -16");
			(%b2 = new fxDtsBrick() {
			client = %this.owner;
			position = %water_pos;
			dataBlock = "brick" @ %size @ "x" @ %size @ "fData";
			scale = "1 1 1";
			rotation = "0 0 1 90.0002";
			colorID = "6";
			isPlanted = "1";
			dungeon_type = "floor_water";
		}).angleID="0";
		%error2 = %b2.plant();
		%b2.floor_brick = %b;
		%this.waterBricks.add(%b2);
	}

	$class::camera.setRotation(%this.owner,vectorAdd(%b.position,"0 0 " @ $class::dungeon.spectate_generation_z) SPC $class::dungeon.spectate_rotation,1);
	return %b;
}

function dungeonInstance::place_chests(%this) {
	if(%this.chests.getCount() <= 0) {
		//talk("in chestssss ");
		%amount = getRandom(1,4);
		for(%i=0;%i<%amount;%i++) {
			%brick = %this.floors.getObject(getRandom(mfloor(%this.floors.getCount() /2), %this.floors.getCount() -1));
			%pos = vectorAdd(%brick.position, getRandom(-16,16) SPC  getRandom(-16,16) SPC 0.65);
			(%b = new fxDtsBrick() {
			client = %this.owner;
			position = %pos;
			dataBlock = "bricktreasurechestdata";
			scale = "1 1 1";
			rotation = %this.random_rotation();
			colorID = "37";
			isPlanted = "1";
			dungeon_type = "chest";
		}).angleID="0";
		
		%error = %b.plant();
		%this.bricks.add(%b);
		%this.chests.add(%b);
		//$class::camera.orbitPos(%this.owner,vectorAdd(%b.position,"0 0 300"));
		}
		$class::chat.to(%this.owner, "Chests Generation completed ( " @ %this.chests.getCount() @ " Chests )");
	}
}

function dungeonInstance::place_farmtile(%this, %pos) {
		%z_offset = "0.4";
		%pos = vectorAdd(%pos,"0 0 " @ %z_offset);
		(%b = new fxDtsBrick() {
			client = %this.owner;
			position = %pos;
			dataBlock = "brickfarming_tileData";
			scale = "1 1 1";
			rotation = "0 0 1 90.0002";
			colorID = "37";
			isPlanted = "1";
			dungeon_type = "farmtile";
		}).angleID="0";
		%error = %b.plant();
		%this.bricks.add(%b);
		%this.farmtiles.add(%b);
		%b.setText("Farm Tile",15);
}

function dungeonInstance::place_islands(%this,%curr_index, %amount) {
	cancel(%this.place_enemiesLoop);
	
	if(%curr_index $= "") {
		%amount = getRandom(5,10);
		%curr_index = -1;
	}
	
	%curr_index++;
	
	%brick = %this.floors.getObject(getRandom(0,%this.floors.getCount()-1));
	%size = 64;
	if(%curr_index > %amount) {
		$class::chat.to(%this.owner, "Island generation completed ( " @ %amount @ " Islands )");
		%this.place_trees();
		return false;
	}
	if(isObject(%brick)) {
		%pos = vectorAdd(%brick.position,"0 0 " @ getRandom(70,120));
		(%b = new fxDtsBrick() {
			client = %this.owner;
			position = %pos;
			dataBlock = "brick" @ %size @ "x" @ %size @ "fData";
			scale = "1 1 1";
			rotation = "0 0 1 90.0002";
			colorID = "20";
			isPlanted = "1";
			dungeon_type = "island";
		}).angleID="0";
		%error = %b.plant();
		%this.islands.add(%b);
		%this.bricks.add(%b);
		
		(%tcb = new fxDtsBrick() {
			client = %this.owner;
			position = vectorAdd(%pos,"0 0 1.2");
			dataBlock = "brickTreasureChestData";
			scale = "1 1 1";
			rotation = %this.random_rotation();
			colorID = "27";
			isPlanted = "1";
			dungeon_type = "chest";
		}).angleID="0";
		%error = %tcb.plant();
		%this.chests.add(%tcb);
		%this.bricks.add(%tcb);
		
		%climb_amount = 100;
		for(%i=3;%i<%climb_amount+3;%i++) {
			%climb_pos = vectorAdd(%brick.position,getRandom(-20,20) SPC getRandom(-20,20) SPC 2);
			%climb_pos = getWords(%climb_pos, 0, 1) SPC (getWord(%climb_pos,2) * (( %i * 0.14 )+ 1));
			if(getWord(%climb_pos,2) >= getWord(%pos,2)) {
				break;
			}
			(%cb = new fxDtsBrick() {
				client = %this.owner;
				position = %climb_pos;
				//dataBlock = "brick2x9x5Data";
				dataBlock = "brick2x2x2octoconeinvdata";
				scale = "1 1 1";
				rotation = %this.random_rotation();
				colorID = "13";
				isPlanted = "1";
				dungeon_type = "island";
				colorfxid = 3; //glow
			}).angleID="1";
			%error = %cb.plant();
			%this.bricks.add(%cb);
			%cb.setColorFx(3);
			//talk("planbting climb brick");
		}
		//$class::camera.orbitPos(%this.owner,vectorAdd(%b.position,"0 0 300"));
	}
	
	%this.place_islandsLoop = %this.schedule(100,place_islands,%curr_index,%amount);
}

function dungeonInstance::place_next(%this,%pos) {
		%np = %this.getNextFreeSpaceFrom(%pos);
		if(%np !$= "-1")
			%this.place_floor(%np);
		else
			error("Dungeon instance could not find next free space");
		
		%this.owner.player.setTransform(vectorAdd(%np,"0 0 20"));
		
		return %pos @ " - > " @ %np;
}

function dungeonInstance::getNextFreeSpaceFrom(%this,%pos) {
	%pos_original = %pos;
	%pos = vectorAdd(%pos,"0 0 -0.5");
	while(true) {
		if(%f && %b && %l && %rr) {
			echo("Failed to find next free space");
			break;
		}
		%r = getRandom(0,4);
		%raycast_s = "1 1 1";
		if(%r == 0) {
			%f = true;
			%rc = $class::raycast.search(%this.shift("f",%pos),  "","water");
			if(isObject(%rc)) {
				// %rc.getObject(0).setColor(10);
				//echo("F - not free, found obj - > " @ %rc);
			} else {
				return %this.shift("f",%pos_original);
			}
		} else  if(%r == 1) {
			%b = true;
			%rc = $class::raycast.search(%this.shift("b",%pos),  "","water");
			if(isObject(%rc)) {
				// %rc.getObject(0).setColor(10);
				//echo("B - not free, found obj - > " @ %rc);
			} else {
				return %this.shift("b",%pos_original);
			}
		} else 	if(%r == 2) {
			%l = true;
			%rc = $class::raycast.search(%this.shift("l",%pos),  "","water");
			if(isObject(%rc)) {
				//%rc.getObject(0).setColor(10);
				//echo("L - not free, found obj - > " @ %rc);
			} else {
				return %this.shift("l",%pos_original);
			}
		} else 	if(%r == 3) {
			%rr = true;
			%rc = $class::raycast.search(%this.shift("r",%pos), "","water");
			if(isObject(%rc)) {
				//%rc.getObject(0).setColor(10);
				//echo("R - not free, found obj - > " @ %rc);
			} else {
				return %this.shift("r",%pos_original);
			}
		}
	}
	return -1;
}

function dungeonInstance::posIsTrapped(%this,%pos) {
	return %this.getNextFreeSpaceFrom(%pos) == -1;
}

function dungeonInstance::shift(%this,%dir,%pos, %brick_size) { 
	if(%brick_size $= "")
		%brick_size = (64/2) + 0.5;
	if(%dir $= "f") {
		return vectorAdd(%pos,"0 " @ %brick_size * -1 @ " 0");
	} else if(%dir $= "b") {
		return vectorAdd(%pos,"0 " @ %brick_size @ " 0");
	} else if(%dir $= "l") {
		return vectorAdd(%pos, %brick_size * -1 @ " 0 0");
	} else if(%dir $= "r") {
		return vectorAdd(%pos, %brick_size @ " 0 0");
	} else {
		// method usage error
	}
}

function dungeonInstance::traverse(%this, %client, %curr_index) {
	cancel(%this.traverseLoop);
	if(%client $= "")
		%client = %this.owner;
	if(%curr_index $= "")
		%curr_index = -1;
	%curr_index += 1;
	if(%curr_index < %this.floors.getcount()) {
		%f = %this.floors.getobject(%curr_index);
		$class::camera.orbitPos(%client,vectorAdd(%f.position,"0 0 10"));
		$class::chat.c_print(%client,"Floor " @ %curr_index @ " / " @ %this.floors.getcount()); 
	} else {
		$class::camera.root(%client);
		return false;
	}
	%this.traverseLoop = %this.schedule(700, traverse, %client, %curr_index);
}

function dungeonInstance::view(%this) {
	$class::camera.setRotation(%this.owner,vectorAdd(%this.floors.getObject(mfloor(%this.floors.getCount() / 2 )).position,"0 0 " @ $class::dungeon.spectate_generation_z) SPC $class::dungeon.spectate_rotation,1);
}

function dungeonInstance::random_rotation(%this) {
	%r[0] = "1 0 0 0";
	%r[1] = "0 0 -1 89.99";
	%r[2] = "0 0 1 180";
	%r[3] = "0 0 1 90.02";
	
	return %r[getRandom(0,3)];
}

function serverCmdReghost(%client) {
	$class::chat.to_all(%client.name @ " is re-ghosting the server.",5000);
	%client.schedule(500,resetGhosting);
	%client.schedule(500,activateGhosting);
}

function servercmdinvite(%client,%w0,%w1,%w2,%w3,%w4,%w6) {
	if(isObject(%client.dungeon) && %client.dungeon.floors.getcount() > 0) {	
		%name = "";
		for(%i=0;%i<6;%i++){
			if(%i > 0 && %w[%i] $= "") {
				break;
			}
			%name = %name SPC %w[%i];
		}

		%name = ltrim(%name);
		%fetch = findclientbyname(%name);
		if(isObject(%fetch)) {
			$class::chat.to(%fetch, %client.name @ " has invited you to their Dungeon instance.",10000);
			$class::chat.to(%fetch, "Type \c4/accept \c6to join.",10000);
			$class::chat.to(%client,%fetch.name @ " has recieved your invite.",10000);
			// %client.dungeon_invited = %fetch;
			%fetch.dungeon_inviter = %client;
		}
	} else {
		$class::chat.to(%client,"You don't have a Dungeon.");
	}
}

function servercmdaccept(%client) {
	if(isObject(%client.dungeon_inviter)) {
		if(isObject(%client.dungeon_inviter.dungeon) && (%client.dungeon_inviter.dungeon.floors.getCount() > 0)) {
			%client.player.setTransform(vectorAdd(%client.dungeon_inviter.dungeon.floors.getObject(0).position,"0 0 15"));
			$class::chat.to(%client, "You teleport to \c4" @ %client.dungeon_inviter.name @ "'s \c6Dungeon instance (1st Floor Tile)");
			$class::chat.to(%client.dungeon_inviter,"\c4" @ %client.name @ " \c6has teleported to your Dungeon (1st Floor Tile)");
			%client.dungeon_inviter = "";
			//serverplay3d(sound_teleport, %client.player.position);
			%client.player.schedule(10,playsound,sound_teleport);
			%client.setMusic("musicData_music_dungeon1",0.7);
		}
	} else {
		$class::chat.to(%client, "You have no Dungeon invites");
	}
}

function servercmdjoin(%client,%w0,%w1,%w2,%w3,%w4,%w6) {
	%name = "";
	for(%i=0;%i<6;%i++){
		if(%i > 0 && %w[%i] $= "") {
			break;
		}
		%name = %name SPC %w[%i];
		%name = ltrim(%name);
		%fetch = findclientbyname(%name);
		if(isObject(%fetch)) {
			if(isObject(%fetch.dungeon) && %fetch.dungeon.floors.getCount() > 0) {
				$class::chat.to(%fetch,"\c5" @%client.name @ " \c6would like to join your Dungeon.",5000);
				$class::chat.to(%fetch,"Type /invite " @ getSubStr(%client.name,0,3) @ " to invite them.",5000);
				$class::chat.to(%client,"\c5" @ %fetch.name @ " \c6has recieved your request, be patient..",5000);
			} else {
				$class::chat.to("\c5" @ %client,%fetch.name @ " \c6does not have a Dungeon instance.", 1500);
			}
		}
	}
}

function servercmddungeons(%client) {
	$class::chat.to(%client,"There are currently \c5" @ $class::dungeon.sessions.getcount() @ "\c6 active \c4Dungeons\c6.",2500);
}

function serverCmdDungeon(%client,%clear) {
	//if(!%client.profile.starter_pack) {
	//	$class::chat.to(%client, "You need a \c4Starter Pack\c6 from the \c5Dungeon Master \c6before trying to create a \c4Dungeon\c6 instance.");
	//	return false;
	//}
	if(%client.dungeon $= "" || !isObject(%client.dungeon))
		%client.dungeon = $class::dungeon.create(%client);
	
	if(%client.dungeon.bricks.getCount() <= 0)
		%client.dungeon = $class::dungeon.create(%client);

	if(%clear $= "view") {
		%client.dungeon.view();
	} else if(%clear $= "clear") {
		if(%client.dungeon.floors.getCount() > 0) {
			$class::chat.to_all("\c4" @ %client.name @ " \c6has started clearing their \c5Dungeon\c6!", 8500);
			%client.dungeon.cleanup();
			%client.player.setTransform(vectorAdd("702 345 28",getRandom(-5,5) SPC getRandom(-5,5) SPC 0));
			%client.player.schedule(10,playsound,sound_teleport);
			%client.schedule(50,setMusic,"musicData_music_home1",0.7);
			//serverplay3d(sound_teleport, %client.player.position);
		}
	} else {
		%owner = %client;
		if(%owner.dungeon.bricks.getCount() > 0) {
			$class:chat.to(%owner,"You have an existing \c5Dungeon\c6, type \c4/dungeon \c5clear");
			return false;
		} else {
			%owner.dungeon.generate();
			%client.player.setTransform(vectorAdd("702 345 28",getRandom(-5,5) SPC getRandom(-5,5) SPC 0));
			//serverplay3d(sound_teleport, %client.player.position);
		}
	}
}

function serverCmdHome(%client) {
	
    if(%client.player.isMounted()){
        $class::chat.c_print(%client,"Please dismount before trying to teleport home!",3);
        return false;
    }
    $class::chat.to(%client, "You teleport \c5Home\c6!", 1500);
	%client.player.setTransform(vectorAdd("702 345 28",getRandom(-5,5) SPC getRandom(-5,5) SPC 0));
	//serverplay3d(sound_teleport, %client.player.position);
	%client.player.schedule(50,playsound,sound_teleport);
	%client.schedule(50,setMusic,"musicData_music_home1",0.7);
}

$menu::dungeon_master.addMenuItem("Starter Pack","servercmdstarterpack(#CLIENT);");
$menu::dungeon_master.addMenuItem("Joining your/friends Dungeon","servercmdDungeonHowToJoin(#CLIENT);");

function servercmdstarterpack(%this) {
	if(!%this.profile.starter_pack) {
		%this.profile.starter_pack = true;
		%this.inventory.addItem("Fancy Sword",1);
		%this.inventory.addItem("Pine Short Bow",1);
		%this.inventory.addItem("Hatchet",1);
		%this.inventory.addItem("Pickaxe",1);
		$class::chat.to_all("\c5" @ %this.name @ " \c6has claimed their \c4Starter Pack \c6from the \c4Dungeon Master\c6.");
		$class::chat.to(%this, "Use your \c4light key\c6 to open your \c5Inventory\c6.");
	} else {
		 $class::chat.to(%this, "You've already claimed your \c4Starter Pack\c6. Use your \c4light key\c6 to open your \c5Inventory\c6.");
	}
}

function servercmdhelp(%this) {
	$class::chat.to(%this ,"There is plenty of information here:  " @ make_link("http://landofblocks2.pythonanywhere.com/", "Website"));
}