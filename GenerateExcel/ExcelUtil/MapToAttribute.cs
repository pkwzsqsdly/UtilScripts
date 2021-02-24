using System;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class MapToAttribute : Attribute
{
	public string Key {get;private set;}
	public MapToAttribute(string key)
	{
		Key = key;
	}
}