// See https://aka.ms/new-console-template for more information
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Timers;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, World! Welcome to the mouse timer!");

        StringBuilder builder = new();
        builder.AppendLine("\n\nThe following arguments are passed:");

        int argumentInterval = 0;

        // Display the command line arguments using the args variable.
        foreach (var arg in args)
        {
            builder.AppendLine($"Argument={arg}");
        }

        Console.WriteLine(builder.ToString());

        string firstArgument = "";
        if (args.Length > 0)
        {
            firstArgument = args[0];
        }

        if (!int.TryParse(firstArgument, out argumentInterval))
        {
            argumentInterval = 5000;
            Console.WriteLine("Failed to set interval. Now set to 5000 Milliseconds.");
        }
        else
        {
            Console.WriteLine("Succeeded to set interval. Milliseconds: " + argumentInterval);
        }

        List<int> xPos = [];
        List<int> yPos = [];
        bool isGo = true;

        DateTime m_dueTime;
        m_dueTime = DateTime.Now.AddMilliseconds(argumentInterval);

        // Create a timer
        System.Timers.Timer myTimer = new()
        {
            Interval = argumentInterval // Set the interval (in milliseconds)
        };

        // after x mins
        // check if the defPoint is different then what is stored
        // do I keep an array of values and check if the last 10 were the same?
        // if true reset the timer
        // if false call suspend

        // Start the timer
        myTimer.Elapsed += Foo;
        myTimer.Start();

        Go();

        void Go()
        {
            while (isGo)
            {
                Point currentPoint = new();
                // New point that will be updated by the function with the current coordinates
                GetCursorPos(ref currentPoint);

                // Now after calling the function, defPnt contains the coordinates which we can read
                //Console.WriteLine("X = " + currentPoint.X.ToString());
                //Console.WriteLine("Y = " + currentPoint.Y.ToString());

                xPos.Add(currentPoint.X);
                yPos.Add(currentPoint.Y); // Not sure how to make it work with hashset

                //Console.WriteLine("Interval: " + myTimer);
                // https://stackoverflow.com/questions/2278525/system-timers-timer-how-to-get-the-time-remaining-until-elapse
                // If I want a prompt warning me.

                var totalSeconds = (m_dueTime - DateTime.Now).TotalSeconds;

                // If we go into the negative lets reset the default. 
                if(totalSeconds < 0)
                {
                    m_dueTime = DateTime.Now.AddMilliseconds(argumentInterval);
                }

                double x = Math.Truncate(totalSeconds * 100) / 100;
                string formattedString = string.Format("{0:N2}%", x);
                Console.ResetColor();
                Console.Write("\rTime Left: {0}", x);
         
                Thread.Sleep(1000);
            }
        }


        void Foo(object? sender, ElapsedEventArgs e)
        {
            Console.ResetColor();
            Console.WriteLine("\n\nTimes Up!");
            //if()
            // check if the X pos changed
            // if it did,
            // then reset the clock
            // then clear x array
            // else
            // call sleep

            // 1 means all are the same for each second.
            // The higher the number, the more distinct values I have.

            bool allXSame = xPos.Distinct().Count() == 1;
            bool allYSame = yPos.Distinct().Count() == 1;

            Console.WriteLine("xPos Distinct Count: " + xPos.Distinct().Count().ToString());
            Console.WriteLine("yPos Distinct Count: " + yPos.Distinct().Count().ToString());
            m_dueTime = DateTime.Now.AddMilliseconds(argumentInterval);

            if (allXSame && allYSame)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine("All X & Y positions are same.");
                Console.WriteLine("Going to sleep!\n");
                SetSuspendState(false, true, true);
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.Green;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine("All X & Y positions are different!\n");
                myTimer.Reset();
                xPos.Clear();
                yPos.Clear();
            }

            Console.ResetColor();

            Console.WriteLine("Date: {0} - {1}", DateTime.Now.ToShortDateString(), DateTime.Now.ToLongTimeString());
            Console.WriteLine("Interval. Milliseconds: " + argumentInterval);
        }

        [DllImport("powrprof.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        static extern bool SetSuspendState(bool hiberate, bool forceCritical, bool disableWakeEvent);
        // Hibernate
        //SetSuspendState(true, true, true);
        // Standby
        //SetSuspendState(false, true, true);


        // We need to use unmanaged code
        [DllImport("user32.dll")]
        // GetCursorPos() makes everything possible
        static extern bool GetCursorPos(ref Point lpPoint);
    }
}

public static class TimerExtensions
{
    public static void Reset(this System.Timers.Timer timer)
    {
        timer.Stop();
        timer.Start();
    }
}