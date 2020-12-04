using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Potter
{
    public class Program
    {
        public static void Main(string[] args)
        {
        }
    }

    public class HarryPriceCalculator
    {
        public double GetBasketPrice(List<int> basket)
        {
            if (basket.Count == 0) return 0;

            int[] books = { 0, 0, 0, 0, 0 };

            foreach (int book in basket)
            {
                if (book <= 5 && book >= 1)
                {
                    books[book - 1] += 1;
                }
            }

            Array.Sort(books);
            books = books.Reverse().ToArray();

            int numberOfDistinctBooks = basket.Distinct().Count();
            int groupSize = 0;

            double bagsPrice = 0;
            double minPrice = Double.MaxValue;

            for (int j = 5; j > 0; j--)
            {
                List<List<int>> bags = new List<List<int>>();
                int[] booksCopy = CopyArray(books);


                while (!isAllPriced(booksCopy))
                {
                    List<int> bookSet = new List<int>();
                    for (int i = 0; i < 5; i++)
                    {
                        if (bookSet.Count >= j)
                        {
                            break;
                        }

                        if (booksCopy[i] > 0)
                        {
                            bookSet.Add(i);
                            booksCopy[i] -= 1;
                        }
                    }

                    bags.Add(bookSet);

                }

                bagsPrice = costOfBags(bags);
                if (bagsPrice < minPrice)
                {
                    minPrice = bagsPrice;
                }

            }
            return minPrice;

        }

        private int[] CopyArray(int[] books)
        {
            int[] copy = new int[books.Length];
            for (int i = 0; i < books.Length; i++)
            {
                copy[i] = books[i];
            }

            return copy;
        }

        private double costOfBags(List<List<int>> bags)
        {
            double cost = 0.0;
            foreach (List<int> bag in bags)
            {
                int bookCount = bag.Count;
                double discount = getDiscount(bookCount);
                cost += bookCount * 8 * (1.0 - discount);
            }

            return cost;
        }

        private double getDiscount(int bookCounter)
        {
            switch (bookCounter)
            {
                case 2:
                    return .05;
                case 3:
                    return .10;
                case 4:
                    return .20;
                case 5:
                    return .25;
                default:
                    return 0;
            }
        }

        private bool isAllPriced(int[] books)
        {
            foreach (int book in books)
            {
                if (book != 0) return false;
            }
            return true;
        }
    }

    public class BooksCombo
    {
        private List<List<Int32>> _bookGroups;

        public BooksCombo()
        {
            this._bookGroups = new List<List<Int32>>();
        }

        public void AddBook(Int32 book)
        {
            this._bookGroups.Add(new List<Int32> { book });
        }

        public void AddBooks(params Int32[] books)
        {
            this._bookGroups.Add(new List<Int32>(books));
        }

        public IEnumerable<List<Int32>> GetGroupsThatDoNotContainBook(Int32 book)
        {
            var containBook = new List<List<int>>();
            foreach (List<int> bg in this._bookGroups)
            {
                if (!bg.Contains(book)) containBook.Add(bg);
            }

            return containBook;
        }

        public IReadOnlyCollection<List<Int32>> Groups => this._bookGroups.AsReadOnly();
    }
}
