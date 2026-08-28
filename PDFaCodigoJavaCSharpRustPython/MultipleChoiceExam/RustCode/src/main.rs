use fltk::{app, button::Button, dialog, enums::{Align, Color, Event, Font, FrameType, Shortcut}, frame::Frame, input::Input, menu::{MenuBar, SysMenuBar}, prelude::*, window::Window};
use std::{cell::RefCell, fs, rc::Rc};
use rand::Rng;

#[allow(non_snake_case)]
struct MultipleChoiceExam {
    headGivenLabel: Frame,
    givenLabel: Frame,
    headAnswerLabel: Frame,
    answerLabel: Vec<Frame>,
    answerTextField: Input,
    commentTextArea: fltk::text::TextDisplay,
    text_buffer: fltk::text::TextBuffer,
    nextButton: Button,
    startButton: Button,
    
    examTitle: String,
    header1: String,
    header2: String,
    numberTerms: usize,
    term1: Vec<String>,
    term2: Vec<String>,
    numberTried: usize,
    numberCorrect: usize,
    correctAnswer: usize,
    
    isHeader1Selected: bool,
    isMcSelected: bool,
}

#[allow(non_snake_case)]
impl MultipleChoiceExam {
    fn parseLeft(s: &str) -> String {
        match s.find(',') {
            Some(idx) => s[..idx].to_string(),
            None => s.to_string(),
        }
    }

    fn parseRight(s: &str) -> String {
        match s.find(',') {
            Some(idx) => s[idx + 1..].to_string(),
            None => "".to_string(),
        }
    }

    fn spacePadding(n: i32) -> String {
        if n > 0 { " ".repeat(n as usize) } else { "".to_string() }
    }

    fn centerTextArea(&self, s: &str) -> String {
        let charsPerLine = 33;
        match s.find('\n') {
            None => format!("\n{}{}", Self::spacePadding((charsPerLine - s.len() as i32) / 2), s),
            Some(j) => {
                let l1 = &s[..j];
                let l2 = &s[j + 1..];
                format!("\n{}{}\n{}{}", Self::spacePadding((charsPerLine - l1.len() as i32) / 2), l1, Self::spacePadding((charsPerLine - l2.len() as i32) / 2), l2)
            }
        }
    }

    fn soundex(&self, w: &str) -> String {
        let wSound = [0,1,2,3,0,1,2,0,0,2,2,4,5,5,0,1,2,6,2,3,0,1,0,2,0,2];
        let wTemp = w.to_uppercase();
        if wTemp.is_empty() { return "".to_string(); }
        
        let chars: Vec<char> = wTemp.chars().collect();
        let mut s = chars[0].to_string();
        let mut wPrev = 0;
        
        for i in 1..chars.len() {
            let c_index = chars[i] as i32 - 65;
            if c_index >= 0 && c_index <= 25 {
                let wSnd = wSound[c_index as usize] + 48;
                if wSnd != 48 && wSnd != wPrev {
                    s.push(wSnd as u8 as char);
                }
                wPrev = wSnd;
            }
        }
        s
    }
}

