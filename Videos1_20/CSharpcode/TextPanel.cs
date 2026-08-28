using System.Windows.Controls;

namespace CSharpCode {
    public class TextPanel : UserControl {
        private TextBox textArea;

        public TextPanel() {
            textArea = new TextBox {
                AcceptsReturn = true,
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                IsReadOnly = true
            };
            Content = textArea;
        }

        public void appendText(string text) {
            textArea.AppendText(text);
            textArea.ScrollToEnd();
        }
    }
}