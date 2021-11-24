function player::doSwordJumpBack(%this)
{
	%this.setvelocity(vectorScale(vectorAdd(%this.getforwardvector(),"0 0 -0.8"),-9.5));
}

function player::doSwordBlockAttack(%this)
{
	%this.schedule(320,playthread,0,"swordswing4");
	%this.setvelocity(vectorScale(vectorAdd(%this.getforwardvector(),"0 0 0.8"),10));
}