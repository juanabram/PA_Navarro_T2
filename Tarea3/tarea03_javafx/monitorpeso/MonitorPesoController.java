package monitorpeso;

import javafx.fxml.FXML;
import javafx.scene.control.Alert;
import javafx.scene.control.DatePicker;
import javafx.scene.control.TextArea;
import javafx.scene.control.TextField;

import java.time.LocalDate;

/**
 * Controlador de MonitorPesoView.fxml.
 * Lee la fecha/peso capturados, delega el guardado y las estadisticas
 * a MonitorPesoProcessor, y refresca la lista visible.
 */
public class MonitorPesoController {

    @FXML
    private DatePicker fecha;
    @FXML
    private TextField tfpeso;
    @FXML
    private TextArea taarchivos;

    private final MonitorPesoProcessor processor = new MonitorPesoProcessor();

    @FXML
    private void onAgregar() {
        LocalDate diaSeleccionado = fecha.getValue();
        if (diaSeleccionado == null) {
            mostrarError("Selecciona una fecha.");
            return;
        }
        try {
            double kilos = Double.parseDouble(tfpeso.getText());
            processor.agregar(diaSeleccionado, kilos);
            tfpeso.clear();
            refrescarLista();
        } catch (NumberFormatException ex) {
            mostrarError("El peso debe ser un numero valido.");
        }
    }

    @FXML
    private void onBorrar() {
        processor.borrarUltimo();
        refrescarLista();
    }

    private void refrescarLista() {
        StringBuilder sb = new StringBuilder();
        processor.getRegistros().forEach(r -> sb.append(r).append("\n"));
        if (!processor.getRegistros().isEmpty()) {
            sb.append(String.format("%nPromedio: %.1f kg%n", processor.promedio()));
            sb.append(String.format("Diferencia total: %.1f kg", processor.diferenciaTotal()));
        }
        taarchivos.setText(sb.toString());
    }

    private void mostrarError(String mensaje) {
        Alert alert = new Alert(Alert.AlertType.ERROR, mensaje);
        alert.showAndWait();
    }
}
