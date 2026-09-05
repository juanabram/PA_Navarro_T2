package choiceexam;

import choiceexam.ChoiceExamProcessor.Pregunta;
import javafx.application.Platform;
import javafx.fxml.FXML;
import javafx.scene.control.Alert;
import javafx.scene.control.Label;
import javafx.scene.control.TextArea;
import javafx.scene.input.MouseEvent;

import java.util.List;

/**
 * Controlador de ChoiceExamView.fxml.
 * Muestra cada pregunta y opciones que arma ChoiceExamProcessor, y
 * reporta el resultado del clic del usuario.
 */
public class ChoiceExamController {

    @FXML private Label lpais;
    @FXML private Label lopcion1;
    @FXML private Label lopcion2;
    @FXML private Label loption3;
    @FXML private TextArea tacorrect;

    private final ChoiceExamProcessor processor = new ChoiceExamProcessor();
    private boolean examenActivo;

    @FXML
    private void onIniciar() {
        processor.reiniciarMarcador();
        examenActivo = true;
        tacorrect.setText("Examen iniciado. Elige la capital correcta.");
        mostrarNuevaPregunta();
    }

    @FXML
    private void onSiguiente() {
        if (examenActivo) {
            mostrarNuevaPregunta();
        }
    }

    @FXML
    private void onOpcionElegida(MouseEvent evt) {
        if (!examenActivo) {
            return;
        }
        Label opcion = (Label) evt.getSource();
        boolean correcta = processor.evaluarRespuesta(opcion.getText());
        tacorrect.setText((correcta ? "Correcto!" : "Incorrecto.") + "\n" + processor.marcadorTexto());
    }

    @FXML
    private void onCerrar() {
        Platform.exit();
    }

    @FXML
    private void onAcercaDe() {
        Alert alert = new Alert(Alert.AlertType.INFORMATION, "Examen de capitales del mundo - Practica ISC.");
        alert.showAndWait();
    }

    private void mostrarNuevaPregunta() {
        Pregunta pregunta = processor.siguientePregunta();
        lpais.setText(pregunta.pais());
        List<String> opciones = pregunta.opciones();
        lopcion1.setText(opciones.get(0));
        lopcion2.setText(opciones.get(1));
        loption3.setText(opciones.get(2));
    }
}
