if (!isObject(MicroGame_HitTargets))
{
	new ScriptObject(MicroGame_HitTargets)
	{
		class = ManiaMicroGame;
	};
}

datablock staticShapeData(MicrogameTarget)
{
	shapeFile = "Add-Ons/Gamemode_Mania/res/shapes/target.dts";
};

function MicroGame_HitTargets::onStart(%this, %obj, %game)
{
	%obj.variation = getRandom(0, 1);
	%game.displayText("<color:FFFFAA>" @ (%obj.variation ? "BREAK A TARGET!": "HIT A TARGET!"), 4);
	%game.miniGame.play2D(ManiaWesternMusic);

	%choice = getRandom(0, 1);

	switch (%choice)
	{
		case 0: %image = BowImage;
		case 1: %image = GunImage;
	}

	for (%i = 0; %i < %game.miniGame.numMembers; %i++)
	{
		%client = %game.miniGame.member[%i];

		if (%client.isAlive())
		{
			%client.player.mountImage(%image, 0);
			fixArmReady(%client.player);
		}
	}

	%calc = %obj.variation ? mCeil(%game.miniGame.numMembers * 1.5) : mCeil(%game.miniGame.numMembers / 3);
	for (%i = 0; %i < %calc; %i++)
	{
		%target = new staticShape()
		{
			dataBlock = MicrogameTarget;
			position = vectorAdd(%game.miniGame.pickSpawnPoint(), "0 0" SPC getRandom(5, 10));
		};
		if (!isObject(targetGroup))
			MissionCleanup.add(new SimGroup(targetGroup));
		targetGroup.add(%target);
		%target.setNodeColor("ALL", "0.4 0.3 0 1");
	}

	%game.endMicroGame = %game.schedule(4000, endMicroGame);
}

function MicroGame_HitTargets::onEnd(%this, %obj, %game)
{
	for (%i = 0; %i < %game.miniGame.numMembers; %i++)
	{
		%client = %game.miniGame.member[%i];

		if (%client.isAlive())
		{
			%client.player.unMountImage(0);
			fixArmReady(%client.player);
		}

		if(targetGroup.getCount() <= 0 && %obj.all == 1)
		{
			%client.setManiaWin(1);
		}
	}
	targetGroup.deleteAll();
}

package MicroGame_HitTargets
{
	function Player::damage(%this, %source, %pos, %damage, %damagetype)
	{
		%microGame = %source.client.miniGame.maniaGame.microGame;
		%type = %microGame.type;

		if (!isObject(%type) || %type != nameToID("MicroGame_HitTargets"))
		{
			return Parent::damage(%this, %source, %pos, %damage, %damageType);
		}

		if (%source.client == %this.client)
		{
			return Parent::damage(%this, %source, %pos, %damage, %damageType);
		}
	}

	function projectileData::onCollision(%this, %obj, %col, %fade, %pos, %normal, %a, %b)
	{
		parent::onCollision(%this, %obj, %col, %fade, %pos, %normal, %a, %b);
		%microGame = %obj.sourceObject.client.miniGame.maniaGame.microGame;
		%type = %microGame.type;
		if (!isObject(%type) || %type != nameToID("MicroGame_HitTargets"))
		{
			return;
		}
		if(%col.getType() & $typeMasks::StaticObjectType && %col.getdatablock() == nameToID(MicrogameTarget))
		{
			%obj.sourceObject.client.setManiaWin(1);
			if(%microGame.variation == 1)
			{
				%p = new Projectile()
				{
					dataBlock = adminWandProjectile;
					initialPosition  = %col.getWorldBoxCenter();
					initialVelocity = "0 0 0";
				};
				%p.setScale("2 2 2");
				%p.explode();
				%col.delete();
			}
		}
	}

	// function targetOldBreakVehicle::damage(%this, %obj, %source, %pos, %damage, %damagetype)
	// {
	// 	%microGame = %source.client.miniGame.maniaGame.microGame;
	// 	%type = %microGame.type;
	// 	if (!isObject(%type) || %type != nameToID("MicroGame_HitTargets"))
	// 	{
	// 		return Parent::damage(%this, %obj, %source, %pos, %damage, %damageType);
	// 	}

	// 	if(isObject(%source.client) && %obj.getDamageLevel() + 50 >= %this.maxDamage)
	// 	{
	// 		if(%microGame.all == 0)
	// 			%source.client.setManiaWin(1);
	// 	}
	// 	Parent::damage(%this, %obj, %source, %pos, 100, %damageType);
	// }
};

activatePackage("MicroGame_HitTargets");