using System.Windows;
using System.Windows.Controls;

namespace CSharpCode {
    public class MainFrame : Window {
        private TextPanel textPanel;
        private Toolbar toolbar;
        private FormPanel formPanel;

        public MainFrame() {
            Title = "Hello World";
            Width = 600; Height = 500;

            DockPanel dock = new DockPanel();
            toolbar = new Toolbar();
            textPanel = new TextPanel();
            formPanel = new FormPanel();

            Menu menuBar = createMenuBar();
            DockPanel.SetDock(menuBar, Dock.Top); dock.Children.Add(menuBar);
            DockPanel.SetDock(toolbar, Dock.Top); dock.Children.Add(toolbar);
            DockPanel.SetDock(formPanel, Dock.Left); dock.Children.Add(formPanel);
            dock.Children.Add(textPanel);

            toolbar.setStringListener(new CustomStringListener(textPanel));
            formPanel.setFormListener(new CustomFormListener(textPanel));

            Content = dock;
        }

        private Menu createMenuBar() {
            Menu menuBar = new Menu();
            
            MenuItem fileMenu = new MenuItem { Header = "File" };
            MenuItem exitItem = new MenuItem { Header = "Exit" };
            exitItem.Click += (s, e) => Application.Current.Shutdown();
            fileMenu.Items.Add(exitItem);

            MenuItem windowMenu = new MenuItem { Header = "Window" };
            MenuItem showMenu = new MenuItem { Header = "Show" };
            MenuItem showFormItem = new MenuItem { Header = "Person Form", IsCheckable = true, IsChecked = true };
            
            showFormItem.Checked += (s, e) => { formPanel.Visibility = Visibility.Visible; };
            showFormItem.Unchecked += (s, e) => { formPanel.Visibility = Visibility.Collapsed; };
            
            showMenu.Items.Add(showFormItem);
            windowMenu.Items.Add(showMenu);

            menuBar.Items.Add(fileMenu);
            menuBar.Items.Add(windowMenu);
            
            return menuBar;
        }

        // Simulación de las clases anónimas de Java
        private class CustomStringListener : StringListener {
            private TextPanel tp;
            public CustomStringListener(TextPanel tp) { this.tp = tp; }
            public void textEmitted(string text) { tp.appendText(text); }
        }

        private class CustomFormListener : FormListener {
            private TextPanel tp;
            public CustomFormListener(TextPanel tp) { this.tp = tp; }
            public void formEventOccurred(FormEvent e) {
                tp.appendText(e.getName() + ": " + e.getOccupation() + ": Age " + e.getAgeCategory() + ": " + 
                              e.getEmploymentCategory() + ", Citizen: " + e.isUsCitizen() + ", Tax ID: " + e.getTaxId() + "\n");
            }
        }
    }
}