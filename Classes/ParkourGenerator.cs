if(isObject($class::parkourGenerator)) {
    $class::parkourGenerator.delete();
}

$class::parkourGenerator = new scriptGroup() {
    class = parkour;
};

function chooseRandomPoint(%pointA, %pointB, %searchRadius) {
    %randomPoint = getRandom(getWord(%pointA,0), getWord(%pointB,0)) SPC getRandom(getWord(%pointA,1), getWord(%pointB,1)) SPC getRandom(getWord(%pointA,2), getWord(%pointB,2));
    %searchResult = containerRayCast(%randomPoint, VectorAdd(%randomPoint, "0 0 -" @ %searchRadius), $TypeMasks::FxBrickAlwaysObjectType);
    if (%searchResult == 0) {
        return %randomPoint;
    } else {
        return false;
    }
    return ;
}

function generateRandomBricks(%numBricks, %pointA, %pointB) {
    %bricktype = "brick2x2x3data";
    for (%i = 0; %i < %numBricks; %i++) {
        %result = chooseRandomPoint(%pointA, %pointB, 10);
        if (getWordCount(%result) >= 3) {
            %brick = new fxDTSBrick() {
                datablock = %brickType;
                position = %result;
                rotation = "1 0 0 0";
                isPlanted = true;
                scale = "1 1 1";
                angleid = "3";
                client = findlocalclient();
                stackbl_id = -1;
            };
            findlocalclient().player.position = %result;
            %brick.plant();
            MissionCleanup.add(%brick);
        }
    }
}