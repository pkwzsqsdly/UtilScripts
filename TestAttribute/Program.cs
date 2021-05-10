using System;


namespace TestMap
{
    class TestAttWrapper : IAttInfoWrapper
    {
        public void Handler(AttInfo info)
        {
            info.AddAtt(new AttObject("atk",10,1));
            info.AddAtt(new AttObject("def",5,1));
            info.AddAtt(new AttObject("hp",100,1));
            info.AddAtt(new AttObject("mp",50,1));
        }
    }

    class Attack : IAttCalculate
    {
        public void Calculate(AttInfo att1, AttInfo att2)
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
            // var list = AttInfoCreator.Create(new TestAttWrapper());
            // var list2 = AttInfoCreator.Create(new TestAttWrapper());
            // Console.WriteLine("list="+list.ToString());
            // Console.WriteLine("list2="+list2.ToString());
            // new Attack().Calculate(list,list2);
            // Console.WriteLine("list="+list.ToString());
            // Console.WriteLine("list2="+list2.ToString());
            BuffInfo info = new BuffInfo();
            info.AddBuff(new BuffObject(1,1));
            info.AddBuff(new BuffObject(2,1));

            for (int i = 0; i < 5; i++)
            {
                info.OnRoundStart();
                info.OnRoundEnd();
            }
            
        }
    }
}