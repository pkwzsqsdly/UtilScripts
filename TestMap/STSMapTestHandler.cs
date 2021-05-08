using System;
using System.Collections.Generic;

public class STSMapTestHandler : ISTSMapGenerateHandler
{
    private Random _rand;
    private List<List<int>> _testList;
    private Dictionary<int, int[]> _typeLinksDic;
    public STSMapTestHandler()
    {
        _typeLinksDic = new Dictionary<int, int[]>();
        _typeLinksDic.Add(1,new int[]{1,2,3,4,5,6,11,12});
        _typeLinksDic.Add(2,new int[]{1,2,3,4,5,6,7,8,9,10,11});
        _typeLinksDic.Add(3,new int[]{3,4,5,6});
        _typeLinksDic.Add(4,new int[]{3,4,5,6});
        _typeLinksDic.Add(5,new int[]{3,4,5,6});
        _typeLinksDic.Add(6,new int[]{3,4,5,6});
        _typeLinksDic.Add(7,new int[]{1,2,7,8,9,10,12});
        _typeLinksDic.Add(8,new int[]{1,2,7,8,9,11,12});
        _typeLinksDic.Add(9,new int[]{1,2,3,4,5,6});
        _typeLinksDic.Add(10,null);
        _typeLinksDic.Add(11,null);
        _typeLinksDic.Add(12,null);
        
        _rand = new Random();
        _testList = new List<List<int>>();
        var list = new List<int>();
        _testList.Add(list);
        list.Add(1);
        list.Add(2);
        
        for (int i = 1; i < 8; i++)
        {
            list = new List<int>();
            _testList.Add(list);
            list.Add(1);
            list.Add(2);
            list.Add(3);
            list.Add(4);
            list.Add(5);
            list.Add(6);
            list.Add(7);
            list.Add(8);
            list.Add(9);
        }
        list = new List<int>();
        _testList.Add(list);
        list.Add(8);
        list.Add(7);
        list.Add(1);
        list.Add(2);
        
        list = new List<int>();
        _testList.Add(list);
        list.Add(10);
        list.Add(11);
        list.Add(12);
    }
    public STSMapNode CreateNode(int nowDepth, int nowCount)
    {
        var node = new STSMapNode();

        var list = _testList[nowDepth];
        int index = _rand.Next(0, list.Count);

        node.branchNum = _rand.Next(1, 4);
        node.nodeType = list[index];

        return node;
    }

    public bool IsCanLink(int currType, int targetType)
    {
        var list = _typeLinksDic[currType];
        for (int i = 0; i < list.Length; i++)
        {
            if (list[i] == targetType)
            {
                return true;
            }
        }

        return false;
    }
}