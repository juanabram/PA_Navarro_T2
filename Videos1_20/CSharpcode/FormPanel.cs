using System.Windows;
using System.Windows.Controls;

namespace CSharpCode {
    public class FormPanel : UserControl {
        private Label nameLabel;
        private Label occupationLabel;
        private TextBox nameField;
        private TextBox occupationField;
        private Button okBtn;
        private FormListener? formListener;
        private ListBox ageList;
        private ComboBox empCombo;
        private CheckBox citizenCheck;
        private TextBox taxField;
        private Label taxLabel;

        public FormPanel() {
            Width = 250;
            GroupBox groupBox = new GroupBox { Header = "Add Person", Margin = new Thickness(5) };
            Grid grid = new Grid { Margin = new Thickness(5) };

            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            for (int i = 0; i < 7; i++) grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            nameLabel = new Label { Content = "Name: ", HorizontalAlignment = HorizontalAlignment.Right };
            occupationLabel = new Label { Content = "Occupation: ", HorizontalAlignment = HorizontalAlignment.Right };
            nameField = new TextBox { Margin = new Thickness(0, 5, 0, 5) };
            occupationField = new TextBox { Margin = new Thickness(0, 5, 0, 5) };

            ageList = new ListBox { Height = 60, Margin = new Thickness(0, 5, 0, 5) };
            ageList.Items.Add("Under 18"); ageList.Items.Add("18 to 65"); ageList.Items.Add("65 or over");
            ageList.SelectedIndex = 1;

            empCombo = new ComboBox { Margin = new Thickness(0, 5, 0, 5) };
            empCombo.Items.Add("Employed"); empCombo.Items.Add("Self-employed"); empCombo.Items.Add("Unemployed");
            empCombo.SelectedIndex = 0;

            citizenCheck = new CheckBox { Margin = new Thickness(0, 10, 0, 5) };
            taxField = new TextBox { IsEnabled = false, Margin = new Thickness(0, 5, 0, 5) };
            taxLabel = new Label { Content = "Tax ID: ", IsEnabled = false, HorizontalAlignment = HorizontalAlignment.Right };

            citizenCheck.Checked += (s, e) => { taxLabel.IsEnabled = true; taxField.IsEnabled = true; };
            citizenCheck.Unchecked += (s, e) => { taxLabel.IsEnabled = false; taxField.IsEnabled = false; };

            okBtn = new Button { Content = "OK", Width = 60, HorizontalAlignment = HorizontalAlignment.Left, Margin = new Thickness(0, 15, 0, 0) };

            okBtn.Click += (s, e) => {
            string name = nameField.Text ?? "";
            string occupation = occupationField.Text ?? "";
            int ageCat = ageList.SelectedIndex;
            string empCat = empCombo.SelectedItem?.ToString() ?? "";
            string taxId = taxField.Text ?? "";
            bool usCitizen = citizenCheck.IsChecked ?? false;
                FormEvent ev = new FormEvent(this, name, occupation, ageCat, empCat, taxId, usCitizen);
                if (formListener != null) formListener.formEventOccurred(ev);
            };

            layoutComponents(grid);
            groupBox.Content = grid;
            Content = groupBox;
        }

        private void layoutComponents(Grid gc) {
            Grid.SetRow(nameLabel, 0); Grid.SetColumn(nameLabel, 0); gc.Children.Add(nameLabel);
            Grid.SetRow(nameField, 0); Grid.SetColumn(nameField, 1); gc.Children.Add(nameField);
            
            Grid.SetRow(occupationLabel, 1); Grid.SetColumn(occupationLabel, 0); gc.Children.Add(occupationLabel);
            Grid.SetRow(occupationField, 1); Grid.SetColumn(occupationField, 1); gc.Children.Add(occupationField);
            
            Label ageLabel = new Label { Content = "Age: ", HorizontalAlignment = HorizontalAlignment.Right };
            Grid.SetRow(ageLabel, 2); Grid.SetColumn(ageLabel, 0); gc.Children.Add(ageLabel);
            Grid.SetRow(ageList, 2); Grid.SetColumn(ageList, 1); gc.Children.Add(ageList);
            
            Label empLabel = new Label { Content = "Employment: ", HorizontalAlignment = HorizontalAlignment.Right };
            Grid.SetRow(empLabel, 3); Grid.SetColumn(empLabel, 0); gc.Children.Add(empLabel);
            Grid.SetRow(empCombo, 3); Grid.SetColumn(empCombo, 1); gc.Children.Add(empCombo);
            
            Label citLabel = new Label { Content = "US Citizen: ", HorizontalAlignment = HorizontalAlignment.Right };
            Grid.SetRow(citLabel, 4); Grid.SetColumn(citLabel, 0); gc.Children.Add(citLabel);
            Grid.SetRow(citizenCheck, 4); Grid.SetColumn(citizenCheck, 1); gc.Children.Add(citizenCheck);
            
            Grid.SetRow(taxLabel, 5); Grid.SetColumn(taxLabel, 0); gc.Children.Add(taxLabel);
            Grid.SetRow(taxField, 5); Grid.SetColumn(taxField, 1); gc.Children.Add(taxField);
            
            Grid.SetRow(okBtn, 6); Grid.SetColumn(okBtn, 1); gc.Children.Add(okBtn);
        }

        public void setFormListener(FormListener listener) {
            this.formListener = listener;
        }
    }
}