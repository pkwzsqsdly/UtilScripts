using System;

namespace TestMap
{
    [AttributeUsage(AttributeTargets.Method,AllowMultiple = true)]
    public class NotificationAttribute : Attribute
    {
        public string Key { get; protected set; }

        public NotificationAttribute(string key)
        {
            Key = key;
        }
    }
}