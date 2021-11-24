package scripts_onProjectileSpawn
{
	function arrowProjectile::onSpawn(%a,%b,%c,%d,%e)
	{
		%p = parent::onSpawn(%a,%b,%c,%d,%e);
		
		talk("spawned proj");
		
		return %p;
	}
	
};
activatePackage(scripts_onProjectileSpawn);

