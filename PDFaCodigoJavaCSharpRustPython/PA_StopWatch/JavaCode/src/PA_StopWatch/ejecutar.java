package PA_StopWatch;

public class ejecutar {
    public static void main(String[] args) {
        Mstopwatch model = new Mstopwatch();
        Vstopwatch view = new Vstopwatch();
        new Cstopwatch(model, view);
    }
}