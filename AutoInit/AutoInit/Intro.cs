using System;

namespace AutoInit
{
    class Intro
    {
        static int counter;
        static int introtime;
        static Random randomposition = new Random();

        static int flowspeed = 180;
        static int fastflow = flowspeed + 30;
        static int textflow = fastflow + 50;

        static ConsoleColor baseColor = ConsoleColor.DarkGreen;
        static ConsoleColor fadeColor = ConsoleColor.White;
        static String endtext = "W O L K E N H O F";

        static char AscciCharecters
        {
            get
            {
                int t = randomposition.Next(10);

                if (t <= 2) return (char)('0' + randomposition.Next(10));
                else if (t <= 4) return (char)('a' + randomposition.Next(27));
                else if (t <= 4) return (char)('A' + randomposition.Next(27));
                else return (char)(randomposition.Next(32, 255));
            }    
        }

        public static void StartIntro()
        {
            Console.ForegroundColor = baseColor;
            Console.WindowLeft = Console.WindowTop = 0;
            Console.SetWindowPosition(0, 0);
            Console.CursorVisible = false;

            int width;
            int height;
            int[] y;
            initialize(out width, out height, out y);

            while (true)
            {
                introtime++;
                counter++;
                columupdate(width, height, y);
                if (counter > (3 * flowspeed))
                    counter = 0;
                if (introtime == 300)
                    return;
            }
        }

        public static int YPositionFields(int yPosition, int height)
        {
            if (yPosition < 0) return yPosition + height;
            else if (yPosition < height) return yPosition;
            else
                return 0;
        }
        private static void initialize(out int width, out int height, out int[] y)
        {
            height = Console.WindowHeight;
            width = Console.WindowWidth - 1;
            y = new int[width];
            Console.Clear();

            for (int x = 0; x < width; ++x) { y[x] = randomposition.Next(height); }
        }
        private static void columupdate(int width, int height, int[] y)
        {
            int x;
            if (counter < flowspeed)
            {
                for (x = 0; x < width; ++x)
                {
                    if (x % 10 == 1) Console.ForegroundColor = fadeColor;
                    else Console.ForegroundColor = baseColor;

                    Console.SetCursorPosition(x, y[x]);
                    Console.Write(AscciCharecters);

                    if (x % 10 == 9) Console.ForegroundColor = fadeColor;
                    else Console.ForegroundColor = baseColor;

                    int temp = y[x] - 2;

                    Console.SetCursorPosition(x, YPositionFields(temp, height));
                    Console.Write(AscciCharecters);

                    int temp2 = y[x] - 20;
                    Console.SetCursorPosition(x, YPositionFields(temp2, height));
                    Console.Write(' ');
                    y[x] = YPositionFields(y[x] + 1, height);
                }
            }

            else if (counter > flowspeed && counter < fastflow)
            {
                for (x = 0; x < width; ++x)
                {
                    Console.SetCursorPosition(x, y[x]);
                    if (x % 10 == 9) Console.ForegroundColor = fadeColor;
                    else Console.ForegroundColor = baseColor;

                    Console.Write(AscciCharecters);

                    y[x] = YPositionFields(y[x] + 1, height);
                }
            }
            else if (counter > fastflow)
            {
                for (x = 0; x < width; ++x)
                {
                    Console.SetCursorPosition(x, y[x]);
                    Console.Write(' ');

                    int temp1 = y[x] - 20;
                    Console.SetCursorPosition(x, YPositionFields(temp1, height));
                    Console.Write(' ');

                    if (counter > fastflow && counter < textflow)
                    {
                        if (x % 10 == 9) Console.ForegroundColor = fadeColor;
                        else Console.ForegroundColor = baseColor;

                        int temp = y[x] - 2;
                        Console.SetCursorPosition(x, YPositionFields(temp, height));
                        Console.Write(AscciCharecters);

                    }
                    Console.SetCursorPosition(width / 2, height / 2);
                    Console.Write(endtext);
                    y[x] = YPositionFields(y[x] + 1, height);
                }
                return;
            }
        }
    }
}
