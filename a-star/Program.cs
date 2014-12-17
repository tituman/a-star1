using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a_star
{
    class Program
    {
        const char CSV_SEPARATOR = ';';

        static void Main(string[] args)
        {
            Console.WindowWidth = 150;
            string[][] streetsMat = csv_einlesen("streets.csv");
            string[][] airlinesMat = csv_einlesen("airline.csv");
            int[][] streetsInt = csv_toInt(streetsMat);
            int[][] airlinesInt = csv_toInt(airlinesMat);
            string[] cities = extractCityNames(streetsMat);
            Console.WriteLine("enter start city: \n================");
            printMat1dIndexes(cities);
            int targetCity;
            while (!Int32.TryParse(Console.ReadLine(), out targetCity))
            {
                Console.Write("enter start city: ");
            }
            printMat2dInt(streetsInt);
            printMat2dInt(airlinesInt);

            // add data to lists

            //add first city
            List<City> cityList = new List<City>();
            

            //then traverse the array contatining the airline while building the list
            for (int i = 0; i < cities.GetLength(0); i++)
            {
                cityList.Add(new City(cities[i], airlinesInt[targetCity][i]));
                //Console.WriteLine(cities[i] + "..." + airlinesInt[targetCity][i]);
                
            }

            //add the street info to each city
            for (int i = 0; i < cityList.Count; i++)
            {
                for (int j = 0; j < cityList.Count; j++)
			    {
                    if (i != j)
                    {
                        cityList.ElementAt(i).addNeighbor(cityList.ElementAt(j), streetsInt[i][j]);
                    }
			    }

            }


            Console.ReadKey();
        }
        static void printMat2d(string[][] mat)
        {
            for (int i = 0; i < mat.GetLength(0); i++)
            {
                for (int j = 0; j < mat[i].GetLength(0); j++)
                {
                    Console.Write("{0,12} ", mat[i][j]);
                }
                Console.WriteLine();
            }
        }
        static void printMat2dInt(int[][] mat)
        {
            for (int i = 0; i < mat.GetLength(0); i++)
            {
                for (int j = 0; j < mat[i].GetLength(0); j++)
                {
                    Console.Write("{0,10} ", mat[i][j]);
                }
                Console.WriteLine();
            }
        }
        static void printMat1d(string[] mat)
        {
            for (int j = 0; j < mat.GetLength(0); j++)
            {
                Console.Write(mat[j] + " ");
            }
            Console.WriteLine();
        }
        static void printMat1dIndexes(string[] mat)
        {
            for (int j = 0; j < mat.GetLength(0); j++)
            {
                Console.WriteLine(j + "..." + mat[j] + " ");
            }
            Console.WriteLine();
        }
        static string[][] csv_einlesen(string eingabePfad)
        {
            StreamReader csvDatei;
            try
            {
                csvDatei = new StreamReader(eingabePfad); //Verbindet Reader mit Datei

                //Initialisierungen
                string zeile;
                string[][] dateiinhalt = new string[0][];

                for (int i = 0; !csvDatei.EndOfStream; i++) //für jede Zeile in der Datei ein mal
                {
                    Array.Resize(ref dateiinhalt, dateiinhalt.GetLength(0) + 1);
                    //Zeile auslesen und in ein Array splitten
                    zeile = csvDatei.ReadLine();
                    dateiinhalt[i] = zeile.Split(CSV_SEPARATOR);
                }
                return dateiinhalt;
            }
            catch (Exception e)
            {
                Console.WriteLine("Problem {0}", e);
            }
            return null;
        }
        static int[][] csv_toInt(string[][] mat)
        {
            int[][] werte = new int[mat.GetLength(0) - 1][];
            for (int i = 0; i < werte.GetLength(0); i++)
            {
                werte[i] = new int[mat[i].GetLength(0) - 1];
                for (int j = 0; j < werte[i].GetLength(0); j++)
                {
                    if (!Int32.TryParse(mat[i + 1][j + 1], out werte[i][j]))
                    {
                        //Parsen geht nicht gut
                        werte[i][j] = int.MaxValue;
                    }
                }
            }


            return werte;
        }
        static string[] extractCityNames(string[][] mat)
        {
            string[] staedtenamen = new string[mat.GetLength(0) - 1];

            for (int i = 1; i < mat.GetLength(0); i++)
            {
                staedtenamen[i - 1] = mat[0][i];
            }
            return staedtenamen;
        }
    }
}
