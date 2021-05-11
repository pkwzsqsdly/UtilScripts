using System;

namespace TestMap
{
    public class TestNotify : INotifier
    {
        [Notification("ceshi")]
        public void Test(NotifiyArgs args)
        {
            Console.WriteLine(args.Key + "_" + args.Data +"_" + args.Sender);
        }
    }
}