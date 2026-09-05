package stopwatch;

import javafx.animation.Animation;
import javafx.animation.KeyFrame;
import javafx.animation.Timeline;
import javafx.application.Platform;
import javafx.fxml.FXML;
import javafx.scene.control.Label;
import javafx.util.Duration;

/**
 * Controlador de StopWatchView.fxml.
 * Solo se encarga de leer eventos de la UI y mostrar resultados;
 * todo el calculo vive en StopWatchProcessor.
 */
public class StopWatchController {

    @FXML
    private Label label;

    private final StopWatchProcessor processor = new StopWatchProcessor();
    private Timeline timeline;

    @FXML
    public void initialize() {
        timeline = new Timeline(new KeyFrame(Duration.seconds(1), e -> refrescarLabel()));
        timeline.setCycleCount(Animation.INDEFINITE);
    }

    @FXML
    private void onIniciar() {
        processor.start();
        timeline.play();
    }

    @FXML
    private void onParar() {
        processor.stop();
        timeline.stop();
        refrescarLabel();
    }

    @FXML
    private void onSalir() {
        Platform.exit();
    }

    private void refrescarLabel() {
        label.setText(processor.getFormattedTime());
    }
}
