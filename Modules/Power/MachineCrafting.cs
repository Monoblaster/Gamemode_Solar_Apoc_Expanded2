//MachineCrafting.cs

function SetupMatterCraftingData()
{
	if (isObject(MatterCraftingData))
	{
		MatterCraftingData.deleteAll();
		MatterCraftingData.delete();
	}

	new SimSet(MatterCraftingData)
	{//Use TABs to seperate material name and amount
		//Matter Rector
		new ScriptObject(MatterCraftType) { type="Matter Reactor";	energycost=100;	input[0]=("Vines" TAB 16);			input[1]=("Moss" TAB 4);		input[2]=("");					output[0]=("Bio Fuel" TAB 20);		};
		new ScriptObject(MatterCraftType) { type="Matter Reactor";	energycost=200;	input[0]=("Hydrogen" TAB 64);		input[1]=("Petroleum" TAB 96);	input[2]=("Fluorine" TAB 32);	output[0]=("Rocket Fuel" TAB 64);	};
		new ScriptObject(MatterCraftType) { type="Matter Reactor";	energycost=200;	input[0]=("Petroleum" TAB 96);		input[1]=("Iron" TAB 128);		input[2]=("Water" TAB 128);		output[0]=("Sulfur" TAB 64);		};
		new ScriptObject(MatterCraftType) { type="Matter Reactor";	energycost=200;	input[0]=("Fluorine" TAB 16);		input[1]=("Sulfur" TAB 64);		input[2]=("");					output[0]=("Dielectrics" TAB 64);	};
		new ScriptObject(MatterCraftType) { type="Matter Reactor";	energycost=200;	input[0]=("Sulfur" TAB 64);			input[1]=("Ethanol" TAB 64);	input[2]=("");					output[0]=("Ethylene" TAB 128);		};
		new ScriptObject(MatterCraftType) { type="Matter Reactor";	energycost=200;	input[0]=("Ethylene" TAB 64);		input[1]=("Oxygen" TAB 64);		input[2]=("");					output[0]=("Plastic" TAB 128);		};
		new ScriptObject(MatterCraftType) { type="Matter Reactor";	energycost=200;	input[0]=("Ethylene" TAB 64);		input[1]=("Fluorine" TAB 64);	input[2]=("");					output[0]=("Teflon" TAB 128);		};
		new ScriptObject(MatterCraftType) { type="Matter Reactor";	energycost=200;	input[0]=("Yellow Cake" TAB 64);	input[1]=("Sulfur" TAB 64);		input[2]=("Fluorine" TAB 64);	output[0]=("Fissile Fuel" TAB 128);	};
		new ScriptObject(MatterCraftType) { type="Matter Reactor";	energycost=200;	input[0]=("Nuclear Waste" TAB 128);	input[1]=("Water" TAB 128);		input[2]=("");					output[0]=("Fissile Fuel" TAB 64);	};
		new ScriptObject(MatterCraftType) { type="Matter Reactor";	energycost=400;	input[0]=("Water" TAB 128);			input[1]=("Sodium" TAB 64);		input[2]=("");					output[0]=("Coolant" TAB 128);		};
		new ScriptObject(MatterCraftType) { type="Matter Reactor";	energycost=800;	input[0]=("Coolant" TAB 128);		input[1]=("Dielectrics" TAB 64);input[2]=("");					output[0]=("Cryostablizer" TAB 128);};
		//Alloy Forge
		new ScriptObject(MatterCraftType) { type="Alloy Forge";		energycost=200;	input[0]=("Iron" TAB 96);			input[1]=("Coal" TAB 32);		output[0]=("Steel" TAB 96);		};
		new ScriptObject(MatterCraftType) { type="Alloy Forge";		energycost=200;	input[0]=("Gold" TAB 96);			input[1]=("Silver" TAB 32);		output[0]=("Electrum" TAB 128);	};
		new ScriptObject(MatterCraftType) { type="Alloy Forge";		energycost=800;	input[0]=("Electrum" TAB 64);		input[1]=("Lithium" TAB 64);	output[0]=("Energium" TAB 128);	};
		new ScriptObject(MatterCraftType) { type="Alloy Forge";		energycost=200;	input[0]=("Gold" TAB 96);			input[1]=("Copper" TAB 32);		output[0]=("Rosium" TAB 128);	};
		new ScriptObject(MatterCraftType) { type="Alloy Forge";		energycost=800;	input[0]=("Rosium" TAB 64);			input[1]=("Ethylene" TAB 64);	output[0]=("Naturum" TAB 128);	};
		new ScriptObject(MatterCraftType) { type="Alloy Forge";		energycost=200;	input[0]=("Sturdium" TAB 40);		input[1]=("Diamond" TAB 8);		output[0]=("Addy Base" TAB 48);	};
		new ScriptObject(MatterCraftType) { type="Alloy Forge";		energycost=800;	input[0]=("Addy Base" TAB 48);		input[1]=("Steel" TAB 96);		output[0]=("Adamantine" TAB 128);	};
		//Refinery
		new ScriptObject(MatterCraftType) { type="Refinery";		energycost=200;	input[0]=("Wood" TAB 128);			output[0]=("Rubber" TAB 16);		};
		new ScriptObject(MatterCraftType) { type="Refinery";		energycost=200;	input[0]=("Water" TAB 96);			output[0]=("Brine" TAB 64);			};
		new ScriptObject(MatterCraftType) { type="Refinery";		energycost=200;	input[0]=("Brine" TAB 64);			output[0]=("Lithium" TAB 32);		};
		new ScriptObject(MatterCraftType) { type="Refinery";		energycost=200;	input[0]=("Lithium" TAB 32);		output[0]=("Tritium" TAB 8);		};
		new ScriptObject(MatterCraftType) { type="Refinery";		energycost=200;	input[0]=("Crude Oil" TAB 128);		output[0]=("Petroleum" TAB 96);		};
		new ScriptObject(MatterCraftType) { type="Refinery";		energycost=200;	input[0]=("Bio Fuel" TAB 40);		output[0]=("Ethanol" TAB 64);		};
		new ScriptObject(MatterCraftType) { type="Refinery";		energycost=200;	input[0]=("Gibs" TAB 40);			output[0]=("Ethanol" TAB 64);		};
		new ScriptObject(MatterCraftType) { type="Refinery";		energycost=200;	input[0]=("Uranium" TAB 64);		output[0]=("Yellow Cake" TAB 64);	};
		new ScriptObject(MatterCraftType) { type="Refinery";		energycost=1600;input[0]=("Nuclear Waste" TAB 512);	output[0]=("Polonium" TAB 16);		};
		//Seperator
		new ScriptObject(MatterCraftType) { type="Seperator";		energycost=200;	input[0]=("Water" TAB 96);			output[0]=("Hydrogen" TAB 64);		output[1]=("Oxygen" TAB 32);	};
		new ScriptObject(MatterCraftType) { type="Seperator";		energycost=400;	input[0]=("Brine" TAB 64);			output[0]=("Sodium" TAB 32);		output[1]=("Deuterium" TAB 16);	};
		//Brewery
		new ScriptObject(MatterCraftType) { type="Brewery";			energycost=100;	input[0]=("Water" TAB 64);		input[1]=("Vines" TAB 32);			input[2]=("Moss" TAB 8);		input[3]=("Oxygen" TAB 32);		output[0]=("Healing Mix" TAB 64);		};
		new ScriptObject(MatterCraftType) { type="Brewery";			energycost=100;	input[0]=("Water" TAB 64);		input[1]=("Rubber" TAB 128);		input[2]=("Ethanol" TAB 64);	input[3]=("Petroleum" TAB 96);	output[0]=("Steroid Mix" TAB 64);		};
		new ScriptObject(MatterCraftType) { type="Brewery";			energycost=100;	input[0]=("Water" TAB 64);		input[1]=("Lithium" TAB 32);		input[2]=("Gibs" TAB 40);		input[3]=("Vines" TAB 64);		output[0]=("Adrenline Mix" TAB 64);		};
		new ScriptObject(MatterCraftType) { type="Brewery";			energycost=100;	input[0]=("Brine" TAB 64);		input[1]=("Gold" TAB 96);			input[2]=("Lead" TAB 96);		input[3]=("Moss" TAB 16);		output[0]=("Gatherer Mix" TAB 64);		};
		new ScriptObject(MatterCraftType) { type="Brewery";			energycost=100;	input[0]=("Brine" TAB 64);		input[1]=("Yellow Cake" TAB 64);	input[2]=("Deuterium" TAB 32);	input[3]=("Lithium" TAB 32);	output[0]=("Overload Mix" TAB 64);		};
		new ScriptObject(MatterCraftType) { type="Brewery";			energycost=100;	input[0]=("Brine" TAB 64);		input[1]=("Leather"	 TAB 64);		input[2]=("Iron" TAB 128);		input[3]=("Sodium" TAB 32);		output[0]=("Leatherskin Mix" TAB 64);	};
	};
}
SetupMatterCraftingData();

