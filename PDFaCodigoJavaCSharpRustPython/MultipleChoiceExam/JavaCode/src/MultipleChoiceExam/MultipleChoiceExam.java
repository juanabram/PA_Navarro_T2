package	MultipleChoiceExam;

import javax.swing.filechooser.*;
import javax.swing.*;
import java.awt.*;
import java.awt.event.*;
import java.io.*;
import java.util.Random;
import java.text.*;

public class MultipleChoiceExam extends JFrame {
    
    JLabel headGivenLabel = new JLabel();
    JLabel givenLabel = new JLabel();
    JLabel headAnswerLabel = new JLabel();
    JLabel[] answerLabel = new JLabel[4];
    JTextField answerTextField = new JTextField();
    JTextArea commentTextArea = new JTextArea();
    JButton nextButton = new JButton();
    JButton startButton = new JButton();
    
    // menu structure
    JMenuBar mainMenuBar = new JMenuBar();
    JMenu fileMenu = new JMenu("File");
    JMenuItem openMenuItem = new JMenuItem("Open");
    JMenuItem exitMenuItem = new JMenuItem("Exit");
    JMenu optionsMenu = new JMenu("Options");
    JRadioButtonMenuItem header1MenuItem = new JRadioButtonMenuItem("Header 1", true);
    JRadioButtonMenuItem header2MenuItem = new JRadioButtonMenuItem("Header 2", false);
    JRadioButtonMenuItem mcMenuItem = new JRadioButtonMenuItem("Multiple Choice Answers", true);
    JRadioButtonMenuItem typeMenuItem = new JRadioButtonMenuItem("Type In Answers", false);
    ButtonGroup nameGroup = new ButtonGroup();
    ButtonGroup typeGroup = new ButtonGroup();
    
    Font headerFont = new Font("Arial", Font.BOLD, 18);
    Font examItemFont = new Font("Arial", Font.BOLD, 16);
    Dimension itemSize = new Dimension(370, 30);
    
    String examTitle;
    String header1, header2;
    int numberTerms;
    String[] term1 = new String[100];
    String[] term2 = new String[100];
    int numberTried, numberCorrect;
    int correctAnswer;
    Random myRandom = new Random();

    public static void main(String args[]) {
        // create frame
        new MultipleChoiceExam().show();
    }

