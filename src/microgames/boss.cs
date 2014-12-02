if (!isObject(MicroGame_Boss))
{
	new ScriptObject(MicroGame_Boss)
	{
		class = ManiaMicroGame;
		boss = 1;

		lastManStanding = 1;
		winByDefault = 1;
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

function MicroGame_Boss::onStart(%this, %obj, %game)
{
	%game.setArena("western_shootout");
	%game.miniGame.setMusic(ManiaBossWestMusic);
	for (%i = 0; %i < %game.miniGame.numMembers; %i++)
	{
		%client = %game.miniGame.member[%i];

		if (%client.isAlive())
		{
			%client.player.mountImage(GunImage, 0);
			fixArmReady(%client.player);
		}
	}
}

function MicroGame_Boss::onEnd(%this, %obj, %game)
{
	%game.setArena("main");
	%game.miniGame.setMusic();
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

package MicroGame_Boss
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

activatePackage("MicroGame_Boss");