using SamplesAPI;
using System.Windows.Controls;
using System.Windows;

namespace SecureStringSamples
{
    public class UISample : ISample
    {
        public void Run()
        {
            DockPanel layout = new DockPanel();

            TextBlock label = new TextBlock { Text = "Password:", FontWeight = FontWeights.Bold, Margin = new Thickness(5) };
            label.SetValue(DockPanel.DockProperty, Dock.Left);

            PasswordBox passwordInput = new PasswordBox { Width = 150, Margin = new Thickness(5) };

            layout.Children.Add(label);
            layout.Children.Add(passwordInput);

            Window window = new Window
            {
                Title = "Your password",
                Content = layout,
                SizeToContent = SizeToContent.WidthAndHeight
            };

            new Application().Run(window);
        }
    }
}
