package PA_StopWatch;

public class Mstopwatch {
    private long startTime;
    private long elapsedTime;
    private boolean running;

    public void start() {
        startTime = System.currentTimeMillis();
        running = true;
    }

    public void stop() {
        elapsedTime = System.currentTimeMillis() - startTime;
        running = false;
    }

    public long getElapsedTime() {
        if (running) {
            return System.currentTimeMillis() - startTime;
        }
        return elapsedTime;
    }

    public String getFormattedTime() {
        long totalSeconds = getElapsedTime() / 1000;
        long hours = totalSeconds / 3600;
        long minutes = (totalSeconds % 3600) / 60;
        long seconds = totalSeconds % 60;
        return String.format("%02d:%02d:%02d", hours, minutes, seconds);
    }

    public boolean isRunning() {
        return running;
    }
}