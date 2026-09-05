package inventario;

import inventario.InventarioProcessor.ItemInventario;
import javafx.application.Platform;
import javafx.event.ActionEvent;
import javafx.fxml.FXML;
import javafx.scene.control.Alert;
import javafx.scene.control.Button;
import javafx.scene.control.CheckBox;
import javafx.scene.control.ComboBox;
import javafx.scene.control.DatePicker;
import javafx.scene.control.TextField;

/**
 * Controlador de InventarioView.fxml.
 * Traduce los campos del formulario a un ItemInventario y viceversa;
 * toda la logica de la lista vive en InventarioProcessor.
 */
public class InventarioController {

    @FXML private TextField tfobjeto;
    @FXML private ComboBox<String> cbubicacion;
    @FXML private TextField tfserie;
    @FXML private TextField tfprecio;
    @FXML private TextField tftienda;
    @FXML private TextField tffoto;
    @FXML private CheckBox cbmarcado;
    @FXML private DatePicker fecha;

    private final InventarioProcessor processor = new InventarioProcessor();

    @FXML
    private void onNuevo() {
        limpiarCampos();
        processor.nuevo();
    }

    @FXML
    private void onGuardar() {
        ItemInventario item = new ItemInventario();
        item.objeto = tfobjeto.getText();
        item.ubicacion = cbubicacion.getValue();
        item.serie = tfserie.getText();
        item.precio = tfprecio.getText();
        item.tienda = tftienda.getText();
        item.foto = tffoto.getText();
        item.marcado = cbmarcado.isSelected();
        item.fechaCompra = fecha.getValue();

        int indice = processor.getIndiceActual();
        if (indice < 0) {
            processor.nuevo();
            indice = processor.getIndiceActual();
        }
        processor.guardar(indice, item);
    }

    @FXML
    private void onBorrar() {
        processor.borrar(processor.getIndiceActual());
        mostrarItem(processor.actual());
    }

    @FXML
    private void onAnterior() {
        mostrarItem(processor.anterior());
    }

    @FXML
    private void onSiguiente() {
        mostrarItem(processor.siguiente());
    }

    @FXML
    private void onImprimir() {
        Alert alert = new Alert(Alert.AlertType.INFORMATION, processor.textoImpresion(processor.actual()));
        alert.setHeaderText("Vista de impresion");
        alert.showAndWait();
    }

    @FXML
    private void onSalir() {
        Platform.exit();
    }

    /** Un solo manejador para los 26 botones del abecedario (buscador rapido). */
    @FXML
    private void onLetra(ActionEvent evt) {
        String letra = ((Button) evt.getSource()).getText();
        if (letra != null && !letra.isEmpty()) {
            mostrarItem(processor.buscarPorLetra(letra.charAt(0)));
        }
    }

    private void mostrarItem(ItemInventario item) {
        if (item == null) {
            limpiarCampos();
            return;
        }
        tfobjeto.setText(item.objeto);
        cbubicacion.setValue(item.ubicacion);
        tfserie.setText(item.serie);
        tfprecio.setText(item.precio);
        tftienda.setText(item.tienda);
        tffoto.setText(item.foto);
        cbmarcado.setSelected(item.marcado);
        fecha.setValue(item.fechaCompra);
    }

    private void limpiarCampos() {
        tfobjeto.clear();
        cbubicacion.setValue(null);
        tfserie.clear();
        tfprecio.clear();
        tftienda.clear();
        tffoto.clear();
        cbmarcado.setSelected(false);
        fecha.setValue(null);
    }
}
