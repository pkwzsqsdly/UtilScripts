using System;

public class CustomAtt
{
    public string AttName { get; protected set; }
    public int AttValue { get; set; }
    public int AttType{ get; protected set; }

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
        AttName = attName;
        AttValue = attValue;
        AttType = attType;
    }
    protected bool Equals(CustomAtt other)
    {
        return AttName == other.AttName && AttValue == other.AttValue && AttType == other.AttType;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == this.GetType() && Equals((CustomAtt) obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(AttName, AttValue, AttType);
    }
    public static bool operator ==(CustomAtt v1, CustomAtt v2)
    {
        return v1 is not null && v1.Equals(v2);
    }

    public static bool operator !=(CustomAtt v1, CustomAtt v2)
    {
        return v1 is not null && !v1.Equals(v2);
    }

    public static CustomAtt operator +(CustomAtt v1, int i2)
    {
        v1.AttValue += i2;
        return v1;
    }
    public static int operator +(CustomAtt v1, CustomAtt v2)
    {
        return v1.AttValue + v2.AttValue;
    }

    public static CustomAtt operator -(CustomAtt v1, int i2)
    {
        v1.AttValue -= i2;
        return v1;
    }
    public static int operator -(CustomAtt v1, CustomAtt v2)
    {
        return v1.AttValue - v2.AttValue;
    }

    public static CustomAtt operator *(CustomAtt v1, int i2)
    {
        v1.AttValue *= i2;
        return v1;
    }
    public static int operator *(CustomAtt v1, CustomAtt v2)
    {
        return v1.AttValue * v2.AttValue;
    }

    public static CustomAtt operator /(CustomAtt v1, int i2)
    {
        v1.AttValue /= i2;
        return v1;
    }
    public static int operator /(CustomAtt v1, CustomAtt v2)
    {
        return v1.AttValue / v2.AttValue;
    }

    public override string ToString()
    {
        return $"n:{AttName},v:{AttValue},t:{AttType}";
    }
}
