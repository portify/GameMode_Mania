registerOutputEvent("GameConnection", "setManiaWin", "bool", 1);
// registerInputEvent(fxDtsBrick,"onMicrogameStart","Self fxDtsBrick\tPlayer Player\tClient GameConnection\tMinigame Minigame");

// package ManiaGame_Events
// {
// 	function ManiaGame::startMicroGame(%this)
// 	{
// 		parent::startMicroGame(%this);
// 		for(%i=0;%i<getWordCount(inputEvent_GetInputEventIdx("onMicrogameStart"));%i++)
// 		{
// 			%brick = getWord(inputEvent_GetInputEventIdx("onMicrogameStart"),%i);
// 			if(isObject(%brick))
// 			{
// 				if(getMinigameFromObject(getBrickGroupFromObject(%brick)) == %mini)
// 				{
// 					if(%mini.useAllPlayersBricks)
// 					{
// 						$inputTarget_Self = %brick;
// 						$inputTarget_Player = %client.player;
// 						$inputTarget_Client = %client;
// 						$inputTarget_Minigame = %mini;
// 						%brick.processInputEvent("onMicrogameStart",%client);
// 					}
// 					else if(!%mini.useAllPlayersBricks && getBrickGroupFromObject(%brick) == getBrickGroupFromObject(%client))
// 					{
// 						$inputTarget_Self = %brick;
// 						$inputTarget_Player = %client.player;
// 						$inputTarget_Client = %client;
// 						$inputTarget_Minigame = %mini;
// 						%brick.processInputEvent("onMicrogameStart",%client);
// 					}
// 				}
// 			}
// 		}
// 	}
// };
// activatePackage(ManiaGame_Events);