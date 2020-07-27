using System;
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
            SampleMoney1 = new Money(1, 1, 1, 1, 1, 1);
            SampleMoney2 = new Money(1, 1, 1, 1, 5, 0);
        }

        [Fact]
        public void TotalMustBeCorrect()
        {
            SampleMoney1.Total().Should().Be(385);
        }

        [Fact]
        public void CanBeAdded()
        {
            var sample = new Money(2, 2, 2, 2, 6, 1);

            sample.Should().Be(SampleMoney1 + SampleMoney2);
        }

        [Fact]
        public void CannotSubtractNegativeValue()
        {
            var value = -1;

            Action action = () =>
            {
                _ = SampleMoney1 - value;
            };

            action.Should().Throw<Exception>().WithMessage("The subtracted amount of money must be greater than 0");
        }

        [Fact]
        public void CannotSubtractBiggerValue()
        {
            var value = 100000;
            
            Action action = () =>
            {
                _ = SampleMoney1 - value;
            };

            action.Should().Throw<Exception>().WithMessage("The subtracted amount is too big");
        }

        [Theory]
        [InlineData(10, 10, 10, 10, 10, 10, 1000, 10, 10, 10, 10, 10, 5)]
        [InlineData(10, 10, 10, 10, 10, 0, 1000, 10, 10, 10, 10, 0, 0)]
        [InlineData(10, 10, 10, 10, 10, 10, 100, 10, 10, 10, 10, 9, 10)]
        [InlineData(10, 10, 10, 10, 0, 10, 100, 10, 10, 10, 8, 0, 10)]
        [InlineData(10, 10, 10, 0, 0, 10, 100, 10, 10, 5, 0, 0, 10)]
        [InlineData(10, 10, 0, 0, 0, 10, 100, 10, 0, 0, 0, 0, 10)]
        public void DoingSubtraction_HaveSufficientAmount(int five, int ten, int twenty, int fifty, int oneHundred, int twoHundred,
            int subtractedAmount, int fiveAfter, int tenAfter, int twentyAfter, int fiftyAfter, int oneHundredAfter,
            int twoHundredAfter)
        {
            var money = new Money(five, ten, twenty, fifty, oneHundred, twoHundred);
            var moneyExpected = new Money(fiveAfter, tenAfter, twentyAfter, fiftyAfter, oneHundredAfter, twoHundredAfter); 
            
            var res = money - subtractedAmount;

            res.Should().Be(moneyExpected);
        }
        
        [Theory]
        [InlineData(0, 0, 1, 1, 1, 5, 1030)]
        public void DoingSubtraction_NotHaveSufficientAmount(int five, int ten, int twenty, int fifty, int oneHundred, int twoHundred,
            int subtractedAmount)
        {
            var money = new Money(five, ten, twenty, fifty, oneHundred, twoHundred);
            
            Action action = () =>
            {
                _ = money - subtractedAmount;
            };

            action.Should().Throw<Exception>().WithMessage("Unable to do subtraction due to insufficient amount");
        }
    }
}