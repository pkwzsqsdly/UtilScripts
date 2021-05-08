namespace TestMap
{
    public class CustomAttribute
    {
        public string attName { get; protected set; }
        public int attValue { get; set; }
        public int attType{ get; protected set; }

        public CustomAttribute(string attName,int attValue,int attType)
        {
            ResetAttribute(attName,attValue,attType);

        }
        public CustomAttribute(int attValue,int attType)
        {
            ResetAttribute(null,attValue,attType);

        }
        public CustomAttribute(int attValue)
        {
            ResetAttribute(null,attValue,0);
        }

        public void ResetAttribute(string attName, int attValue, int attType)
        {
            this.attName = attName;
            this.attValue = attValue;
            this.attType = attType;
        }
        
        
    }
}