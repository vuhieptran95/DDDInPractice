using System;

namespace DDDInPractice.Domains
{
    public class Money : IEquatable<Money>
    {
        public Money()
        {
        }

        public Money(int five, int ten, int twenty, int fifty, int oneHundred, int twoHundred)
        {
            Five = five;
            Ten = ten;
            Twenty = twenty;
            Fifty = fifty;
            OneHundred = oneHundred;
            TwoHundred = twoHundred;
        }

        public int Five { get; }
        public int Ten { get; }
        public int Twenty { get; }
        public int Fifty { get; }
        public int OneHundred { get; }
        public int TwoHundred { get; }

        public bool Equals(Money other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return this.Five == other.Five && this.Ten == other.Ten && this.Twenty == other.Twenty &&
                   this.Fifty == other.Fifty && this.OneHundred == other.OneHundred &&
                   this.TwoHundred == other.TwoHundred;
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
            return new Money(m1.Five + m2.Five, m1.Ten + m2.Ten, m1.Twenty + m2.Twenty,
                m1.Fifty + m2.Fifty, m1.OneHundred + m2.OneHundred, m1.TwoHundred + m2.TwoHundred);
        }

        public static Money operator -(Money money, int amount)
        {
            if (amount < 0)
            {
                throw new Exception("The subtracted amount of money must be greater than 0");
            }

            if (money.Total() - amount < 0)
            {
                throw new Exception("The subtracted amount is too big");
            }

            return new Money();
        }

        public Money DeepClone()
        {
            return new Money(this.Five, this.Ten, this.Twenty, this.Fifty, this.OneHundred, this.TwoHundred);
        }


        public override int GetHashCode()
        {
            return HashCode.Combine(Five, Ten, Twenty, Fifty, OneHundred, TwoHundred);
        }

        public int Total()
        {
            return 5 * Five + 10 * Ten + 20 * Twenty + 50 * Fifty + 100 * OneHundred + 200 * TwoHundred;
        }
    }
}