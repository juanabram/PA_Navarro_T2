package loanassistant;

/**
 * Contiene la matematica financiera del asistente de prestamos.
 * No sabe nada de la interfaz grafica.
 */
public class LoanAssistantProcessor {

    /** Pago mensual dado el balance, tasa anual (%) y numero de pagos. */
    public double calcularPagoMensual(double balance, double tasaAnualPorc, int numeroPagos) {
        double r = tasaMensual(tasaAnualPorc);
        if (r == 0) {
            return balance / numeroPagos;
        }
        return balance * r / (1 - Math.pow(1 + r, -numeroPagos));
    }

    /** Numero de pagos (redondeado hacia arriba) dado el balance, tasa anual (%) y pago mensual. */
    public int calcularNumeroPagos(double balance, double tasaAnualPorc, double pagoMensual) {
        double r = tasaMensual(tasaAnualPorc);
        if (r == 0) {
            return (int) Math.ceil(balance / pagoMensual);
        }
        double n = -Math.log(1 - (r * balance) / pagoMensual) / Math.log(1 + r);
        return (int) Math.ceil(n);
    }

    /** Texto de analisis: total pagado e intereses totales. */
    public String analisis(double balance, double pagoMensual, int numeroPagos) {
        double totalPagado = pagoMensual * numeroPagos;
        double interes = totalPagado - balance;
        return String.format(
                "Balance del prestamo: $%.2f%n" +
                "Pago mensual: $%.2f%n" +
                "Numero de pagos: %d%n" +
                "Total pagado: $%.2f%n" +
                "Interes total pagado: $%.2f",
                balance, pagoMensual, numeroPagos, totalPagado, interes);
    }

    private double tasaMensual(double tasaAnualPorc) {
        return (tasaAnualPorc / 100.0) / 12.0;
    }
}
