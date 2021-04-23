

public struct Coord2d
{
	public int x;
	public int y;
	public Coord2d(int x,int y)
	{
		this.x = x;
		this.y = y;
	}
	public static bool operator ==(Coord2d c1, Coord2d c2)
	{
		return c1.Equals(c2);
	}

	public static bool operator !=(Coord2d c1, Coord2d c2)
	{
		return !c1.Equals(c2);
	}
	public override string ToString()
	{
		return $"{x},{y}";
	}
	public static Coord2d operator +(Coord2d c1, Coord2d c2)
	{
		return new Coord2d(c1.x + c2.x, c1.y + c2.y);
	}

	public static Coord2d operator +(Coord2d c1, int i2)
	{
		return new Coord2d(c1.x + i2, c1.y + i2);
	}

	public static Coord2d operator +(int i1, Coord2d c2)
	{
		return new Coord2d(i1 + c2.x, i1 + c2.y);
	}

	public static Coord2d operator -(Coord2d c1, Coord2d c2)
	{
		return new Coord2d(c1.x - c2.x, c1.y - c2.y);
	}

	public static Coord2d operator -(Coord2d c1, int i2)
	{
		return new Coord2d(c1.x - i2, c1.y - i2);
	}

	public static Coord2d operator -(int i1, Coord2d c2)
	{
		return new Coord2d(i1 - c2.x, i1 - c2.y);
	}

	public static Coord2d operator *(Coord2d c1, int i2)
	{
		return new Coord2d(c1.x * i2, c1.y * i2);
	}

	public static Coord2d operator *(int i1, Coord2d c2)
	{
		return new Coord2d(i1 * c2.x, i1 * c2.y);
	}

	public static Coord2d operator /(Coord2d c1, int i2)
	{
		return new Coord2d(c1.x / i2, c1.y / i2);
	}
	
	public override bool Equals(object obj)
	{
		if (obj is Coord2d)
			return Equals((Coord2d)obj);
		return false;
	}

	public override int GetHashCode()
	{
		return ToString().GetHashCode();
	}

	public static Coord2d zero => new Coord2d(0,0);
	public int mix => x + y;
	public Coord2d left => new Coord2d(x - 1,y);
	public Coord2d right => new Coord2d(x + 1,y);
	public Coord2d top => new Coord2d(x,y - 1);
	public Coord2d bottom => new Coord2d(x,y + 1);
}