# Tarea 03 — Programas JavaFX (MVC)

Cada carpeta es un **paquete independiente** (`package` de Java) que puedes
importar por separado. Dentro de cada uno hay exactamente 3 archivos:

| Archivo | Rol |
|---|---|
| `*View.fxml` | La vista (ya corregida: `id` → `fx:id`, y con `fx:controller` apuntando a su controlador) |
| `*Controller.java` | Solo conecta botones/campos con el procesador. Sin lógica de negocio. |
| `*Processor.java` | El "modelo": todos los cálculos y reglas del programa, sin ninguna referencia a JavaFX. |

## Paquetes incluidos

- **stopwatch** — Cronómetro simple (Iniciar / Parar), formatea `HH:mm:ss`.
- **reloj** — Registra hora de inicio, hora de paro y calcula el tiempo transcurrido.
- **loanassistant** — Asistente de préstamos: calcula pago mensual o número de pagos
  (fórmula de amortización), y arma el análisis (total pagado, interés total). El
  botón **X** cambia entre los dos modos.
- **monitorpeso** — Registra peso por fecha, calcula promedio y diferencia total.
- **inventario** — Alta/baja/edición de objetos, navegación anterior/siguiente,
  y el "abecedario" (a–z) funciona como buscador rápido por letra inicial.
- **choiceexam** — Examen de opción múltiple (país → capital) con banco de
  preguntas fijo, 3 opciones por pregunta y marcador de aciertos.

## Cómo importar un paquete a tu proyecto

1. Copia la carpeta del programa que necesites (por ejemplo `stopwatch/`) dentro
   de tu carpeta `src` (o `src/main/java` si usas Maven/Gradle).
2. Asegúrate de que tu proyecto tenga el **JavaFX SDK** configurado (mismas
   librerías que ya usas: `javafx.controls`, `javafx.fxml`).
3. En tu clase `Application` principal, carga el FXML así:

   ```java
   Parent root = FXMLLoader.load(getClass().getResource("/stopwatch/StopWatchView.fxml"));
   ```

   (ajusta la ruta según dónde quede el archivo dentro de tu carpeta de recursos).
4. Si usas Scene Builder, abre el `.fxml` directamente — ya tiene `fx:controller`
   declarado, así que Scene Builder debería reconocer los `fx:id` sin problema.

## Notas

- En `StopWatchView.fxml` quité la referencia a la imagen local
  (`kyomoto.jpg`) porque esa ruta es de tu equipo y el `FXMLLoader` truena si
  no la encuentra. El `ImageView` (`fx:id="imagen"`) se queda vacío; si quieres
  la foto, cárgala desde el controlador con `imagen.setImage(new Image(...))`.
- En `MonitorPesoView.fxml` había dos elementos con el mismo `id="ancla"` en
  las dos pestañas — lo corregí a `ancla` y `ancla2` porque FXML no permite
  ids duplicados.
- Los cálculos se dejaron intencionalmente simples (sin persistencia en
  archivo/BD) para que sea fácil de leer y extender; si necesitas que
  guarden datos en CSV/archivo, dímelo y lo agrego al `Processor` de cada uno.
