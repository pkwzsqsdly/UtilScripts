namespace TestMap
{
    public class NotifiyArgs
    {
        public string Key { get; protected set; }
        public object Data{ get; protected set; }
        public object Sender{ get; protected set; }

        public NotifiyArgs(string key, object data = null, object sender = null)
        {
            Key = key;
            Data = data;
            Sender = sender;
        }
        
    }
}