using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Win32;

namespace CSharpcode {
    public class MultipleChoiceExam : Window {
        Label headGivenLabel = new Label();
        Label givenLabel = new Label();
        Label headAnswerLabel = new Label();
        Label[] answerLabel = new Label[4];
        TextBox answerTextField = new TextBox();
        TextBox commentTextArea = new TextBox();
        Button nextButton = new Button();
        Button startButton = new Button();

        Menu mainMenuBar = new Menu();
        MenuItem fileMenu = new MenuItem { Header = "File" };
        MenuItem openMenuItem = new MenuItem { Header = "Open" };
        MenuItem exitMenuItem = new MenuItem { Header = "Exit" };
        MenuItem optionsMenu = new MenuItem { Header = "Options" };
        MenuItem header1MenuItem = new MenuItem { Header = "Header 1", IsCheckable = true, IsChecked = true };
        MenuItem header2MenuItem = new MenuItem { Header = "Header 2", IsCheckable = true, IsChecked = false };
        MenuItem mcMenuItem = new MenuItem { Header = "Multiple Choice Answers", IsCheckable = true, IsChecked = true };
        MenuItem typeMenuItem = new MenuItem { Header = "Type In Answers", IsCheckable = true, IsChecked = false };

        string examTitle = "";
        string header1 = "", header2 = "";
        int numberTerms;
        string[] term1 = new string[100];
        string[] term2 = new string[100];
        int numberTried, numberCorrect;
        int correctAnswer;
        Random myRandom = new Random();

