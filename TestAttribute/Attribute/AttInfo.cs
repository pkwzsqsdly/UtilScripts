using System;
using System.Collections.Generic;
using System.Text;
public class AttInfo
{
    protected Dictionary<string,AttObject> _attDic;

    public AttObject this[string name]
    {
        get => GetAtt(name);
        set => AddAtt(value);
    }

    public AttInfo()
    {
        _attDic = new Dictionary<string, AttObject>();
    }

    public void AddAtt(AttObject attObject)
    {
        if (!_attDic.ContainsKey(attObject.AttName))
        {
            _attDic.Add(attObject.AttName,attObject);
        }
    }

    public void RemoveAtt(AttObject attObject)
    {
        RemoveAtt(attObject.AttName);
    }
    public void RemoveAtt(string attName)
    {
        if (_attDic.ContainsKey(attName))
        {
            _attDic.Remove(attName);
        }
    }

    public List<AttObject> Collect(Func<AttObject,bool> condition)
    {
        var list = new List<AttObject>();
        foreach (var item in _attDic)
        {
            if (condition(item.Value))
            {
                list.Add(item.Value);
            }
        }
        return list;
    }

    public AttObject GetAtt(string attName)
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
