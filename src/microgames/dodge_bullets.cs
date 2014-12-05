if (!isObject(MicroGame_Move))
{
	new ScriptObject(MicroGame_Move)
	{
		class = ManiaMicroGame;
		winByDefault = 1;
	};
}

//wip as fuck :(
function MicroGame_Move::onStart(%this, %obj, %game)
{
	%text = "DODGE THE BULLETS!";

	%game.displayText("<color:FFFFAA>" @ %text, 4);
	%game.miniGame.play2D(ManiaPanicMusic);

	%spawn = "_spawn_" @ %game.arena;
	for (%i = 0; %i < BrickGroup_888888.NTObjectCount[%name]; %i++)
	{
		%projectile = new Projectile()
		{
			dataBlock = barrelOldBreakVehicle;
			position = vectorAdd(%game.miniGame.pickSpawnPoint(), "0 0 5");
		};
		if (!isObject(BarrelGroup))
			MissionCleanup.add(new SimGroup(BarrelGroup));
		barrelGroup.add(%barrel);
		%barrel.setNodeColor("ALL", "0.4 0.3 0 1");
	}
	%game.endMicroGame = %game.schedule(4000, endMicroGame);
}

// function MicroGame_Move::onEnd(%this, %obj, %game)
// {

// }