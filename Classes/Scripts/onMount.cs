package scripts_onMount { 
	function armor::onMount(%this,%a,%b,%c,%d,%e) {
		%p = parent::onMount(%this,%a,%b,%c,%d,%e);
		if(%b.dataBlock $= "glidervehicle") {
			if(%a.dataBlock $= "lobarmor")
				%a.changeDatablock("playerstandardarmor");
			%a.playthread(0, "crouch");
		}
		return %p;
	}
	
	function armor::onUnMount(%this,%a,%b,%c,%d,%e) {
		%p = parent::onUnMount(%this,%a,%b,%c,%d,%e);
		%a.changeDatablock(lobarmor);
		return %p;
	}
	
	 function BDartImage::onMount(%this, %obj, %slot) {
		 %obj.playthread(1,"armreadyright");
		 return parent::onMount(%this, %obj, %slot);
	 }
};
activatePackage(scripts_onMount);