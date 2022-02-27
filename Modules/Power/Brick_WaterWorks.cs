$EOTW::SteamToWaterRatio = 1; //A higher ratio would be more realistic but it will result in the funny decimal

datablock fxDTSBrickData(brickEOTWWaterPumpData)
{
	brickFile = "./Bricks/WaterPump.blb";
	category = "Solar Apoc";
	subCategory = "Water Works";
	uiName = "Water Pump";
	energyGroup = "Machine";
	energyMaxBuffer = 100;
    energyWattage = 10;
	loopFunc = "EOTW_WaterPumpLoop";
    inspectFunc = "EOTW_WaterPumpInspectLoop";
	//iconName = "./Bricks/Icon_Generator";

    matterMaxBuffer = 128;
	matterSlots["Output"] = 1;
};
$EOTW::CustomBrickCost["brickEOTWWaterPumpData"] = 1.00 TAB "7a7a7aff" TAB 128 TAB "Rubber" TAB 96 TAB "Steel" TAB 96 TAB "Lead";
$EOTW::BrickDescription["brickEOTWWaterPumpData"] = "Uses energy to pump water from deep underground.";

function Player::EOTW_WaterPumpInspectLoop(%player, %brick)
{
	cancel(%player.PoweredBlockInspectLoop);
	
	if (!isObject(%client = %player.client))
		return;

	if (!isObject(%brick) || !%player.LookingAtBrick(%brick))
	{
		%client.centerPrint("", 1);
		return;
	}

	%data = %brick.getDatablock();
	%printText = "<color:ffffff>";

    %printText = %printText @ (%brick.getPower() + 0) @ "/" @ %data.energyMaxBuffer @ " EU\n";
    for (%i = 0; %i < %data.matterSlots["Output"]; %i++)
	{
		%matter = %brick.Matter["Output", %i];

		if (%matter !$= "")
			%printText = %printText @ "Output " @ (%i + 1) @ ": " @ getField(%matter, 1) SPC getField(%matter, 0) @ "\n";
		else
			%printText = %printText @ "Output " @ (%i + 1) @ ": --" @ "\n";
	}

	%client.centerPrint(%printText, 1);
	
	%player.PoweredBlockInspectLoop = %player.schedule(1000 / $EOTW::PowerTickRate, "EOTW_WaterPumpInspectLoop", %brick);
}


function fxDtsBrick::EOTW_WaterPumpLoop(%obj)
{
    %data = %obj.getDatablock();
    %costPerUnit = 20;
	if (%obj.craftingPower >= %costPerUnit)
	{
		%change = %obj.changeMatter("Water", 1, "Output");
        %obj.craftingPower -= %change * %costPerUnit;
	}
    else
    {
        %change = mMin(mCeil(%data.energyWattage / $EOTW::PowerTickRate), %obj.getPower());
        %obj.craftingPower += %change;
        %obj.changePower(%change * -1);
    }
}

datablock fxDTSBrickData(brickEOTWSteamEngineData)
{
	brickFile = "./Bricks/Generator.blb";
	category = "Solar Apoc";
	subCategory = "Water Works";
	uiName = "Water Boiler";
	energyGroup = "Source";
	energyMaxBuffer = 0;
	matterMaxBuffer = 250;
	matterSlots["Input"] = 2;
	matterSlots["Output"] = 1;
	loopFunc = "EOTW_SteamEngineLoop";
    inspectFunc = "EOTW_SteamEngineInspectLoop";
	//iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2/Modules/Power/Icons/SolarPanel";
};
$EOTW::CustomBrickCost["brickEOTWSteamEngineData"] = 1.00 TAB "7a7a7aff" TAB 256 TAB "Steel" TAB 128 TAB "Electrum" TAB 128 TAB "Rosium";
$EOTW::BrickDescription["brickEOTWSteamEngineData"] = "A more advanced and efficent stirling engine that takes inputted water and fuel and creates steam. Use the steam turbine with this.";

