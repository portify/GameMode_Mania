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
	%this.gameSchedule();
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

function MicroGame_Octagon_Madness::gameSchedule(%this, %obj, %game, %tick)
{
	cancel(%this.gameSchedule);
	if(%tick > 3 && %tick < 7)
	{
		%game.displayText("<color:FFFFFF>" @ (7 - %tick) @ "!", 3);
	}
	if(%tick == 7)
	{
		%game.displayText("<color:AAFFAA>GO!", 3);
	}
	%this.gameSchedule = schedule(1000, 0, gameSchedule, %this, %obj, %game, %tick);
}

function MicroGame_Octagon_Madness::onEnd(%this, %obj, %game)
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