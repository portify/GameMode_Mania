if (!isObject(MicroGame_HitAnEnemy))
{
	new ScriptObject(MicroGame_HitAnEnemy)
	{
		class = ManiaMicroGame;
	};
}

function MicroGame_HitAnEnemy::onStart(%this, %obj, %game)
{
	%game.displayText("<color:FFFFAA>HIT AN ENEMY!", 4);
	%game.miniGame.play2D(ManiaMetalMusic);

	%choice = getRandom(0, 3);

	switch (%choice)
	{
		case 0: %image = BowImage;
		case 1: %image = GunImage;
		case 2: %image = SpearImage;
		case 3: %image = SwordImage;
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

	%game.endMicroGame = %game.schedule(4000, endMicroGame);
}

function MicroGame_HitAnEnemy::onEnd(%this, %obj, %game)
{
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

package MicroGame_HitAnEnemy
{
	function Player::damage(%this, %source, %pos, %damage, %damagetype)
	{
		%microGame = %source.client.miniGame.maniaGame.microGame;
		%type = %microGame.type;

		if (!isObject(%type) || %type != nameToID("MicroGame_HitAnEnemy"))
		{
			return Parent::damage(%this, %source, %pos, %damage, %damageType);
		}

		if (!%source.client.isAlive() || %source.client == %this.client)
		{
			return Parent::damage(%this, %source, %pos, %damage, %damageType);
		}

		%source.client.setManiaWin(1);
		Parent::damage(%this, %source, %pos, 100, %damageType);
	}
};

activatePackage("MicroGame_HitAnEnemy");