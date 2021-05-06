using System;
using System.Collections.Generic;
using System.Text;

public class StoryLineRandom
{
    private List<List<StoryRandomData>> _randList;
    private System.Random _rand;
    private List<List<StoryRandomData>> _resList;
    public StoryLineRandom(int depth)
    {
        _randList = new List<List<StoryRandomData>>(depth);
        _resList = new List<List<StoryRandomData>>(depth);
        _rand = new System.Random();
        
        for (int i = 0; i < depth; i++)
        {
            var list = new List<StoryRandomData>();
            _randList.Add(list);
            var newList = new List<StoryRandomData>();
            _resList.Add(newList);
        }
        /** storyType
         * 1 = 普通
         * 2 = 事件
         * 3 = 地 普通
         * 4 = 水 普通
         * 5 = 火 普通
         * 6 = 风 普通
         * 7 = 隐藏
         * 8 = 商店
         * 9 = 宝箱
         * 10 = 结局1
         * 11 = 结局2
         * 12 = 结局3
         */
        int index = 0;
        var newlist = _randList[0];
        newlist.Add(new StoryRandomData(index++,1).InitLinkTypes(1,2,3,4,5,6));
        newlist.Add(new StoryRandomData(index++,2).InitLinkTypes(1,2,3,4,5,6,7,8,9));
        
        for (int i = 1; i < _randList.Count - 2; i++)
        {
            newlist = _randList[i];
            newlist.Add(new StoryRandomData(index++,1).InitLinkTypes(1,2,3,4,5,6));
            newlist.Add(new StoryRandomData(index++,2).InitLinkTypes(1,2,3,4,5,6));
            newlist.Add(new StoryRandomData(index++,3).InitLinkTypes(3,4,5,6));
            newlist.Add(new StoryRandomData(index++,4).InitLinkTypes(3,4,5,6));
            newlist.Add(new StoryRandomData(index++,5).InitLinkTypes(3,4,5,6));
            newlist.Add(new StoryRandomData(index++,6).InitLinkTypes(3,4,5,6));
            newlist.Add(new StoryRandomData(index++,7).InitLinkTypes(1,2,7,8,9));
            newlist.Add(new StoryRandomData(index++,8).InitLinkTypes(1,2,7,8,9));
            newlist.Add(new StoryRandomData(index++,9).InitLinkTypes(1,2,3,4,5,6));
        }
        newlist = _randList[^2];
        newlist.Add(new StoryRandomData(index++,8).InitLinkTypes(11));
        newlist.Add(new StoryRandomData(index++,7).InitLinkTypes(12));
        newlist.Add(new StoryRandomData(index++,1).InitLinkTypes(10,11));
        newlist.Add(new StoryRandomData(index++,2).InitLinkTypes(10,11));
        newlist = _randList[^1];
        newlist.Add(new StoryRandomData(index++,10));
        newlist.Add(new StoryRandomData(index++,11));
        newlist.Add(new StoryRandomData(index++,12));
        
        InitRandomList();
    }

    private void InitRandomList()
    {
        var first = _randList[0];
        for (int i = 0; i < first.Count; i++)
        {
            _resList[0].Add(first[i]);
        }
        
        for (int i = 1; i < _randList.Count; i++)
        {
            var list = _randList[i];
            int num = _rand.Next(2, 6);
            var newList = _resList[i];
            for (int j = 0; j < num; j++)
            {
                newList.Add(RandFromList(list));
            }
        }
    }

    public T RandFromList<T>(List<T> list)
    {
        int index = _rand.Next(1, list.Count);
        return list[index];
    }

    public void FindLink()
    {
        for (int i = 0; i < _resList.Count-1; i++)
        {
            var list = _resList[i];
            
            var next = _resList[i + 1];
            for (int j = 0; j < list.Count; j++)
            {
                var item = list[j];
                for (int k = 0; k < next.Count; k++)
                {
                    if (item.IsCanLink(next[k].storyType))
                    {
                        item.LinkStoryData(next[k]);
                    }
                }
            }
        }
    }


    public void ConnectStory()
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < _resList.Count; i++)
        {
            var list = _resList[i];
            sb.Append($"i={i}:");
            for (int j = 0; j < list.Count; j++)
            {
                sb.Append(list[j]);
                sb.Append(',');
            }
            sb.Append('\n');
        }
        System.Console.WriteLine(sb.ToString());
    }
}