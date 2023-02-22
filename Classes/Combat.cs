if(isObject($class::combat)) {
	$class::combat.delete();
}

$class::combat = new scriptGroup() {
	class = combat;
    specialAttackTimeout = 7500;
};


function combat::damage(%this,%client,%dmg,%dmger) {
	if(isObject(%client)) {
		if(%client.player.getDamageLevel() + %dmg >= 100) {
			%kill = true;
		}
		%client.player.setDamageLevel(%client.player.getDamageLevel() + %dmg);
		if(%kill) {
			//talk("kill");
			%client.player.kill();
			$class::camera.orbitObj(%client,%client.player);
			//$class::chat.b_print(%client,"You are \c4dead\c6, you will respawn in \c615 seconds.");
		} else {
			//$class::chat.b_print("<just:right>Health: \c5" @ 100 - %client.player.getDamageLevel() @ " \c6/ \c5100",5);
			%this.show_health(%client);
			%client.player.setDamageFlash(0.3);
			//talk("no kill");
		}
	}
	return %dmg;
}

function combat::damage_ai(%this,%ai,%dmg,%dmger) {
	if(isObject(%ai)) {
		if(%ai.getDamageLevel() + %dmg >= 100) {
			%kill = true;
		}
		%ai.setDamageLevel(%ai.getDamageLevel() + %dmg);
		if(%kill) {
			//talk("kill");
			%ai.kill();
			//$class::camera.orbitObj(%ai,%ai.player);
			//$class::chat.b_print(%client,"You are \c4dead\c6, you will respawn in \c615 seconds.");
		} else {
			//$class::chat.b_print("<just:right>Health: \c5" @ 100 - %client.player.getDamageLevel() @ " \c6/ \c5100",5);
			//%this.show_health(%ai);
			%ai.setDamageFlash(0.3);
			//talk("no kill");
		}
		$class::worldText.show_at(vectorAdd(%ai.getEyePoint(),getRandom(-2,2) SPC getRandom(-2,2) SPC 2), %dmg @ " Damage", 2500);
	}
	return %dmg;	
}

function combat::show_health(%this,%client) {
	$class::chat.c_print(%client,"Your Health: \c5" @ 100 - %client.player.getDamageLevel() @ " \c6/ \c5100",5);
}

function combat::show_weapon_bonus(%this,%client) {
	%item = %client.inventory.getObject(%client.lastInventoryItemSelectedIndex).image;
	$class::chat.c_print(%client,"<font:arial:22>\c4" @ %client.inventory.getObject(%client.lastInventoryItemSelectedIndex).rarity @ " \c5" @ %item @ "\n<font:arial:14>\c5" @ strReplace(%client.inventory.getObject(%client.lastInventoryItemSelectedIndex).stats.toString(),",","\n\c5"),12);
}

function combat::specials_isOk(%this, %client) {
    return (getSimTime()-%client.lastSpecialAttackTime >= $class::combat.specialAttackTimeout);
}

function combat::specials_arrowRain(%this, %client) {
    if(%this.specials_isOk(%client)) {
        %range = 100;
        %rc = $class::raycast.getCollidePoint(%client, %range);
        if(getWordCount(%rc) >= 3) {
            %pos = %rc;
            %mi = %client.player.getMountedImage(0);
            %pn = %mi.projectile;
            $class::string.set(strLwr(%mi.projectile));
            if($class::string.contains("bow")) {
                for(%i=0;%i<10;%i++) {
                    %p = new projectile() {
                        dataBlock = fancybowprojectile;
                        scale = "1 1 1";
                        initialposition = vectorAdd(vectorAdd(%pos,"0 0 30"), getRandom(-3,3) SPC getRandom(-3,3) SPC getRandom(-3,7));
                        initialVelocity = "0 0 -30";
                        client = %client;
                        sourceObject = %client.player;
                    };
                }
            }
        }
    }
}