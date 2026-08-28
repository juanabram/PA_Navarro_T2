use fltk::{app, button::Button, dialog, enums::{CallbackTrigger, Color, Font, FrameType}, frame::Frame, input::Input, text::{TextBuffer, TextDisplay}, prelude::*, window::Window};
use std::{cell::RefCell, rc::Rc};

#[allow(non_snake_case)]
struct LoanAssistant {
    balanceTextField: Input,
    interestTextField: Input,
    monthsTextField: Input,
    paymentTextField: Input,
    computeButton: Button,
    newLoanButton: Button,
    monthsButton: Button,
    paymentButton: Button,
    analysisTextArea: TextDisplay,
    text_buffer: TextBuffer,
    
    computePayment: bool,
    lightYellow: Color,
}

#[allow(non_snake_case)]
impl LoanAssistant {
    fn validateDecimalNumber(tf: &mut Input) -> bool {
        let s = tf.value().trim().to_string();
        let mut hasDecimal = false;
        let mut valid = true;
        
        if s.is_empty() {
            valid = false;
        } else {
            for c in s.chars() {
                if c >= '0' && c <= '9' { continue; }
                else if c == '.' && !hasDecimal { hasDecimal = true; }
                else { valid = false; break; }
            }
        }
        
        tf.set_value(&s);
        if !valid { tf.take_focus().ok(); }
        valid
    }
    
    fn monthsButtonActionPerformed(&mut self) {
        self.computePayment = false;
        self.paymentButton.show();
        self.monthsButton.hide();
        
        self.monthsTextField.set_value("");
        self.monthsTextField.set_readonly(true); self.monthsTextField.set_color(self.lightYellow);
        self.paymentTextField.set_readonly(false); self.paymentTextField.set_color(Color::White);
        self.computeButton.set_label("Compute Number of Payments");
        self.balanceTextField.take_focus().ok();
    }
    
    fn paymentButtonActionPerformed(&mut self) {
        self.computePayment = true;
        self.paymentButton.hide();
        self.monthsButton.show();
        
        self.monthsTextField.set_readonly(false); self.monthsTextField.set_color(Color::White);
        self.paymentTextField.set_value("");
        self.paymentTextField.set_readonly(true); self.paymentTextField.set_color(self.lightYellow);
        self.computeButton.set_label("Compute Monthly Payment");
        self.balanceTextField.take_focus().ok();
    }
    
    fn newLoanButtonActionPerformed(&mut self) {
        if self.computePayment { self.paymentTextField.set_value(""); } 
        else { self.monthsTextField.set_value(""); }
        
        self.text_buffer.set_text("");
        self.computeButton.activate();
        self.newLoanButton.deactivate();
        self.balanceTextField.take_focus().ok();
    }
    
