using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Exceptions
{
    class Program
    {
        static void RunAppDomain()
        {
            AppDomain.CurrentDomain.UnhandledException += (s, a) =>
            {
                Console.WriteLine("Ooops! Something bad happened: '{0}'!", (a.ExceptionObject as Exception).Message);
            };

            while (true)
            {
                Console.Write("Type some text to throw an exception or enter to quit: ");
                string message = Console.ReadLine();

                if (message == "") break;

                throw new Exception(message);
            }
        }

        static void RunWpf()
        {
            Application app = new Application();
            app.DispatcherUnhandledException += (s, a) =>
            {
                Console.WriteLine("Ooops! Something bad happened: '{0}'!", a.Exception.Message);
                a.Handled = true;
            };

            TextBox input = new TextBox();
            Button button = new Button{ Content = "Throw it!" };
            button.Click += (s, a) =>
                {
                    throw new Exception(input.Text);
                };

            StackPanel layout = new StackPanel();
            layout.Children.Add(input);
            layout.Children.Add(button);

            app.Run(new Window
            {
                SizeToContent = SizeToContent.WidthAndHeight,
                Content = layout
            });
        }

        [STAThread]
        static void Main(string[] args)
        {
            // RunWpf();
            new RethrowSample(args.Length == 1 && args[0] == "rethrow").Run();
        }
    }
}