$EOTW::CustomBrickCost["brick4x1x5windowData"] = 1.00 TAB "7a7a7aff" TAB 48 TAB "Iron" TAB 16 TAB "Glass";
$EOTW::CustomBrickCost["brickVehicleSpawnData"] = 1.00 TAB "7a7a7aff" TAB 128 TAB "Iron" TAB 64 TAB "Glass";
$EOTW::CustomBrickCost["brickTeledoorData"] = 0.85 TAB "7a7a7aff" TAB 40 TAB "Sturdium" TAB 8 TAB "Diamond" TAB 256 TAB "Iron";

function ServerCmdInput(%client, %slot, %amount, %material, %matB, %matC, %matD) { ServerCmdInsert(%client, %slot, %amount, %material, %matB, %matC, %matD); }
function ServerCmdInsert(%client, %slot, %amount, %material, %matB, %matC, %matD)
{
	if (!isObject(%player = %client.player))
		return;

	%material = trim(%material SPC %matB SPC %matC SPC %matD);

	if (%amount <= 0 || %material $= "" || %slot $= "")
	{
		%client.chatMessage("Usage: /Insert <input/output/buffer> <amount> <material>");
		return;
	}

	%amount = Round(%amount);

	%eye = %player.getEyePoint();
	%dir = %player.getEyeVector();
	%for = %player.getForwardVector();
	%face = getWords(vectorScale(getWords(%for, 0, 1), vectorLen(getWords(%dir, 0, 1))), 0, 1) SPC getWord(%dir, 2);
	%mask = $Typemasks::fxBrickAlwaysObjectType | $Typemasks::TerrainObjectType;
	%ray = containerRaycast(%eye, vectorAdd(%eye, vectorScale(%face, 5)), %mask, %obj);
	if(isObject(%hit = firstWord(%ray)) && %hit.getClassName() $= "fxDtsBrick")
	{
		if (getTrustLevel(%player, %hit) < $TrustLevel::Hammer)
		{
			if (%hit.stackBL_ID $= "" || %hit.stackBL_ID != %client.getBLID())
			{
				%client.chatMessage("The owner of that object does not trust you enough.");
				return;
			}
		}
		%data = %hit.getDatablock();
		if (%data.matterSlots[%slot] > 0)
		{
			if (isObject(%matter = GetMatterType(%material)))
			{
				%change = getMin(%amount, $EOTW::Material[%client.bl_id, %matter.name]);

				%finalChange = %hit.changeMatter(%matter.name, %change, %slot);

				%client.chatMessage("You input " @ %finalChange @ " units of " @ %matter.name @ " into the " @ %slot @ ".");
				$EOTW::Material[%client.bl_id, %matter.name] -= %finalChange;
			}
			else
				%client.chatMessage("Material type " @ %material @ " not found.");
		}
		else
			%client.chatMessage("This block has no compatible \"" @ %slot @ "\" slot.");
	}
}