    fn computeButtonActionPerformed(&mut self) {
        let balance: f64; let interest: f64; let mut payment: f64; let mut months: i32;
        let monthlyInterest: f64; let multiplier: f64; let mut loanBalance: f64; let mut finalPayment: f64;
        
        if Self::validateDecimalNumber(&mut self.balanceTextField) { balance = self.balanceTextField.value().parse().unwrap(); }
        else { dialog::message_default("Invalid or empty Loan Balance entry.\nPlease correct."); return; }
        
        if Self::validateDecimalNumber(&mut self.interestTextField) { interest = self.interestTextField.value().parse().unwrap(); }
        else { dialog::message_default("Invalid or empty Interest Rate entry.\nPlease correct."); return; }
        
        monthlyInterest = interest / 1200.0;
        
        if self.computePayment {
            if Self::validateDecimalNumber(&mut self.monthsTextField) { months = self.monthsTextField.value().parse().unwrap(); }
            else { dialog::message_default("Invalid or empty Number of Payments entry.\nPlease correct."); return; }

            
            if interest == 0.0 { payment = balance / months as f64; }
            else {
                multiplier = (1.0 + monthlyInterest).powf(months as f64);
                payment = balance * monthlyInterest * multiplier / (multiplier - 1.0);
            }
            self.paymentTextField.set_value(&format!("{:.2}", payment));
        } else {
            if Self::validateDecimalNumber(&mut self.paymentTextField) {
                payment = self.paymentTextField.value().parse().unwrap();
                let min_payment = balance * monthlyInterest + 1.0;
                if payment <= min_payment {
                    if dialog::choice2_default(&format!("Minimum payment must be ${:.2}\nDo you want to use the minimum payment?", min_payment.trunc()), "No", "Yes", "") == Some(1) {
                        self.paymentTextField.set_value(&format!("{:.2}", min_payment.trunc()));
                        payment = self.paymentTextField.value().parse().unwrap();
                    } else { self.paymentTextField.take_focus().ok(); return; }
                }
            } else { dialog::message_default("Invalid or empty Monthly Payment entry.\nPlease correct."); return; }
            
            if interest == 0.0 { months = (balance / payment) as i32; }
            else { months = ((payment.ln() - (payment - balance * monthlyInterest).ln()) / (1.0 + monthlyInterest).ln()) as i32; }
            
            self.monthsTextField.set_value(&months.to_string());
        }
        
        payment = self.paymentTextField.value().parse().unwrap();
        
        let mut out_text = format!("Loan Balance: ${:.2}\nInterest Rate: {:.2}%\n\n", balance, interest);
        
        loanBalance = balance;
        for _ in 1..months {
            loanBalance += loanBalance * monthlyInterest - payment;
        }
        
        finalPayment = loanBalance;
        if finalPayment > payment {
            loanBalance += loanBalance * monthlyInterest - payment;
            finalPayment = loanBalance;
            months += 1;
            self.monthsTextField.set_value(&months.to_string());
        }
        
        out_text.push_str(&format!("{} Payments of ${:.2}\n", months - 1, payment));
        out_text.push_str(&format!("Final Payment of: ${:.2}\n", finalPayment));
        out_text.push_str(&format!("Total Payments: ${:.2}\n", (months - 1) as f64 * payment + finalPayment));
        out_text.push_str(&format!("Interest Paid ${:.2}", (months - 1) as f64 * payment + finalPayment - balance));
        
        self.text_buffer.set_text(&out_text);
        self.computeButton.deactivate();
        self.newLoanButton.activate();
        self.newLoanButton.take_focus().ok();
    }
}

