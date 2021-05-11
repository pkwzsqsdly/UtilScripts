using System.Collections.Generic;
using System.Reflection;

namespace TestMap
{
    public class GlobNotification
    {
        private List<INotifier> _notifiers;

        public GlobNotification()
        {
            _notifiers = new List<INotifier>();
        }
        
        public void Notify(string key,object data = null,object sender = null)
        {
            foreach (var noti in _notifiers)
            {
                if(noti == null) continue;
                
                foreach (var method in noti.GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic| BindingFlags.Public))
                {
                    foreach (var att in method.GetCustomAttributes<NotificationAttribute>(true))
                    {
                        if (key.Equals(att.Key))
                        {
                            NotifiyArgs args = new NotifiyArgs(key, data, sender);
                            method.Invoke(noti,new object[]{args});
                        }
                    }
                }
            }
        }

        public void Subscribe(INotifier notifier)
        {
            _notifiers.Add(notifier);
        }

        public void UnSubscribe(INotifier notifier)
        {
            _notifiers.Remove(notifier);
        }

        public void Clear()
        {
            _notifiers.Clear();
        }
    }
}