    public MultipleChoiceExam() {
        // frame constructor
        setTitle("Multiple Choice Exam - No File");
        setResizable(false);
        addWindowListener(new WindowAdapter() {
            public void windowClosing(WindowEvent evt) {
                exitForm(evt);
            }
        });
        
        getContentPane().setLayout(new GridBagLayout());
        GridBagConstraints gridConstraints;
        
        headGivenLabel.setPreferredSize(itemSize);
        headGivenLabel.setFont(headerFont);
        gridConstraints = new GridBagConstraints();
        gridConstraints.gridx = 0;
        gridConstraints.gridy = 0;
        gridConstraints.insets = new Insets(10, 10, 0, 10);
        getContentPane().add(headGivenLabel, gridConstraints);
        
        givenLabel.setPreferredSize(itemSize);
        givenLabel.setFont(examItemFont);
        givenLabel.setBorder(BorderFactory.createLineBorder(Color.BLACK));
        givenLabel.setBackground(Color.WHITE);
        givenLabel.setForeground(Color.BLUE);
        givenLabel.setOpaque(true);
        givenLabel.setHorizontalAlignment(SwingConstants.CENTER);
        gridConstraints = new GridBagConstraints();
        gridConstraints.gridx = 0;
        gridConstraints.gridy = 1;
        gridConstraints.insets = new Insets(0, 10, 0, 10);
        getContentPane().add(givenLabel, gridConstraints);
        
        headAnswerLabel.setPreferredSize(itemSize);
        headAnswerLabel.setFont(headerFont);
        gridConstraints = new GridBagConstraints();
        gridConstraints.gridx = 0;
        gridConstraints.gridy = 2;
        gridConstraints.insets = new Insets(10, 10, 0, 10);
        getContentPane().add(headAnswerLabel, gridConstraints);
        
        for (int i = 0; i < 4; i++) {
            answerLabel[i] = new JLabel();
            answerLabel[i].setPreferredSize(itemSize);
            answerLabel[i].setFont(examItemFont);
            answerLabel[i].setBorder(BorderFactory.createLineBorder(Color.BLACK));
            answerLabel[i].setBackground(Color.WHITE);
            answerLabel[i].setForeground(Color.BLUE);
            answerLabel[i].setOpaque(true);
            answerLabel[i].setHorizontalAlignment(SwingConstants.CENTER);
            gridConstraints = new GridBagConstraints();
            gridConstraints.gridx = 0;
            gridConstraints.gridy = i + 3;
            gridConstraints.insets = new Insets(0, 10, 10, 10);
            getContentPane().add(answerLabel[i], gridConstraints);
            answerLabel[i].addMouseListener(new MouseAdapter() {
                public void mousePressed(MouseEvent e) {
                    answerLabelMousePressed(e);
                }
            });
        }
        
        answerTextField.setPreferredSize(itemSize);
        answerTextField.setFont(examItemFont);
        answerTextField.setBackground(Color.WHITE);
        answerTextField.setForeground(Color.BLUE);
        answerTextField.setVisible(false);
        gridConstraints = new GridBagConstraints();
        gridConstraints.gridx = 0;
        gridConstraints.gridy = 3;
        gridConstraints.insets = new Insets(0, 10, 10, 10);
        getContentPane().add(answerTextField, gridConstraints);
        answerTextField.addActionListener(new ActionListener () {
            public void actionPerformed(ActionEvent e) {
                answerTextFieldActionPerformed(e);
            }
        });
        
        commentTextArea.setPreferredSize(new Dimension(370, 80));
        commentTextArea.setFont(new Font("Courier New", Font.BOLD + Font.ITALIC, 18));
        commentTextArea.setBorder(BorderFactory.createLineBorder(Color.BLACK));
        commentTextArea.setEditable(false);
        commentTextArea.setBackground(new Color(255, 255, 192));
        commentTextArea.setForeground(Color.RED);
        gridConstraints = new GridBagConstraints();
        gridConstraints.gridx = 0;
        gridConstraints.gridy = 7;
        gridConstraints.insets = new Insets(0, 10, 10, 10);
        getContentPane().add(commentTextArea, gridConstraints);
        
        nextButton.setText("Next Question");
        gridConstraints = new GridBagConstraints();
        gridConstraints.gridx = 0;
        gridConstraints.gridy = 8;
        gridConstraints.insets = new Insets(0, 0, 10, 0);
        getContentPane().add(nextButton, gridConstraints);
        nextButton.addActionListener(new ActionListener() {
            public void actionPerformed(ActionEvent e) {
                nextButtonActionPerformed(e);
            }
        });
        
        startButton.setText("Start Exam");
        gridConstraints = new GridBagConstraints();
        gridConstraints.gridx = 0;
        gridConstraints.gridy = 9;
        gridConstraints.insets = new Insets(0, 0, 10, 0);
        getContentPane().add(startButton, gridConstraints);
        startButton.addActionListener(new ActionListener() {
            public void actionPerformed(ActionEvent e) {
                startButtonActionPerformed(e);
            }
        });
        
        // build menu structure
        setJMenuBar(mainMenuBar);
        mainMenuBar.add(fileMenu);
        fileMenu.add(openMenuItem);
        fileMenu.addSeparator();
        fileMenu.add(exitMenuItem);
        
        mainMenuBar.add(optionsMenu);
        optionsMenu.add(header1MenuItem);
        optionsMenu.add(header2MenuItem);
        optionsMenu.addSeparator();
        optionsMenu.add(mcMenuItem);
        optionsMenu.add(typeMenuItem);
        
        nameGroup.add(header1MenuItem);
        nameGroup.add(header2MenuItem);
        typeGroup.add(mcMenuItem);
        typeGroup.add(typeMenuItem);
        
        openMenuItem.addActionListener(new ActionListener() {
            public void actionPerformed(ActionEvent e) {
                openMenuItemActionPerformed(e);
            }
        });
        exitMenuItem.addActionListener(new ActionListener() {
            public void actionPerformed(ActionEvent e) {
                exitMenuItemActionPerformed(e);
            }
        });
        header1MenuItem.addActionListener(new ActionListener() {
            public void actionPerformed(ActionEvent e) {
                header1MenuItemActionPerformed(e);
            }
        });
        header2MenuItem.addActionListener(new ActionListener() {
            public void actionPerformed(ActionEvent e) {
                header2MenuItemActionPerformed(e);
            }
        });
        mcMenuItem.addActionListener(new ActionListener() {
            public void actionPerformed(ActionEvent e) {
                mcMenuItemActionPerformed(e);
            }
        });
        typeMenuItem.addActionListener(new ActionListener() {
            public void actionPerformed(ActionEvent e) {
                typeMenuItemActionPerformed(e);
            }
        });
        
        pack();
        Dimension screenSize = Toolkit.getDefaultToolkit().getScreenSize();
        // Nota: Se ha agregado el operador de multiplicación (*) que faltaba en el PDF para evitar errores de compilación
        setBounds((int) (0.5 * (screenSize.width - getWidth())), (int) (0.5 * (screenSize.height - getHeight())), getWidth(), getHeight());
        
        // initialize form
        startButton.setEnabled(false);
        nextButton.setEnabled(false);
        optionsMenu.setEnabled(false);
        commentTextArea.setText(centerTextArea("Open Exam File to Start"));
    }

