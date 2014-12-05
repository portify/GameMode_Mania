if (!isObject(ManiaMicroGameGroup))
{
	new ScriptGroup(ManiaMicroGameGroup);
}

function ManiaMicroGameGroup::getMicroGame(%this, %boss)
{
	%candidates = Array();

	%boss = %boss ? 1 : 0;
	%count = %this.getCount();

	for (%i = 0; %i < %count; %i++)
	{
		%obj = %this.getObject(%i);

		if ((%obj.boss ? 1 : 0) == %boss)
		{
			%candidates.append(%obj);
		}
	}

	%obj = %candidates.item[getRandom(0, %candidates.size - 1)];
	if(isObject($MicrogameMania::forceMicroGame))
		%obj = nameToID($MicrogameMania::forceMicroGame);
	%candidates.delete();

	if (isObject(%obj))
	{
		return %obj;
	}

	if (%boss)
	{
		return %this.getMicroGame(0);
	}

	return 0;
}

function ManiaMicroGameInstance::exists(%this, %name)
{
	return isFunction(%this.type.getName(), %name);
}

function ManiaMicroGameInstance::call(%this, %name, %a, %b, %c, %d, %e)
{
	if (!%this.exists(%name))
	{
		return "";
	}

	return eval("return %this.type." @ %name @ "(%this,%this.game,%a,%b,%c,%d,%e);");
}

function ManiaMicroGame::onAdd(%this)
{
	ManiaMicroGameGroup.add(%this);
}