package scripts_onSendWrenchData {
	function serverCmdSetWrenchData(%this, %strv, %a,%b,%c,%d,%e) {
		%p = parent:: serverCmdSetWrenchData(%this, %strv, %a,%b,%c,%d,%e);
		%name = getWord(%strv,1);
		%cmds = getWords(%strv, 2, getWordCount(%strv));
		talk(%strv);
		//talk(%name);
		//talk("a " @ %a);
		//talk("cmds - > " @ %cmds);
		
//		if(isFile("base/lob2/classes/bots/enemies/")) {			
//		}
		if(isFile("base/lob2/classes/bots/npcs/" @ %name @ ".cs")) {			
			if(isObject(%this.lastWrenchBrick)) {
				exec("base/lob2/classes/bots/npcs/" @ %name @ ".cs");
				eval("%_npc = $class::npc::" @  %name @ ".spawn(\"" @ vectorAdd(%this.lastWrenchBrick.position,"0 0 3") @ "\");");
				%index["direction"] = 13;
				%direction = getWord(%strv,%index["direction"]);
				if(%direction == 3) {
					%aim_location = vectorAdd(%this.lastWrenchBrick.position,"0 0 " @ getWord(%_npc.bot.getScale(),2) + 2);
					%aim_location = vectorAdd(%aim_location,"15 0 0");
				} else if(%direction == 4) {
					%aim_location = vectorAdd(%this.lastWrenchBrick.position,"0 0 " @ getWord(%_npc.bot.getScale(),2) + 2);
					%aim_location = vectorAdd(%aim_location,"0 -15 0");
				} else if(%direction == 5) {
					%aim_location = vectorAdd(%this.lastWrenchBrick.position,"0 0 " @ getWord(%_npc.bot.getScale(),2) + 2);
					%aim_location = vectorAdd(%aim_location,"-15 0 0");
				}
				if(%aim_location !$= "")
					%_npc.bot.setAimLocation(%aim_location);
				%_npc.wrench_data = %strv;
				%this.lastWrenchBrick.npc = %_npc.bot;
			} else {
				talk("error, no wrench brick on client " @ %this.name);
			}
		}
		return %p;
	}
	
	//on wrench hit
	function fxdtsbrick::sendwrenchdata(%a,%b,%c,%d,%e) {
		%p = parent::sendwrenchdata(%a,%b,%c,%d,%e);
		%b.lastWrenchBrick = %a;
		return %p;
	}
};
activatePackage(scripts_onSendWrenchData);