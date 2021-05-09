using System;


namespace TestMap
{
    class TestAttWrapper : ICustomAttListWrapper
    {
        public void Handler(CustomAttList list)
        {
            list.AddAtt(new CustomAtt("atk",10,1));
            list.AddAtt(new CustomAtt("def",5,1));
            list.AddAtt(new CustomAtt("hp",100,1));
            list.AddAtt(new CustomAtt("mp",50,1));
        }
    }

    class Attack : IAttCalculate
    {
        public void Calculate(CustomAttList att1, CustomAttList att2)
        {
            int res = att1["atk"] - att2["def"];
            att1["mp"] -= 25;
            att2["hp"] -= res;
        }
    }
    class Program
    {
        
        static void Main(string[] args)
        {
            var list = CustomAttListCreator.Create(new TestAttWrapper());
            var list2 = CustomAttListCreator.Create(new TestAttWrapper());
            Console.WriteLine("list="+list.ToString());
            Console.WriteLine("list2="+list2.ToString());
            new Attack().Calculate(list,list2);
            Console.WriteLine("list="+list.ToString());
            Console.WriteLine("list2="+list2.ToString());
        }
    }
}