using System;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class SubscribeAttribute : Attribute
{
	public string Key {get;private set;}
	public SubscribeAttribute(string key)
	{
		Key = key;
	}
}