    private void exitForm(WindowEvent evt) {
        System.exit(0);
    }

    private void answerLabelMousePressed(MouseEvent e) {
        boolean correct = false;
        int labelSelected;
        
        // make sure exam has started and question has not been answered
        if (startButton.getText().equals("Start Exam") || nextButton.isEnabled())
            return;
            
        // determine which label was clicked
        // get upper left corner of clicked label
        Point p = e.getComponent().getLocation();
        
        // determine index based on p
        for (labelSelected = 0; labelSelected < 20; labelSelected++) {
            if (p.x == answerLabel[labelSelected].getX() && p.y == answerLabel[labelSelected].getY()) 
                break;
        }
        
        // If already answered, exit
        numberTried++;
        if (header1MenuItem.isSelected()) {
            if (answerLabel[labelSelected].getText().equals(term1[correctAnswer])) 
                correct = true;
        } else {
            if (answerLabel[labelSelected].getText().equals(term2[correctAnswer])) 
                correct = true;
        }
        updateScore(correct);
    }

    private void answerTextFieldActionPerformed(ActionEvent e) {
        // Check type in answer
        boolean correct;
        String ucTypedAnswer, ucAnswer;
        
        // make sure exam has started and question has not been answered
        if (startButton.getText().equals("Start Exam") || nextButton.isEnabled()) 
            return;
            
        answerTextField.setEditable(false);
        numberTried++;
        ucTypedAnswer = answerTextField.getText().toUpperCase();
        
        if (header1MenuItem.isSelected())
            ucAnswer = term1[correctAnswer].toUpperCase();
        else
            ucAnswer = term2[correctAnswer].toUpperCase();
            
        correct = false;
        if (ucTypedAnswer.equals(ucAnswer) || soundex(ucTypedAnswer).equals(soundex(ucAnswer))) 
            correct = true;
            
        updateScore(correct);
    }

    private void nextButtonActionPerformed(ActionEvent e) {
        // Generate next question
        nextButton.setEnabled(false);
        nextQuestion();
    }

    private void startButtonActionPerformed(ActionEvent e) {
        String message;
        if (startButton.getText().equals("Start Exam")) {
            startButton.setText("Stop Exam");
            nextButton.setEnabled(false);
            // Reset the score
            numberTried = 0;
            numberCorrect = 0;
            commentTextArea.setText("");
            fileMenu.setEnabled(false);
            optionsMenu.setEnabled(false);
            nextQuestion();
        } else {
            startButton.setText("Start Exam");
            nextButton.setEnabled(false);
            if (numberTried > 0) {
                message = "Questions Tried: " + String.valueOf(numberTried) + "\n";
                message += "Questions Correct: " + String.valueOf(numberCorrect) + "\n\n";
                message += "Your Score: " + new DecimalFormat("0.0").format(100.0 * ((double) numberCorrect / numberTried)) + "%";
                JOptionPane.showConfirmDialog(null, message, examTitle + " Results", JOptionPane.DEFAULT_OPTION, JOptionPane.INFORMATION_MESSAGE);
            }
            givenLabel.setText("");
            answerLabel[0].setText("");
            answerLabel[1].setText("");
            answerLabel[2].setText("");
            answerLabel[3].setText("");
            answerTextField.setText("");
            commentTextArea.setText(centerTextArea("Choose Options\nClick Start Exam"));
            fileMenu.setEnabled(true);
            optionsMenu.setEnabled(true);
        }
    }

