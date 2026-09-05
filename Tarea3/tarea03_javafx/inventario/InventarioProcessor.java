package inventario;

import java.time.LocalDate;
import java.util.ArrayList;
import java.util.List;

/**
 * Guarda los objetos del inventario en memoria y ofrece operaciones de
 * alta, baja, navegacion y busqueda por letra inicial. No sabe nada de
 * la interfaz grafica.
 */
public class InventarioProcessor {

    public static class ItemInventario {
        public String objeto = "";
        public String ubicacion = "";
        public String serie = "";
        public String precio = "";
        public String tienda = "";
        public String foto = "";
        public boolean marcado;
        public LocalDate fechaCompra;
    }

    private final List<ItemInventario> items = new ArrayList<>();
    private int indiceActual = -1;

    public ItemInventario nuevo() {
        ItemInventario item = new ItemInventario();
        items.add(item);
        indiceActual = items.size() - 1;
        return item;
    }

    public void guardar(int indice, ItemInventario datos) {
        if (indice >= 0 && indice < items.size()) {
            items.set(indice, datos);
        }
    }

    public void borrar(int indice) {
        if (indice >= 0 && indice < items.size()) {
            items.remove(indice);
            if (indiceActual >= items.size()) {
                indiceActual = items.size() - 1;
            }
        }
    }

    public ItemInventario anterior() {
        if (indiceActual > 0) {
            indiceActual--;
        }
        return actual();
    }

    public ItemInventario siguiente() {
        if (indiceActual < items.size() - 1) {
            indiceActual++;
        }
        return actual();
    }

    public ItemInventario actual() {
        if (indiceActual < 0 || indiceActual >= items.size()) {
            return null;
        }
        return items.get(indiceActual);
    }

    public int getIndiceActual() {
        return indiceActual;
    }

    /** Busca el primer objeto cuyo nombre comience con la letra indicada. */
    public ItemInventario buscarPorLetra(char letra) {
        for (int i = 0; i < items.size(); i++) {
            String objeto = items.get(i).objeto;
            if (objeto != null && !objeto.isEmpty()
                    && Character.toLowerCase(objeto.charAt(0)) == Character.toLowerCase(letra)) {
                indiceActual = i;
                return items.get(i);
            }
        }
        return null;
    }

    /** Texto listo para "imprimir" con los datos del objeto actual. */
    public String textoImpresion(ItemInventario item) {
        if (item == null) {
            return "No hay objeto seleccionado.";
        }
        return String.format(
                "Objeto: %s%nUbicacion: %s%nNo. de Serie: %s%nPrecio: %s%nTienda: %s%nFecha de compra: %s%nMarcado: %s",
                item.objeto, item.ubicacion, item.serie, item.precio, item.tienda,
                item.fechaCompra, item.marcado ? "Si" : "No");
    }

    public int totalItems() {
        return items.size();
    }
}
