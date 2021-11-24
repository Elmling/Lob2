



package script_onRemove {
	
		function fxdtsbrick::onRemove(%this,%a,%b,%c,%d,%e,%f,%g) {
			if(isObject(%this.shapeName)) {
				%this.shapeName.delete();
			}
			if(isObject(%this.npc)) {
				%this.npc.delete();
				%this.npc = "";
			} else if(isObject(%this.enemy)) {
				%this.npc.delete();
				%this.npc = "";
			}
			%p = parent::onRemove(%this,%a,%b,%c,%d,%e,%f,%g) ;
			return %p;
		}
};
activatePackage(script_onRemove);