        public MultipleChoiceExam() {
            Title = "Multiple Choice Exam - No File";
            ResizeMode = ResizeMode.NoResize;
            Width = 450; Height = 600;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Closed += exitForm;

            Grid grid = new Grid();
            for (int i = 0; i < 11; i++) grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            
            // Menu
            fileMenu.Items.Add(openMenuItem);
            fileMenu.Items.Add(new Separator());
            fileMenu.Items.Add(exitMenuItem);
            optionsMenu.Items.Add(header1MenuItem);
            optionsMenu.Items.Add(header2MenuItem);
            optionsMenu.Items.Add(new Separator());
            optionsMenu.Items.Add(mcMenuItem);
            optionsMenu.Items.Add(typeMenuItem);
            mainMenuBar.Items.Add(fileMenu);
            mainMenuBar.Items.Add(optionsMenu);
            
            Grid.SetRow(mainMenuBar, 0);
            grid.Children.Add(mainMenuBar);

            // Labels & Buttons
            headGivenLabel.FontWeight = FontWeights.Bold; headGivenLabel.FontSize = 18;
            headGivenLabel.Margin = new Thickness(10, 10, 10, 0);
            Grid.SetRow(headGivenLabel, 1); grid.Children.Add(headGivenLabel);

            givenLabel.FontWeight = FontWeights.Bold; givenLabel.FontSize = 16;
            givenLabel.Background = Brushes.White; givenLabel.Foreground = Brushes.Blue;
            givenLabel.BorderBrush = Brushes.Black; givenLabel.BorderThickness = new Thickness(1);
            givenLabel.HorizontalContentAlignment = HorizontalAlignment.Center;
            givenLabel.Height = 30; givenLabel.Margin = new Thickness(10, 0, 10, 0);
            Grid.SetRow(givenLabel, 2); grid.Children.Add(givenLabel);

            headAnswerLabel.FontWeight = FontWeights.Bold; headAnswerLabel.FontSize = 18;
            headAnswerLabel.Margin = new Thickness(10, 10, 10, 0);
            Grid.SetRow(headAnswerLabel, 3); grid.Children.Add(headAnswerLabel);

            for (int i = 0; i < 4; i++) {
                answerLabel[i] = new Label();
                answerLabel[i].FontWeight = FontWeights.Bold; answerLabel[i].FontSize = 16;
                answerLabel[i].Background = Brushes.White; answerLabel[i].Foreground = Brushes.Blue;
                answerLabel[i].BorderBrush = Brushes.Black; answerLabel[i].BorderThickness = new Thickness(1);
                answerLabel[i].HorizontalContentAlignment = HorizontalAlignment.Center;
                answerLabel[i].Height = 30; answerLabel[i].Margin = new Thickness(10, 0, 10, 10);
                Grid.SetRow(answerLabel[i], i + 4); grid.Children.Add(answerLabel[i]);
                
                int index = i;
                answerLabel[i].MouseLeftButtonDown += (s, e) => answerLabelMousePressed(answerLabel[index]);
            }

            answerTextField.FontWeight = FontWeights.Bold; answerTextField.FontSize = 16;
            answerTextField.Background = Brushes.White; answerTextField.Foreground = Brushes.Blue;
            answerTextField.Height = 30; answerTextField.Margin = new Thickness(10, 0, 10, 10);
            answerTextField.Visibility = Visibility.Collapsed;
            answerTextField.KeyDown += answerTextFieldActionPerformed;
            Grid.SetRow(answerTextField, 4); grid.Children.Add(answerTextField);

            commentTextArea.FontFamily = new FontFamily("Courier New");
            commentTextArea.FontWeight = FontWeights.Bold; commentTextArea.FontStyle = FontStyles.Italic;
            commentTextArea.FontSize = 16;
            commentTextArea.Background = new SolidColorBrush(Color.FromRgb(255, 255, 192));
            commentTextArea.Foreground = Brushes.Red;
            commentTextArea.BorderBrush = Brushes.Black; commentTextArea.BorderThickness = new Thickness(1);
            commentTextArea.Height = 80; commentTextArea.Margin = new Thickness(10, 0, 10, 10);
            commentTextArea.IsReadOnly = true;
            Grid.SetRow(commentTextArea, 8); grid.Children.Add(commentTextArea);

            nextButton.Content = "Next Question"; nextButton.Margin = new Thickness(100, 0, 100, 10);
            Grid.SetRow(nextButton, 9); grid.Children.Add(nextButton);

            startButton.Content = "Start Exam"; startButton.Margin = new Thickness(100, 0, 100, 10);
            Grid.SetRow(startButton, 10); grid.Children.Add(startButton);

            Content = grid;

            // Events bind
            openMenuItem.Click += openMenuItemActionPerformed;
            exitMenuItem.Click += exitMenuItemActionPerformed;
            header1MenuItem.Click += header1MenuItemActionPerformed;
            header2MenuItem.Click += header2MenuItemActionPerformed;
            mcMenuItem.Click += mcMenuItemActionPerformed;
            typeMenuItem.Click += typeMenuItemActionPerformed;
            nextButton.Click += nextButtonActionPerformed;
            startButton.Click += startButtonActionPerformed;

            // Init
            startButton.IsEnabled = false;
            nextButton.IsEnabled = false;
            optionsMenu.IsEnabled = false;
            commentTextArea.Text = centerTextArea("Open Exam File to Start");
        }

        private void exitForm(object? sender, EventArgs e) => System.Windows.Application.Current.Shutdown();
        private void exitMenuItemActionPerformed(object? sender, RoutedEventArgs e) => System.Windows.Application.Current.Shutdown();

        private void answerLabelMousePressed(Label clickedLabel) {
            if (startButton.Content.ToString() == "Start Exam" || nextButton.IsEnabled) return;

            int labelSelected = Array.IndexOf(answerLabel, clickedLabel);
            numberTried++;
            bool correct = false;

            if (header1MenuItem.IsChecked) {
                if (clickedLabel.Content?.ToString() == term1[correctAnswer]) correct = true;
            } else {
                if (clickedLabel.Content?.ToString() == term2[correctAnswer]) correct = true;
            }
            updateScore(correct);
        }

        private void answerTextFieldActionPerformed(object? sender, KeyEventArgs e) {
            if (e.Key != Key.Enter || startButton.Content.ToString() == "Start Exam" || nextButton.IsEnabled) return;

            answerTextField.IsReadOnly = true;
            numberTried++;
            string ucTypedAnswer = answerTextField.Text.ToUpper();
            string ucAnswer = (header1MenuItem.IsChecked ? term1[correctAnswer] : term2[correctAnswer]).ToUpper();

            bool correct = false;
            if (ucTypedAnswer == ucAnswer || soundex(ucTypedAnswer) == soundex(ucAnswer)) correct = true;
            
            updateScore(correct);
        }

