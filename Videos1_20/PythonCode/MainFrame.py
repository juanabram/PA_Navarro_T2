import tkinter as tk
import sys
from Toolbar import Toolbar
from TextPanel import TextPanel
from FormPanel import FormPanel
from StringListener import StringListener
from FormListener import FormListener

class AppStringListener(StringListener):
    def __init__(self, textPanel):
        self.textPanel = textPanel
    def textEmitted(self, text):
        self.textPanel.appendText(text)

class AppFormListener(FormListener):
    def __init__(self, textPanel):
        self.textPanel = textPanel
    def formEventOccurred(self, e):
        name = e.getName()
        occupation = e.getOccupation()
        ageCat = e.getAgeCategory()
        empCat = e.getEmploymentCategory()
        taxId = e.getTaxId()
        usCitizen = e.isUsCitizen()
        self.textPanel.appendText(f"{name}: {occupation}: Age {ageCat}: {empCat}, Citizen: {usCitizen}, Tax ID: {taxId}\n")

class MainFrame(tk.Tk):
    def __init__(self):
        super().__init__()
        self.title("Hello World")
        self.geometry("600x500")

        self.toolbar = Toolbar(self)
        self.textPanel = TextPanel(self)
        self.formPanel = FormPanel(self)

        self.config(menu=self.createMenuBar())

        self.toolbar.setStringListener(AppStringListener(self.textPanel))
        self.formPanel.setFormListener(AppFormListener(self.textPanel))

        self.toolbar.pack(side=tk.TOP, fill=tk.X)
        self.formPanel.pack(side=tk.LEFT, fill=tk.Y)
        self.textPanel.pack(side=tk.LEFT, fill=tk.BOTH, expand=True)

    def createMenuBar(self):
        menuBar = tk.Menu(self)
        
        fileMenu = tk.Menu(menuBar, tearoff=0)
        fileMenu.add_command(label="Exit", command=lambda: sys.exit(0))
        
        windowMenu = tk.Menu(menuBar, tearoff=0)
        showMenu = tk.Menu(windowMenu, tearoff=0)
        
        self.showFormItem = tk.BooleanVar(value=True)
        showMenu.add_checkbutton(label="Person Form", variable=self.showFormItem, command=self.toggleForm)
        
        windowMenu.add_cascade(label="Show", menu=showMenu)
        
        menuBar.add_cascade(label="File", menu=fileMenu)
        menuBar.add_cascade(label="Window", menu=windowMenu)
        
        return menuBar

    def toggleForm(self):
        if self.showFormItem.get():
            self.formPanel.pack(side=tk.LEFT, fill=tk.Y, before=self.textPanel)
        else:
            self.formPanel.pack_forget()