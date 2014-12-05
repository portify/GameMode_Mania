if (!isObject(MicroGame_ObbyBoss))
{
	new ScriptObject(MicroGame_ObbyBoss)
	{
		class = ManiaMicroGame;
		boss = 1;
	};
}

datablock PlayerData(PlayerShootoutArmor : PlayerStandardArmor)
{
	uiName = "";

	firstPersonOnly = 1;
	canJet = 0;

	runForce = 0;
	jumpForce = 0;

	horizMaxSpeed = 0;
};

function MicroGame_ObbyBoss::onStart(%this, %obj, %game)
{
	%game.setArena("obby");
	%game.miniGame.setMusic(ManiaBossChipMusic);
	%game.endMicroGame = %game.schedule(60000, endMicroGame);
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

function MicroGame_ObbyBoss::onEnd(%this, %obj, %game)
{
	%game.setArena("main");
	%game.miniGame.setMusic();
	// for (%i = 0; %i < %game.miniGame.numMembers; %i++)
	// {
	// 	%client = %game.miniGame.member[%i];

	// 	if (%client.isAlive())
	// 	{
	// 		%client.player.unMountImage(0);
	// 		fixArmReady(%client.player);
	// 	}
	// }
}

function MicroGame_ObbyBoss::checkWinners(%this, %obj, %game)
{
	for (%i = 0; %i < %game.miniGame.numMembers; %i++)
	{
		%client = %game.miniGame.member[%i];

		if (!%client.isAlive() || %client.maniaWinState == 1)
		{
			continue;
		}
		%players++;
	}
	if(%players <= 0) %game.endMicroGame();
}

package MicroGame_ObbyBoss
{
	function GameConnection::onDeath(%this, %obj, %client, %type, %area)
	{
		parent::onDeath(%this, %obj, %client, %type, %area);
		%microGame = %this.miniGame.maniaGame.microGame;
		%type = %microGame.type;

		if (!isObject(%type) || %type != nameToID("MicroGame_ObbyBoss"))
		{
			return;
		}
		%microGame.call("checkWinners");
	}
};

activatePackage("MicroGame_ObbyBoss");