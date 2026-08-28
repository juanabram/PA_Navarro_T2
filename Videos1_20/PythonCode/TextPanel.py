import tkinter as tk
from tkinter import scrolledtext

class TextPanel(tk.Frame):
    def __init__(self, parent):
        super().__init__(parent)
        self.textArea = scrolledtext.ScrolledText(self)
        self.textArea.pack(fill=tk.BOTH, expand=True)

    def appendText(self, text):
        self.textArea.insert(tk.END, text)
        self.textArea.see(tk.END)