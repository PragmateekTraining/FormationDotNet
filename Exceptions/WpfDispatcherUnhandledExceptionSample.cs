using System;
using SamplesAPI;
using System.Windows;
using System.Windows.Controls;

namespace ExceptionsSamples
{
    public class WpfDispatcherUnhandledExceptionSample : ISample
    {
        public void Run()
        {
            Application app = new Application();
            app.DispatcherUnhandledException += (s, a) =>
            {
                Console.WriteLine("Ooops! Something bad happened: '{0}'!", a.Exception.Message);
                a.Handled = true;
            };

            TextBox input = new TextBox();
            Button button = new Button { Content = "Throw it!" };
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
    }
}
