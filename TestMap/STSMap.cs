using System;
using System.Collections.Generic;
using System.Text;


//杀戮尖塔形式的地图Slay The Spire
public class STSMap
{
    //深度 (有多少层)
    private int _depth;
    private List<List<STSMapNode>> _mapList;
    private Random _rand;
    private ISTSMapGenerateHandler _stsNodeHandler;
    public STSMap()
    {
        _mapList = new List<List<STSMapNode>>(_depth);
        _rand = new Random();
    }

    public STSMap AddRandomHandler(ISTSMapGenerateHandler handler)
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
                var node = _stsNodeHandler.CreateNode(i,j);
                node.nodeId = index;
                list.Add(node);
                index++;
            }
        }
    }

    public void ConnectNodes()
    {
        var alreadyList = new List<STSMapNode>();
        for (int i = 1; i < _mapList.Count; i++)
        {
            alreadyList.Clear();
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
                            if (!alreadyList.Contains(cnode))
                            {
                                alreadyList.Add(cnode);
                            }
                        }
                    }
                }
            }
            currNodes.RemoveAll(x => !alreadyList.Contains(x));
            // lastNodes.RemoveAll(x => !x.HasLink);
        }
    }

    public Dictionary<int, STSMapNode> ToDictionary()
    {
        Dictionary<int, STSMapNode> nodeDic = new Dictionary<int, STSMapNode>();
        for (int i = 0; i < _mapList.Count; i++)
        {
            var list = _mapList[i];
            for (int j = 0; j < list.Count; j++)
            {
                var node = list[j];
                nodeDic.Add(node.nodeId,node);
            }
        }
        return nodeDic;
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