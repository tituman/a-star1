// ALD1
// fh-wels.at
// Daniel Peon
// s1010564011
// ATbbMaster2010
// s1010564011@fh-wels.at


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
        static SortedList<int, City> openList; //used instead of a priority queue
        static List<City> closedList;

        static void Main(string[] args)
        {

            // init
            // read csv files from disc
            // adjacency matrices for street and air distances
/*
* This part done together with a student colleague
*          |||||
*          vvvvv
*/
            int[][] streetsInt; int[][] airlinesInt; string[] cities;
            initIO(out streetsInt, out airlinesInt, out cities);
/*
*          ^^^^^^
*          ||||||
* until here 
*/

            // get input from user about desired path
            int startingCity;
            int targetCity;
            userInputStartAndTargetCity(cities, out startingCity, out targetCity);


            // declare new list of cities containing a data structure (City) which
            // integrates street distances and air distances
            // array could also work OK, but lists are more comfortable
            cityList = new List<City>();
            

            //then traverse the array contatining the air distances while populating the list
            // (target city known, so air distances are related to target city
            for (int i = 0; i < cities.GetLength(0); i++)
            {
                cityList.Add(new City(cities[i], airlinesInt[targetCity][i]));
                
            }

            //add the street info of all neighbors to each city
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
            //  * list of neighbors to aech city
            //      * street distance to neighbors ( or max int when not directly reachable)


            // begin finding path
            do_a_star(cityList.ElementAt(startingCity), cityList.ElementAt(targetCity));

            Console.ReadKey();
        }

        private static void do_a_star(City initialCity, City targetCity)
        {
            //declare open and closed lists
            openList = new SortedList<int, City>();
            closedList = new List<City>();
            int current_f;
            City currentCity;

            openList.Add(0, initialCity);

            do
            {
                //get next city in the open cities list
                currentCity = openList.First().Value;
                current_f = openList.First().Key;
                openList.RemoveAt(0);

                // are we done yet?
                if (currentCity.Equals(targetCity))
                {
                    // target found! print path
                    printPath(currentCity);
                    break;
                }

                closedList.Add(currentCity);
                
                expandCity(currentCity, current_f);

            } while (openList.Count > 0);

        }

        /// <summary>
        /// checks all neighbors to find the best next step
        /// </summary>
        /// <param name="currentCity">the city to check for neighbor suitability</param>
        /// <param name="current_f">actual cost until now plus H() for rest of path</param>
        private static void expandCity(City currentCity, int current_f)
        {
            foreach (KeyValuePair<City, int> neighbor in currentCity.Neighbors)
            {
                // for readability:
                City succesorCity = neighbor.Key;
                int succesorDistance = neighbor.Value;
                // end readability

                // skip city if already visited and closed
                if (closedList.Contains(succesorCity))
                {
                    continue;
                }

                // ignore city if "no path to this neighbor"
                if (succesorDistance == int.MaxValue)
                {
                    continue;
                }

                // wonderfully named variables that need no comments!
                int tentative_g = currentCity.DistanceUntilHere + succesorDistance;

                //if possibleSuccesor is already in the open list
                if (openList.ContainsValue(succesorCity))
                {
                    // and tentative g is >= than the G() stored for possibleSuccesor 
                    if (tentative_g >= succesorCity.DistanceUntilHere)
                    {
                        //then ignore it
                        continue;                        
                    }
                }

                succesorCity.DistanceUntilHere = tentative_g;

                int f = tentative_g + succesorCity.AirDistance;

                //to update f value (in case city is already in the openlist), remove old element 
                if (openList.ContainsValue(succesorCity))
                {
                    openList.RemoveAt(openList.IndexOfValue(succesorCity));
                }
                
                openList.Add(f, succesorCity);

                //write this path to this possible successor
                succesorCity.Predecessor = currentCity;
                
            }
            
        }

        /// <summary>
        /// simple method to print out the path found
        /// </summary>
        /// <param name="path">target city, which links to its predecessor</param>
        private static void printPath(City path)
        {
            Console.WriteLine("\n==================\npath found! (in reverse order):\n==================\n");
            while (path != null)
            {
                Console.WriteLine(path.Name);
                path = path.Predecessor;
            }
        }
        private static void userInputStartAndTargetCity(string[] cities, out int startingCity, out int targetCity)
        {
            Console.WriteLine("enter start city: \n================");
            printMat1dIndexes(cities);

            //get starting city

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
        }


// from here down this was made together with another student colleague
// it is only reading the data from the csv files

        private static void initIO(out int[][] streetsInt, out int[][] airlinesInt, out string[] cities)
        {
            Console.WindowWidth = 150;
            string[][] streetsMat = csv_read("../../streets.csv");
            string[][] airlinesMat = csv_read("../../airline.csv");
            streetsInt = csv_toInt(streetsMat);
            airlinesInt = csv_toInt(airlinesMat);
            cities = extractCityNames(streetsMat);
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
