datablock AudioProfile(sound_treeFall)
{
   filename    = "base/lob2/classes/sounds/soundfx_treeFall.wav";
   description = AudioClosest3d;
   preload = true;
};

datablock AudioProfile(sound_crafting_knife)
{
   filename    = "base/lob2/classes/sounds/soundfx_crafting_knife.wav";
   description = AudioClosest3d;
   preload = true;
};

datablock AudioProfile(sound_sword_clash)
{
	filename = "base/lob2/classes/sounds/soundfx_sword_clash.wav";
	description = AudioClosest3d;
	preload = true;
};

datablock AudioProfile(sound_leap_dash)
{
	filename = "base/lob2/classes/sounds/soundfx_leap_dash.wav";
	description = AudioClosest3d;
	preload = true;
};

datablock AudioProfile(sound_levelup)
{
	filename = "base/lob2/classes/sounds/soundfx_levelup.wav";
	description = AudioClosest3d;
	preload = true;
};

datablock AudioProfile(sound_eat)
{
	filename = "base/lob2/classes/sounds/soundfx_eat.wav";
	description = AudioClosest3d;
	preload = true;
};

datablock AudioProfile(sound_smelt)
{
	filename = "base/lob2/classes/sounds/soundfx_smelt.wav";
	description = AudioClosest3d;
	preload = true;
};

datablock AudioProfile(sound_teleport)
{
	filename = "base/lob2/classes/sounds/soundfx_teleport.wav";
	description = AudioClosest3d;
	preload = true;
};

datablock AudioProfile(sound_pickup)
{
	filename = "base/lob2/classes/sounds/soundfx_pickup.wav";
	description = AudioClosest3d;
	preload = true;
};

datablock AudioProfile(sound_heal1) {
	filename = "base/lob2/classes/sounds/soundfx_heal1.wav";
	description = AudioClosest3d;
	preload = true;
};

datablock AudioProfile(sound_skeleton_death) {
	filename = "base/lob2/classes/sounds/soundfx_skeleton_death.wav";
	description = AudioClosest3d;
	preload = true;
};

datablock AudioProfile(sound_ambience_birds) {
	filename = "base/lob2/classes/sounds/ambience_birds.ogg";
	description = AudioClosest3d;
	preload = true;
};

datablock AudioProfile(sound_ambience_wind) {
	filename = "base/lob2/classes/sounds/ambience_wind.ogg";
	description = AudioClosest3d;
	preload = true;
};

function player::playsound(%this,%sounddb) {
	return serverplay3d(%sounddb,%this.position);
}

$class::sounds.register("treefall",sound_treefall);
$class::sounds.register("crafting_knife",sound_crafting_knife);
$class::sounds.register("sword_clash",sound_sword_clash);
$class::sounds.register("leap_dash",sound_sword_clash);
