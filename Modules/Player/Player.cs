function PlayerLoop()
{
	cancel($Loops:Test);
	
	for (%i = 0; %i < ClientGroup.GetCount(); %i++)
	{
		%client = ClientGroup.getObject(%i);
		%client.PrintEOTWInfo();
	}
	
	$Loops:Test = schedule(100,ClientGroup,"PlayerLoop");
}
schedule(100, 0, "PlayerLoop");

function GetTimeStamp()
{
	%hours = mFloor($EOTW::Time);
    %minutes = 60*($EOTW::Time - %hours);
    %minutes = mFloor(%minutes * 0.1) * 10;

    %hours = (%hours + 6) % 24; //corrects for dawn being considered hour 0
	
	if (%minutes < 10)
		%minutes = "0" @ %minutes;
		
    return %hours @ ":" @ %minutes;
}

function GameConnection::PrintEOTWInfo(%client)
{
	if (!isObject(%player = %client.player))
	{
		%client.bottomPrint("<just:center>\c7You died!");
		return;
	}

	%blacklist = "CardsOutImage ChipImage";
	if (isObject(%image = %player.getMountedImage(0)) && hasWord(%blacklist, %image.getName()))
		return;
			
	%health = mCeil(%player.getDatablock().maxDamage - %player.getDamageLevel());
	
	if (%player.getDamagePercent() > 0.75)
		%health = "<color:ff0000>" @ %health;
	else if (%player.getDamagePercent() > 0.25)
		%health = "<color:ffff00>" @ %health;
	else
		%health = "<color:00ff00>" @ %health;
	
	%health = %health @ "<color:ffffff>/" @ %player.getDatablock().maxDamage;
	
	if (isObject(%image = %player.getMountedImage(0)))
	{
		%db = %client.inventory[%client.currInvSlot];

		if (!isObject(%db) && isObject(%player.tempBrick))
			%db = %player.tempBrick.getDatablock();
			
		if (%image.getName() $= "BrickImage" && isObject(%db))
		{
			if (%client.buildMaterial $= "")
				%client.buildMaterial = MatterData.getObject(0).name;

			if ($EOTW::BrickDescription[%db.getName()] !$= "")
			{
				%centerText = "<br><br><br><br>\c6" @ $EOTW::BrickDescription[%db.getName()];
			}
			
			if ($EOTW::CustomBrickCost[%db.getName()] !$= "")
			{
				%cost = $EOTW::CustomBrickCost[%db.getName()];
				
				%brickText = "<br>";
				
				if (getField(%cost, 0) < 1.0)
					%brickText = %brickText @ "(" @ ((1.0 - getField(%cost, 0)) * 100) @ "% Refund Fee!)";
					
				for (%i = 2; %i < getFieldCount(%cost); %i += 2)
				{
					%volume = getField(%cost, %i);
					%matter = getMatterType(getField(%cost, %i + 1));
					%name = %matter.name;
					%color = "<color:" @ %matter.color @ ">";
					%inv = $EOTW::Material[%client.bl_id, %name] + 0;
					if (%inv < %volume) %inv = "\c0" @ %inv;
				
					%brickText = %brickText SPC %inv @ "\c6/" @ %volume SPC %color @ %name @ "\c6,";
				}
				
				%brickText = getSubStr(%brickText, 0, strLen(%brickText) - 1);
			}
			else
			{
				%matter = getMatterType(%client.buildMaterial);
				%name = %matter.name;
				%color = "<color:" @ %matter.color @ ">";
				%inv = $EOTW::Material[%client.bl_id, %name] + 0;
			
				%volume = %db.brickSizeX * %db.brickSizeY * %db.brickSizeZ;
				if (%inv < %volume) %inv = "\c0" @ %inv;
				
				%brickText = "<br>" @ %color @ %name @ "\c6: " @ %inv @ "\c6/" @ %volume SPC "[" @ %db.getName() @ "]";
			}
		}
		else if (%image.printPlayerBattery)
			%brickText = "<br>" @ %player.GetBatteryText();
	}
	
	if (%centerText !$= "")
		%client.centerPrint(%centerText, 1);

	%client.bottomPrint("<just:center>\c3Time\c6:" SPC GetTimeStamp() SPC "| \c3Health\c6:" SPC %health @ %brickText,3);
}

