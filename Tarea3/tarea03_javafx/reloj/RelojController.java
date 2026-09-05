package reloj;

import javafx.application.Platform;
import javafx.fxml.FXML;
import javafx.scene.control.TextField;

/**
 * Controlador de RelojView.fxml.
 * Delega todo el calculo de tiempos a RelojProcessor.
 */
public class RelojController {

    @FXML
    private TextField tfstart;
    @FXML
    private TextField tfstop;
    @FXML
    private TextField tftime;

    private final RelojProcessor processor = new RelojProcessor();

    @FXML
    private void onIniciar() {
        tfstart.setText(processor.iniciar());
        tfstop.clear();
        tftime.clear();
    }

    @FXML
    private void onParar() {
        tfstop.setText(processor.parar());
        tftime.setText(processor.tiempoTranscurrido());
    }

    @FXML
    private void onSalir() {
        Platform.exit();
    }
}
