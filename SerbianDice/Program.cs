using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerbianDice
{
    class Program
    {
        static void Main(string[] args)
        {
            const int targetScore = 10000;
            var playersCount = new CountingPlayers();
            var playlers = new int[playersCount];
            var i = 0;
            

            while (true)
            {
                Console.WriteLine("Player" + (i+1) + "'s turn (total: " + playlers[i] + ")");
                Console.WriteLine("Press enter to roll dices...");
                Console.ReadLine();

                var game = new SerbianDice();
                game.play();
                playlers[i] += game.TotalPoints;
                if (playlers[i] == targetScore) { 
                    Console.WriteLine("player {0} win!", i + 1);
                    break;
                }
                if (playlers[i] > targetScore)
                {
                    playlers[i] = playlers[i] % targetScore;
                }
                i = (i + 1) % playersCount;
                Console.WriteLine();
                Console.WriteLine();
            }
            Console.Read();
        }

        public static void Stats(){
            var diceCount = 6;
            var game = new SerbianDice();
            var granTotal = 0;
            var lost = 0;
            var flush = 0;
            var iter = 1000000;
            game.Dices = new int[diceCount].ToList();
            var repartition = new int[6];
            for (int i = 0; i < iter; i++)
            {
                
                game.RollDices();
                foreach (var d in game.Dices)
                {
                    repartition[d-1]++;
                }

                var total = game.CountPoints(game.Dices);
                if (total == 0)
                    lost++;
                if (total == 2000)
                    flush++;
                granTotal += total;
            }

            Console.WriteLine("Mean: " + (granTotal / iter));
            Console.WriteLine("Lost: {0} ({1}%)", lost, (lost / (float)iter) * 100, 2);
            Console.WriteLine("Flush: {0} ({1}%)", flush, (flush / (float)iter) * 100, 2);

            var totalDices = repartition.Sum();
            Console.WriteLine("Repartition: " + String.Join(" - ", repartition.Select(n => Math.Round(n / (float)totalDices, 2).ToString())));
            Console.Read();
        }
    }
}
