using System;
using System.Collections.Generic;
using System.Text;

namespace TestMap
{
    public class BuffInfo
    {
        protected List<IBuff> _buffs;

        public BuffInfo()
        {
            _buffs = new List<IBuff>();
        }
        public IBuff this[int index]
        {
            get => GetBuff(index);
            set => AddBuff(value);
        }
    
        public void AddBuff(IBuff buff)
        {
            if (buff.BuffOrder == 0)
            {
                _buffs.Add(buff);
            }else if (buff.BuffOrder == -1)
            {
                _buffs.Insert(0,buff);
            }
            buff.OnBuffAdded();
        }
    
        public void RemoveBuff(IBuff buff)
        {
            buff.OnBuffRemoved();
            _buffs.Remove(buff);
        }
        public void RemoveBuffById(int buffId)
        {
            var list = GetBuffsById(buffId);
            for (int i = 0; i < list.Count; i++)
            {
                RemoveBuff(list[i]);
            }
        }
        public void RemoveBuffByType(int buffType)
        {
            var list = GetBuffsByType(buffType);
            for (int i = 0; i < list.Count; i++)
            {
                RemoveBuff(list[i]);
            }
        }
    
        public List<IBuff> Collect(Func<IBuff,bool> condition)
        {
            var list = new List<IBuff>();
            for (int i = 0; i < list.Count; i++)
            {
                var item = list[i];
                if (condition(item))
                {
                    list.Add(item);
                }
            }
            return list;
        }
    
        public IBuff GetBuff(int index)
        {
            if (index > 0 && index < _buffs.Count)
            {
                return _buffs[index];
            }
            return null;
        }
        
        public List<IBuff> GetBuffsByType(int buffType)
        {
            return _buffs.FindAll(x => x.BuffType == buffType);
        }
        public List<IBuff>  GetBuffsById(int buffId)
        {
            return _buffs.FindAll(x => x.BuffId == buffId);
        }
    
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in _buffs)
            {
                sb.Append("(");
                sb.Append(item.ToString());
                sb.Append(")");
            }
            return sb.ToString();
        }

        public void OnRoundStart()
        {
            for (int i = 0; i < _buffs.Count; i++)
            {
                var buff = _buffs[i] as IBuffTimingRound;
                buff?.OnRoundStart();
            }
        }
        
        public void OnRoundEnd()
        {
            for (int i = 0; i < _buffs.Count; i++)
            {
                var buff = _buffs[i] as IBuffTimingRound;
                buff?.OnRoundEnd();
            }
        }
    }
}