if (!isObject(MicroGame_SimonSays))
{
	new ScriptObject(MicroGame_SimonSays)
	{
		class = ManiaMicroGame;
	};
}
 
function MicroGame_SimonSays::onStart(%this, %obj, %game)
{
	%obj.who = getRandom(0, 1);
	%this.winByDefault = !%obj.who;

	%choice = getRandom(0, 9);

	switch (%choice)
	{
		case 0: %obj.action = "jump";
		case 1: %obj.action = "crouch";
		case 2: %obj.action = "jet";
		case 3: %obj.action = "click";
		case 4: %obj.action = "/sit";
		case 5: %obj.action = "/hug";
		case 6: %obj.action = "/love";
		case 7: %obj.action = "/hate";
		case 8: %obj.action = "/wtf";
		case 9: %obj.action = "/alarm";
		default: %obj.action = "Action #" @ %choice;
	}

	%text = (%obj.who ? "SIMON" : "SOMEONE") SPC "SAYS:" SPC strUpr(%obj.action) @ "!";

	%game.displayText("<color:FFFFAA>" @ %text, 3);
	%game.miniGame.play2D(ManiaFunkyMusic);
 
	for (%i = 0; %i < %game.miniGame.numMembers; %i++)
	{
		%client = %game.miniGame.member[%i];
 		%client.win = 0;
		if (%client.isAlive() && %obj.action $= "jet")
		{
			%client.player.setDatablock(PlayerStandardArmor);
		}
	}
 
	%game.endMicroGame = %game.schedule(4000, endMicroGame);
}

function MicroGame_SimonSays::onEnd(%this, %obj, %game)
{
	for (%i = 0; %i < %game.miniGame.numMembers; %i++)
	{
		%client = %game.miniGame.member[%i];

		if (%client.isAlive())
		{
			%client.player.setDatablock(PlayerNoJet);
		}
		//if (%client.win) %client.setManiaWin(%microGame.who);
	}
}

function GameConnection::simonSaysAction(%this, %action)
{
	%microGame = %this.miniGame.maniaGame.microGame;
	%type = %microGame.type;

	if (!isObject(%type) || %type != nameToID("MicroGame_SimonSays"))
	{
		return;
	}

	if (!%this.isAlive() || %this.maniaWinState !$= "")
	{
		return;
	}

	if (%action $= %microGame.action)
	{
		//%this.win;//%this.setManiaWin(%microGame.who);
        %this.setManiaWin(%microGame.who);
	}
}

package MicroGame_SimonSays
{
	function Armor::onTrigger(%this, %obj, %slot, %value)
	{
		Parent::onTrigger(%this, %obj, %slot, %value);

		if (!isObject(%obj.client))
		{
			return;
		}

		switch (%slot)
		{
			case 0: %obj.client.simonSaysAction("click");
			case 1: %obj.client.simonSaysAction("jet");
			case 2: %obj.client.simonSaysAction("jump");
			case 3: %obj.client.simonSaysAction("crouch");
		}
	}

	function serverCmdSit(%client)
	{
		Parent::serverCmdSit(%client);
		%client.simonSaysAction("/sit");
	}

	function serverCmdHug(%client)
	{
		Parent::serverCmdHug(%client);
		%client.simonSaysAction("/hug");
	}

	function serverCmdLove(%client)
	{
		Parent::serverCmdLove(%client);
		%client.simonSaysAction("/love");
	}

	function serverCmdWtf(%client)
	{
		Parent::serverCmdWtf(%client);
		%client.simonSaysAction("/wtf");
	}

	function serverCmdConfusion(%client)
	{
		Parent::serverCmdConfusion(%client);
		%client.simonSaysAction("/wtf");
	}

	function serverCmdHate(%client)
	{
		Parent::serverCmdHate(%client);
		%client.simonSaysAction("/hate");
	}

	function serverCmdAlarm(%client)
	{
		Parent::serverCmdAlarm(%client);
		%client.simonSaysAction("/alarm");
	}
};

activatePackage("MicroGame_SimonSays");
