if (!isObject(MicroGame_Octagon_Madness))
{
	new ScriptObject(MicroGame_Octagon_Madness)
	{
		class = ManiaMicroGame;
		boss = 1;

		lastManStanding = 1;
		winByDefault = 1;
	};
}

function MicroGame_Octagon_Madness::onStart(%this, %obj, %game)
{
	%game.displayText("<color:FFFFAA>OCTAGON MADNESS!", 4);
	%game.setArena("hexmadness");
	%game.miniGame.setMusic(ManiaBossChipMusic);
	%obj.delay = 1200;
	%this.gameSchedule(%obj, %game);
	// for (%i = 0; %i < %game.miniGame.numMembers; %i++)
	// {
	// 	%client = %game.miniGame.member[%i];

	// 	if (%client.isAlive())
	// 	{
	// 		%client.player.mountImage(GunImage, 0);
	// 		fixArmReady(%client.player);
	// 	}
	// }
}

function MicroGame_Octagon_Madness::gameSchedule(%this, %obj, %game)
{
	cancel(%this.gameSchedule);
	if(!%obj.started)
	{
		if(%obj.tick > 3 && %obj.tick < 7)
		{
			%game.displayText("<color:FFFFFF>" @ (7 - %obj.tick) @ "!", 3);
		}
		if(%obj.tick == 7)
		{
			%game.displayText("<color:AAFFAA>GO!", 3);
			%obj.started = true;
			%obj.tick = 0;
		}
	}
	else
	{
		if (%obj.tick == 2)
		{
			// pink yellow blue red green purple
			%choice = getRandom(0, 5);
			%obj.cnum = %choice;
			switch (%choice)
			{
				case 0: %color = "RED";
				case 1: %color = "GREEN";
				case 2: %color = "BLUE";
				case 3: %color = "PURPLE";
				case 4: %color = "YELLOW";
				case 5: %color = "PINK";
			}

			if (getProbability(20))
				%choice = getRandom(0, 5); //Randomize the colortext

			switch (%choice)
			{
				case 0: %colortext = "<color:FF0000>";
				case 1: %colortext = "<color:008000>";
				case 2: %colortext = "<color:0000FF>";
				case 3: %colortext = "<color:800080>";
				case 4: %colortext = "<color:FFFF00>";
				case 5: %colortext = "<color:FFB6C1>";
			}
			%game.displayText(%colortext @ %color @ "\c6!", 3);
		}
		if (%obj.tick == 4)
		{
			%obj.cycles++;
			for (%i = 0; %i < 6; %i++)
			{
				if (%i == %obj.cnum)
					continue;

				switch (%i)
				{
					case 0: %color = "RED";
					case 1: %color = "GREEN";
					case 2: %color = "BLUE";
					case 3: %color = "PURPLE";
					case 4: %color = "YELLOW";
					case 5: %color = "PINK";
				}
				%name = "_hexmadness_" @ %color;
				for(%a=0;%a<BrickGroup_888888.NTObjectCount[%name];%a++)
				{
					%brick = BrickGroup_888888.NTObject[%name, %a];
					%brick.disappear(3);
				}
			}
		}
		if (%obj.tick >= 7)
		{
			%obj.tick = 0;
			if (%obj.cycles > 0 && %obj.cycles % 5 == 0)
			{
				%obj.delay = getMax(500, %obj.delay - 100);
				%game.displayText("<color:AAAAFF>FASTER!", 3);
			}
		}
	}
	%obj.tick++;
	%this.gameSchedule = %this.schedule(%obj.delay, gameSchedule, %obj, %game);
}

function MicroGame_Octagon_Madness::onEnd(%this, %obj, %game)
{
	cancel(%this.gameSchedule);
	%game.setArena("main");
	%game.miniGame.setMusic("");
	for (%i = 0; %i < %game.miniGame.numMembers; %i++)
	{
		%client = %game.miniGame.member[%i];

		if (%client.isAlive())
		{
			%client.player.unMountImage(0);
			fixArmReady(%client.player);
		}
	}
}

package MicroGame_Octagon_Madness
{
	function GunImage::onFire(%this, %obj, %slot)
	{
		return Parent::onFire(%this, %obj, %slot);
	}

	function Player::damage(%this, %source, %pos, %damage, %damagetype)
	{
		Parent::damage(%this, %source, %pos, %damage, %damageType);
	}
};

activatePackage("MicroGame_Octagon_Madness");