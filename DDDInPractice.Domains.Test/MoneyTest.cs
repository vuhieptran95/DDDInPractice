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
            SampleMoney1 = new Money(1, 1, 1, 1, 1, 1,1);
            SampleMoney2 = new Money(1, 1, 1, 1, 5, 0, 0);
        }

        [Fact]
        public void TotalMustBeCorrect()
        {
            SampleMoney1.Total().Should().Be(885);
        }

        [Fact]
        public void CanBeAdded()
        {
            var sample = new Money(2, 2, 2, 2, 6, 1, 1);

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

            action.Should().Throw<Exception>().WithMessage("The subtract amount must be greater than 0");
        }

        [Fact]
        public void CannotSubtractBiggerValue()
        {
            var value = 100000;
            
            Action action = () =>
            {
                _ = SampleMoney1 - value;
            };

            action.Should().Throw<Exception>().WithMessage("The subtract amount is too big");
        }

        [Fact]
        public void CannotSubtractOrBreakDownAmountThatIsNotMultipleOf5()
        {
            var amount = 123;

            Action action = () => BreakDownMoneyLargeToSmall(amount);
            Action action2 = () => { _ = new Money() - amount; };

            action.Should().Throw<Exception>().WithMessage("The amount to breakdown must be multiple of 5");
            action2.Should().Throw<Exception>().WithMessage("The subtract amount must be multiple of 5");
        }

        [Theory]
        [InlineData(1,1,1,1,1,1,1,  1,1,1,1,1,1,1,  0,0,0,0,0,0,0)]
        [InlineData(1,1,1,1,1,1,1,  1,1,1,0,1,1,1,  0,0,0,1,0,0,0)]
        [InlineData(5,5,5,5,5,2,0,  1,1,1,1,1,0,1,  4,4,4,4,3,0,0)]
        [InlineData(5,5,5,5,5,2,0,  1,1,1,1,1,0,2,  4,4,4,0,0,0,0)]
        [InlineData(5,5,5,5,5,2,0,  1,1,1,1,2,0,2,  4,2,0,0,0,0,0)]
        [InlineData(5,5,5,5,5,2,0,  1,4,1,1,2,0,2,  2,0,0,0,0,0,0)]
        public void Subtract2Moneys(int fiveFirst, int tenFirst, int twentyFirst, int fiftyFirst, int oneHundredFirst, int twoHundredFirst, int fiveHundredFirst,
            int fiveSecond, int tenSecond, int twentySecond, int fiftySecond, int oneHundredSecond, int twoHundredSecond, int fiveHundredSecond,
            int fiveThird, int tenThird, int twentyThird, int fiftyThird, int oneHundredThird, int twoHundredThird, int fiveHundredThird)
        {
            var moneyFirst = new Money(fiveFirst, tenFirst, twentyFirst, fiftyFirst, oneHundredFirst, twoHundredFirst, fiveHundredFirst);
            var moneySecond = new Money(fiveSecond, tenSecond, twentySecond, fiftySecond, oneHundredSecond, twoHundredSecond, fiveHundredSecond);
            
            var moneyThird = new Money(fiveThird, tenThird, twentyThird, fiftyThird, oneHundredThird, twoHundredThird, fiveHundredThird);

            (moneyFirst - moneySecond).Should().Be(moneyThird);
        }
        
        [Theory]
        [InlineData(0,0,1,1,1,5,0,  0,1,1,0,0,0,2)]
        public void Subtract2Moneys_InSufficientAmount_ThrowException(int fiveFirst, int tenFirst, int twentyFirst, int fiftyFirst, int oneHundredFirst, int twoHundredFirst, int fiveHundredFirst,
            int fiveSecond, int tenSecond, int twentySecond, int fiftySecond, int oneHundredSecond, int twoHundredSecond, int fiveHundredSecond)
        {
            var moneyFirst = new Money(fiveFirst, tenFirst, twentyFirst, fiftyFirst, oneHundredFirst, twoHundredFirst, fiveHundredFirst);
            var moneySecond = new Money(fiveSecond, tenSecond, twentySecond, fiftySecond, oneHundredSecond, twoHundredSecond, fiveHundredSecond);

            Action action = () =>
            {
                var a = moneyFirst - moneySecond;
            };
            
            action.Should().Throw<Exception>()
                .WithMessage("Unable to do subtraction due to insufficient amount of notes");
        }

        [Theory]
        [InlineData(10, 10, 10, 10, 10, 10, 10, 1000, 10, 10, 10, 10, 10, 10, 8)]
        [InlineData(10, 10, 10, 10, 10, 10, 0, 1000, 10, 10, 10, 10, 10, 5, 0)]
        [InlineData(10, 10, 10, 10, 10, 10, 0, 100, 10, 10, 10, 10, 9, 10, 0)]
        [InlineData(10, 10, 10, 10, 0, 10, 0, 100, 10, 10, 10, 8, 0, 10, 0)]
        [InlineData(10, 10, 10, 0, 0, 10, 0, 100, 10, 10, 5, 0, 0, 10, 0)]
        [InlineData(10, 10, 0, 0, 0, 10, 0, 100, 10, 0, 0, 0, 0, 10, 0)]
        public void DoingSubtraction_HaveSufficientAmount(int five, int ten, int twenty, int fifty, int oneHundred, int twoHundred, int fiveHundred,
            int subtractedAmount, int fiveAfter, int tenAfter, int twentyAfter, int fiftyAfter, int oneHundredAfter,
            int twoHundredAfter, int fiveHundredAfter)
        {
            var money = new Money(five, ten, twenty, fifty, oneHundred, twoHundred, fiveHundred);
            var moneyExpected = new Money(fiveAfter, tenAfter, twentyAfter, fiftyAfter, oneHundredAfter, twoHundredAfter, fiveHundredAfter); 
            
            var res = money - subtractedAmount;

            res.Should().Be(moneyExpected);
        }
        
        [Theory]
        [InlineData(0, 0, 1, 1, 1, 5, 0, 1030)]
        public void DoingSubtraction_NotHaveSufficientAmount(int five, int ten, int twenty, int fifty, int oneHundred, int twoHundred,
            int fiveHundred,int subtractedAmount)
        {
            var money = new Money(five, ten, twenty, fifty, oneHundred, twoHundred, fiveHundred);
            
            Action action = () =>
            {
                _ = money - subtractedAmount;
            };

            action.Should().Throw<Exception>().WithMessage("Unable to do subtraction due to insufficient amount of notes");
        }

        [Theory]
        [InlineData(1000,  0,0,0,0,0,0,2)]
        [InlineData(10,  0,1,0,0,0,0,0)]
        [InlineData(685,  1,1,1,1,1,0,1)]
        [InlineData(1685,  1,1,1,1,1,0,3)]
        public void BreakDownMoneyLargeToSmall_ReturnCorrectResult(int amount, int five, int ten, int twenty, int fifty, int oneHundred, int twoHundred, int fiveHundred)
        {
            var money = BreakDownMoneyLargeToSmall(amount);
            var expectedMoney = new Money(five, ten, twenty, fifty, oneHundred, twoHundred, fiveHundred);

            money.Should().Be(expectedMoney);
        }

        [Fact]
        public void BreakDownMoneyLargeToSmall_NegativeAmount_ThrowException()
        {
            var amount = -100;

            Action action = () => BreakDownMoneyLargeToSmall(amount);

            action.Should().Throw<Exception>().WithMessage("The amount to breakdown must be greater than 0");
        }
        
        [Fact]
        public void BreakDownMoneyLargeToSmall_AmountNotMultipleOf5_ThrowException()
        {
            var amount = 34;

            Action action = () => BreakDownMoneyLargeToSmall(amount);

            action.Should().Throw<Exception>().WithMessage("The amount to breakdown must be multiple of 5");
        }
    }
}