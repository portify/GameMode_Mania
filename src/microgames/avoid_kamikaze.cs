if (!isObject(MicroGame_AvoidKamikaze))
{
	new ScriptObject(MicroGame_AvoidKamikaze)
	{
		class = ManiaMicroGame;
	};
}
function MicroGame_AvoidKamikaze::onStart(%this, %obj, %game)
{
	%game.displayText("<color:FFFFAA>AVOID THE KAMIKAZE!", 4);
	%game.miniGame.play2D(ManiaPanicMusic);

	for (%i = 0; %i < %game.miniGame.numMembers; %i++)
	{
		%client = %game.miniGame.member[%i];

		if (%client.isAlive())
		{
			%players = %players @ (%players $= "" ? "" : " ") @ %client.player;
		}
	}

	%index = getRandom(0, getWordCount(%players) - 1);

	%obj.Kamikaze = getWord(%players, %index);
	%obj.Kamikaze.setPlayerScale("1 1 1.5");
    %obj.Kamikaze.client.centerPrint("<font:arial bold:40><color:FFFFAA>BLOW UP THE OTHERS!", 4);
    %obj.Kamikaze.blowUpSchedule();
	%game.endMicroGame = %game.schedule(4000, endMicroGame);
}

datablock explosionData(kamikazeBoomExplosion : rocketExplosion)
{
	damageRadius = 10;
	radiusDamage = 1000;

	impulseRadius = 0;
	impulseForce = 0;
};

datablock projectileData(kamikazeBoomProjectile : rocketLauncherProjectile)
{
	explosion = kamikazeBoomExplosion;
	uiName = "";
};

function Player::blowUpSchedule(%this, %times)
{
	cancel(%this.blowUpSchedule);
	if(%times > 2)
	{
		%p = new Projectile()
		{
			dataBlock = kamikazeBoomProjectile;
			initialPosition  = %this.getPosition();
			initialVelocity = "0 0 0";
			client = %this.client;
			sourceObject = %this;
		};
		// %p.setScale("1.5 1.5 1.5");
		%p.explode();
		// MissionCleanup.Add(%p);
		%this.kill();
		return;
	}
	%this.spawnExplosion(PongProjectile, 1);
	%times++;
	%this.blowUpSchedule = %this.schedule(1000, blowUpSchedule, %times);
}

function MicroGame_AvoidKamikaze::onEnd(%this, %obj, %game)
{
	if (isObject(%obj.Kamikaze))
	{
		%obj.Kamikaze.setPlayerScale(1);
	}

	for (%i = 0; %i < %game.miniGame.numMembers; %i++)
	{
		%client = %game.miniGame.member[%i];

		if (%client.isAlive() && !%client.player.kamikaze)
		{
			%client.setManiaWin(1);
		}
	}
	// if (isObject(%obj.Kamikaze))
	// {
	// 	// echo(%obj.Kamikaze.client.kills);
	// 	// %obj.Kamikaze.client.setManiaWin(%obj.Kamikaze.client.kills >= mFloor(%game.miniGame.numMembers / 3));
	// }
}


package MicroGame_AvoidKamikaze
{
	function Player::damage(%this, %source, %pos, %damage, %damagetype)
	{
		%microGame = %source.client.miniGame.maniaGame.microGame;
		%type = %microGame.type;
		if (!isObject(%type) || %type != nameToID("MicroGame_AvoidKamikaze"))
		{
			return Parent::damage(%this, %source, %pos, %damage, %damageType);
		}
		%source.client.setManiaWin(1);
		Parent::damage(%this, %source, %pos, 100, %damageType);
	}
};
activatePackage("MicroGame_AvoidKamikaze");