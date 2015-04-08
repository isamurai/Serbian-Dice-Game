using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerbianDice
{
    public class SerbianDice
    {
        const int diceMax = 6;
        public List<int> Dices;
        public int TotalPoints;
        public Random Random = new Random();

        public void play()
        {
            while (true)
            {
                RollDices();
                DisplayDices(Dices);
                var total = CountPoints(Dices);
                Console.WriteLine("Total: " + total + ", (Previous total : "+TotalPoints+")");
                if (total == 0)
                {
                    TotalPoints = 0;
                    Console.WriteLine("You loose, looser!");
                    return;
                }

                if (!SelectDices())
                {
                    TotalPoints += total;
                    Console.WriteLine("End of the turn. Total points: " + TotalPoints);
                    return;
                }
                
            }            
        }

        public void DisplayDices(IEnumerable<int> dices)
        {
            Console.WriteLine(String.Join(" ", dices.Select(d => d.ToString()).ToArray()));
        }

        public void RollDices()
        {
            var count = Dices == null || Dices.Count == 0 ? 6 : Dices.Count;
            Dices = new List<int>();
            for (var i = 0; i < count; i++)
            {
                Dices.Add(Random.Next(1, diceMax + 1));
            }
        }

        public int CountPoints(List<int> dices)
        {
            if (IsFlush(dices))
                return 2000;

            var total = 0;
            for (var i = 1; i <= diceMax; i++)
            {
                if (dices.Where(v => v == i).Count() >= 3)
                {
                    if (i == 1)
                        total += 700;
                    else if (i == 5)
                        total += 500 - 150;
                    else
                        total += i * 100;
                }
            }

            total += dices.Select(d =>
                    d == 1 ? 100 :
                    d == 5 ? 50 : 0
            ).Sum();

            return total;
        }

        public bool IsFlush(List<int> dices)
        {
            if (dices.Count != 6)
                return false;

            var previous = -1;
            foreach (int d in dices.OrderBy(d => d))
            {
                if(previous != -1 && d != previous + 1)
                    return false;
                previous = d;
            };

            return true;
        }

        public List<int> GetNoValueDices(List<int> dices)
        {
            //Copy input to keep it unchanged
            dices = new List<int>(dices);

            if (IsFlush(dices))
                return new List<int>();
            
            return dices

                //Drop triplets
                .GroupBy(d => d)
                .SelectMany(g => g.Take(g.Count() % 3))

                //Drop 1 and 5
                .Where(d => d != 1 && d != 5)
                
                .ToList();
        }

        public bool SelectDices()
        {
            Console.WriteLine("Pick dices to keep, none to end turn, \"a\" for all");
            var userChoice = Console.ReadLine();

            if (userChoice == "")
            {
                return false;
            }

            if (userChoice.ToLower() == "a")
            {
                TotalPoints += CountPoints(Dices);
                Dices = GetNoValueDices(Dices);
                return true;
            }

            var dices = new List<int>(Dices);
            var storedDices = new List<int>();

            try
            {
                var selected = userChoice.Select(c => int.Parse(c.ToString())).ToList();

                foreach(var d in selected){
                    if (!dices.Remove(d))
                        throw new FormatException();
                    storedDices.Add(d);
                }

                if (GetNoValueDices(storedDices).Count > 0)
                    throw new FormatException();

                Dices = dices;
                TotalPoints += CountPoints(storedDices);
            }
            catch (FormatException e)
            {
                Console.WriteLine("Entrée incorrecte");
                SelectDices();
            }
            return true;
        }
    }
}