function ServerCmdOutput(%client, %slot, %amount, %material, %matB, %matC, %matD) { ServerCmdExtract(%client, %slot, %amount, %material, %matB, %matC, %matD); }
function ServerCmdExtract(%client, %slot, %amount, %material, %matB, %matC, %matD)
{
	if (!isObject(%player = %client.player))
		return;

	%material = trim(%material SPC %matB SPC %matC SPC %matD);

	if (%amount <= 0 || %material $= "" || %slot $= "")
	{
		%client.chatMessage("Usage: /Extract <input/output/buffer> <amount> <material>");
		return;
	}

	%amount = Round(%amount);

	%eye = %player.getEyePoint();
	%dir = %player.getEyeVector();
	%for = %player.getForwardVector();
	%face = getWords(vectorScale(getWords(%for, 0, 1), vectorLen(getWords(%dir, 0, 1))), 0, 1) SPC getWord(%dir, 2);
	%mask = $Typemasks::fxBrickAlwaysObjectType | $Typemasks::TerrainObjectType;
	%ray = containerRaycast(%eye, vectorAdd(%eye, vectorScale(%face, 5)), %mask, %obj);
	if(isObject(%hit = firstWord(%ray)) && %hit.getClassName() $= "fxDtsBrick")
	{
		if (getTrustLevel(%player, %hit) < $TrustLevel::Hammer)
		{
			if (%hit.stackBL_ID $= "" || %hit.stackBL_ID != %client.getBLID())
			{
				%client.chatMessage("The owner of that object does not trust you enough.");
				return;
			}
		}
		%data = %hit.getDatablock();
		if (%data.matterSlots[%slot] > 0)
		{
			if (isObject(%matter = GetMatterType(%material)))
			{
				%change = getMin(%amount, %hit.GetMatter(%matter.name, %slot)) * -1;

				%finalChange = %hit.changeMatter(%matter.name, %change, %slot) * -1;

				%client.chatMessage("Extracted " @ %finalChange @ " units of " @ %matter.name @ ".");
				$EOTW::Material[%client.bl_id, %matter.name] += %finalChange;
			}
			else
				%client.chatMessage("Material type " @ %material @ " not found.");
		}
		else
			%client.chatMessage("This brick has no " @ %slot @ " slot.");
	}
}