using System;
using System.Collections.Generic;
using System.Text;

// public interface ISTSMapRandomItem
// {
//     int typeId { get; }
//     int[] canLinkIds { get; }
// }
//
// public class STSMapTestItem : ISTSMapRandomItem
// {
//     public int typeId { get; protected set; }
//     public int[] canLinkIds { get; protected set;}
//
//     public STSMapTestItem(int typeId, int[] linkIds)
//     {
//         this.typeId = typeId;
//         this.canLinkIds = linkIds;
//     }
// }

public class STSMapTestHandler : ISTSRandomHandler
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
    public STSMapNode RandomNode(int nowDepth, int nowCount)
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
public class STSMapNode
{
    //节点id  自动生成
    public int nodeId;
    //节点类型 判断是否可以连接
    public int nodeType;
    //分支数量
    public int branchNum;
    public bool HasLink => linkNodeIds.Count > 0;

    public List<int> linkNodeIds;
    
    public STSMapNode()
    {
        linkNodeIds = new List<int>();
    }

    public bool CanLinkNode()
    {
        return branchNum > linkNodeIds.Count;
    }

    public void LinkNode(int nodeId)
    {
        if (!linkNodeIds.Contains(nodeId))
        {
            linkNodeIds.Add(nodeId);
        }
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("[");
        for (int i = 0; i < linkNodeIds.Count; i++)
        {
            sb.Append(linkNodeIds[i]);
            sb.Append(",");
        }
        sb.Append("]");
		
        return $"(id={nodeId},st={nodeType},link={sb.ToString()})";
    }
}

public interface ISTSRandomHandler
{
    /**
     * @param nowDepth 现在深度
     * @param nowCount 现在第几个节点
     */
    STSMapNode RandomNode(int nowDepth,int nowCount);

    bool IsCanLink(int currType, int targetType);
}

//杀戮尖塔形式的地图Slay The Spire
public class STSMap
{
    //深度 (有多少层)
    private int _depth;
    private List<List<STSMapNode>> _mapList;
    private Random _rand;
    private ISTSRandomHandler _stsNodeHandler;
    public STSMap()
    {
        _mapList = new List<List<STSMapNode>>(_depth);
        _rand = new Random();
    }

    public STSMap AddRandomHandler(ISTSRandomHandler handler)
    {
        _stsNodeHandler = handler;
        return this;
    }

    public void Init(int depth,int width)
    {
        _depth = depth;
        int index = 0;
        for (int i = 0; i < _depth; i++)
        {
            var list = new List<STSMapNode>();
            _mapList.Add(list);
            int nodeNum = _rand.Next(3, width);
            for (int j = 0; j < nodeNum; j++)
            {
                var node = _stsNodeHandler.RandomNode(i,j);
                node.nodeId = index;
                list.Add(node);
                index++;
            }
        }
    }

    public void ConnectNodes()
    {
        var removeList = new List<STSMapNode>();
        for (int i = 1; i < _mapList.Count; i++)
        {
            removeList.Clear();
            var lastNodes = _mapList[i - 1];
            
            var currNodes = _mapList[i];
            for (int j = 0; j < lastNodes.Count; j++)
            {
                var lnode = lastNodes[j];
                for (int k = 0; k < currNodes.Count; k++)
                {
                    var cnode = currNodes[k];
                    if (_stsNodeHandler.IsCanLink(lnode.nodeType,cnode.nodeType))
                    {
                        if (lnode.CanLinkNode())
                        {   
                            lnode.LinkNode(cnode.nodeId);
                        }
                    }
                }

                if (!lnode.HasLink)
                {
                    removeList.Add(lnode);
                }
            }
            for (int x = 0; x < removeList.Count; x++)
            {
                lastNodes.Remove(removeList[x]);
            }
        }
    }
    
    public void Print()
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < _mapList.Count; i++)
        {
            var list = _mapList[i];
            sb.Append($"i={i}:");
            for (int j = 0; j < list.Count; j++)
            {
                sb.Append(list[j].ToString());
                sb.Append(',');
            }
            sb.Append('\n');
        }
        System.Console.WriteLine(sb.ToString());
    }
}