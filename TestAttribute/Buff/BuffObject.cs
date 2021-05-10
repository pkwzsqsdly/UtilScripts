using System;

namespace TestMap
{
    
    public class BuffObject : IBuff
    {
        public int BuffId { get; protected set; }
        public int BuffType { get; protected set; }
        public int BuffOrder => 0;
        public int BuffValue = 0;

        public BuffObject(int buffId,int buffType)
        {
            BuffId = buffId;
            BuffType = buffType;
        }

        public void OnBuffAdded()
        {
        }

        public void OnBuffRemoved()
        {
        }
    }
}