    private void openMenuItemActionPerformed(ActionEvent e) {
        String myLine;
        JFileChooser openChooser = new JFileChooser();
        openChooser.setDialogType(JFileChooser.OPEN_DIALOG);
        openChooser.setDialogTitle("Open Exam File");
        openChooser.addChoosableFileFilter(new FileNameExtensionFilter("Exam Files", "csv"));
        
        if (openChooser.showOpenDialog(this) == JFileChooser.APPROVE_OPTION) {
            try {
                BufferedReader inputFile = new BufferedReader(new FileReader(openChooser.getSelectedFile()));
                myLine = inputFile.readLine();
                examTitle = parseLeft(myLine);
                myLine = inputFile.readLine();
                header1 = parseLeft(myLine);
                header2 = parseRight(myLine);
                numberTerms = 0;
                
                do {
                    numberTerms++;
                    myLine = inputFile.readLine();
                    term1[numberTerms - 1] = parseLeft(myLine);
                    term2[numberTerms - 1] = parseRight(myLine);
                } while (inputFile.ready() && numberTerms < 100);
                
                if (numberTerms < 5) {
                    JOptionPane.showConfirmDialog(null, "Must have at least 5 entries in exam file.", "Exam File Error", JOptionPane.DEFAULT_OPTION, JOptionPane.ERROR_MESSAGE);
                    return;
                }
                
                inputFile.close();
                
                // establish frame title
                this.setTitle("Multiple Choice Exam - " + examTitle);
                
                // set up menu items
                header1MenuItem.setText(header1 + ", Given " + header2);
                header2MenuItem.setText(header2 + ", Given " + header1);
                
                if (header1MenuItem.isSelected()) {
                    headGivenLabel.setText(header2);
                    headAnswerLabel.setText(header1);
                } else {
                    headGivenLabel.setText(header1);
                    headAnswerLabel.setText(header2);
                }
                
                startButton.setEnabled(true);
                optionsMenu.setEnabled(true);
                commentTextArea.setText(centerTextArea("File Loaded, Choose Options\nClick Start Exam"));
                
            } catch (Exception ex) {
                JOptionPane.showConfirmDialog(null, "Error reading in input file - make sure file is correct format.", "Multiple Choice Exam File Error", JOptionPane.DEFAULT_OPTION, JOptionPane.ERROR_MESSAGE);
                return;
            }
        }
    }

    private void exitMenuItemActionPerformed(ActionEvent e) {
        System.exit(0);
    }

    private void header1MenuItemActionPerformed(ActionEvent e) {
        // Set up for naming header1, given header2
        headGivenLabel.setText(header2);
        headAnswerLabel.setText(header1);
    }

    private void header2MenuItemActionPerformed(ActionEvent e) {
        // Set up for naming header2, given header1
        headGivenLabel.setText(header1);
        headAnswerLabel.setText(header2);
    }

    private void mcMenuItemActionPerformed(ActionEvent e) {
        answerLabel[0].setVisible(true);
        answerLabel[1].setVisible(true);
        answerLabel[2].setVisible(true);
        answerLabel[3].setVisible(true);
        answerTextField.setVisible(false);
    }

    private void typeMenuItemActionPerformed(ActionEvent e) {
        answerLabel[0].setVisible(false);
        answerLabel[1].setVisible(false);
        answerLabel[2].setVisible(false);
        answerLabel[3].setVisible(false);
        answerTextField.setVisible(true);
    }

    private String parseLeft(String s) {
        int cl;
        // find comma
        cl = s.indexOf(",");
        return (s.substring(0, cl));
    }

    private String parseRight(String s) {
        int cl;
        // find comma
        cl = s.indexOf(",");
        return (s.substring(cl + 1));
    }

