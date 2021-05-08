public interface ISTSMapGenerateHandler
{
    /**
     * @param nowDepth 现在深度
     * @param nowCount 现在第几个节点
     */
    STSMapNode CreateNode(int nowDepth,int nowCount);

    bool IsCanLink(int currType, int targetType);
}