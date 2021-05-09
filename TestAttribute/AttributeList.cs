using System;
using System.Collections.Generic;
using System.Linq;

namespace TestMap
{
    public class CustomAttList
    {
        protected Dictionary<string,CustomAtt> _attDic;

        public CustomAtt this[string name] => Seek(name);

        public CustomAttList(string[] tample)
        {
            _attDic = new Dictionary<string, CustomAtt>();
            
        }

        public void AddAtt(CustomAtt att)
        {
            if (!_attDic.ContainsKey(att.attName))
            {
                _attDic.Add(att.attName,att);
            }
        }

        public void RemoveAtt(CustomAtt att)
        {
            RemoveAtt(att.attName);
        }
        public void RemoveAtt(string attName)
        {
            if (_attDic.ContainsKey(attName))
            {
                _attDic.Remove(attName);
            }
        }

        public List<CustomAtt> Collect(Func<CustomAtt,bool> condition)
        {
            var list = new List<CustomAtt>();
            foreach (var item in _attDic)
            {
                if (condition(item.Value))
                {
                    list.Add(item.Value);
                }
            }
            return list;
        }

        public CustomAtt Seek(string attName)
        {
            if (_attDic.ContainsKey(attName))
            {
                return _attDic[attName];
            }
            return null;
        }
    }
    
    

}