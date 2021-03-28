using System.Collections.Generic;

namespace LG.Probability
{
    public sealed class ProbabilityPatternFake : IProbabilityPattern
    {
        private System.Random random;
        private int fakeTemp;
        public ProbabilityPatternFake()
        {
            fakeTemp = 0;
            random = new System.Random();
        }
        public bool IsHit(int chance)
        {
            int res = random.Next(Probability.PercentageMax);
            fakeTemp += chance;
            if (res < chance || fakeTemp >= Probability.PercentageMax)
            {
                fakeTemp = 0;
                return true;
            }
            return false;
        }
        public IProbabilityPattern Restore()
        {
            return this;
        }
    }

    public sealed class ProbabilityPatternN : IProbabilityPattern
    {
        private System.Random random;
        public ProbabilityPatternN()
        {
            random = new System.Random();
        }

        public bool IsHit(int chance)
        {
            int res = random.Next(Probability.PercentageMax);
            return res < chance;
        }

        public IProbabilityPattern Restore()
        {
            return this;
        }
    }
    public sealed class ProbabilityPatternNList : IProbabilityPattern
    {
        private System.Random random;
        private int randomValue;
        public ProbabilityPatternNList()
        {
            random = new System.Random();
        }
        public bool IsHit(int chance)
        {
            if(randomValue >= chance)
            {
                randomValue -= chance;
                return false;
            }
            return true;
        }

        public IProbabilityPattern Restore()
        {
            randomValue = random.Next(Probability.PercentageMax);
            return this;
        }
    }

    public class ProbabilitySingle : Probability
    {
        protected IProbabilityPattern pattern;
        public ProbabilitySingle(int chance) : base(chance)
        {
            pattern = new ProbabilityPatternN();
        }
        
        public bool IsHit()
        {
            return pattern.IsHit(chance);
        }
    }

    public class Probability : IProbabilityItem
    {
        public const int PercentageMax = 10000;
        public int chance { get; protected set; }
        public float rate { get; protected set; }
        public int mark { get; private set; }

        public virtual ProbabilityType probabilityType => ProbabilityType.Item;

        public Probability(int chance, int mark = -1)
        {
            this.chance = chance;
            this.rate = (float)chance / PercentageMax;
            this.mark = mark;
        }

        public override bool Equals(object obj)
        {
            return obj is Probability probability &&
                    chance == probability.chance &&
                    rate == probability.rate &&
                    mark == probability.mark &&
                    probabilityType == probability.probabilityType;
        }

        public override int GetHashCode()
        {
            return System.HashCode.Combine(chance, rate, mark, probabilityType);
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public static Probability operator +(Probability a, Probability b)
        {
            return new Probability(a.chance + b.chance, a.mark);
        }
        public static Probability operator -(Probability a, Probability b)
        {
            return new Probability(a.chance - b.chance, a.mark);
        }
        public static Probability operator +(Probability a, int b)
        {
            return new Probability(a.chance + b, a.mark);
        }
        public static Probability operator -(Probability a, int b)
        {
            return new Probability(a.chance - b, a.mark);
        }
        public static bool operator ==(Probability a, int b)
        {
            return a.chance == b;
        }
        public static bool operator !=(Probability a, int b)
        {
            return a.chance != b;
        }
        public static bool operator ==(Probability a, Probability b)
        {
            return a.chance == b.chance;
        }
        public static bool operator !=(Probability a, Probability b)
        {
            return a.chance != b.chance;
        }
    }
}

