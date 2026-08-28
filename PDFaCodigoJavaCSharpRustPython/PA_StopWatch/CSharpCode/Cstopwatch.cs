using System;
using System.Windows.Threading;

namespace CSharpCode {
    public class Cstopwatch {
        private Mstopwatch model;
        private Vstopwatch view;
        private DispatcherTimer timer;

        public Cstopwatch(Mstopwatch model, Vstopwatch view) {
            this.model = model;
            this.view = view;

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1000);
            timer.Tick += (sender, e) => {
                view.setTime(model.getFormattedTime());
            };

            view.getStartButton().Click += (sender, e) => {
                model.start();
                timer.Start();
            };

            view.getStopButton().Click += (sender, e) => {
                model.stop();
                timer.Stop();
            };

            view.getExitButton().Click += (sender, e) => {
                System.Windows.Application.Current.Shutdown(); // System.exit(0) equivalente
            };
        }
    }
}