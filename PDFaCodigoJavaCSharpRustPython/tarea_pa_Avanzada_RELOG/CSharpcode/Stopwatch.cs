using System;
using System.Windows;
using System.Windows.Controls;

namespace CSharpcode {
    public class Stopwatch : Window {
        // declare controls used
        Button startButton = new Button();
        Button stopButton = new Button();
        Button exitButton = new Button();
        Label startLabel = new Label();
        Label stopLabel = new Label();
        Label elapsedLabel = new Label();
        TextBox startTextField = new TextBox();
        TextBox stopTextField = new TextBox();
        TextBox elapsedTextField = new TextBox();

        // declare class level variables
        long startTime;
        long stopTime;
        double elapsedTime;

        public Stopwatch() {
            Title = "Stopwatch Application";
            Width = 400;
            Height = 200;
            Closed += exitForm; // addWindowListener equivalente

            // Configurar el GridLayout
            Grid grid = new Grid();
            grid.Margin = new Thickness(10);
            for (int i = 0; i < 3; i++) {
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            }

            // Configurar y añadir controles
            startButton.Content = "Start Timing";
            startButton.Click += startButtonActionPerformed;
            Grid.SetColumn(startButton, 0); Grid.SetRow(startButton, 0);
            grid.Children.Add(startButton);

            stopButton.Content = "Stop Timing";
            stopButton.Click += stopButtonActionPerformed;
            Grid.SetColumn(stopButton, 0); Grid.SetRow(stopButton, 1);
            grid.Children.Add(stopButton);

            exitButton.Content = "Exit";
            exitButton.Click += exitButtonActionPerformed;
            Grid.SetColumn(exitButton, 0); Grid.SetRow(exitButton, 2);
            grid.Children.Add(exitButton);

            startLabel.Content = "Start Time";
            Grid.SetColumn(startLabel, 1); Grid.SetRow(startLabel, 0);
            grid.Children.Add(startLabel);

            stopLabel.Content = "Stop Time";
            Grid.SetColumn(stopLabel, 1); Grid.SetRow(stopLabel, 1);
            grid.Children.Add(stopLabel);

            elapsedLabel.Content = "Elapsed Time (sec)";
            Grid.SetColumn(elapsedLabel, 1); Grid.SetRow(elapsedLabel, 2);
            grid.Children.Add(elapsedLabel);

            startTextField.Text = "";
            Grid.SetColumn(startTextField, 2); Grid.SetRow(startTextField, 0);
            grid.Children.Add(startTextField);

            stopTextField.Text = "";
            Grid.SetColumn(stopTextField, 2); Grid.SetRow(stopTextField, 1);
            grid.Children.Add(stopTextField);

            elapsedTextField.Text = "";
            Grid.SetColumn(elapsedTextField, 2); Grid.SetRow(elapsedTextField, 2);
            grid.Children.Add(elapsedTextField);

            Content = grid;
        }

        private void startButtonActionPerformed(object? sender, RoutedEventArgs e) {
            startTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            startTextField.Text = startTime.ToString();
            stopTextField.Text = "";
            elapsedTextField.Text = "";
        }

        private void stopButtonActionPerformed(object? sender, RoutedEventArgs e) {
            stopTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            stopTextField.Text = stopTime.ToString();
            elapsedTime = (stopTime - startTime) / 1000.0;
            elapsedTextField.Text = elapsedTime.ToString();
        }

        private void exitButtonActionPerformed(object? sender, RoutedEventArgs e) {
            System.Windows.Application.Current.Shutdown();
        }

private void exitForm(object? sender, EventArgs e) {
        System.Windows.Application.Current.Shutdown();
    }

    // Pega el método Main AQUÍ, dentro de la clase Stopwatch
    [STAThread]
    public static void Main(string[] args) {
        Application app = new Application();
        Stopwatch window = new Stopwatch();
        app.Run(window);
    }
} // <- Llave que cierra la clase Stopwatch
} // <- Llave que cierra el namespace
