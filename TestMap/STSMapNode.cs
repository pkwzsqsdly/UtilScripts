using System.Collections.Generic;
using System.Text;

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