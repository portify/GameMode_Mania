function ManiaGame(%miniGame)
{
	return new ScriptObject()
	{
		class = ManiaGame;
		miniGame = %miniGame;
	};
}

function ManiaGame::onAdd(%this)
{
	%this.arena = "main";
	%this.microGameCount = 0;
	%this.lastSpeedUp = 0;

	%this.setSpeedUp(0);
	%this.schedule(0, doIntro, 0);

	%this.miniGame.play2D(ManiaIntroMusic);
}

function ManiaGame::onRemove(%this)
{
	if (isObject(%this.microGame))
	{
		%this.microGame.delete();
	}

	%this.setSpeedUp(0);
}

function ManiaGame::end(%this)
{
	%this.setSpeedUp(0);
    %this.miniGame.respawnDeadPlayers();

	%winner = %this.getLeader();

	if (isObject(%winner))
	{
		%text = "<color:FFFFAA>" @ %winner.getPlayerName() NL "<color:AAAAFF>WON!";
	}
	else
	{
		%text = "<color:AAAAFF>NOBODY WON!";
	}

	%this.displayText(%text, 5);

	%this.miniGame.play2D(ManiaGameEndMusic);
	%this.miniGame.scheduleReset();

	%image = AkimboGunImage;
	%winner.player.mountImage(%image, 0);
	%winner.player.fixArmReady(%client.player);
}

function ManiaGame::doIntro(%this, %index)
{
	cancel(%this.doIntro);

	%text0 = "Welcome to Mania";
	%text1 = "Follow the on-screen instructions to win";
	%text2 = "The game will become gradually faster";
	%text3 = "Normal minigames give 1 points\n\c6Boss games give 4!";
	%text4 = "After the boss game, the player with the most points wins!";

	if (%index < 0 || %index > 4)
	{
		%this.startMicroGame();
		return;
	}

	%this.miniGame.bottomPrintAll("<just:center><font:palatino linotype:28>\c6" @ %text[%index] @ "\n", 4, 1);
	%this.doIntro = %this.schedule(2400, doIntro, %index + 1);
}

function ManiaGame::startMicroGame(%this)
{
	cancel(%this.startMicroGame);

	if (!isObject(ManiaMicroGameGroup) || isObject(%this.microGame))
		return;

	if (%this.microGameCount > $MicrogameMania::MaxMicrogames)
	{
		%this.end();
		return;
	}
	else
        %boss = %this.microGameCount == $MicrogameMania::MaxMicrogames; 

	%speedUp = mFloor(%this.microGameCount / 3);
	if (%this.setSpeedUp(%speedUp))
	{
		%this.startMicroGame = %this.schedule(4000, startMicroGame);
		return;
	}

	%this.microGameCount++;

	%this.miniGame.resetManiaWins();
	%this.miniGame.respawnDeadPlayers();

	%type = ManiaMicroGameGroup.getMicroGame(%boss);

	if (!isObject(%type))
		return;

	%this.microGame = new ScriptObject()
	{
		class = ManiaMicroGameInstance;

		game = %this;
		type = %type;
	};

    for (%i = 0; %i < %this.miniGame.numMembers; %i++)
        %this.miniGame.member[%i].winHint = "";

	if(%boss)
	{
		setTimeScale(1);
		commandToAll('TimeScale', 1);
		%this.minigame.play2d(nameToID("ManiaGameBossMusic"));
		%this.displayText("<color:AAAAFF>BOSS STAGE!", 4);
		%this.miniGame.schedule(4000, play2D, nameToID("ManiaGameStart" @ getRandom(1, 2) @ "Music"));
		%this.miniGame.schedule(6000, respawnDeadPlayers);
		%this.microGame.schedule(6000, call, "onStart");
	}
	else
	{
		%this.miniGame.play2D(nameToID("ManiaGameStart" @ getRandom(1, 2) @ "Music"));

		%this.miniGame.schedule(2000, respawnDeadPlayers);
		%this.microGame.schedule(2000, call, "onStart");
	}
	%leader = %this.getLeader();
	%text = "<color:FFFFAA>Microgame<color:AAAAFF>" SPC %this.microGameCount @ "<color:FFFFAA><br><color:FFFFAA>Lead:" SPC %leader.getPlayerName() SPC "<color:AAAAFF>(" @ %leader.score SPC "points)";
	%this.miniGame.bottomPrintAll("<just:center>" @ %text @ "\n", 0, 1);
}

function ManiaGame::endMicroGame(%this)
{
	cancel(%this.endMicroGame);

	if (!isObject(%this.microGame))
	{
		return;
	}

	%this.microGame.call("onEnd");

	%this.miniGame.setManiaWin(
		%this.microGame.type.winByDefault,
		%this.microGame.type.allowDeadWin
	);

	%score = %this.microGame.type.boss ? 4 : 1;

	for (%i = 0; %i < %this.miniGame.numMembers; %i++)
	{
		%client = %this.miniGame.member[%i];

		if (%client.maniaWinState)
		{
			%client.setScore(%client.score + %score);
			%client.play2D(ManiaGameWinMusic);

			%message = "<color:AAFFAA>YOU WIN!";
		}
		else
		{
			%client.play2D(ManiaGameFailMusic);
			%message = "<color:FFAAAA>YOU LOSE!";
		}

        if (%client.winHint !$= "")
            %message = %message @ "\n<font:arial:24>\c6" @ %client.winHint;

		%client.centerPrint("<font:arial bold:40>" @ %message, 2);
	}

	%this.microGame.delete();
	%this.startMicroGame = %this.schedule(2000, startMicroGame);
	%leader = %this.getLeader();
	%text = "<color:FFFFAA>Microgame<color:AAAAFF>" SPC %this.microGameCount @ "<color:FFFFAA><br><color:FFFFAA>Lead:" SPC %leader.getPlayerName() SPC "<color:AAAAFF>(" @ %leader.score SPC "points)";
	%this.miniGame.bottomPrintAll("<just:center>" @ %text @ "\n", 0, 1);
}

