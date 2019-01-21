using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Text.RegularExpressions;

namespace Poker
{
    class Program
    {

        static void Main(string[] args)
        {
            UtilityMethods UM = new UtilityMethods();
            UM.CalltheGame();
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Do you Want to Re-Play :Y/N (Y for Yes/N for No)");

            Regex regex1 = new Regex(@"^[Y/N]{1}$");

            string Choice = Console.ReadLine();
            while (!regex1.IsMatch(Choice.ToUpper()))
            {

                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Please enter your choice as Y (Yes) or N(no)");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                Choice = Console.ReadLine();


            }
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            if (Choice.ToUpper() == "Y")
            {
                UM.CalltheGame();
            }
            else
            {
                Environment.Exit(0);
            }
        }



    }
}