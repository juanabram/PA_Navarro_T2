using System;
using System.Windows;

namespace CSharpCode {
    public class ejecutar {
        [STAThread]
        public static void Main(string[] args) {
            Application app = new Application();
            Mstopwatch model = new Mstopwatch();
            Vstopwatch view = new Vstopwatch();
            new Cstopwatch(model, view);
            app.Run(view); // Inicia el JFrame equivalente
        }
    }
}