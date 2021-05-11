namespace TestMap
{
    public class TestAttWrapper : IAttInfoWrapper
    {
        public void Handler(AttInfo info)
        {
            info.AddAtt(new AttObject("atk",10,1));
            info.AddAtt(new AttObject("def",5,1));
            info.AddAtt(new AttObject("hp",100,1));
            info.AddAtt(new AttObject("mp",50,1));
        }
    }

    public class Attack : IAttCalculate
    {
        public void Calculate(AttInfo att1, AttInfo att2)
        {
            int res = att1["atk"] - att2["def"];
            att1["mp"] -= 25;
            att2["hp"] -= res;
        }
    }
}