#[allow(non_snake_case)]
fn main() {
    let app = app::App::default();
    let mut wind = Window::default().with_size(620, 320).with_label("Loan Assistant");
    let myFont = Font::Helvetica;
    let lightYellow = Color::from_rgb(255, 255, 128);
    
    // Labels (Col 0)
    let mut l1 = Frame::default().with_pos(10, 20).with_size(150, 30).with_label("Loan Balance");
    l1.set_label_font(myFont); l1.set_align(fltk::enums::Align::Left | fltk::enums::Align::Inside);
    let mut l2 = Frame::default().with_pos(10, 60).with_size(150, 30).with_label("Interest Rate");
    l2.set_label_font(myFont); l2.set_align(fltk::enums::Align::Left | fltk::enums::Align::Inside);
    let mut l3 = Frame::default().with_pos(10, 100).with_size(150, 30).with_label("Number of Payments");
    l3.set_label_font(myFont); l3.set_align(fltk::enums::Align::Left | fltk::enums::Align::Inside);
    let mut l4 = Frame::default().with_pos(10, 140).with_size(150, 30).with_label("Monthly Payment");
    l4.set_label_font(myFont); l4.set_align(fltk::enums::Align::Left | fltk::enums::Align::Inside);

    // TextFields (Col 1)
    let mut balanceTextField = Input::default().with_pos(160, 20).with_size(100, 30);
    balanceTextField.set_text_font(myFont);
    let mut interestTextField = Input::default().with_pos(160, 60).with_size(100, 30);
    interestTextField.set_text_font(myFont);
    let mut monthsTextField = Input::default().with_pos(160, 100).with_size(100, 30);
    monthsTextField.set_text_font(myFont);
    let mut paymentTextField = Input::default().with_pos(160, 140).with_size(100, 30);
    paymentTextField.set_text_font(myFont);
    
    // X Buttons (Col 2)
    let mut monthsButton = Button::default().with_pos(270, 100).with_size(30, 30).with_label("X");
    monthsButton.clear_visible_focus();
    let mut paymentButton = Button::default().with_pos(270, 140).with_size(30, 30).with_label("X");
    paymentButton.clear_visible_focus();
    
    // Main Buttons (Col 0-1)
    let mut computeButton = Button::default().with_pos(30, 190).with_size(230, 30).with_label("Compute Monthly Payment");
    let mut newLoanButton = Button::default().with_pos(30, 230).with_size(230, 30).with_label("New Loan Analysis");
    newLoanButton.deactivate();
    
    // Analysis Area (Col 3)
    let mut l5 = Frame::default().with_pos(330, 20).with_size(150, 20).with_label("Loan Analysis:");
    l5.set_label_font(myFont); l5.set_align(fltk::enums::Align::Left | fltk::enums::Align::Inside);
    
    let text_buffer = TextBuffer::default();
    let mut analysisTextArea = TextDisplay::default().with_pos(330, 50).with_size(250, 150);
    analysisTextArea.set_buffer(text_buffer.clone());
    analysisTextArea.set_frame(FrameType::DownBox); analysisTextArea.set_color(Color::White); analysisTextArea.set_text_font(Font::Courier);
    
    let mut exitButton = Button::default().with_pos(410, 230).with_size(80, 30).with_label("Exit");
    exitButton.clear_visible_focus();
    
    wind.end();
    wind.show();

    let state = Rc::new(RefCell::new(LoanAssistant {
        balanceTextField: balanceTextField.clone(), interestTextField: interestTextField.clone(),
        monthsTextField: monthsTextField.clone(), paymentTextField: paymentTextField.clone(),
        computeButton: computeButton.clone(), newLoanButton: newLoanButton.clone(),
        monthsButton: monthsButton.clone(), paymentButton: paymentButton.clone(),
        analysisTextArea, text_buffer, computePayment: true, lightYellow,
    }));

    // Transfer Focus Actions (Enter Key)
    let mut i_tf = interestTextField.clone();
    balanceTextField.set_trigger(CallbackTrigger::EnterKey); balanceTextField.set_callback(move |_| { i_tf.take_focus().ok(); });
    let mut m_tf = monthsTextField.clone();
    interestTextField.set_trigger(CallbackTrigger::EnterKey); interestTextField.set_callback(move |_| { m_tf.take_focus().ok(); });
    let mut p_tf = paymentTextField.clone();
    monthsTextField.set_trigger(CallbackTrigger::EnterKey); monthsTextField.set_callback(move |_| { p_tf.take_focus().ok(); });
    let mut c_btn = computeButton.clone();
    paymentTextField.set_trigger(CallbackTrigger::EnterKey); paymentTextField.set_callback(move |_| { c_btn.take_focus().ok(); });

    // Buttons Callbacks
    let s_exit = Rc::clone(&state); exitButton.set_callback(move |_| { app::quit(); });
    let s_mbtn = Rc::clone(&state); monthsButton.set_callback(move |_| { s_mbtn.borrow_mut().monthsButtonActionPerformed(); });
    let s_pbtn = Rc::clone(&state); paymentButton.set_callback(move |_| { s_pbtn.borrow_mut().paymentButtonActionPerformed(); });
    let s_new = Rc::clone(&state); newLoanButton.set_callback(move |_| { s_new.borrow_mut().newLoanButtonActionPerformed(); });
    let s_comp = Rc::clone(&state); computeButton.set_callback(move |_| { s_comp.borrow_mut().computeButtonActionPerformed(); });

    // init state
    state.borrow_mut().paymentButtonActionPerformed();
    
    app.run().unwrap();
}