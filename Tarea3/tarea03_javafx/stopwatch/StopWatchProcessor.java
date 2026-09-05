package stopwatch;

/**
 * Procesa los calculos del cronometro: guarda el instante de inicio,
 * si esta corriendo o no, y entrega el tiempo transcurrido formateado.
 * No conoce nada de la interfaz grafica.
 */
public class StopWatchProcessor {

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

    public long getElapsedMillis() {
        if (running) {
            return System.currentTimeMillis() - startTime;
        }
        return elapsedTime;
    }

    public boolean isRunning() {
        return running;
    }

    /** Regresa el tiempo transcurrido en formato HH:mm:ss */
    public String getFormattedTime() {
        long totalSeconds = getElapsedMillis() / 1000;
        long hours = totalSeconds / 3600;
        long minutes = (totalSeconds % 3600) / 60;
        long seconds = totalSeconds % 60;
        return String.format("%02d:%02d:%02d", hours, minutes, seconds);
    }
}