function GetRandomSpawnLocation(%initPos, %failCount)
{
	
	if (%initPos !$= "")
		%eye = (getField(%initPos,0) + (getRandom() < 0.5 ? getRandom(32, 64) : getRandom(-64, -32)) / 2) SPC (getField(%initPos,1) + (getRandom() < 0.5 ? getRandom(32, 64) : getRandom(-64, -32)) / 2) SPC 495; //getRandom(0, 1664)
	else
		%eye = (getRandom(getWord($EOTW::WorldBounds, 0), getWord($EOTW::WorldBounds, 2)) / 1) SPC (getRandom(getWord($EOTW::WorldBounds, 1), getWord($EOTW::WorldBounds, 3)) / 1) SPC 495;
	%dir = "0 0 -1";
	%for = "0 1 0";
	%face = getWords(vectorScale(getWords(%for, 0, 1), vectorLen(getWords(%dir, 0, 1))), 0, 1) SPC getWord(%dir, 2);
	%mask = $Typemasks::fxBrickAlwaysObjectType | $Typemasks::TerrainObjectType;
	%ray = containerRaycast(%eye, vectorAdd(%eye, vectorScale(%face, 500)), %mask, %this);
	
	if (getWord(%ray,3) < $EOTW::LavaHeight && %failCount < 500)
		return GetRandomSpawnLocation(%initPos, %failCount + 1); //Try again lol
		
	%pos = getWord(%ray,1) SPC getWord(%ray,2) SPC (getWord(%ray,3) + 0.1);
	if(isObject(%hit = firstWord(%ray)))
	{
		%pos = vectorAdd(%pos,"0 0 5");
		return %pos;
	}
}

function Player::LookingAtBrick(%obj, %brick)
{
	%eye = %obj.getEyePoint();
	%dir = %obj.getEyeVector();
	%for = %obj.getForwardVector();
	%face = getWords(vectorScale(getWords(%for, 0, 1), vectorLen(getWords(%dir, 0, 1))), 0, 1) SPC getWord(%dir, 2);
	%mask = $Typemasks::fxBrickAlwaysObjectType | $Typemasks::TerrainObjectType;
	%ray = containerRaycast(%eye, vectorAdd(%eye, vectorScale(%face, 5)), %mask, %obj);
	if(isObject(%hit = firstWord(%ray)) && %hit.getID() $= %brick.getID())
		return true;

	return false;
}

function Player::GetMatterCount(%obj, %matter)
{
	if (!isObject(%client = %obj.client))
		return;

	return $EOTW::Material[%client.bl_id, %matter] + 0;
}

function Player::ChangeMatterCount(%obj, %matter, %count)
{
	if (!isObject(%client = %obj.client))
		return;

	$EOTW::Material[%client.bl_id, %matter] += %count;

	if ($EOTW::Material[%client.bl_id, %matter] < 0)
		$EOTW::Material[%client.bl_id, %matter] = 0;

	return $EOTW::Material[%client.bl_id, %matter];
}

package EOTW_Player
{
	function Armor::onTrigger(%data, %obj, %trig, %tog)
	{
		if(isObject(%client = %obj.client))
		{
			if(%trig == 0 && %tog && !isObject(%obj.getMountedImage(0)))
			{
				%eye = %obj.getEyePoint();
				%dir = %obj.getEyeVector();
				%for = %obj.getForwardVector();
				%face = getWords(vectorScale(getWords(%for, 0, 1), vectorLen(getWords(%dir, 0, 1))), 0, 1) SPC getWord(%dir, 2);
				%mask = $Typemasks::fxBrickAlwaysObjectType | $Typemasks::TerrainObjectType;
				%ray = containerRaycast(%eye, vectorAdd(%eye, vectorScale(%face, 5)), %mask, %obj);
				if(isObject(%hit = firstWord(%ray)) && %hit.getClassName() $= "fxDtsBrick")
				{
					%data = %hit.getDatablock();
					if (%data.inspectFunc !$= "")
					{
						cancel(%obj.PoweredBlockInspectLoop);
						%obj.PoweredBlockInspectLoop = %obj.schedule(2000 / $EOTW::PowerTickRate, %data.inspectFunc, %hit);
					}
				}
			}
		}
		Parent::onTrigger(%data, %obj, %trig, %tog);
	}
};
activatePackage("EOTW_Player");

exec("./Player_SolarApoc.cs");
exec("./Support_MultipleSlots.cs");
exec("./Support_PlayerBattery.cs");