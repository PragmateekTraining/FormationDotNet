using System;
using System.Threading;

namespace SamplesAPI
{
    public class Color : IDisposable
    {
        private ConsoleColor originalForeground;

        public static Color Red { get { return new Color(ConsoleColor.Red); } }
        public static Color Yellow { get { return new Color(ConsoleColor.Yellow); } }
        public static Color Green { get { return new Color(ConsoleColor.Green); } }
        public static Color Gray { get { return new Color(ConsoleColor.Gray); } }
        public static Color Blue { get { return new Color(ConsoleColor.Blue); } }
        public static Color Cyan { get { return new Color(ConsoleColor.Cyan); } }

        bool isSafe = false;

        public Color(ConsoleColor foreground, bool isSafe = false)
        {
            this.isSafe = isSafe;

            if (isSafe)
            {
                Monitor.Enter(Console.Out);
            }

            // keep track of the current color
            originalForeground = Console.ForegroundColor;

            // change the console color
            Console.ForegroundColor = foreground;
        }

        public void Dispose()
        {
            // revert the console color
            Console.ForegroundColor = originalForeground;

            if (isSafe)
            {
                Monitor.Exit(Console.Out);
            }
        }
    }
}
