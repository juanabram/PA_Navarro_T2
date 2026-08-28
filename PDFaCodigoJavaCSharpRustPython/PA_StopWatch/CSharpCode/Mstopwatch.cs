using System;

namespace CSharpCode {
    public class Mstopwatch {
        private long startTime;
        private long elapsedTime;
        private bool running;

        public void start() {
            startTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            running = true;
        }

        public void stop() {
            elapsedTime = DateTimeOffset.Now.ToUnixTimeMilliseconds() - startTime;
            running = false;
        }

        public long getElapsedTime() {
            if (running) {
                return DateTimeOffset.Now.ToUnixTimeMilliseconds() - startTime;
            }
            return elapsedTime;
        }

        public string getFormattedTime() {
            long totalSeconds = getElapsedTime() / 1000;
            long hours = totalSeconds / 3600;
            long minutes = (totalSeconds % 3600) / 60;
            long seconds = totalSeconds % 60;
            return string.Format("{0:D2}:{1:D2}:{2:D2}", hours, minutes, seconds);
        }

        public bool isRunning() {
            return running;
        }
    }
}