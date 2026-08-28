import tkinter as tk
import time

class Stopwatch(tk.Tk):
    def __init__(self):
        super().__init__()
        self.title("Stopwatch Application")
        
        # Al cerrar la ventana
        self.protocol("WM_DELETE_WINDOW", self.exitForm)

        # declare class level variables
        self.startTime = 0
        self.stopTime = 0
        self.elapsedTime = 0.0

        # declare controls used
        self.startButton = tk.Button(self, text="Start Timing", command=self.startButtonActionPerformed)
        self.stopButton = tk.Button(self, text="Stop Timing", command=self.stopButtonActionPerformed)
        self.exitButton = tk.Button(self, text="Exit", command=self.exitButtonActionPerformed)
        
        self.startLabel = tk.Label(self, text="Start Time")
        self.stopLabel = tk.Label(self, text="Stop Time")
        self.elapsedLabel = tk.Label(self, text="Elapsed Time (sec)")
        
        self.startTextField = tk.Entry(self, width=15)
        self.stopTextField = tk.Entry(self, width=15)
        self.elapsedTextField = tk.Entry(self, width=15)

        # add controls (Equivalente a GridBagConstraints)
        self.startButton.grid(row=0, column=0, padx=5, pady=5)
        self.stopButton.grid(row=1, column=0, padx=5, pady=5)
        self.exitButton.grid(row=2, column=0, padx=5, pady=5)

        self.startLabel.grid(row=0, column=1, padx=5, pady=5)
        self.stopLabel.grid(row=1, column=1, padx=5, pady=5)
        self.elapsedLabel.grid(row=2, column=1, padx=5, pady=5)

        self.startTextField.grid(row=0, column=2, padx=5, pady=5)
        self.stopTextField.grid(row=1, column=2, padx=5, pady=5)
        self.elapsedTextField.grid(row=2, column=2, padx=5, pady=5)

    def startButtonActionPerformed(self):
        # click of start timing button
        self.startTime = int(time.time() * 1000)
        
        self.startTextField.delete(0, tk.END)
        self.startTextField.insert(0, str(self.startTime))
        
        self.stopTextField.delete(0, tk.END)
        self.elapsedTextField.delete(0, tk.END)

    def stopButtonActionPerformed(self):
        # click of stop timing button
        self.stopTime = int(time.time() * 1000)
        
        self.stopTextField.delete(0, tk.END)
        self.stopTextField.insert(0, str(self.stopTime))
        
        self.elapsedTime = (self.stopTime - self.startTime) / 1000.0
        
        self.elapsedTextField.delete(0, tk.END)
        self.elapsedTextField.insert(0, str(self.elapsedTime))

    def exitButtonActionPerformed(self):
        self.destroy()

    def exitForm(self):
        self.destroy()

if __name__ == "__main__":
    app = Stopwatch()
    app.mainloop()