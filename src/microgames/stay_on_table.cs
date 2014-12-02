if (!isObject(MicroGame_StayOnTable))
{
	new ScriptObject(MicroGame_StayOnTable)
	{
		class = ManiaMicroGame;
		winByDefault = 1;
	};
}

function MicroGame_StayOnTable::onStart(%this, %obj, %game)
{
	%game.setArena("airblast");

	%game.displayText("<color:FFFFAA>STAY ON THE TABLE!", 4);
	%game.miniGame.play2D(ManiaMetalLongMusic);
	for (%i = 0; %i < %game.miniGame.numMembers; %i++)
	{
		%client = %game.miniGame.member[%i];

		if (%client.isAlive())
		{
			%client.player.mountImage(PushBroomImage, 0);
			fixArmReady(%client.player);
		}
	}

	%game.endMicroGame = %game.schedule(8000, endMicroGame);
}

function MicroGame_StayOnTable::onEnd(%this, %obj, %game)
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

	%game.setArena("main");
}