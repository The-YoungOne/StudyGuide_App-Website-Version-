using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using statement
using NaseDll1;

namespace TestingBaseDLL1
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Wokring with DLL files - AKA Dynamic Link Libraries");

            Console.WriteLine($"5 snd 10 added: {Class1.addUp(5, 10)}\n20 minus 5: {Class1.subtract(5,20)}");

            Console.ReadLine();
        }
        /*
         * public access sepcifier
         * time to fix namespace issue
         * 1 --> add a reference to the .dll file
         * 2 --> add a using statement --> pull .dll
         * write code --> to use what's in the .dll
         * 
         * to add reference --> solution explorer / references / right click / add reference / browse / find program bin --> debug / add debug folder
         * using statement added above
         
         */
    }
}
