using System.Windows;
using System.Windows.Controls;

namespace CSharpCode {
    public class Toolbar : UserControl {
        private Button helloButton;
        private Button goodbyeButton;
        private StringListener? textListener;

        public Toolbar() {
            StackPanel panel = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(5) };
            
            helloButton = new Button { Content = "Hello", Width = 60, Margin = new Thickness(5) };
            goodbyeButton = new Button { Content = "Goodbye", Width = 60, Margin = new Thickness(5) };
            
            helloButton.Click += (s, e) => {
                if (textListener != null) textListener.textEmitted("Hello\n");
            };
            
            goodbyeButton.Click += (s, e) => {
                if (textListener != null) textListener.textEmitted("Goodbye\n");
            };
            
            panel.Children.Add(helloButton);
            panel.Children.Add(goodbyeButton);
            Content = panel;
        }

        public void setStringListener(StringListener listener) {
            this.textListener = listener;
        }
    }
}