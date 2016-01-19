using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymulacjaSieci1
{
    class Sciezka
    {
        #region zmienne
        public int[,] sciezka_startowa; //tablica poprzednikow Floyda
        public double[,] tablica_sciezki; //tablica kosztow wyliczonych za pomocą algorytmu Floyda
        public int[,] sciezka_losowa; //losowa sciezka 
        public Random rnd = new Random(DateTime.Now.Millisecond);
        private int liczba_zapetlen= 0;
        private int ostatni = 0;
        private bool warunek = false;
        public int[,] krawedzie_zapotrzebowan1; //krawdzie zapotrzebowan odpowiednio dla rozwiazania bazowego i iteracyjnego
        public int[,] krawedzie_zapotrzebowan2; //
        public double[,] tablica_modulow1,tablica_modulow2; // tablice przechowujaca zuzycia modulow
        #endregion
        public Sciezka(Siec siec)
        {

            tablica_sciezki = new double[siec.liczba_wezlow + 1, siec.liczba_wezlow + 1];
            sciezka_startowa = new int[siec.liczba_wezlow + 1, siec.liczba_wezlow + 1];
            sciezka_losowa = new int[siec.liczba_zapotrzebowan + 1, siec.liczba_wezlow + 1];
            krawedzie_zapotrzebowan1 = new int[siec.liczba_zapotrzebowan + 1, siec.liczba_laczy + 1];
            krawedzie_zapotrzebowan2 = new int[siec.liczba_zapotrzebowan + 1, siec.liczba_laczy + 1];

            sciezkaBazowa(siec);
            ustalSciezkiBazowe(siec);
        }
        public void sciezkaBazowa(Siec siec) //liczona na podstawie algorytmu Floyda
        {
            
                //tablica_sciezki = new double[siec.liczba_wezlow + 1, siec.liczba_wezlow + 1];
                //sciezka_startowa = new int[siec.liczba_wezlow + 1, siec.liczba_wezlow + 1];
                //sciezka_losowa = new int[siec.liczba_zapotrzebowan+1,siec.liczba_wezlow + 1];
                //krawedzie_zapotrzebowan1 = new int[siec.liczba_zapotrzebowan + 1, siec.liczba_laczy + 1];
                //krawedzie_zapotrzebowan2 = new int[siec.liczba_zapotrzebowan + 1, siec.liczba_laczy + 1];

                for (int i = 1; i <= siec.liczba_wezlow; i++)
                    for (int j = 1; j <= siec.liczba_wezlow; j++)
                    {
                        tablica_sciezki[i, j] = siec.nieskonczonosc;
                    }

                for (int i = 1; i <= siec.liczba_laczy; i++)
                {
                    tablica_sciezki[siec.lacza[i].wezel_poczatkowy, siec.lacza[i].wezel_koncowy] = siec.lacza[i].koszt_modulu;
                }

                for (int i = 1; i <= siec.liczba_wezlow; i++)
                    for (int j = 1; j <= siec.liczba_wezlow; j++)
                    {
                        sciezka_startowa[i, j] = 0;
                    }

                for (int i = 1; i <= siec.liczba_laczy; i++)
                {
                    sciezka_startowa[siec.lacza[i].wezel_poczatkowy, siec.lacza[i].wezel_koncowy] = siec.lacza[i].wezel_poczatkowy;
                }

                for (int k = 1; k <= siec.liczba_wezlow; k++)
                    for (int i = 1; i <= siec.liczba_wezlow; i++)
                        for (int j = 1; j <= siec.liczba_wezlow; j++)
                        {
                            if (tablica_sciezki[i, j] > (tablica_sciezki[i, k] + tablica_sciezki[k, j])) //&& tablica_sciezki[i,j] != siec.nieskonczonosc)
                            {
                                if (i != j)
                                {
                                    tablica_sciezki[i, j] = tablica_sciezki[i, k] + tablica_sciezki[k, j];
                                    sciezka_startowa[i, j] = sciezka_startowa[k, j];
                                }

                            }
                        }
            }
        public void sciezkaWyzarzona(int s,int k, int p, Siec siec, int indeks_zapotrzebowania) // metoda służąca do wyznaczania randomowych sciezek
        {
            int n;
            if (p == k)
            {
                warunek = true;
                liczba_zapetlen = 0;
                return;
            }
            else if (siec.wezly[p].liczba_sasiadow == 0 || liczba_zapetlen > siec.liczba_laczy)
            {
                liczba_zapetlen = 0;
                for (int i = 1; i <= siec.liczba_wezlow; i++) sciezka_losowa[indeks_zapotrzebowania, i] = 0;
                sciezkaWyzarzona(s, k, s, siec, indeks_zapotrzebowania);
                warunek = false;
                ostatni = indeks_zapotrzebowania;
                return;
            }
            else
            {
                liczba_zapetlen++;
                rnd = new Random(DateTime.Now.Millisecond);
                n = rnd.Next(siec.wezly[p].liczba_sasiadow) + 1;
                sciezka_losowa[indeks_zapotrzebowania, siec.wezly[p].tablica_sasiadow[n]] = p;
                sciezkaWyzarzona(s, k, siec.wezly[p].tablica_sasiadow[n], siec, indeks_zapotrzebowania);
            }

        }
        public void sciezkaWyzarzonaAll(Siec siec)
        {
            ostatni = 1;
            warunek = false;
            while (warunek != true)
            {
                for (int i = ostatni; i <= siec.liczba_zapotrzebowan; i++)
                {
                    if (sciezka_startowa[siec.zapotrzebowanie[i].wezel_poczatkowy, siec.zapotrzebowanie[i].wezel_koncowy] == 0) break;
                    sciezkaWyzarzona(siec.zapotrzebowanie[i].wezel_poczatkowy, siec.zapotrzebowanie[i].wezel_koncowy, siec.zapotrzebowanie[i].wezel_poczatkowy, siec, i);
                    if (warunek == false) break;
                }
            }
        }
        public void ustalSciezkiBazowe(Siec siec) //ustalanie dla danych zapotrzebowan wykorzystanych laczy
        {
            int wezelPocz,next1,next2,m;
            for(int i =1 ; i <= siec.liczba_zapotrzebowan;i++)
            {
                m = 1;
                wezelPocz = siec.zapotrzebowanie[i].wezel_poczatkowy;
                next1 = siec.zapotrzebowanie[i].wezel_koncowy;
                next2 = sciezka_startowa[wezelPocz, next1];
                while(sciezka_startowa[wezelPocz,next1]!=0)
                {
                    krawedzie_zapotrzebowan1[i,m] = siec.tablica_grafu[next2,next1];
                    next1 = next2;
                    next2 = sciezka_startowa[wezelPocz, next2];
                    m++;
                }
            }
           tablica_modulow1 = liczenieModulow(siec, krawedzie_zapotrzebowan1);
        }
        public void ustalSciezkiLosowe(Siec siec) // tablica indeksow laczy danych sciezek dla kolejnych zapotrzebowan 
        {
            int  next1, next2, m;
            for (int i = 1; i <= siec.liczba_zapotrzebowan; i++)
            {
                m = 1;
                next1 = siec.zapotrzebowanie[i].wezel_koncowy;
                next2 = sciezka_losowa[i, next1];
                while (sciezka_losowa[i, next1] != 0)
                {
                    if(m > siec.liczba_laczy)
                    {
                        sciezkaWyzarzonaAll(siec);
                        ustalSciezkiLosowe(siec);
                        return;
                    }
                    else
                    {
                        krawedzie_zapotrzebowan2[i, m] = siec.tablica_grafu[next2, next1];
                        next1 = next2;
                        next2 = sciezka_losowa[i, next2];
                        m++;
                    }

                }
            }
            tablica_modulow2 = liczenieModulow(siec, krawedzie_zapotrzebowan2);
        }
        public double[,] liczenieModulow(Siec siec, int[,] _krawedzie_zapotrzebowan)
        {
            double[,] tablica_modulow = new double[siec.liczba_laczy + 1, 2];
            double rozmiar_zapotrzebowanie;
            double liczba_do_wykupienia;
            int krawedz;
            for (int i = 1; i <= siec.liczba_zapotrzebowan; i++)
                for (int j = 1; j <= siec.liczba_laczy; j++)
                {
                    rozmiar_zapotrzebowanie = siec.zapotrzebowanie[i].rozmiar;
                    if (_krawedzie_zapotrzebowan[i, j] == 0) break;

                    krawedz = _krawedzie_zapotrzebowan[i, j];
                    if (rozmiar_zapotrzebowanie > (tablica_modulow[krawedz, 0] * siec.lacza[krawedz].pojemnosc_modulu - tablica_modulow[krawedz, 1]))
                    {

                        rozmiar_zapotrzebowanie = rozmiar_zapotrzebowanie - (tablica_modulow[krawedz, 0] * siec.lacza[krawedz].pojemnosc_modulu - tablica_modulow[krawedz, 1]);
                        liczba_do_wykupienia = Math.Ceiling(rozmiar_zapotrzebowanie / siec.lacza[krawedz].pojemnosc_modulu);
                        tablica_modulow[krawedz, 1] += rozmiar_zapotrzebowanie + (tablica_modulow[krawedz, 0] * siec.lacza[krawedz].pojemnosc_modulu - tablica_modulow[krawedz, 1]);
                        tablica_modulow[krawedz, 0] += liczba_do_wykupienia;
                        //liczenie kosztow na podstawie modułów (w tablicy z indeksami [0,0])
                        tablica_modulow[0, 0] += liczba_do_wykupienia * siec.lacza[krawedz].koszt_modulu;
                    }
                    else
                    {
                        tablica_modulow[krawedz, 1] += rozmiar_zapotrzebowanie;
                    } 
                }

                return tablica_modulow;
        }
    }
}
