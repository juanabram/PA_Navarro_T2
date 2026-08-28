using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CSharpCode {
    public class Vstopwatch : Window {
        private Button startButton = new Button { Content = "Start", Width = 60, Margin = new Thickness(5) };
        private Button stopButton = new Button { Content = "Stop", Width = 60, Margin = new Thickness(5) };
        private Button exitButton = new Button { Content = "Exit", Width = 60, Margin = new Thickness(5) };
        private TextBox timeField = new TextBox { Text = "00:00:00" };

        public Vstopwatch() {
            Title = "Stopwatch MVC";
            Width = 300; Height = 200;

            timeField.IsReadOnly = true;
            timeField.FontSize = 24;
            timeField.FontWeight = FontWeights.Bold;
            timeField.TextAlignment = TextAlignment.Center;
            timeField.Margin = new Thickness(10);

            // Equivalente a FlowLayout
            WrapPanel panel = new WrapPanel { HorizontalAlignment = HorizontalAlignment.Center };
            panel.Children.Add(timeField);
            panel.Children.Add(startButton);
            panel.Children.Add(stopButton);
            panel.Children.Add(exitButton);

            Content = panel;
        }

        public Button getStartButton() { return startButton; }
        public Button getStopButton() { return stopButton; }
        public Button getExitButton() { return exitButton; }
        public void setTime(string time) { timeField.Text = time; }
    }
}