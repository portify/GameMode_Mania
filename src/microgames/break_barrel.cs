if (!isObject(MicroGame_BreakBarrel))
{
	new ScriptObject(MicroGame_BreakBarrel)
	{
		class = ManiaMicroGame;
	};
}

function MicroGame_BreakBarrel::onStart(%this, %obj, %game)
{
	%obj.all = 0;//getRandom(0, 1);
	%text = "<color:FFFFAA>BREAK" SPC (%obj.all ? "ALL THE BARRELS!" : "A BARREL!");
	%game.displayText(%text, 4);
	%game.miniGame.play2D(ManiaPanicMusic);

	%image = SwordImage;
	for (%i = 0; %i < %game.miniGame.numMembers; %i++)
	{
		%client = %game.miniGame.member[%i];

		if (%client.isAlive())
		{
			%client.player.mountImage(%image, 0);
			fixArmReady(%client.player);
		}
	}

	%calc = %obj.all ? %game.miniGame.numMembers : mCeil(%game.miniGame.numMembers / 1.5);
	for (%i = 0; %i < %calc; %i++)
	{
		%barrel = new WheeledVehicle()
		{
			dataBlock = barrelOldBreakVehicle;
			position = vectorAdd(%game.miniGame.pickSpawnPoint(), "0 0" SPC getRandom(5, 10));
		};
		if (!isObject(BarrelGroup))
			MissionCleanup.add(new SimGroup(BarrelGroup));
		barrelGroup.add(%barrel);
		%barrel.setNodeColor("ALL", "0.4 0.3 0 1");
	}

	%game.endMicroGame = %game.schedule(4000, endMicroGame);
}

function MicroGame_BreakBarrel::onEnd(%this, %obj, %game)
{
	for (%i = 0; %i < %game.miniGame.numMembers; %i++)
	{
		%client = %game.miniGame.member[%i];

		if (%client.isAlive())
		{
			%client.player.unMountImage(0);
			fixArmReady(%client.player);
		}

		if(barrelGroup.getCount() <= 0 && %obj.all == 1)
		{
			%client.setManiaWin(1);
		}
	}
	barrelGroup.deleteAll();
}


package MicroGame_BreakBarrel
{
	function Player::damage(%this, %source, %pos, %damage, %damagetype)
	{
		%microGame = %source.client.miniGame.maniaGame.microGame;
		%type = %microGame.type;

		if (!isObject(%type) || %type != nameToID("MicroGame_BreakBarrel"))
		{
			return Parent::damage(%this, %source, %pos, %damage, %damageType);
		}

		if (%source.client == %this.client)
		{
			return Parent::damage(%this, %source, %pos, %damage, %damageType);
		}
	}

	function barrelOldBreakVehicle::damage(%this, %obj, %source, %pos, %damage, %damagetype)
	{
		%microGame = %source.client.miniGame.maniaGame.microGame;
		%type = %microGame.type;
		if (!isObject(%type) || %type != nameToID("MicroGame_BreakBarrel"))
		{
			return Parent::damage(%this, %obj, %source, %pos, %damage, %damageType);
		}

		if(isObject(%source.client) && %obj.getDamageLevel() + 50 >= %this.maxDamage)
		{
			if(%microGame.all == 0)
				%source.client.setManiaWin(1);
		}
		Parent::damage(%this, %obj, %source, %pos, 100, %damageType);
	}
};

activatePackage("MicroGame_BreakBarrel");