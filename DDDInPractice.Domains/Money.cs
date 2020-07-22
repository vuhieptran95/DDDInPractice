using System;

namespace DDDInPractice.Domains
{
    public class Money : IEquatable<Money>
    {
        public Money()
        {
        }

        public Money(int oneCent, int penny, int quarter, int oneDollar, int fiveDollar, int twentyDollar)
        {
            OneCent = oneCent;
            Penny = penny;
            Quarter = quarter;
            OneDollar = oneDollar;
            FiveDollar = fiveDollar;
            TwentyDollar = twentyDollar;
        }

        public int OneCent { get; }
        public int Penny { get; }
        public int Quarter { get; }
        public int OneDollar { get; }
        public int FiveDollar { get; }
        public int TwentyDollar { get; }

        public bool Equals(Money other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return this.TotalInCent() == other.TotalInCent();
        }
        
        public bool FullyEquals(Money other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return this.OneCent == other.OneCent && this.Penny == other.Penny && this.Quarter == other.Quarter &&
                   this.OneDollar == other.OneDollar && this.FiveDollar == other.FiveDollar &&
                   this.TwentyDollar == other.TwentyDollar;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Money) obj);
        }

        public static Money operator +(Money m1, Money m2)
        {
            return new Money(m1.OneCent + m2.OneCent, m1.Penny + m2.Penny, m1.Quarter + m2.Quarter,
                m1.OneDollar + m2.OneDollar, m1.FiveDollar + m2.FiveDollar, m1.TwentyDollar + m2.TwentyDollar);
        }
        
        
        public override int GetHashCode()
        {
            return HashCode.Combine(OneCent, Penny, Quarter, OneDollar, FiveDollar, TwentyDollar);
        }

        public int TotalInCent()
        {
            return OneCent + 10 * Penny + 25 * Quarter + 100 * OneDollar + 500 * FiveDollar + 2000 * TwentyDollar;
        }

        public decimal TotalInDollars()
        {
            return Math.Round(Convert.ToDecimal((double) TotalInCent() / 100), 2);
        }
    }
}