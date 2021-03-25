
using System.Collections.Generic;
using System.Reflection;

public interface ITableObserver
{
	void receiveNotification();
}
public interface INotifyArgs
{
	string notifyName{get;}
	object notifyBody{get;}
}

public class NotifyArgs : INotifyArgs
{
	public string notifyName{get;set;}

	public object notifyBody{get;set;}
	public NotifyArgs(string name,object body)
	{
		notifyName = name;
		notifyBody = body;
	}
}

public class TableDataController 
{
	private List<ITableObserver> observerList;
	public TableDataController()
	{
		observerList = new List<ITableObserver>();
	}

	public void AddObserver(ITableObserver obs)
	{
		if (observerList.IndexOf(obs) < 0)
		{
			observerList.Add(obs);
		}
	}

	public void NotifyObservers(string name,object body)
	{
		for (int i = 0; i < observerList.Count; i++)
		{
			var obs = observerList[i];
			FindMethodAndRun(obs,name,new NotifyArgs(name,body));
		}
	}

	private void FindMethodAndRun(ITableObserver o,string key,INotifyArgs args)
	{
		var atts = o.GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance);
		foreach(var att in atts)
		{
			if(att.IsDefined(typeof(SubscribeAttribute)))
			{
				var ktm = att.GetCustomAttribute<SubscribeAttribute>();
				if (ktm != null && ktm.Key.Equals(key))
				{
					object[] pms = new object[]{args};
					att.Invoke(o,pms);
				}
			}
		}
	}

	
}