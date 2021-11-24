package scripts_onGrapple
{
	function onGrapple(){}
};
activatePackage(scripts_onGrapple);

function player::grapplerPropel(%this,%impactPos)
{
	%v = %this.getVelocity();
	%vx = getWord(%v,0);
	%vy = getword(%v,1);
	%vz = getword(%v,2);
	
	//clearRopes(%this.client.bl_id);
	
	//%group = _getRopeGroup(getSimTime(), %this.client.getBLID(), "");
	//createRope(vectorScale(vectorSub(%this.getEyePoint(),%this.getForwardVector()),1), %impactPos, "1 1 1 1", "0.1", "1", %group);
	
	if(getWord(%this.position,2) >= getWord(%impactPos,2))
	{
		
	}
	else
	{
		
		%pp = %this.position;
		%sub = vectorSub(%impactPos,%pp);
		%scale = vectorScale(%sub,0.35);
		
		%z = getword(%scale,2);
		%z = %z * 0.30;
		
		%scale = vectorScale(%scale,0.25);
		
		
		%scale = getWords(%scale,0,1) SPC %z;
		
		%final = %scale;
		%final = mclamp(getWord(%final,0),-1,1) SPC mclamp(getWord(%final,1),-1,1) SPC getWord(%final,2);
		
		//talk("final = " @ %final);
		
		
		%this.addvelocity(%final);
	}
}

function player::grapplerReel(%this,%impactPos)
{
	if(%this.ropeToolPosA $= "")
	{
		//%this.setTransform(%impactpos);
		%this.ropeToolPosA = %impactPos;
		%this.ropeToolPosANorm = "0 0 1";
		%this.client.ropeToolSlack = "4";
		%this.client.ropeToolDiameter = "0.15";
		//%this.client.updateRopeToolBP();
		%this.client.ropeToolGhostLoop();
	}
	
	cancel(%this.grapplerReelLoop);
	
	if(vectorDist(%this.position,%impactPos) <= 4)
	{
		messageclient(%this.client,'',"\c6Grappler has been disconnected via distance trigger.");
		return false;
	}
	
	%this.grapplerPropel(%impactPos);
	
	%this.grapplerReelLoop = %this.schedule(20,grapplerReel,%impactPos);
}