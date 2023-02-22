$scripts_onCollision::utilizing = $class::arrays.create("horsearmor");

package scripts_onCollision {
    function armor::onCollision(%this,%b,%c,%d,%e,%f,%g) {
        %p = parent::onCollision(%this,%b,%c,%d,%e,%f,%g);
        %ai = false;
        %collider = false;
        if(%b.getClassName() $= "aiplayer") {
            %ai = %b;
            %collider = %c;
        } else if(%c.getClassName() $= "aiplayer") {
            %ai = %c;
            %collider = %b;
        }
        if(isObject(%ai)) {
            if(%collider.getClassname() $= "player") {
                if(getsimtime() - %ai.lastCollidingTime[%collider] >= 2000) {
                    if($scripts_onCollision::utilizing.contains(%ai.dataBlock)) {
                        %ai.lastCollidingTime[%collider] = getsimtime();
                        %aip = %ai.position;
                        %pp = %collider.position;
                        %collider.addVelocity(vectorscale(vectorsub(%aip,%pp),7*-1));
                        $class::combat.damage(%collider.client, 5);
                    }
                }
            }
        }
        
        return %p;
    }
};
activatePackage(scripts_onCollision);