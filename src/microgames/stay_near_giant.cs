if (!isObject(MicroGame_StayNearGiant))
{
	new ScriptObject(MicroGame_StayNearGiant)
	{
		class = ManiaMicroGame;
	};
}

datablock PlayerData(PlayerGiantArmor : PlayerStandardArmor)
{
	uiName = "";

	maxForwardSpeed = PlayerStandardArmor.maxForwardSpeed * 0.8;
	maxBackwardSpeed = PlayerStandardArmor.maxBackwardSpeed * 0.8;
	maxSideSpeed = PlayerStandardArmor.maxSideSpeed * 0.8;
	maxForwardCrouchSpeed = PlayerStandardArmor.maxForwardCrouchSpeed * 0.8;
	maxSideCrouchSpeed = PlayerStandardArmor.maxSideCrouchSpeed * 0.8;
	maxBackwardCrouchSpeed = PlayerStandardArmor.maxBackwardCrouchSpeed * 0.8;
	jumpForce = PlayerStandardArmor.jumpForce * 0.8;
	mass = 90;
};

function MicroGame_StayNearGiant::onStart(%this, %obj, %game)
{
    %obj.flip = getRandom() < 0.35;

	%game.displayText((%obj.flip ?
        "<color:FFFFAA>STAY AWAY FROM THE GIANT!" :
        "<color:FFFFAA>STAY NEAR THE GIANT!"), 4);
	%game.miniGame.play2D(ManiaRuskieMusic);

	for (%i = 0; %i < %game.miniGame.numMembers; %i++)
	{
		%client = %game.miniGame.member[%i];

		if (%client.isAlive())
		{
			%players = %players @ (%players $= "" ? "" : " ") @ %client.player;
		}
	}

	%index = getRandom(0, getWordCount(%players) - 1);

	%obj.giant = getWord(%players, %index);
	%obj.giant.setPlayerScale(2);
	%obj.giant.setDatablock(PlayerGiantArmor);
    %obj.giant.client.centerPrint((%obj.flip ?
        "<font:arial bold:40><color:FFFFAA>STAY NEAR THE OTHERS!" :
        "<font:arial bold:40><color:FFFFAA>STAY AWAY FROM THE OTHERS!"), 4);

	%game.endMicroGame = %game.schedule(4000, endMicroGame);
}

function MicroGame_StayNearGiant::onEnd(%this, %obj, %game)
{
	if (isObject(%obj.giant))
	{
		%obj.giant.setPlayerScale(1);
		%obj.giant.setDatablock(playerNoJet);
	}

    %near = 0;

	for (%i = 0; %i < %game.miniGame.numMembers; %i++)
	{
		%client = %game.miniGame.member[%i];

		if (%client.isAlive())
		{
			if (isObject(%obj.giant))
			{
                if (%client.player == %obj.giant)
                    continue;

				%distance = vectorDist(%obj.giant.position, %client.player.position);
                %win = %distance <= 7.5;
                
                if (%obj.flip)
                    %win = !%win;

				%client.setManiaWin(%win, 1);

                if (%win)
                    %near++;
			}
			else
			{
				%client.setManiaWin(1);
			}
		}
	}

    if (isObject(%obj.giant))
        %obj.giant.client.setManiaWin(%near < mFloor(%game.miniGame.numMembers / 2));
}
