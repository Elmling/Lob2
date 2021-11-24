package scripts_onUseLight
{
	function serverCmdLight(%a,%b,%c,%d)
	{
		if($class::inventoryInterface.isactive[%a] !$= "") {
			$class::inventoryInterface.hide(%a.inventory);
		}
		else {
			$class::InventoryInterface.display(%a.inventory,0);
		}
			
		//%p = parent::serverCmdLight(%a,%b,%c,%d);
		
		//return %p;
	}
};
activatePackage(scripts_onUseLight);