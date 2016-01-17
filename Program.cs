using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymulacjaSieci1
{
    class Program
    {
        static void Main(string[] args)
        {
            Wyzarzanie wyzarzanie= new Wyzarzanie();
            wyzarzanie.symulowaneWyzarzanie();
            wyzarzanie.zapisywanie();
            Console.WriteLine();
        }
    }
}
