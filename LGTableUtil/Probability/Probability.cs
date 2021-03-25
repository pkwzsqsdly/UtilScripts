

public interface IProbabilityItem
{
	int GetResult();
}

public class ProbabilityFake : Probability
{
	private int fakeTemp;

	public ProbabilityFake(int chance) : base(chance)
	{
		fakeTemp = 0;
	}

	public override bool Equals(object obj)
	{
		return base.Equals(obj);
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public override string ToString()
	{
		return base.ToString();
	}
	public override int GetResult()
	{
		int res = random.Next(PercentageMax);
		fakeTemp +=	chance;
		if (res <= chance || fakeTemp >= PercentageMax)
		{
			fakeTemp = 0;
			return data;
		}
		return 0;
	}
	private bool fakeRandom()
	{
		int res = random.Next(PercentageMax);
		fakeTemp +=	chance;
		LG.TableUtil.UserSetting.Log("fakeTemp" + fakeTemp);
		if (res <= chance || fakeTemp >= PercentageMax)
		{
			fakeTemp = 0;
			return true;
		}
		return false;
	}

}
public class Probability : IProbabilityItem
{
	public const int PercentageMax = 10000;
	public int chance {get;protected set;}
	public float rate {get;protected set;}
	public int data {get;private set;}

	protected System.Random random;

	public Probability(int chance)
	{
		this.chance = chance;
		this.rate = (float)chance / PercentageMax;
		random = new System.Random();
	}
	
	public Probability(int chance,int data) : this(chance)
	{
		SetData(data);
	}
	public void SetData(int data)
	{
		this.data = data;
	}

	public virtual int GetResult()
	{
		int res = random.Next(PercentageMax);
		return res <= chance ? data : 0;
	}


	public override bool Equals(object obj)
	{
		return base.Equals(obj);
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public override string ToString()
	{
		return base.ToString();
	}

	public static Probability operator+ (Probability a, Probability b)
	{
		return new Probability(a.chance + b.chance,a.data);
	}
	public static Probability operator- (Probability a, Probability b)
	{
		return new Probability(a.chance - b.chance,a.data);
	}
	public static Probability operator+ (Probability a, int b)
	{
		return new Probability(a.chance + b,a.data);
	}
	public static Probability operator- (Probability a, int b)
	{
		return new Probability(a.chance - b,a.data);
	}
	public static bool operator== (Probability a, int b)
	{
		return a.chance == b;
	}
	public static bool operator!= (Probability a, int b)
	{
		return a.chance != b;
	}
	public static bool operator== (Probability a, Probability b)
	{
		return a.chance == b.chance;
	}
	public static bool operator!= (Probability a, Probability b)
	{
		return a.chance != b.chance;
	}
}