function fxDtsBrick::EOTW_SteamEngineLoop(%obj)
{
	%wattage = 100;
	if (%obj.storedFuel > 0)
	{
		%fuelConsumption = getMin(%obj.storedFuel, %wattage / $EOTW::PowerTickRate);
		%waterChange -= %obj.changeMatter("Water", (%fuelConsumption * -1) / $EOTW::SteamToWaterRatio, "Input");
		%steamCreated = %obj.changeMatter("Steam", %waterChange * $EOTW::SteamToWaterRatio, "Output");
		%obj.storedFuel -= getMax(mFloor(%steamCreated * 0.85), 1); //The steam engine has a +15% efficency to power. Get on steam power.
		//Also compesates for the small loss of water when using the steam turbine.
	}	

	if (%obj.storedFuel < 1)
	{
		for (%i = 0; %i < %obj.getDatablock().matterSlots["Input"]; %i++)
		{
			%matterType = getMatterType(getField(%obj.matter["Input", %i], 0));
			if (isObject(%matterType) && %matterType.fuelCapacity > 0)
			{
				%obj.storedFuel += mFloor(%obj.changeMatter(%matterType.name, -32, "Input") * %matterType.fuelCapacity * -1);
				break;
			}
		}
	}
}

function Player::EOTW_SteamEngineInspectLoop(%player, %brick)
{
	cancel(%player.PoweredBlockInspectLoop);
	
	if (!isObject(%client = %player.client))
		return;

	if (!isObject(%brick) || !%player.LookingAtBrick(%brick))
	{
		%client.centerPrint("", 1);
		return;
	}

	%data = %brick.getDatablock();
	%printText = "<color:ffffff>";
    for (%i = 0; %i < %data.matterSlots["Input"]; %i++)
	{
		%matter = %brick.Matter["Input", %i];

		if (%matter !$= "")
			%printText = %printText @ "Input " @ (%i + 1) @ ": " @ getField(%matter, 1) SPC getField(%matter, 0) @ "\n";
		else
			%printText = %printText @ "Input " @ (%i + 1) @ ": --" @ "\n";
	}
	%printText = %printText @ (%brick.storedFuel + 0) @ "u of Unburned Fuel<br>";

	 for (%i = 0; %i < %data.matterSlots["Output"]; %i++)
	{
		%matter = %brick.Matter["Output", %i];

		if (%matter !$= "")
			%printText = %printText @ "Output " @ (%i + 1) @ ": " @ getField(%matter, 1) SPC getField(%matter, 0) @ "\n";
		else
			%printText = %printText @ "Output " @ (%i + 1) @ ": --" @ "\n";
	}

	%client.centerPrint(%printText, 1);
	
	%player.PoweredBlockInspectLoop = %player.schedule(1000 / $EOTW::PowerTickRate, "EOTW_SteamEngineInspectLoop", %brick);
}

datablock fxDTSBrickData(brickEOTWThermoelectricBoilerData)
{
	brickFile = "./Bricks/Generator.blb";
	category = "Solar Apoc";
	subCategory = "Water Works";
	uiName = "Heat Exchanger";
	energyGroup = "Source";
	energyMaxBuffer = 400;
	loopFunc = "EOTW_ThermoelectricBoilerLoop";
	inspectFunc = "EOTW_DefaultInspectLoop";
	matterMaxBuffer = 256;
	matterSlots["Input"] = 2;
	matterSlots["Output"] = 2;
	//iconName = "./Bricks/Icon_Generator";
};
$EOTW::CustomBrickCost["brickEOTWThermoelectricBoilerData"] = 1.00 TAB "7a7a7aff" TAB 1 TAB "Infinity";
$EOTW::BrickDescription["brickEOTWThermoelectricBoilerData"] = "[[(WIP)]] Uses hot coolant or hot cryostablizer to heat water into steam. Use power for slight burn boost.";

function fxDtsBrick::EOTW_ThermoelectricBoilerLoop(%obj)
{

}