if(!isobject($class::attackMode))
	$class::attackMode = new scriptGroup(attackMode)
	{
		
	};
	
function attackMode::setPrint(%this,%client,%string)
{
	bottomPrint(%client,%string,4);
}