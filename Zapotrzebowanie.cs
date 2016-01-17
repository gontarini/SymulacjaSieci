using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymulacjaSieci1
{
    class Zapotrzebowanie
    {
        #region zmienne
        public int id;
        public int wezel_poczatkowy;
        public int wezel_koncowy;
        public double rozmiar;
        #endregion

        public Zapotrzebowanie(int _id, int _wezel_poczatkowy, int _wezel_koncowy, double _rozmiar)
        {
            id = _id;
            wezel_poczatkowy = _wezel_poczatkowy;
            wezel_koncowy = _wezel_koncowy;
            rozmiar = _rozmiar;
        }

    }
}
