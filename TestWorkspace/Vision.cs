using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestWorkspace;

namespace TestWorkspace
{
    class Vision
    {
        //All units are metric.
        private const double gravity = -9.8; //Meters per second squared.
        private const double final_velocity_y = 0; //Meters per second.
        private const double distance_y = 2.3114; //Meters.
        private const double acceleratiod_x = 0; //The boulder is in freefall. Don't change me!

        private static double? time; //Seconds. Define it if you already know it. !!DON'T define it to be really precise!!
        private static double? time_total; //Seconds. Includes time up and time down. Probably don't need.

        private static double distance_x; //Will be calculated via center_y amount.
        private static double velocity_x;

        //Runs methods to calculate net velocity.
        public static double shoot(int center_y, double theta)
        {
            //If time doesn't have a value then calculate time.
            if(time.HasValue == false)
            {
                time = findTime();
            }

            /*
            / If the total time isn't known, calculate the total time by multiplying time by 2.
            / Currently unused since time_total isn't necessary for our calculations.
            if(time.HasValue == true && time_total.HasValue == false)
            {
                time_total = 2 * time;
            }
            */

            //Find distance in the horizontal axis.
            distance_x = findDistance_x(center_y);

            //Return the velocity in the x needed in meter per second to get the ball in the target.
            velocity_x = findVelocity();

            //Returns net velocity.
            return findNetVelocity(theta);
        }

        //Calculates time given the vertical distance. References constants gravity and final_velocity_y.
        private static double findTime()
        {
            //Ensures that gravity is negative. If not, break ALL the code! >:)
            if(gravity >= 0)
            {
                return 0;
            }
            //If gravity is negative then calculate time.
            else
            {
                return Math.Sqrt(-2 * gravity * distance_y);
            }
        }

        //Calculates velocity given an x distance and time.
        private static double findVelocity()
        {
            
            return distance_x / (double) time;
        }

        //
        private static double findNetVelocity(double theta)
        {
            return velocity_x / Math.Cos(degToRad(theta));
        }

        private const double a = 0.001250543776;
        private const double b = 0.1427930784;
        private const double c = 133.6466934;

        private static double findDistance_x(double center_y)
        {
            return a * Math.Pow(center_y, 2) + b * center_y + c;
        }

        private static double degToRad(double deg)
        {
            return Math.PI * deg / 180;
        }
    }
}

public class Vision_Data_Handler
{
    public static void main()
    {
        var stopwatch = Stopwatch.StartNew();

        List<string> list = new List<string>();

        for (int x = 20; x <= 450; x = x + 20)
        {
            for (int i = 1; i <= 90; i++)
            {
                Console.WriteLine("Center y: {0}", x);
                list.Add("Center y: " + x);
                Console.WriteLine("Angle: {0}", i);
                list.Add("Angle: " + i);
                Console.WriteLine("Net velocity: {0}", Vision.shoot(x, i));
                list.Add("Net velocity: " + Vision.shoot(x, i));
                list.Add("");
            }
        }
        string[] str = list.ToArray();

        System.IO.StreamWriter file = new System.IO.StreamWriter("C:\\Users\\Zach\\Desktop\\Velocity Outputs.txt");

        foreach (string i in str)
        {
            file.WriteLine(i);
        }

        file.WriteLine("Elapsed time: {0}", stopwatch.Elapsed);
        file.Close();

        stopwatch.Stop();
        Console.WriteLine(stopwatch.IsRunning);
    }
}

 