using System;
using System.Collections.Generic;
using System.Text;
public class CustomAttList
{
    protected Dictionary<string,CustomAtt> _attDic;

    public CustomAtt this[string name]
    {
        get => GetAtt(name);
        set => AddAtt(value);
    }

    public CustomAttList()
    {
        _attDic = new Dictionary<string, CustomAtt>();
    }

    public void AddAtt(CustomAtt att)
    {
        if (!_attDic.ContainsKey(att.AttName))
        {
            _attDic.Add(att.AttName,att);
        }
    }

    public void RemoveAtt(CustomAtt att)
    {
        RemoveAtt(att.AttName);
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

    public CustomAtt GetAtt(string attName)
    {
        if (_attDic.ContainsKey(attName))
        {
            return _attDic[attName];
        }
        return null;
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        foreach (var item in _attDic)
        {
            sb.Append("(");
            sb.Append(item.Value.ToString());
            sb.Append(")");
        }
        return sb.ToString();
    }
}
