package PA_StopWatch;
import javax.swing.Timer;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;

public class Cstopwatch {
    private Mstopwatch model;
    private Vstopwatch view;
    private Timer timer;

    public Cstopwatch(Mstopwatch model, Vstopwatch view) {
        this.model = model;
        this.view = view;
        
        timer = new Timer(1000, new ActionListener() {
            public void actionPerformed(ActionEvent e) {
                view.setTime(model.getFormattedTime());
            }
        });
        
        view.getStartButton().addActionListener(new ActionListener() {
            public void actionPerformed(ActionEvent e) {
                model.start();
                timer.start();
            }
        });
        
        view.getStopButton().addActionListener(new ActionListener() {
            public void actionPerformed(ActionEvent e) {
                model.stop();
                timer.stop();
            }
        });
        
        view.getExitButton().addActionListener(new ActionListener() {
            public void actionPerformed(ActionEvent e) {
                System.exit(0);
            }
        });
    }
}