    private String centerTextArea(String s) {
        // centers up to two lines in text area
        int charsPerLine = 33;
        String sOut = "";
        int j = s.indexOf("\n");
        
        if (j == -1) {
            // single line
            sOut = "\n" + spacePadding((int) ((charsPerLine - s.length()) / 2)) + s;
        } else {
            // first line
            String l = s.substring(0, j);
            sOut = "\n" + spacePadding((int) ((charsPerLine - l.length()) / 2)) + l;
            // second line
            l = s.substring(j + 1);
            sOut += "\n" + spacePadding((int) ((charsPerLine - l.length()) / 2)) + l ;
        }
        return(sOut);
    }

    private String spacePadding(int n) {
        String s = "";
        if (n != 0)
            for (int i = 0; i < n; i++)
                s += " ";
        return(s);
    }

    private void nextQuestion() {
        boolean[] termUsed = new boolean[numberTerms];
        int[] index = new int[4];
        int j;
        commentTextArea.setText("");
        
        // Generate the next question based on selected options 
        correctAnswer = myRandom.nextInt(numberTerms);
        
        if (header1MenuItem.isSelected()) {
            givenLabel.setText(term2[correctAnswer]);
        } else {
            givenLabel.setText(term1[correctAnswer]);
        }
        
        if (mcMenuItem.isSelected()) {
            // Multiple choice answers
            for (int i = 0; i < numberTerms; i++) {
                termUsed[i] = false;
            }
            // Pick four random possiblities
            for (int i = 0; i < 4; i++) {
                do {
                    j = myRandom.nextInt(numberTerms);
                } while (termUsed[j] || j == correctAnswer);
                termUsed[j] = true;
                index[i] = j;
            }
            // Replace one with correct answer
            index[myRandom.nextInt(4)] = correctAnswer;
            
            // Display multiple choice answers in label boxes
            if (header1MenuItem.isSelected()) {
                answerLabel[0].setText(term1[index[0]]);
                answerLabel[1].setText(term1[index[1]]);
                answerLabel[2].setText(term1[index[2]]);
                answerLabel[3].setText(term1[index[3]]);
            } else {
                answerLabel[0].setText(term2[index[0]]);
                answerLabel[1].setText(term2[index[1]]);
                answerLabel[2].setText(term2[index[2]]);
                answerLabel[3].setText(term2[index[3]]);
            }
        } else {
            // Type-in answers
            answerTextField.setEditable(true);
            answerTextField.setText("");
            answerTextField.requestFocus();
        }
    }

    private void updateScore(boolean correct) {
        // Check if answer is correct
        if (correct) {
            numberCorrect++;
            commentTextArea.setText(centerTextArea("Correct!"));
        } else {
            commentTextArea.setText(centerTextArea("Sorry ... Correct Answer Shown"));
        }
        
        // Display correct answer
        if (mcMenuItem.isSelected()) {
            if (header1MenuItem.isSelected())
                answerLabel[0].setText(term1[correctAnswer]);
            else
                answerLabel[0].setText(term2[correctAnswer]);
                
            answerLabel[1].setText("");
            answerLabel[2].setText("");
            answerLabel[3].setText("");
        } else {
            if (header1MenuItem.isSelected())
                answerTextField.setText(term1[correctAnswer]);
            else
                answerTextField.setText(term2[correctAnswer]);
        }
        
        startButton.setEnabled(true);
        nextButton.setEnabled(true);
        nextButton.requestFocus();
    }

    public String soundex(String w) {
        // Generates Soundex code for W based on Unicode value
        // Allows answers whose spelling is close, but not exact 
        String wTemp, s = "";
        int l;
        int wPrev, wSnd, cIndex;
        
        // Load soundex function array 
        int[] wSound = {0, 1, 2, 3, 0, 1, 2, 0, 0, 2, 2, 4, 5, 5, 0, 1, 2, 6, 2, 3, 0, 1, 0, 2, 0, 2};
        wTemp = w.toUpperCase();
        l = w.length();
        
        if (l != 0) {
            s = String.valueOf(w.charAt(0));
            wPrev = 0;
            if (l > 1) {
                for (int i = 1; i < l; i++) {
                    cIndex = (int) wTemp.charAt(i) - 65;
                    if (cIndex >= 0 && cIndex <= 25) {
                        wSnd = wSound[cIndex] + 48;
                        if (wSnd != 48 && wSnd != wPrev) {
                            s += String.valueOf((char) wSnd);
                        }
                        wPrev = wSnd;
                    }
                }
            }
        } else {
            s = "";
        }
        return(s);
    }
}
