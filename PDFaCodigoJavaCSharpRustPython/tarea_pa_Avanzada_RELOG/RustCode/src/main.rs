use fltk::{app, button::Button, frame::Frame, input::Input, prelude::*, window::Window};
use std::cell::RefCell;
use std::rc::Rc;
use std::time::{SystemTime, UNIX_EPOCH};

#[allow(non_snake_case)]
struct Stopwatch {
    startTextField: Input,
    stopTextField: Input,
    elapsedTextField: Input,
    startTime: i64,
    stopTime: i64,
    elapsedTime: f64,
}

#[allow(non_snake_case)]
impl Stopwatch {
    fn startButtonActionPerformed(&mut self) {
        self.startTime = Self::current_time_millis();
        self.startTextField.set_value(&self.startTime.to_string());
        self.stopTextField.set_value("");
        self.elapsedTextField.set_value("");
    }

    fn stopButtonActionPerformed(&mut self) {
        self.stopTime = Self::current_time_millis();
        self.stopTextField.set_value(&self.stopTime.to_string());
        
        self.elapsedTime = (self.stopTime - self.startTime) as f64 / 1000.0;
        self.elapsedTextField.set_value(&self.elapsedTime.to_string());
    }

    fn current_time_millis() -> i64 {
        SystemTime::now().duration_since(UNIX_EPOCH).unwrap().as_millis() as i64
    }
}

#[allow(non_snake_case)]
fn main() {
    let app = app::App::default();
    
    // frame constructor
    let mut wind = Window::default().with_size(450, 150).with_label("Stopwatch Application");
    
    // declare controls used (usando coordenadas absolutas para emular GridBagLayout)
    let mut startButton = Button::new(20, 10, 100, 30, "Start Timing");
    let mut stopButton = Button::new(20, 50, 100, 30, "Stop Timing");
    let mut exitButton = Button::new(20, 90, 100, 30, "Exit");

    let _startLabel = Frame::new(140, 10, 120, 30, "Start Time");
    let _stopLabel = Frame::new(140, 50, 120, 30, "Stop Time");
    let _elapsedLabel = Frame::new(140, 90, 120, 30, "Elapsed Time (sec)");

    let startTextField = Input::new(280, 10, 140, 30, "");
    let stopTextField = Input::new(280, 50, 140, 30, "");
    let elapsedTextField = Input::new(280, 90, 140, 30, "");

    wind.end();
    wind.show();

    // Instanciar nuestra clase principal monolítica
    let stopwatch = Rc::new(RefCell::new(Stopwatch {
        startTextField,
        stopTextField,
        elapsedTextField,
        startTime: 0,
        stopTime: 0,
        elapsedTime: 0.0,
    }));

    // Action Listeners
    let sw_clone1 = Rc::clone(&stopwatch);
    startButton.set_callback(move |_| {
        sw_clone1.borrow_mut().startButtonActionPerformed();
    });

    let sw_clone2 = Rc::clone(&stopwatch);
    stopButton.set_callback(move |_| {
        sw_clone2.borrow_mut().stopButtonActionPerformed();
    });

    exitButton.set_callback(|_| {
        app::quit(); // exitButtonActionPerformed & exitForm
    });

    app.run().unwrap();
}