import tkinter as tk

class Toolbar(tk.Frame):
    def __init__(self, parent):
        super().__init__(parent, relief=tk.GROOVE, borderwidth=2)
        self.textListener = None
        
        self.helloButton = tk.Button(self, text="Hello", command=lambda: self.actionPerformed("hello"))
        self.helloButton.pack(side=tk.LEFT, padx=5, pady=5)
        
        self.goodbyeButton = tk.Button(self, text="Goodbye", command=lambda: self.actionPerformed("goodbye"))
        self.goodbyeButton.pack(side=tk.LEFT, padx=5, pady=5)

    def setStringListener(self, listener):
        self.textListener = listener

    def actionPerformed(self, action):
        if action == "hello" and self.textListener:
            self.textListener.textEmitted("Hello\n")
        elif action == "goodbye" and self.textListener:
            self.textListener.textEmitted("Goodbye\n")