use fltk::{button::Button, enums::Font, group::Pack, input::Input, prelude::*, window::Window};

#[allow(non_snake_case)]
pub struct Vstopwatch {
    pub wind: Window,
    pub startButton: Button,
    pub stopButton: Button,
    pub exitButton: Button,
    pub timeField: Input,
}

impl Vstopwatch {
    pub fn new() -> Self {
        let mut wind = Window::default().with_size(300, 200).with_label("Stopwatch MVC");
        let mut pack = Pack::default_fill().with_size(300, 200).center_of_parent();
        
        let mut timeField = Input::default().with_size(200, 40);
        timeField.set_value("00:00:00");
        timeField.set_readonly(true);
        timeField.set_text_font(Font::HelveticaBold);
        timeField.set_text_size(24);
        
        let mut btn_pack = Pack::default().with_size(200, 40).with_type(fltk::group::PackType::Horizontal);
        let startButton = Button::default().with_size(60, 40).with_label("Start");
        let stopButton = Button::default().with_size(60, 40).with_label("Stop");
        let exitButton = Button::default().with_size(60, 40).with_label("Exit");
        btn_pack.end();
        
        pack.end();
        wind.end();
        wind.show();

        Self { wind, startButton, stopButton, exitButton, timeField }
    }

    #[allow(non_snake_case)]
    pub fn getStartButton(&mut self) -> &mut Button { &mut self.startButton }
    #[allow(non_snake_case)]
    pub fn getStopButton(&mut self) -> &mut Button { &mut self.stopButton }
    #[allow(non_snake_case)]
    pub fn getExitButton(&mut self) -> &mut Button { &mut self.exitButton }
    #[allow(non_snake_case)]
    pub fn setTime(&mut self, time: &str) { self.timeField.set_value(time); }
}