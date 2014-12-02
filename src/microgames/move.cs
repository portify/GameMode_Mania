if (!isObject(MicroGame_Move))
{
	new ScriptObject(MicroGame_Move)
	{
		class = ManiaMicroGame;
		winByDefault = 1;
	};
}

function MicroGame_Move::onStart(%this, %obj, %game)
{
	%obj.flip = getRandom(0, 1);
	%text = (%obj.flip ? "DON'T MOVE!" : "KEEP MOVING!");

	%game.displayText("<color:FFFFAA>" @ %text, 4);
	%game.miniGame.play2D(ManiaPanicMusic);

	for (%i = 0; %i < %game.miniGame.numMembers; %i++)
	{
		%client = %game.miniGame.member[%i];

		if (%client.isAlive())
		{
			%client.player.schedule(1600, moveMicroGameSchedule);
		}
	}

	%game.endMicroGame = %game.schedule(4000, endMicroGame);
}

function Player::moveMicroGameSchedule(%this)
{
	cancel(%this.moveMicroGameSchedule);

	%microGame = %this.client.miniGame.maniaGame.microGame;
	%type = %microGame.type;

	if (!isObject(%type) || %type != nameToID("MicroGame_Move"))
	{
		return;
	}

	if (%this.getState() $= "Dead" || %this.client.maniaWinState !$= "")
	{
		return;
	}

	%moving = vectorLen(setWord(%this.getVelocity(), 2, 0)) > 0.1;

	if (%moving == %microGame.flip)
	{
		%this.kill();
		return;
	}

	%this.moveMicroGameSchedule = %this.schedule(50, moveMicroGameSchedule);
}
