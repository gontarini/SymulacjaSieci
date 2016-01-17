using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymulacjaSieci1
{
    class Lacze
    {
        #region zmienne
        public int id;
        public int wezel_poczatkowy;
        public int wezel_koncowy;
        public double pojemnosc_modulu;
        public double koszt_modulu;
        #endregion

        public Lacze(int _id, int _wezel_poczatkowy, int _wezel_koncowy, double _pojemnosc_modulu, double _koszt_modulu)
        {
            id = _id;
            wezel_poczatkowy = _wezel_poczatkowy;
            wezel_koncowy = _wezel_koncowy;
            pojemnosc_modulu = _pojemnosc_modulu;
            koszt_modulu = _koszt_modulu;
        }
    }
}
