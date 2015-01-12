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
        static List<City> cityList;
        static SortedList<int, City> openList;
        static List<City> closedList;

        static void Main(string[] args)
        {
            Console.WindowWidth = 150;
            string[][] streetsMat = csv_read("streets.csv");
            string[][] airlinesMat = csv_read("airline.csv");
            int[][] streetsInt = csv_toInt(streetsMat);
            int[][] airlinesInt = csv_toInt(airlinesMat);
            string[] cities = extractCityNames(streetsMat);
            Console.WriteLine("enter start city: \n================");
            printMat1dIndexes(cities);

            //get starting city
            int startingCity;
            while (true)
            {
                Int32.TryParse(Console.ReadLine(), out startingCity);
                try
                {
                    cities.ElementAt(startingCity);
                }
                catch (Exception)
                {
                    Console.Write("enter a valid index for initial city: ");
                    continue;
                }
                break;
            }
            Console.WriteLine("Chosen initial city: " + cities.ElementAt(startingCity) + "\n");

            // get target city
            Console.WriteLine("enter target city: \n================");
            int targetCity;
            while (true)
            {
                Int32.TryParse(Console.ReadLine(), out targetCity);
                try
                {
                    cities.ElementAt(targetCity);
                }
                catch (Exception)
                {
                    Console.Write("enter a valid index for target city: ");
                    continue;
                }
                break;
            }

            Console.WriteLine("Chosen target city: " + cities.ElementAt(targetCity) + "\n");
            
            //printMat2dInt(streetsInt);
            //printMat2dInt(airlinesInt);

            // add data to lists

            //add first city
            cityList = new List<City>();
            

            //then traverse the array contatining the airline while building the list
            for (int i = 0; i < cities.GetLength(0); i++)
            {
                cityList.Add(new City(cities[i], i, airlinesInt[targetCity][i]));
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

            // now cityList has the following info:
            //  * air distances from chosen city to all the others
            //  * list of neighbors to chosen city
            //  * street distance to neighbors ( or max int when not directly reachable)

            do_a_star(startingCity, targetCity);


            Console.ReadKey();
        }

        private static void do_a_star(int initialCityIdx, int targetCityIdx)
        {
            City initialCity = cityList.ElementAt(initialCityIdx);
            City targetCity = cityList.ElementAt(targetCityIdx);
            openList = new SortedList<int, City>();
            closedList = new List<City>();
            int current_f;

            openList.Add(0, initialCity);
            City currentCity;
            do
            {
                //get next city in the open cities list
                currentCity = openList.First().Value;
Console.WriteLine(currentCity.name);
                current_f = openList.First().Key;
                openList.RemoveAt(0);

                // are we done yet?
                if (currentCity.Equals(targetCity))
                {
                    // target found!
                    // print path
                    Console.WriteLine("\n==================\npath found!");
                    City path = currentCity;
                    while (path != null)
                    {
                        Console.WriteLine(path.name);
                        path = path.predecessor;
                    }
                    break;
                }

                closedList.Add(currentCity);

                expandCity(currentCity, current_f);


                
            } while (openList.Count > 0);



        }

        /// <summary>
        /// checks all neighbors to find the best next step
        /// </summary>
        /// <param name="currentCity"> the city to check for neighbor suitability</param>
        private static void expandCity(City currentCity, int current_f)
        {
            foreach (KeyValuePair<City, int> neighbor in currentCity.neighbors)
            {
                // readability
                City succesorCity = neighbor.Key;
                int succesorDistance = neighbor.Value;
                // end readability

                if (closedList.Contains(succesorCity))
                {
                    continue;
                }

                if (succesorDistance == int.MaxValue)
                {
                    continue;
                }

                int tentative_g = currentCity.distanceUntilHere + succesorDistance;

                //if possibleSuccesor is already in the open list
                if (openList.ContainsValue(succesorCity))
                {
                    // and tentative g is >= than the f stored for possibleSuccesor 
                    if (tentative_g >= succesorCity.distanceUntilHere)
                    {
                        //then ignore it
                        continue;                        
                    }
                }

                succesorCity.distanceUntilHere = tentative_g;

                int f = tentative_g + succesorCity.airDistance;

                //to update f value (in case city is already in the openlist), remove old element 
                if (openList.ContainsValue(succesorCity))
                {
                    openList.RemoveAt(openList.IndexOfValue(succesorCity));
                }
                
                openList.Add(f, succesorCity);

                //write this path to this possible successor
                succesorCity.predecessor = currentCity;
                


                
            }
            
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
        static string[][] csv_read(string eingabePfad)
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
