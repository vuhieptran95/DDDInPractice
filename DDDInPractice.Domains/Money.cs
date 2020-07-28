using System;
using System.Linq;

namespace DDDInPractice.Domains
{
    public class Money : IEquatable<Money>
    {
        public Money()
        {
        }

        public Money(int five, int ten, int twenty, int fifty, int oneHundred, int twoHundred, int fiveHundred)
        {
            Five = five;
            Ten = ten;
            Twenty = twenty;
            Fifty = fifty;
            OneHundred = oneHundred;
            TwoHundred = twoHundred;
            FiveHundred = fiveHundred;
        }

        public int Five { get; }
        public int Ten { get; }
        public int Twenty { get; }
        public int Fifty { get; }
        public int OneHundred { get; }
        public int TwoHundred { get; }
        public int FiveHundred { get; }

        public bool Equals(Money other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return this.Five == other.Five && this.Ten == other.Ten && this.Twenty == other.Twenty &&
                   this.Fifty == other.Fifty && this.OneHundred == other.OneHundred &&
                   this.TwoHundred == other.TwoHundred && this.FiveHundred == other.FiveHundred;
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
                m1.Fifty + m2.Fifty, m1.OneHundred + m2.OneHundred, m1.TwoHundred + m2.TwoHundred,
                m1.FiveHundred + m2.FiveHundred);
        }

        public static Money operator -(Money money, int amount)
        {
            if (amount < 0)
            {
                throw new Exception("The subtract amount must be greater than 0");
            }

            if (amount % 5 != 0)
            {
                throw new Exception("The subtract amount must be multiple of 5");
            }

            if (money.Total() - amount < 0)
            {
                throw new Exception("The subtract amount is too big");
            }

            var subtractMoney = BreakDownMoneyLargeToSmall(amount);

            return money - subtractMoney;
        }

        public static Money operator -(Money m1, Money m2)
        {
            if (m1.Total() - m2.Total() < 0)
            {
                throw new Exception("The subtract amount is too big");
            }

            m2 = BreakDownMoneyLargeToSmall(m2.Total());

            var m1Array = new[]
            {
                m1.Five * 5, m1.Ten * 10, m1.Twenty * 20, m1.Fifty * 50, m1.OneHundred * 100, m1.TwoHundred * 200,
                m1.FiveHundred * 500
            };

            var m2Array = new[]
            {
                m2.Five * 5, m2.Ten * 10, m2.Twenty * 20, m2.Fifty * 50, m2.OneHundred * 100, m2.TwoHundred * 200,
                m2.FiveHundred * 500
            };

            // subtract 2 arrays
            var m3Array = new[]
            {
                m1Array[0] - m2Array[0], m1Array[1] - m2Array[1], m1Array[2] - m2Array[2], m1Array[3] - m2Array[3],
                m1Array[4] - m2Array[4], m1Array[5] - m2Array[5], m1Array[6] - m2Array[6]
            };

            if (m3Array.All(i => i >= 0))
            {
                return new Money(m3Array[0] / 5, m3Array[1] / 10, m3Array[2] / 20, m3Array[3] / 50, m3Array[4] / 100,
                    m3Array[5] / 200, m3Array[6] / 500);
            }

            while (m3Array.Any(i => i < 0))
            {
                for (int i = 6; i >= 0; i--)
                {
                    if (m3Array[i] < 0)
                    {
                        if (i == 0)
                        {
                            throw new Exception("Unable to do subtraction due to insufficient amount of notes");
                        }
                        
                        m3Array[i - 1] += m3Array[i];
                        m3Array[i] = 0;

                        break;
                    }
                }
            }

            return new Money(m3Array[0] / 5, m3Array[1] / 10, m3Array[2] / 20, m3Array[3] / 50, m3Array[4] / 100,
                m3Array[5] / 200, m3Array[6] / 500);
        }
        
        public static Money BreakDownMoneyLargeToSmall(int amount)
        {
            if (amount < 0)
            {
                throw new Exception("The amount to breakdown must be greater than 0");
            }

            if (amount % 5 != 0)
            {
                throw new Exception("The amount to breakdown must be multiple of 5");
            }
            
            (int Five, int Ten, int Twenty, int Fifty, int OneHundred, int TwoHundred, int FiveHundred) money = (0, 0, 0, 0, 0,
                0, 0);
            var remain = amount;

            money.FiveHundred = remain / 500;
            remain = remain % 500;
            money.TwoHundred = remain / 200;
            remain = remain % 200;
            money.OneHundred = remain / 100;
            remain = remain % 100;
            money.Fifty = remain / 50;
            remain = remain % 50;
            money.Twenty = remain / 20;
            remain = remain % 20;
            money.Ten = remain / 10;
            remain = remain % 10;
            money.Five = remain / 5;
            
            return new Money(money.Five, money.Ten, money.Twenty, money.Fifty, money.OneHundred, money.TwoHundred, money.FiveHundred);
        }

        public Money DeepClone()
        {
            return new Money(this.Five, this.Ten, this.Twenty, this.Fifty, this.OneHundred, this.TwoHundred,
                this.FiveHundred);
        }


        public override int GetHashCode()
        {
            return HashCode.Combine(Five, Ten, Twenty, Fifty, OneHundred, TwoHundred);
        }

        public int Total()
        {
            return 5 * Five + 10 * Ten + 20 * Twenty + 50 * Fifty + 100 * OneHundred + 200 * TwoHundred +
                   500 * FiveHundred;
        }
    }
}