#[allow(non_snake_case)]
fn main() {
    let app = app::App::default();
    let mut wind = Window::default().with_size(400, 580).with_label("Multiple Choice Exam - No File");
    
    let mut menu = SysMenuBar::default().with_size(400, 30);
    
    let mut headGivenLabel = Frame::default().with_pos(15, 40).with_size(370, 30);
    headGivenLabel.set_label_font(Font::HelveticaBold); headGivenLabel.set_label_size(18);
    
    let mut givenLabel = Frame::default().with_pos(15, 75).with_size(370, 30);
    givenLabel.set_frame(FrameType::BorderBox); givenLabel.set_color(Color::White); givenLabel.set_label_color(Color::Blue); givenLabel.set_label_font(Font::HelveticaBold); givenLabel.set_label_size(16);

    let mut headAnswerLabel = Frame::default().with_pos(15, 115).with_size(370, 30);
    headAnswerLabel.set_label_font(Font::HelveticaBold); headAnswerLabel.set_label_size(18);

    let mut answerLabel = Vec::new();
    for i in 0..4 {
        let mut lbl = Frame::default().with_pos(15, 150 + i * 40).with_size(370, 30);
        lbl.set_frame(FrameType::BorderBox); lbl.set_color(Color::White); lbl.set_label_color(Color::Blue); lbl.set_label_font(Font::HelveticaBold); lbl.set_label_size(16);
        answerLabel.push(lbl);
    }

    let mut answerTextField = Input::default().with_pos(15, 150).with_size(370, 30);
    answerTextField.set_text_color(Color::Blue); answerTextField.set_text_font(Font::HelveticaBold); answerTextField.set_text_size(16);
    answerTextField.hide();

    let mut text_buffer = fltk::text::TextBuffer::default();
    let mut commentTextArea = fltk::text::TextDisplay::default().with_pos(15, 330).with_size(370, 80);
    commentTextArea.set_buffer(text_buffer.clone());
    commentTextArea.set_color(Color::from_rgb(255, 255, 192)); commentTextArea.set_text_color(Color::Red); commentTextArea.set_text_font(Font::CourierBoldItalic); commentTextArea.set_text_size(16);

    let mut nextButton = Button::default().with_pos(135, 430).with_size(130, 30).with_label("Next Question");
    let mut startButton = Button::default().with_pos(135, 470).with_size(130, 30).with_label("Start Exam");

    wind.end();
    wind.show();

    let state = Rc::new(RefCell::new(MultipleChoiceExam {
        headGivenLabel, givenLabel, headAnswerLabel, answerLabel, answerTextField, commentTextArea, text_buffer, nextButton, startButton,
        examTitle: "".to_string(), header1: "".to_string(), header2: "".to_string(),
        numberTerms: 0, term1: vec!["".to_string(); 100], term2: vec!["".to_string(); 100],
        numberTried: 0, numberCorrect: 0, correctAnswer: 0,
        isHeader1Selected: true, isMcSelected: true,
    }));

    // Init UI 
    state.borrow_mut().startButton.deactivate();
    state.borrow_mut().nextButton.deactivate();
    let initial_msg = state.borrow().centerTextArea("Open Exam File to Start");
    state.borrow_mut().text_buffer.set_text(&initial_msg);

    // Menus
    let s_open = Rc::clone(&state);
    let mut wind_clone = wind.clone();
    menu.add("File/Open", Shortcut::None, fltk::menu::MenuFlag::Normal, move |_| {
        let mut chooser = dialog::FileChooser::new(".", "*.csv", dialog::FileChooserType::Single, "Open Exam File");
        chooser.show();
        while chooser.shown() { app::wait(); }
        if let Some(val) = chooser.value(1) {
            if let Ok(content) = fs::read_to_string(&val) {
                let lines: Vec<&str> = content.lines().collect();
                if lines.len() >= 3 {
                    let mut s = s_open.borrow_mut();
                    s.examTitle = MultipleChoiceExam::parseLeft(lines[0]);
                    s.header1 = MultipleChoiceExam::parseLeft(lines[1]);
                    s.header2 = MultipleChoiceExam::parseRight(lines[1]);
                    s.numberTerms = 0;
                    for i in 2..lines.len() {
                        if s.numberTerms >= 100 { break; }
                        s.numberTerms += 1;
                        
                        // Guardamos el índice en una variable local para evitar el doble préstamo
                        let idx = s.numberTerms - 1; 
                        s.term1[idx] = MultipleChoiceExam::parseLeft(lines[i]);
                        s.term2[idx] = MultipleChoiceExam::parseRight(lines[i]);
                    }
                    if s.numberTerms < 5 {
                        dialog::message_default("Must have at least 5 entries in exam file.");
                        return;
                    }
                    wind_clone.set_label(&format!("Multiple Choice Exam - {}", s.examTitle));
                    
                    let h1 = s.header1.clone(); let h2 = s.header2.clone(); let sel = s.isHeader1Selected;
                    s.headGivenLabel.set_label(if sel { &h2 } else { &h1 });
                    s.headAnswerLabel.set_label(if sel { &h1 } else { &h2 });
                    
                    s.startButton.activate();
                    let msg = s.centerTextArea("File Loaded, Choose Options\nClick Start Exam");
                    s.text_buffer.set_text(&msg);
                }
            }
        }
    });

    menu.add("File/Exit", Shortcut::None, fltk::menu::MenuFlag::Normal, |_| { app::quit(); });

    let s_opt1 = Rc::clone(&state);
    menu.add("Options/Header 1", Shortcut::None, fltk::menu::MenuFlag::Radio, move |_| {
        let mut s = s_opt1.borrow_mut(); s.isHeader1Selected = true;
        let h2 = s.header2.clone(); let h1 = s.header1.clone();
        s.headGivenLabel.set_label(&h2); s.headAnswerLabel.set_label(&h1);
    });

    let s_opt2 = Rc::clone(&state);
    menu.add("Options/Header 2", Shortcut::None, fltk::menu::MenuFlag::Radio, move |_| {
        let mut s = s_opt2.borrow_mut(); s.isHeader1Selected = false;
        let h2 = s.header2.clone(); let h1 = s.header1.clone();
        s.headGivenLabel.set_label(&h1); s.headAnswerLabel.set_label(&h2);
    });

    let s_mc = Rc::clone(&state);
    menu.add("Options/Multiple Choice Answers", Shortcut::None, fltk::menu::MenuFlag::Radio, move |_| {
        let mut s = s_mc.borrow_mut(); s.isMcSelected = true;
        for i in 0..4 { s.answerLabel[i].show(); } s.answerTextField.hide();
    });

    let s_type = Rc::clone(&state);
    menu.add("Options/Type In Answers", Shortcut::None, fltk::menu::MenuFlag::Radio, move |_| {
        let mut s = s_type.borrow_mut(); s.isMcSelected = false;
        for i in 0..4 { s.answerLabel[i].hide(); } s.answerTextField.show();
    });

    // Start Button Logic
    let s_start = Rc::clone(&state);
    state.borrow_mut().startButton.set_callback(move |btn| {
        let mut s = s_start.borrow_mut();
        if btn.label() == "Start Exam" {
            btn.set_label("Stop Exam"); s.nextButton.deactivate();
            s.numberTried = 0; s.numberCorrect = 0; s.text_buffer.set_text("");
            s.nextButton.do_callback(); // Trigger next question logic
        } else {
            btn.set_label("Start Exam"); s.nextButton.deactivate();
            if s.numberTried > 0 {
                let score = (100.0 * s.numberCorrect as f64) / s.numberTried as f64;
                dialog::message_default(&format!("Questions Tried: {}\nQuestions Correct: {}\n\nYour Score: {:.1}%", s.numberTried, s.numberCorrect, score));
            }
            s.givenLabel.set_label("");
            for i in 0..4 { s.answerLabel[i].set_label(""); }
            s.answerTextField.set_value("");
            let msg = s.centerTextArea("Choose Options\nClick Start Exam"); s.text_buffer.set_text(&msg);
        }
    });

    // Next Button (generates next question)
    let s_next = Rc::clone(&state);
    state.borrow_mut().nextButton.set_callback(move |btn| {
        btn.deactivate();
        let mut s = s_next.borrow_mut();
        s.text_buffer.set_text("");
        
        let mut rng = rand::thread_rng();
        s.correctAnswer = rng.gen_range(0..s.numberTerms);
        
        let ans_str = if s.isHeader1Selected { s.term2[s.correctAnswer].clone() } else { s.term1[s.correctAnswer].clone() };
        s.givenLabel.set_label(&ans_str);

        if s.isMcSelected {
            let mut termUsed = vec![false; s.numberTerms];
            let mut index = vec![0; 4];
            for i in 0..4 {
                let mut j; loop { j = rng.gen_range(0..s.numberTerms); if !termUsed[j] && j != s.correctAnswer { break; } }
                termUsed[j] = true; index[i] = j;
            }
            index[rng.gen_range(0..4)] = s.correctAnswer;
            
            for i in 0..4 {
                let lbl_str = if s.isHeader1Selected { s.term1[index[i]].clone() } else { s.term2[index[i]].clone() };
                s.answerLabel[i].set_label(&lbl_str);
            }
        } else {
            s.answerTextField.set_value(""); s.answerTextField.take_focus().ok();
        }
    });

    // Update Score Logic Closure helper
    let update_score = |s: &mut MultipleChoiceExam, correct: bool| {
        if correct { s.numberCorrect += 1; let msg = s.centerTextArea("Correct!"); s.text_buffer.set_text(&msg); }
        else { let msg = s.centerTextArea("Sorry ... Correct Answer Shown"); s.text_buffer.set_text(&msg); }
        
        let correct_ans = if s.isHeader1Selected { s.term1[s.correctAnswer].clone() } else { s.term2[s.correctAnswer].clone() };
        if s.isMcSelected {
            s.answerLabel[0].set_label(&correct_ans);
            for i in 1..4 { s.answerLabel[i].set_label(""); }
        } else { s.answerTextField.set_value(&correct_ans); }
        
        s.startButton.activate(); s.nextButton.activate(); s.nextButton.take_focus().ok();
    };

    // Label click handling
    for i in 0..4 {
        let s_lbl = Rc::clone(&state);
        state.borrow_mut().answerLabel[i].handle(move |lbl, ev| {
            if ev == Event::Push {
                let mut s = s_lbl.borrow_mut();
                if s.startButton.label() == "Start Exam" || s.nextButton.active() { return true; }
                s.numberTried += 1;
                let clicked_text = lbl.label();
                let correct_text = if s.isHeader1Selected { s.term1[s.correctAnswer].clone() } else { s.term2[s.correctAnswer].clone() };
                let is_correct = clicked_text == correct_text;
                update_score(&mut *s, is_correct);
                return true;
            }
            false
        });
    }

    // TextField Enter logic
    let s_txt = Rc::clone(&state);
    state.borrow_mut().answerTextField.set_trigger(fltk::enums::CallbackTrigger::EnterKey);
    state.borrow_mut().answerTextField.set_callback(move |txt| {
        let mut s = s_txt.borrow_mut();
        if s.startButton.label() == "Start Exam" || s.nextButton.active() { return; }
        s.numberTried += 1;
        let ucTypedAnswer = txt.value().to_uppercase();
        let ucAnswer = (if s.isHeader1Selected { s.term1[s.correctAnswer].clone() } else { s.term2[s.correctAnswer].clone() }).to_uppercase();
        let correct = ucTypedAnswer == ucAnswer || s.soundex(&ucTypedAnswer) == s.soundex(&ucAnswer);
        update_score(&mut *s, correct);
    });

    app.run().unwrap();
}