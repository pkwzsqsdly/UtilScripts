using System;

namespace TestMap
{
    public class CustomAtt
    {
        public string attName { get; protected set; }
        public int attValue { get; set; }
        public int attType{ get; protected set; }

        public CustomAtt(string attName,int attValue,int attType)
        {
            ResetAttribute(attName,attValue,attType);

        }
        public CustomAtt(int attValue,int attType)
        {
            ResetAttribute(null,attValue,attType);

        }
        public CustomAtt(int attValue)
        {
            ResetAttribute(null,attValue,0);
        }

        public void ResetAttribute(string attName, int attValue, int attType)
        {
            this.attName = attName;
            this.attValue = attValue;
            this.attType = attType;
        }
        protected bool Equals(CustomAtt other)
        {
            return attName == other.attName && attValue == other.attValue && attType == other.attType;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CustomAtt) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(attName, attValue, attType);
        }
        public static bool operator ==(CustomAtt v1, CustomAtt v2)
        {
            return v1.Equals(v2);
        }

        public static bool operator !=(CustomAtt v1, CustomAtt v2)
        {
            return !v1.Equals(v2);
        }

        public static CustomAtt operator +(CustomAtt v1, int i2)
        {
            v1.attValue += i2;
            return v1;
        }

        public static CustomAtt operator -(CustomAtt v1, int i2)
        {
            v1.attValue -= i2;
            return v1;
        }

        public static CustomAtt operator *(CustomAtt v1, int i2)
        {
            v1.attValue *= i2;
            return v1;
        }

        public static CustomAtt operator /(CustomAtt v1, int i2)
        {
            v1.attValue /= i2;
            return v1;
        }
    }
}