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
	if(!isObject($SM::MusicBrick))
	{
		$SM::MusicBrick = new fxDTSBrick()
		{
			client = %client;
			dataBlock = brickMusicData;
			isPlanted = true;
			isServerMusic = true;
			position = "0 0 -10000";
		};
		Brickgroup_888888.add($SM::MusicBrick);
	}
	$SM::MusicBrick.setMusic(%profile);
}

datablock AudioDescription(SM_AudioMusicLooping2d : AudioMusicLooping3d)
{
	maxDistance = 999999;
	referenceDistance = 4;
	outsideAmbient = 1;
	is3D = 0;
	coneOutsideVolume = 1;
	volume = 1;
	enableVisualFeedback = 1;
};

package serverMusicPackage
{
	function fxDtsBrick::setSound(%this, %music, %client)
	{
		if(%this.isServerMusic)
		{
			%this.disappear(-1);

			if(isObject(%this.audioEmitter))
				%this.audioEmitter.delete();

			if(!isObject(%music))
				return 1;

			%pos = "0 0 0";
			if(%this.isServerMusicPos)
				%pos = %this.getPosition();

			%this.audioEmitter = new AudioEmitter()
			{
				position = %pos;
				rotation = "1 0 0 0";
				scale = "1 1 1";
				profile = %music;
				useProfileDescription = 0;
				description = SM_AudioMusicLooping2d;
				type = 0;
				volume = SM_AudioMusicLooping2d.volume;
				outsideAmbient = SM_AudioMusicLooping2d.outsideAmbient;
				referenceDistance = SM_AudioMusicLooping2d.referenceDistance;
				maxDistance = SM_AudioMusicLooping2d.maxDistance;
				isLooping = 1;
				loopCount = -1;
				minLoopGap = 0;
				maxLoopGap = 0;
				enableVisualFeedback = SM_AudioMusicLooping2d.enableVisualFeedback;
				is3D = SM_AudioMusicLooping2d.is3D;
				coneInsideAngle = 360;
				coneOutsideAngle = 360;
				coneOutsideVolume = SM_AudioMusicLooping2d.coneOutsideVolume;
				coneVector = "0 1 0";
			};
			missionCleanUp.add(%this.audioEmitter);
			%this.audioEmitter.setScopeAlways();
		}
		else
			return Parent::setSound(%this, %music, %client);
	}
};
activatepackage(serverMusicPackage);