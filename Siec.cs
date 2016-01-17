using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymulacjaSieci1
{
    class Siec
    {
        #region zmienne i tablice
        public int[,] tablica_grafu;
        public int liczba_wezlow;
        public int liczba_laczy;
        public Lacze[] lacza;
        public Wezel[] wezly;
        private string sciezka_wejscia;
        public int liczba_zapotrzebowan;
        public Zapotrzebowanie[] zapotrzebowanie;
        public int nieskonczonosc = 1000000;
        #endregion

        public Siec()
        {
            wczytywanie();
            tablica_grafu = new int[liczba_wezlow + 1, liczba_wezlow + 1];
            stworzSiec();
            generujSasiadow();
        }
        public void wczytywanie()
        {
            bool ok = false;
            while (!ok)
            {
                System.IO.StreamReader sr;
                string[] wyrazy, wyrazy2;
                ok = true;
                try
                {
                    Console.WriteLine("Przeciagnij tu plik wejsciowy i wcisnij ENTER...");
                    sciezka_wejscia = Console.ReadLine();
                    if (sciezka_wejscia[0] == '\"') sciezka_wejscia = sciezka_wejscia.Substring(1, sciezka_wejscia.Length - 2);
                    Console.WriteLine(" ");
                    sr = new System.IO.StreamReader(sciezka_wejscia);
                    String linia = "";
                    #region wezly
                    linia = "";
                    while (linia.Length < 2 || linia[0] == '#')
                    {
                        linia = sr.ReadLine();
                    }
                    wyrazy = linia.Split(' ');
                    if (wyrazy[0] == "WEZLY" && wyrazy[2] != "") liczba_wezlow = int.Parse(wyrazy[2]);
                    else throw (new Exception("Zla liczba wezlow"));
                    #endregion
                    #region lacza
                    linia = "";
                    while (linia.Length < 2 || linia[0] == '#')
                    {
                        linia = sr.ReadLine();
                    }
                    wyrazy = linia.Split(' ');
                    if (wyrazy[0] == "LACZA" && wyrazy[2] != "") liczba_laczy = int.Parse(wyrazy[2]);
                    else throw (new Exception("Zla liczba pojemności kolejki"));
                    #endregion

                    #region wczytywanie konfiguracji laczy

                    lacza = new Lacze[liczba_laczy + 1];
                    wezly = new Wezel[liczba_wezlow + 1];
                    for (int i = 1; i <= liczba_wezlow; i++)
                    {
                        wezly[i] = new Wezel(i); // wezel[0] jest 0
                    }
                    wezly[0] = null;

                    for (int i = 1; i <= liczba_laczy; i++)
                    {
                        linia = "";
                        while (linia.Length < 2 || linia[0] == '#')
                        {
                            linia = sr.ReadLine();
                        }
                        wyrazy = linia.Split(' ');
                        wyrazy2 = wyrazy[3].Split('.');
                        wyrazy[3] = (wyrazy2[0] + "," + wyrazy2[1]);
                        wyrazy2 = wyrazy[4].Split('.');
                        wyrazy[4] = (wyrazy2[0] + "," + wyrazy2[1]);

                        lacza[i] = new Lacze(int.Parse(wyrazy[0]), int.Parse(wyrazy[1]), int.Parse(wyrazy[2]), double.Parse(wyrazy[3]), double.Parse(wyrazy[4]));
                    }
                    #endregion

                    #region wczytywanie zapotrzebowan
                    #region liczba zapotrzebowan
                    linia = "";
                    while (linia.Length < 2 || linia[0] == '#')
                    {
                        linia = sr.ReadLine();
                    }
                    wyrazy = linia.Split(' ');
                    if (wyrazy[0] == "ZAPOTRZEBOWANIA" && wyrazy[2] != "") liczba_zapotrzebowan = int.Parse(wyrazy[2]);
                    else throw (new Exception("Zla liczba zapotrzebowan"));
                    #endregion

                    zapotrzebowanie = new Zapotrzebowanie[liczba_zapotrzebowan + 1];

                    #region konfiguracja
                    for (int i = 1; i <= liczba_zapotrzebowan; i++)
                    {
                        linia = "";
                        while (linia.Length < 2 || linia[0] == '#')
                        {
                            linia = sr.ReadLine();
                        }
                        wyrazy = linia.Split(' ');
                        wyrazy2 = wyrazy[3].Split('.');
                        wyrazy[3] = (wyrazy2[0] + "," + wyrazy2[1]);
                        zapotrzebowanie[i] = new Zapotrzebowanie(int.Parse(wyrazy[0]), int.Parse(wyrazy[1]), int.Parse(wyrazy[2]), double.Parse(wyrazy[3]));
                    }
                    #endregion
                    #endregion
                    Console.ReadLine();

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    ok = false;
                }
            }

        }
        public void stworzSiec() //siec dla Floyda
        {

            for (int i = 1; i <= liczba_wezlow; i++)
                for (int j = 1; j <= liczba_wezlow; j++)
                {
                    tablica_grafu[i, j] = nieskonczonosc;
                }

            for (int i = 1; i <= liczba_laczy; i++)
            {
                tablica_grafu[lacza[i].wezel_poczatkowy, lacza[i].wezel_koncowy] = lacza[i].id;
            }
        }
        public void generujSasiadow() //siec dla wyzarzania
        {
            int s;

            for (int k = 1; k <= liczba_wezlow; k++)
            {
                s = 1;
                for (int i = 1; i <= liczba_laczy; i++)
                {

                    if (lacza[i].wezel_poczatkowy == k)
                    {
                        wezly[k].liczba_sasiadow++;
                    }
                }
                wezly[k].tablica_sasiadow = new int[wezly[k].liczba_sasiadow + 1];

                for (int i = 1; i <= liczba_laczy; i++)
                {
                    if (lacza[i].wezel_poczatkowy == k)
                    {
                        wezly[k].tablica_sasiadow[s] = lacza[i].wezel_koncowy;
                        s++;
                    }
                }
            }
        }
    }
}
