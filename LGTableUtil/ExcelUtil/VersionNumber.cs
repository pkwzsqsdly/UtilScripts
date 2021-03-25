

namespace LG.TableUtil
{
	public sealed class VersionNumber 
	{
		private string versionString;
		private int bigNum;
		private int midNum;
		private int endNum;

		public VersionNumber()
		{
			bigNum = 0;
			midNum = 0;
			endNum = 1;
			UpdateString();
		}
		public void Upgrade(int end = 1,int mid = 0,int big = 0)
		{
			bigNum += big;
			midNum += mid;
			endNum += end;
			UpdateString();
		}
		public void SetVersion(string ver)
		{
			if (string.IsNullOrEmpty(versionString))
				return;

			string newStr = versionString.Replace("ver:","");
			var split = newStr.Split('.');

			if (split.Length == 3)
			{
				bigNum = int.Parse(split[0]);
				midNum = int.Parse(split[1]);
				endNum = int.Parse(split[2]);
			}else if (split.Length == 2)
			{
				bigNum = 0;
				midNum = int.Parse(split[0]);
				endNum = int.Parse(split[1]);
			}else if (split.Length == 1)
			{
				bigNum = 0;
				midNum = 0;
				endNum = int.Parse(split[0]);
			}
			UpdateString();
		}
		private void UpdateString()
		{
			versionString = $"ver:{bigNum}.{midNum}.{endNum}";
		}

		public override string ToString()
		{
			return versionString;
		}

		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public static bool operator == (VersionNumber num1,VersionNumber num2)
        {
			return num1.bigNum == num2.bigNum && 
				num1.midNum == num2.midNum &&
				num1.endNum == num2.endNum;
        }
		public static bool operator != (VersionNumber num1,VersionNumber num2)
        {
			if(num1.bigNum != num2.bigNum)
				return true;
			if (num1.midNum != num2.midNum)
				return true;
			return num1.endNum != num2.endNum;
        }
		public static bool operator > (VersionNumber num1,VersionNumber num2)
        {
			if(num1.bigNum > num2.bigNum)
				return true;
			if (num1.bigNum == num2.bigNum && num1.midNum > num2.midNum)
				return true;
			if(num1.midNum == num2.midNum && num1.endNum > num2.endNum)
				return true;
			return false;
        }
		public static bool operator < (VersionNumber num1,VersionNumber num2)
        {
			if(num1.bigNum < num2.bigNum)
				return true;
			if (num1.bigNum == num2.bigNum && num1.midNum < num2.midNum)
				return true;
			if(num1.midNum == num2.midNum && num1.endNum < num2.endNum)
				return true;
			return false;
        }
        public static implicit operator VersionNumber(string value)
        {
			var ver = new VersionNumber();
			ver.SetVersion(value);
            return ver;
        }
	}
}