function ManiaGame::displayText(%this, %text, %time)
{
	if (%time $= "")
		%time = 4;

	%this.miniGame.centerPrintAll("<font:arial bold:40>" @ %text, %time);
}

function ManiaGame::setSpeedUp(%this, %speedUp)
{
	%speedUp = mClamp(%speedUp, 0, 5);
	%timeScale = 1 + %speedUp * 0.1;
	setTimeScale(%timeScale);
	commandToAll('TimeScale', %timeScale);

	%last = %this.lastSpeedUp;
	%this.lastSpeedUp = %speedUp;

	if (%speedUp > %last)
	{
		%this.displayText("<color:AAAAFF>SPEED UP!", 4);
		%this.miniGame.play2D(ManiaGameSpeedUpMusic);

		return 1;
	}

	return 0;
}

function ManiaGame::setArena(%this, %arena)
{
	%this.arena = %arena;

	for (%i = 0; %i < %this.miniGame.numMembers; %i++)
	{
		%client = %this.miniGame.member[%i];

		if (%client.isAlive())
		{
			%client.player.setTransform(%this.miniGame.pickSpawnPoint());
			%client.player.setVelocity("0 0 0");
		}
	}
}

function ManiaGame::getLeader(%this)
{
	for (%i = 0; %i < %this.miniGame.numMembers; %i++)
	{
		%client = %this.miniGame.member[%i];

		if (%client.score > %record || !isObject(%winner))
		{
			%record = %client.score;
			%winner = %client;
		}
	}

	return %winner;
}

function Player::checkManiaHeight(%this)
{
	cancel(%this.checkManiaHeight);

	if (%this.getState() $= "Dead" || !isObject(%this.client.miniGame.maniaGame))
		return;

	if (%this.client.miniGame.maniaGame.arena $= "airblast")
		%floor = 30;
	else
		%floor = 10;

	if (getWord(%this.position, 2) < %floor)
		%this.kill();
	else
		%this.checkManiaHeight = %this.schedule(100, checkManiaHeight);
}

package ManiaGamePackage
{
	function MiniGameSO::onRemove(%this)
	{
		if (isObject(%this.maniaGame))
		{
			%this.maniaGame.delete();
		}

		Parent::onRemove(%this);
	}

	function MiniGameSO::addMember(%this, %client)
	{
		%restore = %this._noCheckLastManStanding;
		%this._noCheckLastManStanding = 1;

		Parent::addMember(%this, %client);
		%this._noCheckLastManStanding = %restore;

		if (%this.owner == 0 && %this.numMembers && !isObject(%this.maniaGame))
		{
			%this.reset(0);
		}
	}

	function MiniGameSO::removeMember(%this, %client)
	{
		Parent::removeMember(%this, %client);

		if (!%this.numMembers && isObject(%this.maniaGame))
		{
			%this.maniaGame.delete();
		}
	}

	function MiniGameSO::reset(%this, %client)
	{
		if (getSimTime() - %this.lastResetTime < 5000)
		{
			Parent::reset(%this, %client);
			return;
		}

		Parent::reset(%this, %client);

		if (isObject(%this.maniaGame))
		{
			%this.maniaGame.delete();
			%existed = 1;
		}

		if (%this.numMembers && (%existed || %this.owner == 0)) {
			%this.maniaGame = ManiaGame(%this);
		}

		if(isObject(barrelGroup)) barrelGroup.deleteAll();
		if(isObject(targetGroup)) targetGroup.deleteAll();
	}

	function MiniGameSO::checkLastManStanding(%this)
	{
		if (%this._noCheckLastManStanding || isObject(%this.maniaGame))
		{
			if (%this.maniaGame.microGame.type.lastManStanding)
			{
				for (%i = 0; %i < %this.numMembers; %i++)
				{
					%living += %this.member[%i].isAlive();
				}

				if (%living < 2)
				{
					%this.maniaGame.endMicroGame();
				}
			}

			return 0;
		}

		return Parent::checkLastManStanding(%this);
	}

	function MiniGameSO::pickSpawnPoint(%this)
	{
		if (!isObject(%this.maniaGame))
		{
			return Parent::pickSpawnPoint(%this);
		}

		%name = "_spawn_" @ %this.maniaGame.arena;

		if (!BrickGroup_888888.NTObjectCount[%name])
		{
			return Parent::pickSpawnPoint(%this);
		}

		%index = getRandom(0, BrickGroup_888888.NTObjectCount[%name] - 1);
		%brick = BrickGroup_888888.NTObject[%name, %index];

		%point = %brick.getTransform();
		%point = setWord(%point, 2, getWord(%brick.getWorldBox(), 5) + 0.1);
		
		return %point;
	}

	function GameConnection::spawnPlayer(%this)
	{
		Parent::spawnPlayer(%this);

		if (isObject(%this.player) && isObject(%this.miniGame.maniaGame))
		{
			%this.player.schedule(0, checkManiaHeight);
		}
	}

	function serverCmdMessageSent(%client, %text)
	{
		%microGame = %client.miniGame.maniaGame.microGame;

		if (!isObject(%microGame))
		{
			Parent::serverCmdMessageSent(%client, %text);
			return;
		}

		%block = %microGame.call("onChat", %client, %text);

		if (!%block)
		{
			Parent::serverCmdMessageSent(%client, %text);
		}
	}
};

activatePackage("ManiaGamePackage");
