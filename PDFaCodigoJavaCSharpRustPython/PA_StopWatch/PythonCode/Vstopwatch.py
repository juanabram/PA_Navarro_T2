import tkinter as tk

class Vstopwatch(tk.Tk):
    def __init__(self):
        super().__init__()
        self.title("Stopwatch MVC")
        self.geometry("300x200")
        
        # Emular FlowLayout
        self.timeField = tk.Entry(self, font=("Arial", 24, "bold"), justify="center")
        self.timeField.insert(0, "00:00:00")
        self.timeField.config(state="readonly")
        self.timeField.pack(pady=20)
        
        btn_frame = tk.Frame(self)
        btn_frame.pack()
        
        self.startButton = tk.Button(btn_frame, text="Start")
        self.startButton.pack(side=tk.LEFT, padx=5)
        
        self.stopButton = tk.Button(btn_frame, text="Stop")
        self.stopButton.pack(side=tk.LEFT, padx=5)
        
        self.exitButton = tk.Button(btn_frame, text="Exit")
        self.exitButton.pack(side=tk.LEFT, padx=5)

    def getStartButton(self):
        return self.startButton

    def getStopButton(self):
        return self.stopButton

    def getExitButton(self):
        return self.exitButton

    def setTime(self, time_str):
        self.timeField.config(state="normal")
        self.timeField.delete(0, tk.END)
        self.timeField.insert(0, time_str)
        self.timeField.config(state="readonly")