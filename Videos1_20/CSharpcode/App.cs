using System;
using System.Windows;

namespace CSharpCode {
    public class App {
        [STAThread]
        public static void Main(string[] args) {
            Application app = new Application();
            MainFrame window = new MainFrame();
            app.Run(window);
        }
    }
}