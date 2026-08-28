package PA_StopWatch;

import javax.swing.*;
import java.awt.*;

public class Vstopwatch extends JFrame {
    private JButton startButton = new JButton("Start");
    private JButton stopButton = new JButton("Stop");
    private JButton exitButton = new JButton("Exit");
    private JTextField timeField = new JTextField("00:00:00");

    public Vstopwatch() {
        setTitle("Stopwatch MVC");
        setSize(300, 200);
        setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        setLayout(new FlowLayout());
        
        timeField.setEditable(false);
        timeField.setFont(new Font("Arial", Font.BOLD, 24));
        timeField.setHorizontalAlignment(JTextField.CENTER);
        
        add(timeField);
        add(startButton);
        add(stopButton);
        add(exitButton);
        
        setVisible(true);
    }

    public JButton getStartButton() {
        return startButton;
    }

    public JButton getStopButton() {
        return stopButton;
    }

    public JButton getExitButton() {
        return exitButton;
    }

    public void setTime(String time) {
        timeField.setText(time);
    }
}