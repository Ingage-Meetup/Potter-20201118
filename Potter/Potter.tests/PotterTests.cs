using System.Collections.Generic;
using NUnit.Framework;

namespace Potter.tests
{
    public class PotterTests
    {
        private HarryPriceCalculator calculator;
        private List<int> basket;

        [SetUp]
        public void Setup()
        {
            calculator = new HarryPriceCalculator();
            basket = new List<int>();
        }

        [Test]
        public void IfIBuyNoBooks_ThenThePriceIsZero()
        {
            var cart = basket;
            Assert.AreEqual(0.0, calculator.GetBasketPrice(cart), "Expected price to be $0.00 for empty cart!");
        }

        [Test]
        public void IfIBuyOneBook_ThenIPayFullPrice()
        {
            basket.Add(1);
            var price = calculator.GetBasketPrice(basket);
            Assert.AreEqual(8, price);
        }

        [Test]
        public void IfIBuyThreeOfTheSameBook_ThenIPayFullPrice()
        {
            List<int> basket = new List<int>()
            {
                1, 1, 1
            };
            var price = calculator.GetBasketPrice(basket);
            Assert.AreEqual(24, price);
        }


        [Test]
        public void IfIBuyFourDifferentBooks_ThenIGetATwentyPercentDiscount()
        {
            List<int> basket = new List<int>()
            {
                1, 2, 3, 4
            };
            var price = calculator.GetBasketPrice(basket);
            Assert.AreEqual(4 * 8 * .8, price);

        }

        [Test]
        public void IfIBuyABunchAndTheyAreAllEligibleForDiscount_ThenAllAreDiscounted()
        {
            List<int> basket = new List<int>()
            {
                1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4, 4, 5, 5, 5, 5
            };
            var price = calculator.GetBasketPrice(basket);

            Assert.AreEqual(141.60, price);
        }

        [Test]
        public void IfIBuyABunchAndTheyAreNotAllEligibleForDiscount_ThenIPayFullPriceForSome()
        {
            List<int> basket = new List<int>()
            {
                1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4, 4, 5, 5, 5, 5
            };
            var price = calculator.GetBasketPrice(basket);

            Assert.AreEqual(149.60, price);
        }

        [Test]
        public void IfIBuyTwoEachOfTwoDifferentBooks_ThenIGetAFivePercentDiscountOnAllBooks()
        {
            List<int> basket = new List<int>()
            {
                1, 1, 5, 5
            };
            var price = calculator.GetBasketPrice(basket);

            Assert.AreEqual(30.4, price);
        }

        [Test]
        public void IfIBuyTwoCopiesOfThreeAndOneCopyOfTheOtherTwo_IGet()
        {
            List<int> basket = new List<int>()
            {
                1, 1, 2, 3, 3, 4, 5, 5
            };
            var price = calculator.GetBasketPrice(basket);

            Assert.AreEqual((.8 * 64), price);
        }

        [Test]
        public void IfIBuyTwoCopiesOfThreeAndOneCopyOfTheOtherTwoPlusOne_IPayFullPriceForPlusOne()
        {
            List<int> basket = new List<int>()
            {
                1, 1, 2, 3, 3, 4, 5, 5, 5
            };
            var price = calculator.GetBasketPrice(basket);

            Assert.AreEqual((.8 * 64 + 8), price);
        }

        [Test]
        public void IfIBuyAnExtraBook_ThenThatBookIsNotDiscounted()
        {
            List<int> basket = new List<int>()
            {
                1, 1, 2, 3
            };
            var price = calculator.GetBasketPrice(basket);

            Assert.AreEqual((.9 * 24 + 8), price);
        }

        [Test]
        public void IfIBuyABunchOfOneBookAndOneOfAnother_IGetADiscountOnTwoBooks()
        {
            List<int> basket = new List<int>()
            {
                1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3 
            };
            var price = calculator.GetBasketPrice(basket);

            Assert.AreEqual((10*8 + (.95*2*8)), price);
        }
}
}