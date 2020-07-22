using FluentAssertions;
using Xunit;

namespace DDDInPractice.Domains.Test
{
    public class MoneyTest : Money
    {
        protected Money SampleMoney1;
        protected Money SampleMoney2;

        public MoneyTest()
        {
            SampleMoney1 = new Money(1,1,1,1,1,1);
            SampleMoney2 = new Money(1,1,1,1,5,0);
        }
        
        [Fact]
        public void TotalMustBeCorrect()
        {
            SampleMoney1.TotalInCent().Should().Be(2636);

            SampleMoney1.TotalInDollars().Should().Be((decimal)26.36);
        }

        [Fact]
        public void FullyEqualWhenAllPropertiesEqual()
        {
            SampleMoney1.FullyEquals(new Money(1, 1, 1, 1, 1, 1)).Should().BeTrue();
            SampleMoney1.FullyEquals(SampleMoney2).Should().BeFalse();
        }

        [Fact]
        public void SameTotalIsSameMoney()
        {
            SampleMoney1.Should().Be(SampleMoney2);
        }

        [Fact]
        public void CanBeAdded()
        {
            var sample = new Money(2,2,2,2,6,1);

            sample.Should().Be(SampleMoney1 + SampleMoney2);
        }
    }
}