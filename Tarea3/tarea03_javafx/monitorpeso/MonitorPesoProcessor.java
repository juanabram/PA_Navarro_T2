package monitorpeso;

import java.time.LocalDate;
import java.util.ArrayList;
import java.util.List;

/**
 * Guarda el historial de registros de peso y calcula estadisticas
 * simples (promedio, diferencia contra el primer registro). No sabe
 * nada de la interfaz grafica.
 */
public class MonitorPesoProcessor {

    public record RegistroPeso(LocalDate fecha, double kilos) {
        @Override
        public String toString() {
            return fecha + "   " + String.format("%.1f kg", kilos);
        }
    }

    private final List<RegistroPeso> registros = new ArrayList<>();

    public void agregar(LocalDate fecha, double kilos) {
        registros.add(new RegistroPeso(fecha, kilos));
    }

    /** Elimina el ultimo registro agregado (equivalente a "Borrar Seleccion" simplificado). */
    public void borrarUltimo() {
        if (!registros.isEmpty()) {
            registros.remove(registros.size() - 1);
        }
    }

    public List<RegistroPeso> getRegistros() {
        return registros;
    }

    public double promedio() {
        return registros.stream().mapToDouble(RegistroPeso::kilos).average().orElse(0.0);
    }

    /** Diferencia entre el ultimo registro y el primero (positivo = subio de peso). */
    public double diferenciaTotal() {
        if (registros.size() < 2) {
            return 0.0;
        }
        return registros.get(registros.size() - 1).kilos() - registros.get(0).kilos();
    }
}
