if (!isObject(MicroGame_Math))
{
	new ScriptObject(MicroGame_Math)
	{
		class = ManiaMicroGame;
	};
}

function MicroGame_Math::onStart(%this, %obj, %game)
{
	%obj.a = getRandom(1, 41);

	if (%obj.a == 41)
	{
		%obj.a = 1337;
	}

	if (getRandom() < 0.3)
	{
		%obj.b = %obj.a + getRandom(-2, 2);
	}
	else
	{
		%obj.b = getRandom(1, 10);
	}

	if (getRandom() < 0.4)
	{
		%obj.method = "-";
		%obj.result = %obj.a - %obj.b;
	}
	else
	{
		%obj.method = "+";
		%obj.result = %obj.a + %obj.b;
	}

	%text = %obj.a SPC %obj.method SPC %obj.b SPC "= ?";

	%game.miniGame.centerPrintAll("<font:palatino linotype:64><color:FFFFAA>" @ %text, 4);
	%game.miniGame.play2D(ManiaWeirdMusic);

	%game.endMicroGame = %game.schedule(4000, endMicroGame);
}

function MicroGame_Math::onEnd(%this, %obj, %game)
{
	%game.miniGame.chatmsgAll("<color:AAAAFF>The correct answer was:" SPC %obj.result);
}

function MicroGame_Math::onChat(%this, %obj, %game, %client, %text)
{
	if (%text !$= mFloor(%text) || %client.maniaWinStatus !$= "")
	{
		return 0;
	}

	%client.setManiaWin(%text $= %obj.result);
	return %text $= %obj.result;
}