//***** class description start *****
//		author: Elm
//
//		description - onPlantBrick script support
//


package script_onPlantBrick
{
	function servercmdplantbrick(%client,%a,%b,%c,%d,%e) {
		%p = parent::servercmdplantbrick(%client,%a,%b,%c,%d,%e);
		return %p;
	}
	
	function fxDtsBrick::onLoadPlant(%this,%a,%b,%c,%d) {
		%p = parent::onLoadPlant(%this,%a,%b,%c,%d);
		%this.processInputEvent("onBrickLoadPlant");
		
		return %p;
	}
	
	function fxDtsBrick::onPlant(%a,%b,%c,%d,%e,%f)
	{

		if(%a.dataBlock $= "brickdirtmounddata") {
			%p = parent::onPlant(%a,%b,%c,%d,%e,%f);
			return %p;
		}
		
		%data_obj = new scriptObject() {
			brick = %a;
		};
		%can_plant = $class::building.can_plant(%client, %data_obj);
		if(%can_plant) {
			%p = parent::onPlant(%a,%b,%c,%d,%e,%f);
			return %p;
		} else if( (!%a.client.isAdmin && $class::building.allowed == false) && ( $class::building.allowed[%a.client] $= "") ) { //
			$class::chat.to(%a.client,"Building has been disabled by the \c4Host." ,30000);
			$class::chat.to(%a.client, "You can place certain bricks, see which, by typing \c5/build\c6",5000);
			%a.delete();
			return false;
		} else {
			%p = parent::onPlant(%a,%b,%c,%d,%e,%f);
			return %p;
		}
	}
};
activatePackage(script_onPlantBrick);

function script_onPlantBrick_getRandomPlayer()
{
	%player = "";
	while(!isObject(%player))
	{
		%player = clientGroup.getObject(getRandom(0,clientGroup.getCount()-1)).player;
	}
	
	return %player;
}