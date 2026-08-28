import tkinter as tk
from tkinter import ttk
from FormEvent import FormEvent

class FormPanel(tk.Frame):
    def __init__(self, parent):
        super().__init__(parent, width=250)
        self.pack_propagate(False)
        self.formListener = None

        inner_frame = tk.LabelFrame(self, text="Add Person", padx=5, pady=5)
        inner_frame.pack(fill=tk.BOTH, expand=True, padx=5, pady=5)

        self.nameLabel = tk.Label(inner_frame, text="Name: ")
        self.nameField = tk.Entry(inner_frame, width=15)

        self.occupationLabel = tk.Label(inner_frame, text="Occupation: ")
        self.occupationField = tk.Entry(inner_frame, width=15)

        tk.Label(inner_frame, text="Age: ").grid(row=2, column=0, sticky="ne", padx=5, pady=5)
        self.ageList = tk.Listbox(inner_frame, height=3, exportselection=False)
        self.ageList.insert(0, "Under 18")
        self.ageList.insert(1, "18 to 65")
        self.ageList.insert(2, "65 or over")
        self.ageList.selection_set(1)

        tk.Label(inner_frame, text="Employment: ").grid(row=3, column=0, sticky="ne", padx=5, pady=5)
        self.empCombo = ttk.Combobox(inner_frame, values=["Employed", "Self-employed", "Unemployed"], state="readonly", width=12)
        self.empCombo.current(0)

        tk.Label(inner_frame, text="US Citizen: ").grid(row=4, column=0, sticky="ne", padx=5, pady=5)
        self.isCitizen = tk.BooleanVar(value=False)
        self.citizenCheck = tk.Checkbutton(inner_frame, variable=self.isCitizen, command=self.checkToggled)

        self.taxLabel = tk.Label(inner_frame, text="Tax ID: ", state=tk.DISABLED)
        self.taxField = tk.Entry(inner_frame, width=15, state=tk.DISABLED)

        self.okBtn = tk.Button(inner_frame, text="OK", command=self.okBtnActionPerformed)

        self.layoutComponents(inner_frame)

    def layoutComponents(self, frame):
        self.nameLabel.grid(row=0, column=0, sticky="e", padx=5, pady=5)
        self.nameField.grid(row=0, column=1, sticky="w", padx=5, pady=5)
        
        self.occupationLabel.grid(row=1, column=0, sticky="e", padx=5, pady=5)
        self.occupationField.grid(row=1, column=1, sticky="w", padx=5, pady=5)
        
        self.ageList.grid(row=2, column=1, sticky="nw", padx=5, pady=5)
        self.empCombo.grid(row=3, column=1, sticky="nw", padx=5, pady=5)
        self.citizenCheck.grid(row=4, column=1, sticky="nw", padx=5, pady=5)
        
        self.taxLabel.grid(row=5, column=0, sticky="e", padx=5, pady=5)
        self.taxField.grid(row=5, column=1, sticky="w", padx=5, pady=5)
        
        self.okBtn.grid(row=6, column=1, sticky="nw", padx=5, pady=15)

    def checkToggled(self):
        state = tk.NORMAL if self.isCitizen.get() else tk.DISABLED
        self.taxLabel.config(state=state)
        self.taxField.config(state=state)

    def setFormListener(self, listener):
        self.formListener = listener

    def okBtnActionPerformed(self):
        name = self.nameField.get()
        occupation = self.occupationField.get()
        ageCat = self.ageList.curselection()[0] if self.ageList.curselection() else 0
        empCat = self.empCombo.get()
        taxId = self.taxField.get()
        usCitizen = self.isCitizen.get()

        ev = FormEvent(self, name, occupation, ageCat, empCat, taxId, usCitizen)
        
        if self.formListener:
            self.formListener.formEventOccurred(ev)