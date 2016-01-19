using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SymulacjaSieci1
{
    class Wyzarzanie
    {
        private Sciezka sciezka;
        private Siec siec;
        public double koszt_bazowy;
        public double koszt_iteracji;
        private int warunek_zapetlenia;
        private int liczba_zapetlen;
        private double temperatura;
        private double df;
        private string sciezka_wyjscia;

        private int[,] najlepsze_krawedzie;
        private double[,] najlepsze_moduly;

        public Wyzarzanie()
        {
            siec = new Siec();
            sciezka = new Sciezka(siec); //wyliczamy sciezke bazowa 
            liczba_zapetlen = 0;
            warunek_zapetlenia = 0;
            temperatura = 100;
            sciezka.tablica_modulow1[0, 0] = 150;
            najlepsze_moduly = sciezka.tablica_modulow1;
            najlepsze_krawedzie = sciezka.krawedzie_zapotrzebowan1;
        }
        public void symulowaneWyzarzanie()
        {
            warunek_zapetlenia = 0;
            while(warunek_zapetlenia != 3)
            {
                koszt_bazowy = sciezka.tablica_modulow1[0, 0];

                sciezka.sciezkaWyzarzonaAll(siec);
                sciezka.ustalSciezkiLosowe(siec);

                koszt_iteracji = sciezka.tablica_modulow2[0, 0];
                liczba_zapetlen = 0;
                temperatura = 100;
                for (int i =0; i < 10;i++)
                {
                    if (liczba_zapetlen == 9)
                    {
                        if (koszt_bazowy < najlepsze_moduly[0, 0])
                        {
                            najlepsze_moduly = sciezka.tablica_modulow1;
                            najlepsze_krawedzie = sciezka.krawedzie_zapotrzebowan1;
                        }
                        warunek_zapetlenia++;
                        break;
                    }

                    df = koszt_iteracji - koszt_bazowy;
                    if (df == 0) liczba_zapetlen++;
                    else
                    {

                        if (df < 0)
                        {
                            koszt_bazowy = koszt_iteracji;
                            sciezka.tablica_modulow1 = sciezka.tablica_modulow2;
                            sciezka.krawedzie_zapotrzebowan1 = sciezka.krawedzie_zapotrzebowan2;
                            liczba_zapetlen = 0;
                        }
                        else
                        {
                            Random rnd = new Random(DateTime.Now.Millisecond);
                            double losowa = rnd.NextDouble();
                            double funkcja = Math.Exp((-df) / temperatura);
                            if (funkcja > losowa)
                            {
                                koszt_bazowy = koszt_iteracji;
                                sciezka.tablica_modulow1 = sciezka.tablica_modulow2;
                                sciezka.krawedzie_zapotrzebowan1 = sciezka.krawedzie_zapotrzebowan2;
                                temperatura /= 2; 
                                liczba_zapetlen = 0;
                            }
                            else
                                liczba_zapetlen++;
                        }
                    }
                    if(i == 9)
                    {
                        if (koszt_bazowy < najlepsze_moduly[0, 0])
                        {
                            najlepsze_moduly = sciezka.tablica_modulow1;
                            najlepsze_krawedzie = sciezka.krawedzie_zapotrzebowan1;
                        }
                        warunek_zapetlenia = 3;

                    }
                    else
                    {
                        sciezka.sciezkaWyzarzonaAll(siec);
                        sciezka.ustalSciezkiLosowe(siec);
                        koszt_iteracji = sciezka.tablica_modulow2[0, 0];
                    }
                }
            }

        }
        public void zapisywanie()
        {
            #region nazwa pliku zapisowego
            try
            {
                Console.WriteLine("Przeciagnij tu plik wyjsciowy i wcisnij ENTER...");
                sciezka_wyjscia = Console.ReadLine();
                if (sciezka_wyjscia[0] == '\"') sciezka_wyjscia = sciezka_wyjscia.Substring(1, sciezka_wyjscia.Length - 2);
                Console.WriteLine(" ");

                #region Wyniki i zapis do plikow
                StreamWriter zapis = new StreamWriter(sciezka_wyjscia);
                zapis.WriteLine("# koszt rozwiazania");
                zapis.WriteLine("KOSZT = " + najlepsze_moduly[0,0]);
                zapis.WriteLine(" ");
                zapis.WriteLine("# liczba zapotrzebowan");
                zapis.WriteLine("ZAPOTRZEBOWANIA = " + siec.liczba_zapotrzebowan);
                zapis.WriteLine("# kazde zapotrzebowanie to id. zapotrzebowania oraz zbior uzytych krawedzi");
                Console.WriteLine("Koszt: " + najlepsze_moduly[0,0]);
                for (int i = 1; i <= siec.liczba_zapotrzebowan; i++)
                {
                    String write_tekst = (i.ToString() + " ");
                    String zapotrzebowanie_tekst = ("Zapotrzebowanie" + i + ": ");
                    if(najlepsze_krawedzie[i,1] == 0)
                    {
                        zapotrzebowanie_tekst += ("Brak dostepnej trasy z " + siec.zapotrzebowanie[i].wezel_poczatkowy + " do " + siec.zapotrzebowanie[i].wezel_koncowy + " w grafie");
                        write_tekst += "Brak dostepnej trasy dla tego zapotrzebowania";
                    }
                    for (int a = siec.liczba_laczy; a > 0; a--)
                    {
                        if (najlepsze_krawedzie[i,a]!=0)
                        {
                            zapotrzebowanie_tekst += (najlepsze_krawedzie[i,a] + " ");
                            write_tekst += (najlepsze_krawedzie[i,a] + " ");
                        }
                    }
                    zapis.WriteLine(write_tekst);
                }
                
                zapis.WriteLine(" ");

                zapis.WriteLine("# liczba krawedzi");
                zapis.WriteLine("LACZA = " + siec.liczba_laczy);
                zapis.WriteLine("# kazde lacze to: id, liczba zainstalowanych modulow przepustowosci");
                Console.WriteLine("Lacza i zainstalowana ilosc modulow: ");
                for (int i = 1; i <= siec.liczba_laczy; i++)
                {
                    if (najlepsze_moduly[i,0]!=0)
                    {
                        Console.WriteLine(i + " " + najlepsze_moduly[i,0]);
                        zapis.WriteLine(i + " " + najlepsze_moduly[i, 0]);
                    }
                }
                zapis.WriteLine("# lacza na ktorych nie ma zainstalowanych modulow nie sa wyswietlane");
                zapis.Close();
                Console.WriteLine(" ");
                #endregion
                Console.WriteLine("Wcisnij ENTER by zakonczyc");
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            #endregion
        }

    }
}
