datablock fxDTSBrickData(brickMFRHullData)
{
	brickFile = "./Bricks/MFRHull.blb";
	category = "Nuclear";
	subCategory = "Base Parts";
	uiName = "MFR Hull";
	//energyGroup = "Machine";
	loopFunc = "EOTW_FissionReactorLoop";
	maxHeatCapacity = 20000;

	blacklistFromAdjacentScan = true;
};

function brickMFRHullData::onPlant(%this,%brick)
{
	Parent::onPlant(%this,%brick);

	%brick.CreateFissionHull();
}

function fxDtsBrick::CreateFissionHull(%obj)
{
	%fission = new SimSet()
	{
		class = "EOTW_FissionHull";
	};
	%obj.fissionParent = %fission;
	%fission.hullBrick = %obj;
}

function SimSet::GetFissionPart(%obj, %x, %y)
{
	if (isObject(%obj.fissionPart[%x, %y]))
		return %obj.fissionPart[%x, %y];
	else
	{
		%obj.fissionPart[%x, %y] = "";
		return -1;
	}
}

function SimSet::AddFissionPart(%obj, %part)
{
	%hull = %obj.hullBrick;
	%part.fissionParent = %obj;
	%partData = %part.getDatablock();
	%volXRadius = %partData.brickSizeX / 4;
	%volyRadius = %partData.brickSizeY / 4;
	%placedPos = getWords(vectorSub(%hull.getPosition(), %part.getPosition()), 0, 1);
	%obj.add(%part);

	for (%x = (%volXRadius * -1) + 0.5; %x <= %volXRadius; %x += 0.5)
		for (%y = (%volYRadius * -1) + 0.5; %y <= %volYRadius; %y += 0.5)
			%obj.fissionPart[getWord(%placedPos, 0) + %x, getWord(%placedPos, 1) +  %y] = %part.getID();
}

function SimSet::RemoveFissionPart(%obj, %part)
{
	if (%obj != %part.fissionParent)
		return;

	%hull = %obj.hullBrick;
	%partData = %part.getDatablock();
	%volXRadius = %partData.brickSizeX / 4;
	%volyRadius = %partData.brickSizeY / 4;
	%placedPos = getWords(vectorSub(%hull.getPosition(), %part.getPosition()), 0, 1);
	%obj.remove(%part);

	for (%x = (%volXRadius * -1) + 0.5; %x <= %volXRadius; %x += 0.5)
		for (%y = (%volYRadius * -1) + 0.5; %y <= %volYRadius; %y += 0.5)
			if (%obj.fissionPart[getWord(%placedPos, 0) + %x, getWord(%placedPos, 1) +  %y] == %part)
				%obj.fissionPart[getWord(%placedPos, 0) + %x, getWord(%placedPos, 1) +  %y] = "";
}

function SimSet::GetComponentsByType(%obj, %type)
{
	for (%i = 0; %i < %obj.getCount(); %i++)
	{
		%part = %obj.getObject(%i);

		if (%part.getDatablock().ComponentType $= %type)
			%partList = %partList SPC %part;
	}

	return trim(%partList);
}

function SimSet::GetAdjacentParts(%obj, %part)
{
	if (%obj != %part.fissionParent)
		return;

	%hull = %obj.hullBrick;
	%partData = %part.getDatablock();
	%volXRadius = %partData.brickSizeX / 4;
	%volYRadius = %partData.brickSizeY / 4;
	%placedPos = getWords(vectorSub(%hull.getPosition(), %part.getPosition()), 0, 1);

	for (%x = (%volXRadius * -1) + 0.5; %x <= %volXRadius; %x += 0.5)
		if (isObject(%newPart = %obj.GetFissionPart(getWord(%placedPos, 0) + %x, getWord(%placedPos, 1) + (%volYRadius * -1))) && !%newPart.getDatablock().blacklistFromAdjacentScan && !hasWord(%partList, %newPart))
			%partList = %partList SPC %newPart;
	
	for (%x = (%volXRadius * -1) + 0.5; %x <= %volXRadius; %x += 0.5)
		if (isObject(%newPart = %obj.GetFissionPart(getWord(%placedPos, 0) + %x, getWord(%placedPos, 1) + (%volYRadius + 0.5))) && !%newPart.getDatablock().blacklistFromAdjacentScan && !hasWord(%partList, %newPart))
			%partList = %partList SPC %newPart;

	for (%y = (%volYRadius * -1) + 0.5; %y <= %volYRadius; %y += 0.5)
		if (isObject(%newPart = %obj.GetFissionPart(getWord(%placedPos, 0) + (%volXRadius * -1), getWord(%placedPos, 1) + %y)) && !%newPart.getDatablock().blacklistFromAdjacentScan && !hasWord(%partList, %newPart))
			%partList = %partList SPC %newPart;

	for (%y = (%volYRadius * -1) + 0.5; %y <= %volYRadius; %y += 0.5)
		if (isObject(%newPart = %obj.GetFissionPart(getWord(%placedPos, 0) + (%volXRadius + 0.5), getWord(%placedPos, 1) + %y)) && !%newPart.getDatablock().blacklistFromAdjacentScan && !hasWord(%partList, %newPart))
			%partList = %partList SPC %newPart;

	return trim(%partList);
}

function SimSet::TestGetAdjacentParts(%obj, %part)
{
	%list = %obj.GetAdjacentParts(%part);

	for (%i = 0; %i < getWordCount(%list); %i++)
	{
		%brick = getWord(%list, %i);
		%brick.setColorFx(3);
		%brick.schedule(500, "setcolorfx",0);
	}
}

function fxDtsBrick::EOTW_FissionReactorLoop(%obj)
{
	if (getSimTime() - %obj.lastFissionTick < 1000)
		return;

	%obj.lastFissionTick = getSimTime();
	%fission = %obj.fissionParent;
	%fission.shuffle();
	
	//Tick all fission components
	for (%i = 0; %i < %fission.getCount(); %i++)
	{
		%part = %fission.getObject(%i);
		%partData = %part.getDatablock();
		if(%partData.fissionLoopFunc !$= "" && !%part.machineDisabled)
			%part.doCall(%partData.fissionLoopFunc);

		//Build list to process queued stuff later (ie heat)
		if (%partData.ComponentType !$= "")
			%componentList[%partData.ComponentType, %componentCount[%partData.ComponentType]++] = %part;
	}

	//Transfer queued heat to coolant or hull
	for (%i = 0; %i < %componentCount["Port"]; %i++)
	{
		%part = %componentList["Port", %i];
		if (%part.getDatablock().getName() !$= "brickMFRCoolantPortBrick")
			continue;

		//Do cooling thing
	}

	%obj.changeHeat(%obj.queuedHeat);
	%obj.queuedHeat = 0;
}