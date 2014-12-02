function GameConnection::setManiaWin(%this, %win, %quiet)
{
	if (%this.maniaWinState !$= "")
	{
		return;
	}

	%this.maniaWinState = %win ? 1 : 0;

	if (%win && isObject(%this.player))
	{
        if (!%quiet)
        {
    		%this.player.spawnExplosion(WandProjectile, 1);
    		%this.play2d(ManiaCompleteMe);
        }

		for (%i = 0; %i < $defaultMiniGame.numMembers; %i++)
		{
			if($defaultMiniGame.member[%i] != %this)
				$defaultMiniGame.member[%i].play3d(ManiaCompleteYou, %this.player.getPosition());
		}
	}
}

function MiniGameSO::setManiaWin(%this, %win, %allowDeadWin)
{
	for (%i = 0; %i < %this.numMembers; %i++)
	{
		%client = %this.member[%i];
		%client.setManiaWin(%win && (%allowDeadWin || %client.isAlive()), 1);
	}
}

function MiniGameSO::resetManiaWins(%this)
{
	for (%i = 0; %i < %this.numMembers; %i++)
	{
		%this.member[%i].maniaWinState = "";
	}
}
