package choiceexam;

import java.util.ArrayList;
import java.util.Collections;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.Random;

/**
 * Guarda el banco de preguntas (pais -> capital), arma cada pregunta
 * con 3 opciones y lleva el marcador. No sabe nada de la interfaz
 * grafica.
 */
public class ChoiceExamProcessor {

    public record Pregunta(String pais, String capitalCorrecta, List<String> opciones) {
    }

    private final Map<String, String> paisesCapitales = new LinkedHashMap<>();
    private final Random random = new Random();

    private String capitalActual;
    private int correctas;
    private int total;

    public ChoiceExamProcessor() {
        paisesCapitales.put("Mexico", "Ciudad de Mexico");
        paisesCapitales.put("Francia", "Paris");
        paisesCapitales.put("Japon", "Tokio");
        paisesCapitales.put("Canada", "Ottawa");
        paisesCapitales.put("Brasil", "Brasilia");
        paisesCapitales.put("Egipto", "El Cairo");
        paisesCapitales.put("Australia", "Canberra");
        paisesCapitales.put("Espana", "Madrid");
        paisesCapitales.put("Italia", "Roma");
        paisesCapitales.put("Turquia", "Ankara");
    }

    public void reiniciarMarcador() {
        correctas = 0;
        total = 0;
    }

    /** Elige un pais al azar y arma 3 opciones (una correcta, dos distractoras). */
    public Pregunta siguientePregunta() {
        List<String> paises = new ArrayList<>(paisesCapitales.keySet());
        String pais = paises.get(random.nextInt(paises.size()));
        capitalActual = paisesCapitales.get(pais);

        List<String> capitales = new ArrayList<>(paisesCapitales.values());
        Collections.shuffle(capitales, random);
        List<String> opciones = new ArrayList<>();
        opciones.add(capitalActual);
        for (String c : capitales) {
            if (opciones.size() == 3) {
                break;
            }
            if (!c.equals(capitalActual) && !opciones.contains(c)) {
                opciones.add(c);
            }
        }
        Collections.shuffle(opciones, random);
        return new Pregunta(pais, capitalActual, opciones);
    }

    public boolean evaluarRespuesta(String capitalElegida) {
        total++;
        boolean correcta = capitalActual != null && capitalActual.equals(capitalElegida);
        if (correcta) {
            correctas++;
        }
        return correcta;
    }

    public String marcadorTexto() {
        return String.format("Aciertos: %d/%d", correctas, total);
    }
}
