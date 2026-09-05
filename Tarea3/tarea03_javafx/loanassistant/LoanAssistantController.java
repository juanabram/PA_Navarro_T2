package loanassistant;

import javafx.fxml.FXML;
import javafx.scene.control.Alert;
import javafx.scene.control.TextField;

/**
 * Controlador de LoanAssistantView.fxml.
 * Lee/escribe los campos de texto y delega todo el calculo a
 * LoanAssistantProcessor.
 */
public class LoanAssistantController {

    @FXML
    private TextField tfbalance;
    @FXML
    private TextField tfrate;
    @FXML
    private TextField tfnumber;
    @FXML
    private TextField tfmonthly;
    @FXML
    private TextField tfoutput;

    private final LoanAssistantProcessor processor = new LoanAssistantProcessor();

    /** true = se conoce el numero de pagos y se calcula el pago mensual (modo por defecto) */
    private boolean modoCalcularPago = true;

    @FXML
    private void onCompute() {
        try {
            double balance = Double.parseDouble(tfbalance.getText());
            double tasa = Double.parseDouble(tfrate.getText());

            if (modoCalcularPago) {
                int numeroPagos = Integer.parseInt(tfnumber.getText());
                double pago = processor.calcularPagoMensual(balance, tasa, numeroPagos);
                tfmonthly.setText(String.format("%.2f", pago));
                tfoutput.setText(processor.analisis(balance, pago, numeroPagos));
            } else {
                double pago = Double.parseDouble(tfmonthly.getText());
                int numeroPagos = processor.calcularNumeroPagos(balance, tasa, pago);
                tfnumber.setText(String.valueOf(numeroPagos));
                tfoutput.setText(processor.analisis(balance, pago, numeroPagos));
            }
        } catch (NumberFormatException ex) {
            mostrarError("Revisa que Balance, Tasa y " +
                    (modoCalcularPago ? "Numero de Pagos" : "Pago Mensual") + " sean numeros validos.");
        } catch (ArithmeticException | IllegalArgumentException ex) {
            mostrarError("No fue posible calcular con esos valores.");
        }
    }

    @FXML
    private void onNuevo() {
        tfoutput.clear();
        if (modoCalcularPago) {
            tfmonthly.clear();
        } else {
            tfnumber.clear();
        }
    }

    /** Botón "X": cambia entre calcular el pago mensual o el numero de pagos. */
    @FXML
    private void onCambiarModo() {
        modoCalcularPago = !modoCalcularPago;
        tfnumber.setEditable(modoCalcularPago);
        tfmonthly.setEditable(!modoCalcularPago);
        tfnumber.clear();
        tfmonthly.clear();
        tfoutput.clear();
    }

    private void mostrarError(String mensaje) {
        Alert alert = new Alert(Alert.AlertType.ERROR, mensaje);
        alert.showAndWait();
    }
}
