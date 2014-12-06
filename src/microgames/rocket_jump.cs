if (!isObject(MicroGame_RocketJump))
{
	new ScriptObject(MicroGame_RocketJump)
	{
		class = ManiaMicroGame;
		winByDefault = 1;
	};
}

function MicroGame_RocketJump::onStart(%this, %obj, %game)
{
	%choice = getRandom(0, 1);
	switch (%choice)
	{
		case 0: %image = RocketLauncherImage;
		case 1: %image = SpearImage;
	}
	%type = %choice ? "SPEAR" : "ROCKET";
	%obj.flip = getRandom() < 0.4;
	%text = %obj.flip ? "STAY ON THE GROUND" : %type SPC "JUMP";

	%game.displayText("<color:FFFFAA>" @ %text @ "!", 4);
	%game.miniGame.play2D(%choice ? ManiaPipesMusic : ManiaScotsMusic);

	for (%i = 0; %i < %game.miniGame.numMembers; %i++)
	{
		%client = %game.miniGame.member[%i];

		if (%client.isAlive())
		{
			%client.player.mountImage(%image, 0);
			fixArmReady(%client.player);
		}
	}

	%game.endMicroGame = %game.schedule(4000, endMicroGame);
}

function MicroGame_RocketJump::onEnd(%this, %obj, %game)
{
	for (%i = 0; %i < %game.miniGame.numMembers; %i++)
	{
		%client = %game.miniGame.member[%i];

		if (%client.isAlive())
		{
			%ray = containerRayCast(
				%client.player.position,
				vectorSub(%client.player.position, "0 0 5"),
				$TypeMasks::FxBrickObjectType
			);

			if (%obj.flip ? !%ray : %ray)
			{
				%client.player.spawnExplosion(RocketLauncherProjectile, 0.5);
				%client.player.kill();
			}
			else
			{
				%client.player.unMountImage(0);
				fixArmReady(%client.player);
			}
		}
	}
}

package MicroGame_RocketJump
{
	function Player::damage(%this, %source, %pos, %damage, %damagetype)
	{
		%microGame = %source.client.miniGame.maniaGame.microGame;
		%type = %microGame.type;

		if (!isObject(%type) || %type != nameToID("MicroGame_RocketJump"))
		{
			return Parent::damage(%this, %source, %pos, %damage, %damageType);
		}

		if (!%source.client.isAlive())
		{
			return Parent::damage(%this, %source, %pos, %damage, %damageType);
		}

		if(%damageType == $DamageType::Fall || %damageType == $DamageType::Impact || %damageType == $DamageType::Suicide)
		{
			return Parent::damage(%this, %source, %pos, %damage, %damageType);
		}
	}
};

activatePackage("MicroGame_RocketJump");