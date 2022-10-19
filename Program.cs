using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1._1
{
    class Program
    {
        static Random r = new Random();
        static void Main(string[] args)
        {
            //convert the true type of user inputs
            Console.WriteLine("Please give 2 example set inputs (dot number - width - height)");
            int n = Convert.ToInt32(Console.ReadLine()); //variable that holds how many point will be create
            double width = Convert.ToDouble(Console.ReadLine()); // variable that holds how long will be the x axis of point matrix
            double height = Convert.ToDouble(Console.ReadLine()); // variable that holds how long will be the y axis of point matrix

            int n2 = Convert.ToInt32(Console.ReadLine()); 
            double width2 = Convert.ToDouble(Console.ReadLine());  
            double height2 = Convert.ToDouble(Console.ReadLine());


            double[,] euclideanSpaceArray = RandomPointGenerator(n, width, height); //called RandomPointGenerator and created random points assigned  to 2d array 
            double[,] euclideanSpaceArray2 = RandomPointGenerator(n2, width2, height2); //called RandomPointGenerator and created random points assigned  to 2d array 
            double[,] distanceMatrix = DistanceMatrixGenerator(euclideanSpaceArray); // called DistanceMatrixGenerator and calculated every point distances with each other
            Console.WriteLine("First Random Dots"); //random points table printed
            Console.WriteLine("\t" + "X\t" + "Y"); //random points table printed
            for(int i = 0; i<n; i++)
            {
                Console.Write(i+ ".)" +"\t");

                for (int j = 0; j < 2; j++)
                {
                    
                    Console.Write(string.Format("{0:0.00}\t",euclideanSpaceArray[i, j]));
                    
                }
                Console.WriteLine(Environment.NewLine);
            }
            Console.WriteLine(Environment.NewLine + Environment.NewLine);

            Console.WriteLine("Second Random Dots"); //random points table printed
            Console.WriteLine("\t" + "X\t" + "Y"); //random points table printed
            for (int i = 0; i < n2; i++)
            {
                Console.Write(i + ".)" + "\t");

                for (int j = 0; j < 2; j++)
                {

                    Console.Write(string.Format("{0:0.00}\t", euclideanSpaceArray2[i, j]));

                }
                Console.WriteLine(Environment.NewLine);
            }
            Console.WriteLine(Environment.NewLine + Environment.NewLine);

            Console.WriteLine("********************************************Distance Matrix**********************************************************");
            for (int i = 0; i < distanceMatrix.GetLength(0); i++) // distance matrix table printed
            {
                if(i == 0)
                {
                    for(int k = 0; k < distanceMatrix.GetLength(1); k++)
                    {
                        Console.Write("\t"+k);
                        
                    }
                    Console.Write(Environment.NewLine+ Environment.NewLine);
                }
                Console.Write(i + ".)" + "\t");

                for (int j = 0; j < distanceMatrix.GetLength(1); j++)
                {

                    Console.Write(string.Format("{0:0.00}\t", distanceMatrix[i, j]));

                }
                Console.WriteLine(Environment.NewLine);
            }

            /* determine 10 random starting points 
             * calculated begin from starting point and travel every point in euclidian matrix with minimum distance */
            Console.WriteLine("******************************************************MINIMUM DISTANCE TOURS*****************************************************");
            ArrayList previousStartingPoints = new ArrayList(); // holds  previous random starting points for each tour.
            for (int i = 0; i<10;i++) 
            {
                int startingPoint = 0;
                Console.Write( "Tour " + (i + 1) + ")\t");
                double sumDistance = 0;
                do
                {
                    startingPoint = r.Next(0, distanceMatrix.GetLength(0));
                }
                while (previousStartingPoints.Contains(startingPoint)); // this loop checks if the random point created before
                previousStartingPoints.Add(startingPoint); // if point not created before then point is valid and add to arraylist to check next starting point

                ArrayList nearestNeighbor = NearestNeighborMethod(distanceMatrix, ref sumDistance, startingPoint); // method called
                for (int j = 0; j < nearestNeighbor.Count; j++)
                {
                    if(j == nearestNeighbor.Count - 1)
                    {
                        Console.Write(nearestNeighbor[j]);
                    }
                    else
                    {
                        Console.Write(nearestNeighbor[j] + " - ");
                    }
                    
                }

                Console.Write(Environment.NewLine);
                Console.WriteLine("Sum distance: " + sumDistance);
                Console.Write(Environment.NewLine+Environment.NewLine);
                // Console prints are done!
            }
            


            Console.ReadKey();
        }

        /*This method creates random values and holds them in a 2d double type array and return that array */
        public static double[,] RandomPointGenerator(int n, double width, double height)
        {
            Random random = new Random(); // created an object
            double[,] arraytmp = new double[n,2];   //created an array to return 
            for(int i = 0; i< n; i++)
            {
                double x = random.NextDouble() * (width - 0) + 0; // created value of x axes of point
                arraytmp[i, 0] = x;
                double y = random.NextDouble() * (height - 0) + 0;  // created value of y axes of point
                arraytmp[i, 1] = y;
            }


            return arraytmp; // return 2d array
        }

        /*This method takes random points array as a parameter and return a 2d double type array*/
        public static double[,] DistanceMatrixGenerator(double[,] arr)
        {
            double[,] matrixtmp = new double[arr.GetLength(0), arr.GetLength(0)]; // created array with appropriate index lengths  
            
            /*Two nested for loop provides calculate distances every points with each other*/
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                double x1 = arr[i, 0];
                double y1 = arr[i, 1];
                for (int j = 0; j<arr.GetLength(0); j++)
                {
                    double x2;
                    x2= arr[j,0];
                    double y2;
                    y2= arr[j,1];
                    matrixtmp[i, j] = DistanceCalculation(x1, x2, y1, y2); //method called

                }
            }

            return matrixtmp; //return 2d array
        }
        public static double DistanceCalculation(double x1, double x2, double y1, double y2)
        {
            return Math.Sqrt(Math.Pow(x2-x1,2) + Math.Pow(y2 -y1,2)); //I used formula that in the project doc.
        }

        /*This method takes distance matrix as a parameter and return an arraylist.
         *sumDistance variable called with ref key because I want to change the value of the variable outside of the method scope
         */
        public static ArrayList NearestNeighborMethod(double[,]distanceMatrix, ref double sumDistance, int startingPoint)
        {
            
            
            ArrayList nearestNeighborList = new ArrayList(); //Created an arraylist for hold travelled point indexes.
            
            nearestNeighborList.Add(startingPoint); // added starting point which created randomly, before the loop
            
            int newStartingPointtmp = 0; // next point index

            while (true)
            {
                double minDistance = Double.PositiveInfinity; // assign MAX value in the first tour in this loop this value will change compulsorily
                for (int j = 0; j < distanceMatrix.GetLength(1); j++)
                {
                        
                        if (distanceMatrix[startingPoint, j] < minDistance && !nearestNeighborList.Contains(j)) // checks if point not travelled before and if smaller than tmp value
                        {
                        minDistance = distanceMatrix[startingPoint, j] ;
                        newStartingPointtmp = j;
                        
                        }
                    
                }

                nearestNeighborList.Add(newStartingPointtmp); // added index after finish the for loop
                startingPoint = newStartingPointtmp; //I assigned new starting point for the rest of this tour
                sumDistance += minDistance; // sum distance calculation

                /*if index list size equal to first line of distance matrix
                *which means travelled point number equals the all number of points while loop will be ended*/
                if(nearestNeighborList.Count == distanceMatrix.GetLength(0))
                {
                    break;
                }
            }

            return nearestNeighborList; // return arraylist
        }

    }
}
