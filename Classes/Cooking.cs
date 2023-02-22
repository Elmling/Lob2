//***** class description start *****
//		Author: Elm
//
//		Requires: N/A
//
//		Description:
//		Class that handles all Cooking functionality
//
//***** class description end ***** 

if(isObject($class::cooking))
	$class::cooking.delete();

$class::cooking = new scriptGroup()
{
	class = cooking;
	exp["fish"] = 80;
	exp["horse meat"] = 80;
	alive_time["pine"] = 15000;
	alive_time["oak"] = 15000 *2;
	alive_time["willow"] = 15000 *3;
	alive_time["maple"] = 15000 *4;
	alive_time["yew"] = 15000*5;
};

function cooking::buildFire(%this, %client, %woodtype)
{
    if(%client.inventory.getItem(%woodType @ " Wood").amount <= 0) {
        talk("Not enough wood rip");
        return false;
    }
    %client.inventory.removeItem(%woodtype @ " Wood", 1);
	%pos = vectorAdd(%client.player.position,vectorScale(%client.player.getForwardVector(),2));
	%pos = vectorAdd(%pos,"0 0 0.25");
	$p = %pos;

	(%b = new fxDtsBrick() {
		dataBlock="brickFirePitData";
		scale = "1 1 1";
		position = %pos;
		bl_id = %client.bl_id;
		rotation = "0 0 1 180";
		client = %client;
		isplanted=true;
		colorId="1";
	}).angleID="0";
    

	%b.plant();
    
    %em = new particleEmitterNode() {
        dataBlock = "GenericEmitterNode";
        emitter = burnemitterb;
        scale = "0.5 0.5 0.5";
        position = %b.getWorldBoxCenter();
        velocity = 1;
    };
	%b.schedule($class::cooking.alive_time[%woodtype],delete);
    %em.schedule($class::cooking.alive_time[%woodtype],delete);
}

function cooking::fuelFire(%this, %firebrick, %woodtype) {
	// place wood to keep fire going
}

function cooking::place_raw(%this,%rawfoodtype, %amount) {
	// place raw meat
}

function cooking::show_status(%this,%fireBrick) {
	// show the current meat being cooked
}

function cooking::remove_cooked(%this, %firebrick) {
	// get all of the cooked food from the fire
}