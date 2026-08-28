import tkinter as tk
from tkinter import filedialog, messagebox
import random
import os

class MultipleChoiceExam(tk.Tk):
    def __init__(self):
        super().__init__()
        self.title("Multiple Choice Exam - No File")
        self.resizable(False, False)
        self.protocol("WM_DELETE_WINDOW", self.exitForm)

        self.headerFont = ("Arial", 18, "bold")
        self.examItemFont = ("Arial", 16, "bold")
        
        # Class level variables
        self.examTitle = ""
        self.header1 = ""
        self.header2 = ""
        self.numberTerms = 0
        self.term1 = [""] * 100
        self.term2 = [""] * 100
        self.numberTried = 0
        self.numberCorrect = 0
        self.correctAnswer = 0

        # Menu structure
        self.mainMenuBar = tk.Menu(self)
        self.config(menu=self.mainMenuBar)
        
        self.fileMenu = tk.Menu(self.mainMenuBar, tearoff=0)
        self.openMenuItem = self.fileMenu.add_command(label="Open", command=self.openMenuItemActionPerformed)
        self.fileMenu.add_separator()
        self.exitMenuItem = self.fileMenu.add_command(label="Exit", command=self.exitMenuItemActionPerformed)
        self.mainMenuBar.add_cascade(label="File", menu=self.fileMenu)

        self.optionsMenu = tk.Menu(self.mainMenuBar, tearoff=0)
        self.nameGroup = tk.IntVar(value=1)
        self.optionsMenu.add_radiobutton(label="Header 1", variable=self.nameGroup, value=1, command=self.header1MenuItemActionPerformed)
        self.optionsMenu.add_radiobutton(label="Header 2", variable=self.nameGroup, value=2, command=self.header2MenuItemActionPerformed)
        self.optionsMenu.add_separator()
        
        self.typeGroup = tk.IntVar(value=1)
        self.optionsMenu.add_radiobutton(label="Multiple Choice Answers", variable=self.typeGroup, value=1, command=self.mcMenuItemActionPerformed)
        self.optionsMenu.add_radiobutton(label="Type In Answers", variable=self.typeGroup, value=2, command=self.typeMenuItemActionPerformed)
        self.mainMenuBar.add_cascade(label="Options", menu=self.optionsMenu)

        # Controls
        self.headGivenLabel = tk.Label(self, font=self.headerFont, width=30)
        self.headGivenLabel.grid(row=0, column=0, pady=(10, 0), padx=10)

        self.givenLabel = tk.Label(self, font=self.examItemFont, bg="white", fg="blue", relief="solid", borderwidth=1, width=30, height=2)
        self.givenLabel.grid(row=1, column=0, padx=10)

        self.headAnswerLabel = tk.Label(self, font=self.headerFont, width=30)
        self.headAnswerLabel.grid(row=2, column=0, pady=(10, 0), padx=10)

        self.answerLabel = []
        for i in range(4):
            lbl = tk.Label(self, font=self.examItemFont, bg="white", fg="blue", relief="solid", borderwidth=1, width=30, height=2)
            lbl.grid(row=i+3, column=0, pady=(0, 10), padx=10)
            lbl.bind("<Button-1>", self.answerLabelMousePressed)
            self.answerLabel.append(lbl)

        self.answerTextField = tk.Entry(self, font=self.examItemFont, bg="white", fg="blue", width=30)
        self.answerTextField.bind("<Return>", self.answerTextFieldActionPerformed)
        # Se oculta inicialmente sacándolo del grid
        
        self.commentTextArea = tk.Text(self, font=("Courier New", 14, "bold italic"), bg="#FFFFC0", fg="red", width=40, height=3)
        self.commentTextArea.grid(row=7, column=0, pady=(0, 10), padx=10)
        self.commentTextArea.config(state=tk.DISABLED)

        self.nextButton = tk.Button(self, text="Next Question", command=self.nextButtonActionPerformed)
        self.nextButton.grid(row=8, column=0, pady=(0, 10))

        self.startButton = tk.Button(self, text="Start Exam", command=self.startButtonActionPerformed)
        self.startButton.grid(row=9, column=0, pady=(0, 10))

        self.centerWindow()

        # Initialize form
        self.startButton.config(state=tk.DISABLED)
        self.nextButton.config(state=tk.DISABLED)
        self.mainMenuBar.entryconfig("Options", state=tk.DISABLED)
        self.setCommentText(self.centerTextArea("Open Exam File to Start"))

    def centerWindow(self):
        self.update_idletasks()
        width = self.winfo_width()
        height = self.winfo_height()
        x = (self.winfo_screenwidth() // 2) - (width // 2)
        y = (self.winfo_screenheight() // 2) - (height // 2)
        self.geometry(f'{width}x{height}+{x}+{y}')

    def setCommentText(self, text):
        self.commentTextArea.config(state=tk.NORMAL)
        self.commentTextArea.delete(1.0, tk.END)
        self.commentTextArea.insert(tk.END, text)
        self.commentTextArea.config(state=tk.DISABLED)

    def exitForm(self):
        self.destroy()
    def exitMenuItemActionPerformed(self):
        self.destroy()

    def answerLabelMousePressed(self, event):
        if self.startButton.cget("text") == "Start Exam" or self.nextButton.cget("state") == tk.NORMAL:
            return
        
        labelSelected = self.answerLabel.index(event.widget)
        self.numberTried += 1
        correct = False

        if self.nameGroup.get() == 1:
            if self.answerLabel[labelSelected].cget("text") == self.term1[self.correctAnswer]:
                correct = True
        else:
            if self.answerLabel[labelSelected].cget("text") == self.term2[self.correctAnswer]:
                correct = True
                
        self.updateScore(correct)

    def answerTextFieldActionPerformed(self, event):
        if self.startButton.cget("text") == "Start Exam" or self.nextButton.cget("state") == tk.NORMAL:
            return
            
        self.answerTextField.config(state=tk.DISABLED)
        self.numberTried += 1
        ucTypedAnswer = self.answerTextField.get().upper()
        
        if self.nameGroup.get() == 1:
            ucAnswer = self.term1[self.correctAnswer].upper()
        else:
            ucAnswer = self.term2[self.correctAnswer].upper()
            
        correct = False
        if ucTypedAnswer == ucAnswer or self.soundex(ucTypedAnswer) == self.soundex(ucAnswer):
            correct = True
            
        self.updateScore(correct)

    def nextButtonActionPerformed(self):
        self.nextButton.config(state=tk.DISABLED)
        self.nextQuestion()

    def startButtonActionPerformed(self):
        if self.startButton.cget("text") == "Start Exam":
            self.startButton.config(text="Stop Exam")
            self.nextButton.config(state=tk.DISABLED)
            self.numberTried = 0
            self.numberCorrect = 0
            self.setCommentText("")
            self.mainMenuBar.entryconfig("File", state=tk.DISABLED)
            self.mainMenuBar.entryconfig("Options", state=tk.DISABLED)
            self.nextQuestion()
        else:
            self.startButton.config(text="Start Exam")
            self.nextButton.config(state=tk.DISABLED)
            if self.numberTried > 0:
                score = (self.numberCorrect / self.numberTried) * 100
                message = f"Questions Tried: {self.numberTried}\nQuestions Correct: {self.numberCorrect}\n\nYour Score: {score:.1f}%"
                messagebox.showinfo(f"{self.examTitle} Results", message)
            
            self.givenLabel.config(text="")
            for i in range(4): self.answerLabel[i].config(text="")
            self.answerTextField.config(state=tk.NORMAL)
            self.answerTextField.delete(0, tk.END)
            self.setCommentText(self.centerTextArea("Choose Options\nClick Start Exam"))
            self.mainMenuBar.entryconfig("File", state=tk.NORMAL)
            self.mainMenuBar.entryconfig("Options", state=tk.NORMAL)

    def openMenuItemActionPerformed(self):
        filepath = filedialog.askopenfilename(title="Open Exam File", filetypes=(("Exam Files", "*.csv"), ("All Files", "*.*")))
        if not filepath: return
        
        try:
            with open(filepath, 'r', encoding='utf-8') as inputFile:
                lines = [line.strip() for line in inputFile.readlines() if line.strip()]
            
            self.examTitle = self.parseLeft(lines[0])
            self.header1 = self.parseLeft(lines[1])
            self.header2 = self.parseRight(lines[1])
            self.numberTerms = 0
            
            for i in range(2, len(lines)):
                if self.numberTerms >= 100: break
                self.numberTerms += 1
                self.term1[self.numberTerms - 1] = self.parseLeft(lines[i])
                self.term2[self.numberTerms - 1] = self.parseRight(lines[i])
                
            if self.numberTerms < 5:
                messagebox.showerror("Exam File Error", "Must have at least 5 entries in exam file.")
                return
                
            self.title(f"Multiple Choice Exam - {self.examTitle}")
            
            if self.nameGroup.get() == 1:
                self.headGivenLabel.config(text=self.header2)
                self.headAnswerLabel.config(text=self.header1)
            else:
                self.headGivenLabel.config(text=self.header1)
                self.headAnswerLabel.config(text=self.header2)
                
            self.startButton.config(state=tk.NORMAL)
            self.mainMenuBar.entryconfig("Options", state=tk.NORMAL)
            self.setCommentText(self.centerTextArea("File Loaded, Choose Options\nClick Start Exam"))
            
        except Exception as e:
            messagebox.showerror("Multiple Choice Exam File Error", "Error reading in input file - make sure file is correct format.")

    def header1MenuItemActionPerformed(self):
        self.headGivenLabel.config(text=self.header2)
        self.headAnswerLabel.config(text=self.header1)

    def header2MenuItemActionPerformed(self):
        self.headGivenLabel.config(text=self.header1)
        self.headAnswerLabel.config(text=self.header2)

    def mcMenuItemActionPerformed(self):
        for i in range(4): self.answerLabel[i].grid(row=i+3, column=0, pady=(0, 10), padx=10)
        self.answerTextField.grid_forget()

    def typeMenuItemActionPerformed(self):
        for i in range(4): self.answerLabel[i].grid_forget()
        self.answerTextField.grid(row=3, column=0, pady=(0, 10), padx=10)

    def parseLeft(self, s):
        cl = s.find(",")
        return s[:cl] if cl != -1 else s

    def parseRight(self, s):
        cl = s.find(",")
        return s[cl+1:] if cl != -1 else ""

    def centerTextArea(self, s):
        charsPerLine = 33
        j = s.find("\n")
        if j == -1:
            return "\n" + self.spacePadding((charsPerLine - len(s)) // 2) + s
        else:
            l1 = s[:j]
            l2 = s[j+1:]
            return "\n" + self.spacePadding((charsPerLine - len(l1)) // 2) + l1 + "\n" + self.spacePadding((charsPerLine - len(l2)) // 2) + l2

    def spacePadding(self, n):
        return " " * max(0, n)

    def nextQuestion(self):
        termUsed = [False] * self.numberTerms
        index = [0] * 4
        self.setCommentText("")
        
        self.correctAnswer = random.randint(0, self.numberTerms - 1)
        
        if self.nameGroup.get() == 1:
            self.givenLabel.config(text=self.term2[self.correctAnswer])
        else:
            self.givenLabel.config(text=self.term1[self.correctAnswer])
            
        if self.typeGroup.get() == 1:
            for i in range(4):
                while True:
                    j = random.randint(0, self.numberTerms - 1)
                    if not termUsed[j] and j != self.correctAnswer:
                        break
                termUsed[j] = True
                index[i] = j
                
            index[random.randint(0, 3)] = self.correctAnswer
            
            for i in range(4):
                val = self.term1[index[i]] if self.nameGroup.get() == 1 else self.term2[index[i]]
                self.answerLabel[i].config(text=val)
        else:
            self.answerTextField.config(state=tk.NORMAL)
            self.answerTextField.delete(0, tk.END)
            self.answerTextField.focus_set()

    def updateScore(self, correct):
        if correct:
            self.numberCorrect += 1
            self.setCommentText(self.centerTextArea("Correct!"))
        else:
            self.setCommentText(self.centerTextArea("Sorry ... Correct Answer Shown"))
            
        if self.typeGroup.get() == 1:
            ans = self.term1[self.correctAnswer] if self.nameGroup.get() == 1 else self.term2[self.correctAnswer]
            self.answerLabel[0].config(text=ans)
            for i in range(1, 4): self.answerLabel[i].config(text="")
        else:
            ans = self.term1[self.correctAnswer] if self.nameGroup.get() == 1 else self.term2[self.correctAnswer]
            self.answerTextField.config(state=tk.NORMAL)
            self.answerTextField.delete(0, tk.END)
            self.answerTextField.insert(0, ans)
            
        self.startButton.config(state=tk.NORMAL)
        self.nextButton.config(state=tk.NORMAL)
        self.nextButton.focus_set()

    def soundex(self, w):
        wSound = [0, 1, 2, 3, 0, 1, 2, 0, 0, 2, 2, 4, 5, 5, 0, 1, 2, 6, 2, 3, 0, 1, 0, 2, 0, 2]
        wTemp = w.upper()
        l = len(w)
        s = ""
        
        if l != 0:
            s = wTemp[0]
            wPrev = 0
            if l > 1:
                for i in range(1, l):
                    cIndex = ord(wTemp[i]) - 65
                    if 0 <= cIndex <= 25:
                        wSnd = wSound[cIndex] + 48
                        if wSnd != 48 and wSnd != wPrev:
                            s += chr(wSnd)
                        wPrev = wSnd
        return s

if __name__ == "__main__":
    app = MultipleChoiceExam()
    app.mainloop()