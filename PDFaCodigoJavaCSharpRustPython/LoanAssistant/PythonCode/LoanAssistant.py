import tkinter as tk
from tkinter import messagebox
import math

class LoanAssistant(tk.Tk):
    def __init__(self):
        super().__init__()
        self.title("Loan Assistant")
        self.resizable(False, False)
        self.protocol("WM_DELETE_WINDOW", self.exitForm)

        self.myFont = ("Arial", 12)
        self.lightYellow = "#FFFF80"  # Equivalent to (255, 255, 128)
        self.computePayment = True

        # Controls
        self.balanceLabel = tk.Label(self, text="Loan Balance", font=self.myFont)
        self.balanceTextField = tk.Entry(self, font=self.myFont, justify="right", width=12)
        
        self.interestLabel = tk.Label(self, text="Interest Rate", font=self.myFont)
        self.interestTextField = tk.Entry(self, font=self.myFont, justify="right", width=12)
        
        self.monthsLabel = tk.Label(self, text="Number of Payments", font=self.myFont)
        self.monthsTextField = tk.Entry(self, font=self.myFont, justify="right", width=12)
        
        self.paymentLabel = tk.Label(self, text="Monthly Payment", font=self.myFont)
        self.paymentTextField = tk.Entry(self, font=self.myFont, justify="right", width=12)
        
        self.computeButton = tk.Button(self, text="Compute Monthly Payment", command=self.computeButtonActionPerformed)
        self.newLoanButton = tk.Button(self, text="New Loan Analysis", command=self.newLoanButtonActionPerformed)
        self.newLoanButton.config(state=tk.DISABLED)
        
        self.monthsButton = tk.Button(self, text="X", command=self.monthsButtonActionPerformed, takefocus=0)
        self.paymentButton = tk.Button(self, text="X", command=self.paymentButtonActionPerformed, takefocus=0)
        
        self.analysisLabel = tk.Label(self, text="Loan Analysis:", font=self.myFont)
        self.analysisTextArea = tk.Text(self, font=("Courier New", 10), width=30, height=10, borderwidth=1, relief="solid")
        self.analysisTextArea.config(state=tk.DISABLED, bg="white")
        
        self.exitButton = tk.Button(self, text="Exit", command=self.exitButtonActionPerformed, takefocus=0)

        # Layout (Equivalent to GridBagLayout)
        self.balanceLabel.grid(row=0, column=0, sticky="w", padx=(10, 0), pady=(10, 0))
        self.balanceTextField.grid(row=0, column=1, padx=10, pady=(10, 0))
        
        self.interestLabel.grid(row=1, column=0, sticky="w", padx=(10, 0), pady=(10, 0))
        self.interestTextField.grid(row=1, column=1, padx=10, pady=(10, 0))
        
        self.monthsLabel.grid(row=2, column=0, sticky="w", padx=(10, 0), pady=(10, 0))
        self.monthsTextField.grid(row=2, column=1, padx=10, pady=(10, 0))
        self.monthsButton.grid(row=2, column=2, padx=(0, 10), pady=(10, 0))
        
        self.paymentLabel.grid(row=3, column=0, sticky="w", padx=(10, 0), pady=(10, 0))
        self.paymentTextField.grid(row=3, column=1, padx=10, pady=(10, 0))
        self.paymentButton.grid(row=3, column=2, padx=(0, 10), pady=(10, 0))
        
        self.computeButton.grid(row=4, column=0, columnspan=2, pady=(10, 0))
        self.newLoanButton.grid(row=5, column=0, columnspan=2, pady=10)
        
        self.analysisLabel.grid(row=0, column=3, sticky="w", padx=(10, 10))
        self.analysisTextArea.grid(row=1, column=3, rowspan=4, padx=(0, 10))
        self.exitButton.grid(row=5, column=3, pady=(0, 10))

        # Event Bindings for Enter key (transferFocus)
        self.balanceTextField.bind("<Return>", self.balanceTextFieldActionPerformed)
        self.interestTextField.bind("<Return>", self.interestTextFieldActionPerformed)
        self.monthsTextField.bind("<Return>", self.monthsTextFieldActionPerformed)
        self.paymentTextField.bind("<Return>", self.paymentTextFieldActionPerformed)

        # Initialize location and state
        self.centerWindow()
        self.paymentButtonActionPerformed()

    def centerWindow(self):
        self.update_idletasks()
        width, height = self.winfo_width(), self.winfo_height()
        x = (self.winfo_screenwidth() // 2) - (width // 2)
        y = (self.winfo_screenheight() // 2) - (height // 2)
        self.geometry(f'{width}x{height}+{x}+{y}')

    def exitForm(self):
        self.destroy()

    def exitButtonActionPerformed(self):
        self.destroy()

    def balanceTextFieldActionPerformed(self, event=None):
        self.interestTextField.focus_set()

    def interestTextFieldActionPerformed(self, event=None):
        self.monthsTextField.focus_set()

    def monthsTextFieldActionPerformed(self, event=None):
        self.paymentTextField.focus_set()

    def paymentTextFieldActionPerformed(self, event=None):
        self.computeButton.focus_set()

    def validateDecimalNumber(self, tf):
        s = tf.get().strip()
        hasDecimal = False
        valid = True
        
        if len(s) == 0:
            valid = False
        else:
            for c in s:
                if '0' <= c <= '9':
                    continue
                elif c == '.' and not hasDecimal:
                    hasDecimal = True
                else:
                    valid = False
                    break
        
        tf.delete(0, tk.END)
        tf.insert(0, s)
        if not valid:
            tf.focus_set()
        return valid

    def monthsButtonActionPerformed(self, event=None):
        self.computePayment = False
        self.paymentButton.grid()
        self.monthsButton.grid_remove()
        
        self.monthsTextField.delete(0, tk.END)
        self.monthsTextField.config(state=tk.DISABLED, bg=self.lightYellow, takefocus=0)
        
        self.paymentTextField.config(state=tk.NORMAL, bg="white", takefocus=1)
        self.computeButton.config(text="Compute Number of Payments")
        self.balanceTextField.focus_set()

    def paymentButtonActionPerformed(self, event=None):
        self.computePayment = True
        self.paymentButton.grid_remove()
        self.monthsButton.grid()
        
        self.monthsTextField.config(state=tk.NORMAL, bg="white", takefocus=1)
        self.paymentTextField.delete(0, tk.END)
        self.paymentTextField.config(state=tk.DISABLED, bg=self.lightYellow, takefocus=0)
        
        self.computeButton.config(text="Compute Monthly Payment")
        self.balanceTextField.focus_set()

    def newLoanButtonActionPerformed(self, event=None):
        if self.computePayment:
            self.paymentTextField.config(state=tk.NORMAL)
            self.paymentTextField.delete(0, tk.END)
            self.paymentTextField.config(state=tk.DISABLED)
        else:
            self.monthsTextField.config(state=tk.NORMAL)
            self.monthsTextField.delete(0, tk.END)
            self.monthsTextField.config(state=tk.DISABLED)
            
        self.analysisTextArea.config(state=tk.NORMAL)
        self.analysisTextArea.delete(1.0, tk.END)
        self.analysisTextArea.config(state=tk.DISABLED)
        
        self.computeButton.config(state=tk.NORMAL)
        self.newLoanButton.config(state=tk.DISABLED)
        self.balanceTextField.focus_set()

    def computeButtonActionPerformed(self, event=None):
        if self.validateDecimalNumber(self.balanceTextField):
            balance = float(self.balanceTextField.get())
        else:
            messagebox.showinfo("Balance Input Error", "Invalid or empty Loan Balance entry.\nPlease correct.")
            return

        if self.validateDecimalNumber(self.interestTextField):
            interest = float(self.interestTextField.get())
        else:
            messagebox.showinfo("Interest Input Error", "Invalid or empty Interest Rate entry.\nPlease correct.")
            return

        monthlyInterest = interest / 1200.0

        if self.computePayment:
            if self.validateDecimalNumber(self.monthsTextField):
                months = int(self.monthsTextField.get())
            else:
                messagebox.showinfo("Number of Payments Input Error", "Invalid or empty Number of Payments entry.\nPlease correct.")
                return

            if interest == 0:
                payment = balance / months
            else:
                multiplier = math.pow(1 + monthlyInterest, months)
                payment = balance * monthlyInterest * multiplier / (multiplier - 1)
            
            self.paymentTextField.config(state=tk.NORMAL)
            self.paymentTextField.delete(0, tk.END)
            self.paymentTextField.insert(0, f"{payment:.2f}")
            self.paymentTextField.config(state=tk.DISABLED)

        else:
            if self.validateDecimalNumber(self.paymentTextField):
                payment = float(self.paymentTextField.get())
                min_payment = balance * monthlyInterest + 1.0
                if payment <= min_payment:
                    resp = messagebox.askyesno("Input Error", f"Minimum payment must be ${int(min_payment):.2f}\nDo you want to use the minimum payment?")
                    if resp:
                        self.paymentTextField.delete(0, tk.END)
                        self.paymentTextField.insert(0, f"{int(min_payment):.2f}")
                        payment = float(self.paymentTextField.get())
                    else:
                        self.paymentTextField.focus_set()
                        return
            else:
                messagebox.showinfo("Payment Input Error", "Invalid or empty Monthly Payment entry.\nPlease correct.")
                return

            if interest == 0:
                months = int(balance / payment)
            else:
                months = int((math.log(payment) - math.log(payment - balance * monthlyInterest)) / math.log(1 + monthlyInterest))
            
            self.monthsTextField.config(state=tk.NORMAL)
            self.monthsTextField.delete(0, tk.END)
            self.monthsTextField.insert(0, str(months))
            self.monthsTextField.config(state=tk.DISABLED)

        payment = float(self.paymentTextField.get())

        # show analysis
        self.analysisTextArea.config(state=tk.NORMAL)
        self.analysisTextArea.delete(1.0, tk.END)
        self.analysisTextArea.insert(tk.END, f"Loan Balance: ${balance:.2f}")
        self.analysisTextArea.insert(tk.END, f"\nInterest Rate: {interest:.2f}%")

        loanBalance = balance
        for paymentNumber in range(1, months):
            loanBalance += loanBalance * monthlyInterest - payment

        finalPayment = loanBalance
        if finalPayment > payment:
            loanBalance += loanBalance * monthlyInterest - payment
            finalPayment = loanBalance
            months += 1
            self.monthsTextField.config(state=tk.NORMAL)
            self.monthsTextField.delete(0, tk.END)
            self.monthsTextField.insert(0, str(months))
            self.monthsTextField.config(state=tk.DISABLED)

        self.analysisTextArea.insert(tk.END, f"\n\n{months - 1} Payments of ${payment:.2f}")
        self.analysisTextArea.insert(tk.END, f"\nFinal Payment of: ${finalPayment:.2f}")
        self.analysisTextArea.insert(tk.END, f"\nTotal Payments: ${(months - 1) * payment + finalPayment:.2f}")
        self.analysisTextArea.insert(tk.END, f"\nInterest Paid ${((months - 1) * payment + finalPayment - balance):.2f}")
        self.analysisTextArea.config(state=tk.DISABLED)

        self.computeButton.config(state=tk.DISABLED)
        self.newLoanButton.config(state=tk.NORMAL)
        self.newLoanButton.focus_set()

if __name__ == "__main__":
    app = LoanAssistant()
    app.mainloop()