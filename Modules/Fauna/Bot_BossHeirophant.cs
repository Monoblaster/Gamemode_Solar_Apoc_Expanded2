datablock PlayerData(HeirophantHoleBot : UnfleshedHoleBot)
{
	runforce			= 40 * 90;
	maxForwardSpeed		= 6;
	maxBackwardSpeed	= 3;
	maxSideSpeed		= 6;
	maxDamage			= 6000;
	lavaImmune			= true;

	//can have unique types, nazis will attack zombies but nazis will not attack other bots labeled nazi
	hName = "Heirophant";				//cannot contain spaces
	hTickRate = 1500;

	//Searching options
	hSearch	= 1;						//Search for Players
		hSearchRadius = 256;			//in brick units
		hSight = 0;						//Require bot to see player before pursuing
		hStrafe = 1;					//Randomly strafe while following player
	hSearchFOV = 1;						//if enabled disables normal hSearch
		hFOVRadius = 64;				//max 10

	//Attack Options
	hMelee = 1;							//Melee
		hAttackDamage = 35;				//Melee Damage
		hDamageType = "";
	hShoot = 1;
		hWep = "HeirophantBossWeaponImage";
		hShootTimes = 4;				//Number of times the bot will shoot between each tick
		hMaxShootRange = 256;			//The range in which the bot will shoot the player
		hAvoidCloseRange = 0;
			hTooCloseRange = 0;			//in brick units
		isChargeWeapon = 0;				//If weapons should be charged to fire (ie spears)

	//Misc options
	hAvoidObstacles = 0;
	hSuperStacker = 1;
	hSpazJump = 0;						//Makes bot jump when the user their following is higher than them

	hAFKOmeter = 0.0;					//Determines how often the bot will wander or do other idle actions, higher it is the less often he does things
	hIdle = 0;							//Enables use of idle actions, actions which are done when the bot is not doing anything else
		hIdleAnimation = 0;				//Plays random animations/emotes, sit, click, love/hate/etc
		hIdleLookAtOthers = 1;			//Randomly looks at other players/bots when not doing anything else
			hIdleSpam = 0;				//Makes them spam click and spam hammer/spraycan
		hSpasticLook = 1;				//Makes them look around their environment a bit more.
	hEmote = 0;

	hPlayerscale = "1.0 1.0 1.0";		//The size of the bot

	//Total Weight, % Chance to be gibbable on death
	//Note: Extra weight can be added to the loot table weight sum for a chance to drop nothing
	EOTWLootTableData = 0.9 TAB 0.0;
	//Weight, Min Loot * 3, Max Loot * 3, Material Name
	EOTWLootTable[0] = 1.0 TAB 100 TAB 125 TAB "Boss Essence";
};

datablock shapeBaseImageData(HeirophantBossWeaponImage)
{
	shapeFile = "base/data/shapes/empty.dts";
	item = "";
	
	mountPoint = 0;
	offset = "0 0 0";
	rotation = 0;
	
	eyeOffset = "0 0 0";
	eyeRotation = 0;
	
	correctMuzzleVector = true;
	className = "WeaponImage";
	
	melee = false;
	armReady = false;

	doColorShift = false;
	colorShiftColor = "0.0 0.0 0.0 1.0";
	
	stateName[0]					= "Start";
	stateTimeoutValue[0]			= 0.5;
	stateTransitionOnTimeout[0]	 	= "Ready";
	stateSound[0]					= weaponSwitchSound;
	
	stateName[1]					= "Ready";
	stateTransitionOnTriggerDown[1] = "Fire";
	stateAllowImageChange[1]		= true;
	
	stateName[2]					= "Fire";
	stateScript[2]					= "onFire";
	stateTimeoutValue[2]			= 0.1;
	stateAllowImageChange[2]		= false;
	stateTransitionOnTimeout[2]		= "Ready";
};

function HeirophantBossWeaponImage::onFire(%this, %obj, %slot)
{
	if(%obj.getState() $= "DEAD")
		return;
	
	//Epic code
}