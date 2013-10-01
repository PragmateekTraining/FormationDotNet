using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SamplesAPI
{
    public class Color : IDisposable
    {
        private ConsoleColor originalForeground;

        public static Color Red { get { return new Color(ConsoleColor.Red); } }
        public static Color Green { get { return new Color(ConsoleColor.Green); } }
        public static Color Blue { get { return new Color(ConsoleColor.Blue); } }

        public Color(ConsoleColor foreground)
        {
            originalForeground = Console.ForegroundColor;

            // change the console color
            Console.ForegroundColor = foreground;
        }

        public void Dispose()
        {
            // revert the console color
            Console.ForegroundColor = originalForeground;
        }
    }
}
