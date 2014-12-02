function GameConnection::isAlive(%this)
{
	return isObject(%this.player) && %this.player.getState() !$= "Dead";
}

function MiniGameSO::respawnDeadPlayers(%this)
{
	for (%i = 0; %i < %this.numMembers; %i++)
	{
		%client = %this.member[%i];

		if (!%client.isAlive())
		{
			%client.instantRespawn();
		}
	}
}

function MiniGameSO::commandToAll(%this, %command, %a, %b, %c, %d, %e, %f, %g, %h, %i, %j, %k, %l, %m)
{
	for (%i = 0; %i < %this.numMembers; %i++)
	{
		commandToClient(%this.member[%i], %command, %a, %b, %c, %d, %e, %f, %g, %h, %i, %j, %k, %l, %m);
	}
}

function MiniGameSO::play2D(%this, %profile)
{
	for (%i = 0; %i < %this.numMembers; %i++)
	{
		%this.member[%i].play2D(%profile);
	}
}

function MiniGameSO::setMusic(%this, %profile)
{
	if(isObject(%this.wideMusic)) %this.wideMusic.delete();
	if(%profile $= "") return;
	%this.WideMusic = new AudioEmitter()
	{
		position = "0 0 0";
		profile = %profile;
		useProfileDescription = "0";
		description = "ManiaLoopingMusicDescription";
		type = "0";
		volume = "2";
		outsideAmbient = "1";
		ReferenceDistance = "9001";
		maxDistance = "9001";
		isLooping = "1";
	};
}