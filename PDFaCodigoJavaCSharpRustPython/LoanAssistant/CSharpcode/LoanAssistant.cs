using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CSharpcode {
    public class LoanAssistant : Window {
        Label balanceLabel = new Label();
        TextBox balanceTextField = new TextBox();
        Label interestLabel = new Label();
        TextBox interestTextField = new TextBox();
        Label monthsLabel = new Label();
        TextBox monthsTextField = new TextBox();
        Label paymentLabel = new Label();
        TextBox paymentTextField = new TextBox();
        Button computeButton = new Button();
        Button newLoanButton = new Button();
        Button monthsButton = new Button();
        Button paymentButton = new Button();
        Label analysisLabel = new Label();
        TextBox analysisTextArea = new TextBox();
        Button exitButton = new Button();
        
        FontFamily myFont = new FontFamily("Arial");
        SolidColorBrush lightYellow = new SolidColorBrush(Color.FromRgb(255, 255, 128));
        bool computePayment;

        public LoanAssistant() {
            Title = "Loan Assistant";
            ResizeMode = ResizeMode.NoResize;
            Width = 650; Height = 350;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Closed += exitForm;

            Grid grid = new Grid();
            for (int i = 0; i < 6; i++) grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            for (int i = 0; i < 4; i++) grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            // Balance
            balanceLabel.Content = "Loan Balance"; balanceLabel.FontFamily = myFont; balanceLabel.FontSize = 14;
            balanceLabel.Margin = new Thickness(10, 10, 10, 0);
            Grid.SetRow(balanceLabel, 0); Grid.SetColumn(balanceLabel, 0); grid.Children.Add(balanceLabel);

            balanceTextField.Width = 100; balanceTextField.Height = 25; balanceTextField.FontFamily = myFont;
            balanceTextField.TextAlignment = TextAlignment.Right; balanceTextField.Margin = new Thickness(0, 10, 10, 0);
            balanceTextField.KeyDown += balanceTextFieldActionPerformed;
            Grid.SetRow(balanceTextField, 0); Grid.SetColumn(balanceTextField, 1); grid.Children.Add(balanceTextField);

            // Interest
            interestLabel.Content = "Interest Rate"; interestLabel.FontFamily = myFont; interestLabel.FontSize = 14;
            interestLabel.Margin = new Thickness(10, 10, 10, 0);
            Grid.SetRow(interestLabel, 1); Grid.SetColumn(interestLabel, 0); grid.Children.Add(interestLabel);

            interestTextField.Width = 100; interestTextField.Height = 25; interestTextField.FontFamily = myFont;
            interestTextField.TextAlignment = TextAlignment.Right; interestTextField.Margin = new Thickness(0, 10, 10, 0);
            interestTextField.KeyDown += interestTextFieldActionPerformed;
            Grid.SetRow(interestTextField, 1); Grid.SetColumn(interestTextField, 1); grid.Children.Add(interestTextField);

            // Months
            monthsLabel.Content = "Number of Payments"; monthsLabel.FontFamily = myFont; monthsLabel.FontSize = 14;
            monthsLabel.Margin = new Thickness(10, 10, 10, 0);
            Grid.SetRow(monthsLabel, 2); Grid.SetColumn(monthsLabel, 0); grid.Children.Add(monthsLabel);

            monthsTextField.Width = 100; monthsTextField.Height = 25; monthsTextField.FontFamily = myFont;
            monthsTextField.TextAlignment = TextAlignment.Right; monthsTextField.Margin = new Thickness(0, 10, 10, 0);
            monthsTextField.KeyDown += monthsTextFieldActionPerformed;
            Grid.SetRow(monthsTextField, 2); Grid.SetColumn(monthsTextField, 1); grid.Children.Add(monthsTextField);

            monthsButton.Content = "X"; monthsButton.Width = 30; monthsButton.Height = 25; monthsButton.Margin = new Thickness(0, 10, 10, 0);
            monthsButton.Focusable = false; monthsButton.Click += monthsButtonActionPerformed;
            Grid.SetRow(monthsButton, 2); Grid.SetColumn(monthsButton, 2); grid.Children.Add(monthsButton);

            // Payment
            paymentLabel.Content = "Monthly Payment"; paymentLabel.FontFamily = myFont; paymentLabel.FontSize = 14;
            paymentLabel.Margin = new Thickness(10, 10, 10, 0);
            Grid.SetRow(paymentLabel, 3); Grid.SetColumn(paymentLabel, 0); grid.Children.Add(paymentLabel);

            paymentTextField.Width = 100; paymentTextField.Height = 25; paymentTextField.FontFamily = myFont;
            paymentTextField.TextAlignment = TextAlignment.Right; paymentTextField.Margin = new Thickness(0, 10, 10, 0);
            paymentTextField.KeyDown += paymentTextFieldActionPerformed;
            Grid.SetRow(paymentTextField, 3); Grid.SetColumn(paymentTextField, 1); grid.Children.Add(paymentTextField);

            paymentButton.Content = "X"; paymentButton.Width = 30; paymentButton.Height = 25; paymentButton.Margin = new Thickness(0, 10, 10, 0);
            paymentButton.Focusable = false; paymentButton.Click += paymentButtonActionPerformed;
            Grid.SetRow(paymentButton, 3); Grid.SetColumn(paymentButton, 2); grid.Children.Add(paymentButton);

            // Buttons
            computeButton.Content = "Compute Monthly Payment"; computeButton.Margin = new Thickness(10, 10, 0, 0);
            computeButton.Click += computeButtonActionPerformed;
            Grid.SetRow(computeButton, 4); Grid.SetColumnSpan(computeButton, 2); grid.Children.Add(computeButton);

            newLoanButton.Content = "New Loan Analysis"; newLoanButton.Margin = new Thickness(10, 10, 0, 10);
            newLoanButton.IsEnabled = false; newLoanButton.Click += newLoanButtonActionPerformed;
            Grid.SetRow(newLoanButton, 5); Grid.SetColumnSpan(newLoanButton, 2); grid.Children.Add(newLoanButton);

            // Analysis Area
            analysisLabel.Content = "Loan Analysis: "; analysisLabel.FontFamily = myFont; analysisLabel.FontSize = 14;
            analysisLabel.Margin = new Thickness(10, 10, 0, 0);
            Grid.SetRow(analysisLabel, 0); Grid.SetColumn(analysisLabel, 3); grid.Children.Add(analysisLabel);

            analysisTextArea.Width = 250; analysisTextArea.Height = 150; analysisTextArea.Margin = new Thickness(10, 0, 10, 0);
            analysisTextArea.FontFamily = new FontFamily("Courier New"); analysisTextArea.FontSize = 14;
            analysisTextArea.IsReadOnly = true; analysisTextArea.Focusable = false;
            analysisTextArea.Background = Brushes.White; analysisTextArea.BorderBrush = Brushes.Black;
            analysisTextArea.TextWrapping = TextWrapping.Wrap; analysisTextArea.AcceptsReturn = true;
            Grid.SetRow(analysisTextArea, 1); Grid.SetColumn(analysisTextArea, 3); Grid.SetRowSpan(analysisTextArea, 4); grid.Children.Add(analysisTextArea);

            exitButton.Content = "Exit"; exitButton.Width = 60; exitButton.Height = 25; exitButton.Margin = new Thickness(10, 10, 10, 10);
            exitButton.Focusable = false; exitButton.Click += exitButtonActionPerformed;
            Grid.SetRow(exitButton, 5); Grid.SetColumn(exitButton, 3); grid.Children.Add(exitButton);

            Content = grid;
            
            // Simular el click inicial de Java
            paymentButtonActionPerformed(null, null);
        }

        private void exitForm(object? sender, EventArgs e) => Application.Current.Shutdown();
        private void exitButtonActionPerformed(object? sender, RoutedEventArgs e) => Application.Current.Shutdown();

        private void balanceTextFieldActionPerformed(object? sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) (sender as UIElement)?.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
        }
        private void interestTextFieldActionPerformed(object? sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) (sender as UIElement)?.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
        }
        private void monthsTextFieldActionPerformed(object? sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) (sender as UIElement)?.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
        }
        private void paymentTextFieldActionPerformed(object? sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) (sender as UIElement)?.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
        }

        private bool validateDecimalNumber(TextBox tf) {
            string s = tf.Text.Trim();
            bool hasDecimal = false;
            bool valid = true;

            if (s.Length == 0) valid = false;
            else {
                for (int i = 0; i < s.Length; i++) {
                    char c = s[i];
                    if (c >= '0' && c <= '9') continue;
                    else if (c == '.' && !hasDecimal) hasDecimal = true;
                    else { valid = false; break; }
                }
            }
            tf.Text = s;
            if (!valid) tf.Focus();
            return valid;
        }

        private void monthsButtonActionPerformed(object? sender, RoutedEventArgs? e) {
            computePayment = false;
            paymentButton.Visibility = Visibility.Visible;
            monthsButton.Visibility = Visibility.Hidden;
            
            monthsTextField.Text = "";
            monthsTextField.IsReadOnly = true; monthsTextField.Background = lightYellow; monthsTextField.Focusable = false;
            paymentTextField.IsReadOnly = false; paymentTextField.Background = Brushes.White; paymentTextField.Focusable = true;
            
            computeButton.Content = "Compute Number of Payments";
            balanceTextField.Focus();
        }

        private void paymentButtonActionPerformed(object? sender, RoutedEventArgs? e) {
            computePayment = true;
            paymentButton.Visibility = Visibility.Hidden;
            monthsButton.Visibility = Visibility.Visible;
            
            monthsTextField.IsReadOnly = false; monthsTextField.Background = Brushes.White; monthsTextField.Focusable = true;
            paymentTextField.Text = "";
            paymentTextField.IsReadOnly = true; paymentTextField.Background = lightYellow; paymentTextField.Focusable = false;
            
            computeButton.Content = "Compute Monthly Payment";
            balanceTextField.Focus();
        }

        private void newLoanButtonActionPerformed(object? sender, RoutedEventArgs e) {
            if (computePayment) paymentTextField.Text = "";
            else monthsTextField.Text = "";
            
            analysisTextArea.Text = "";
            computeButton.IsEnabled = true;
            newLoanButton.IsEnabled = false;
            balanceTextField.Focus();
        }

        private void computeButtonActionPerformed(object? sender, RoutedEventArgs e) {
            double balance, interest, payment;
            int months;
            double monthlyInterest, multiplier, loanBalance, finalPayment;

            if (validateDecimalNumber(balanceTextField)) balance = double.Parse(balanceTextField.Text);
            else { MessageBox.Show("Invalid or empty Loan Balance entry.\nPlease correct.", "Balance Input Error", MessageBoxButton.OK, MessageBoxImage.Information); return; }

            if (validateDecimalNumber(interestTextField)) interest = double.Parse(interestTextField.Text);
            else { MessageBox.Show("Invalid or empty Interest Rate entry.\nPlease correct.", "Interest Input Error", MessageBoxButton.OK, MessageBoxImage.Information); return; }

            monthlyInterest = interest / 1200;

            if (computePayment) {
                if (validateDecimalNumber(monthsTextField)) months = int.Parse(monthsTextField.Text);
                else { MessageBox.Show("Invalid or empty Number of Payments entry.\nPlease correct.", "Number of Payments Input Error", MessageBoxButton.OK, MessageBoxImage.Information); return; }

                if (interest == 0) payment = balance / months;
                else {
                    multiplier = Math.Pow(1 + monthlyInterest, months);
                    payment = balance * monthlyInterest * multiplier / (multiplier - 1);
                }
                paymentTextField.Text = payment.ToString("F2");
            } else {
                if (validateDecimalNumber(paymentTextField)) {
                    payment = double.Parse(paymentTextField.Text);
                    if (payment <= (balance * monthlyInterest + 1.0)) {
                        if (MessageBox.Show("Minimum payment must be $" + ((int)(balance * monthlyInterest + 1.0)).ToString("F2") + "\nDo you want to use the minimum payment?", "Input Error", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes) {
                            paymentTextField.Text = ((int)(balance * monthlyInterest + 1.0)).ToString("F2");
                            payment = double.Parse(paymentTextField.Text);
                        } else { paymentTextField.Focus(); return; }
                    }
                } else { MessageBox.Show("Invalid or empty Monthly Payment entry.\nPlease correct.", "Payment Input Error", MessageBoxButton.OK, MessageBoxImage.Information); return; }

                if (interest == 0) months = (int)(balance / payment);
                else months = (int)((Math.Log(payment) - Math.Log(payment - balance * monthlyInterest)) / Math.Log(1 + monthlyInterest));
                
                monthsTextField.Text = months.ToString();
            }

            payment = double.Parse(paymentTextField.Text);
            analysisTextArea.Text = "Loan Balance: $" + balance.ToString("F2");
            analysisTextArea.Text += "\nInterest Rate: " + interest.ToString("F2") + "%";

            loanBalance = balance;
            for (int paymentNumber = 1; paymentNumber <= months - 1; paymentNumber++) {
                loanBalance += loanBalance * monthlyInterest - payment;
            }

            finalPayment = loanBalance;
            if (finalPayment > payment) {
                loanBalance += loanBalance * monthlyInterest - payment;
                finalPayment = loanBalance;
                months++;
                monthsTextField.Text = months.ToString();
            }

            analysisTextArea.Text += "\n\n" + (months - 1).ToString() + " Payments of $" + payment.ToString("F2");
            analysisTextArea.Text += "\nFinal Payment of: $" + finalPayment.ToString("F2");
            analysisTextArea.Text += "\nTotal Payments: $" + ((months - 1) * payment + finalPayment).ToString("F2");
            analysisTextArea.Text += "\nInterest Paid $" + ((months - 1) * payment + finalPayment - balance).ToString("F2");

            computeButton.IsEnabled = false;
            newLoanButton.IsEnabled = true;
            newLoanButton.Focus();
        }

        [STAThread]
        public static void Main(string[] args) {
            Application app = new Application();
            LoanAssistant window = new LoanAssistant();
            app.Run(window);
        }
    }
}