        private void nextButtonActionPerformed(object? sender, RoutedEventArgs e) {
            nextButton.IsEnabled = false;
            nextQuestion();
        }

        private void startButtonActionPerformed(object? sender, RoutedEventArgs e) {
            if (startButton.Content.ToString() == "Start Exam") {
                startButton.Content = "Stop Exam";
                nextButton.IsEnabled = false;
                numberTried = 0; numberCorrect = 0;
                commentTextArea.Text = "";
                fileMenu.IsEnabled = false; optionsMenu.IsEnabled = false;
                nextQuestion();
            } else {
                startButton.Content = "Start Exam";
                nextButton.IsEnabled = false;
                if (numberTried > 0) {
                    string msg = $"Questions Tried: {numberTried}\nQuestions Correct: {numberCorrect}\n\nYour Score: {(100.0 * numberCorrect / numberTried):0.0}%";
                    MessageBox.Show(msg, examTitle + " Results", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                givenLabel.Content = "";
                for (int i=0; i<4; i++) answerLabel[i].Content = "";
                answerTextField.Text = "";
                commentTextArea.Text = centerTextArea("Choose Options\nClick Start Exam");
                fileMenu.IsEnabled = true; optionsMenu.IsEnabled = true;
            }
        }

        private void openMenuItemActionPerformed(object? sender, RoutedEventArgs e) {
            OpenFileDialog openChooser = new OpenFileDialog { Filter = "Exam Files|*.csv" };
            if (openChooser.ShowDialog() == true) {
                try {
                    string[] lines = File.ReadAllLines(openChooser.FileName);
                    if (lines.Length < 3) throw new Exception();

                    examTitle = parseLeft(lines[0]);
                    header1 = parseLeft(lines[1]);
                    header2 = parseRight(lines[1]);
                    numberTerms = 0;

                    for (int i = 2; i < lines.Length && numberTerms < 100; i++) {
                        if (string.IsNullOrWhiteSpace(lines[i])) continue;
                        numberTerms++;
                        term1[numberTerms - 1] = parseLeft(lines[i]);
                        term2[numberTerms - 1] = parseRight(lines[i]);
                    }

                    if (numberTerms < 5) {
                        MessageBox.Show("Must have at least 5 entries in exam file.", "Exam File Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    Title = "Multiple Choice Exam - " + examTitle;
                    header1MenuItem.Header = header1 + ", Given " + header2;
                    header2MenuItem.Header = header2 + ", Given " + header1;

                    if (header1MenuItem.IsChecked) {
                        headGivenLabel.Content = header2; headAnswerLabel.Content = header1;
                    } else {
                        headGivenLabel.Content = header1; headAnswerLabel.Content = header2;
                    }

                    startButton.IsEnabled = true; optionsMenu.IsEnabled = true;
                    commentTextArea.Text = centerTextArea("File Loaded, Choose Options\nClick Start Exam");

                } catch {
                    MessageBox.Show("Error reading in input file - make sure file is correct format.", "Multiple Choice Exam File Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void header1MenuItemActionPerformed(object? sender, RoutedEventArgs e) {
            header1MenuItem.IsChecked = true; header2MenuItem.IsChecked = false;
            headGivenLabel.Content = header2; headAnswerLabel.Content = header1;
        }

        private void header2MenuItemActionPerformed(object? sender, RoutedEventArgs e) {
            header2MenuItem.IsChecked = true; header1MenuItem.IsChecked = false;
            headGivenLabel.Content = header1; headAnswerLabel.Content = header2;
        }

        private void mcMenuItemActionPerformed(object? sender, RoutedEventArgs e) {
            mcMenuItem.IsChecked = true; typeMenuItem.IsChecked = false;
            for (int i=0; i<4; i++) answerLabel[i].Visibility = Visibility.Visible;
            answerTextField.Visibility = Visibility.Collapsed;
        }

        private void typeMenuItemActionPerformed(object? sender, RoutedEventArgs e) {
            typeMenuItem.IsChecked = true; mcMenuItem.IsChecked = false;
            for (int i=0; i<4; i++) answerLabel[i].Visibility = Visibility.Collapsed;
            answerTextField.Visibility = Visibility.Visible;
        }

        private string parseLeft(string s) {
            int cl = s.IndexOf(",");
            return cl != -1 ? s.Substring(0, cl) : s;
        }

        private string parseRight(string s) {
            int cl = s.IndexOf(",");
            return cl != -1 ? s.Substring(cl + 1) : "";
        }

        private string centerTextArea(string s) {
            int charsPerLine = 33;
            int j = s.IndexOf("\n");
            if (j == -1) return "\n" + spacePadding((charsPerLine - s.Length) / 2) + s;
            
            string l1 = s.Substring(0, j);
            string l2 = s.Substring(j + 1);
            return "\n" + spacePadding((charsPerLine - l1.Length) / 2) + l1 + "\n" + spacePadding((charsPerLine - l2.Length) / 2) + l2;
        }

        private string spacePadding(int n) => n > 0 ? new string(' ', n) : "";

        private void nextQuestion() {
            bool[] termUsed = new bool[numberTerms];
            int[] index = new int[4];
            commentTextArea.Text = "";

            correctAnswer = myRandom.Next(numberTerms);
            givenLabel.Content = header1MenuItem.IsChecked ? term2[correctAnswer] : term1[correctAnswer];

            if (mcMenuItem.IsChecked) {
                for (int i = 0; i < 4; i++) {
                    int j;
                    do { j = myRandom.Next(numberTerms); } while (termUsed[j] || j == correctAnswer);
                    termUsed[j] = true; index[i] = j;
                }
                index[myRandom.Next(4)] = correctAnswer;

                for (int i = 0; i < 4; i++) {
                    answerLabel[i].Content = header1MenuItem.IsChecked ? term1[index[i]] : term2[index[i]];
                }
            } else {
                answerTextField.IsReadOnly = false;
                answerTextField.Text = "";
                answerTextField.Focus();
            }
        }

        private void updateScore(bool correct) {
            if (correct) {
                numberCorrect++;
                commentTextArea.Text = centerTextArea("Correct!");
            } else {
                commentTextArea.Text = centerTextArea("Sorry ... Correct Answer Shown");
            }

            if (mcMenuItem.IsChecked) {
                answerLabel[0].Content = header1MenuItem.IsChecked ? term1[correctAnswer] : term2[correctAnswer];
                for (int i=1; i<4; i++) answerLabel[i].Content = "";
            } else {
                answerTextField.Text = header1MenuItem.IsChecked ? term1[correctAnswer] : term2[correctAnswer];
            }

            startButton.IsEnabled = true; nextButton.IsEnabled = true; nextButton.Focus();
        }

        public string soundex(string w) {
            int[] wSound = {0,1,2,3,0,1,2,0,0,2,2,4,5,5,0,1,2,6,2,3,0,1,0,2,0,2};
            string wTemp = w.ToUpper();
            int l = w.Length;
            string s = "";

            if (l != 0) {
                s = wTemp[0].ToString();
                int wPrev = 0;
                if (l > 1) {
                    for (int i = 1; i < l; i++) {
                        int cIndex = (int)wTemp[i] - 65;
                        if (cIndex >= 0 && cIndex <= 25) {
                            int wSnd = wSound[cIndex] + 48;
                            if (wSnd != 48 && wSnd != wPrev) {
                                s += (char)wSnd;
                            }
                            wPrev = wSnd;
                        }
                    }
                }
            }
            return s;
        }

        [STAThread]
        public static void Main(string[] args) {
            Application app = new Application();
            MultipleChoiceExam window = new MultipleChoiceExam();
            app.Run(window);
        }
    }
}