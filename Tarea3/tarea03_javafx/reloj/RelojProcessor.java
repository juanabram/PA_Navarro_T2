package reloj;

import java.time.Duration;
import java.time.LocalTime;
import java.time.format.DateTimeFormatter;

/**
 * Calcula la hora en que se presiono Iniciar, la hora en que se presiono
 * Parar y el tiempo transcurrido entre ambas. Sin logica de UI.
 */
public class RelojProcessor {

    private static final DateTimeFormatter FORMATO = DateTimeFormatter.ofPattern("HH:mm:ss");

    private LocalTime horaInicio;
    private LocalTime horaParo;

    public String iniciar() {
        horaInicio = LocalTime.now();
        horaParo = null;
        return horaInicio.format(FORMATO);
    }

    public String parar() {
        if (horaInicio == null) {
            return "";
        }
        horaParo = LocalTime.now();
        return horaParo.format(FORMATO);
    }

    /** Regresa el tiempo transcurrido en formato HH:mm:ss, o vacio si aun no hay datos. */
    public String tiempoTranscurrido() {
        if (horaInicio == null || horaParo == null) {
            return "";
        }
        Duration duracion = Duration.between(horaInicio, horaParo);
        if (duracion.isNegative()) {
            duracion = duracion.plusDays(1); // cruzo medianoche
        }
        long horas = duracion.toHours();
        long minutos = duracion.toMinutesPart();
        long segundos = duracion.toSecondsPart();
        return String.format("%02d:%02d:%02d", horas, minutos, segundos);
    }
}
