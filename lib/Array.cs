// Create and return a new Array object.
function Array()
{
	return new ScriptObject()
	{
		class = "Array";
	};
}

// Private: Initialize an Array object.
function Array::onAdd(%this)
{
	%this.size = 0;
}

// Create and return a copy of the Array.
function Array::copy(%this)
{
	%copy = new ScriptObject()
	{
		class = "Array";
		size = %this.size;
	};

	for (%i = 0; %i < %this.size; %i++)
	{
		%copy.item[%i] = %this.item[%i];
	}

	return %copy;
}

// Append *item* to the Array.
function Array::append(%this, %item)
{
	%this.item[%this.size] = %item;
	%this.size++;
}

// Remove a single *item* from the Array.
function Array::remove(%this, %item)
{
	for (%i = %this.find(%item); %i != -1 && %i < %this.size; %i++)
	{
		%this.item[%i] = %this.item[%i + 1];
	}

	if (%i != -1)
	{
		%this.item[%this.size--] = "";
	}
}

// Remove all items in the Array.
function Array::clear(%this)
{
	for (%i = 0; %i < %this.size; %i++)
	{
		%this.item[%i] = "";
	}

	%this.size = 0;
}

// Search the Array for *item* and return it's index if found, -1 otherwise.
function Array::find(%this, %item)
{
	for (%i = 0; %i < %this.size; %i++)
	{
		if (strStr(%item, %this.item[%i]) == 0)
		{
			return %i;
		}
	}

	return -1;
}

// Test if *item* is in the Array.
function Array::contains(%this, %item)
{
	return %this.find(%item) != -1;
}

// Remove and return the last item in the Array.
function Array::pop(%this)
{
	%item = %this.item[%this.size--];
	%this.item[%this.size] = "";

	return %item;
}