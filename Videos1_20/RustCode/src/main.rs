// Archivo 1: string_listener.rs
pub type StringListener = Box<dyn FnMut(String)>;

// Archivo 2: form_listener.rs
pub type FormListener = Box<dyn FnMut(form_event::FormEvent)>;

// Archivo 3: form_event.rs
pub mod form_event {
    pub struct FormEvent {
        name: String,
        occupation: String,
        age_category: usize,
        emp_cat: String,
        tax_id: String,
        us_citizen: bool,
    }

    impl FormEvent {
        pub fn new(name: String, occupation: String, age_cat: usize, emp_cat: String, tax_id: String, us_citizen: bool) -> Self {
            Self { name, occupation, age_category: age_cat, emp_cat, tax_id, us_citizen }
        }
        pub fn get_name(&self) -> &str { &self.name }
        pub fn get_occupation(&self) -> &str { &self.occupation }
        pub fn get_age_category(&self) -> usize { self.age_category }
        pub fn get_employment_category(&self) -> &str { &self.emp_cat }
        pub fn get_tax_id(&self) -> &str { &self.tax_id }
        pub fn is_us_citizen(&self) -> bool { self.us_citizen }
    }
}

// Archivo 4: toolbar.rs
pub mod toolbar {
    use fltk::{button::Button, group::Pack, prelude::*, enums::FrameType};
    use super::StringListener;
    use std::{cell::RefCell, rc::Rc};

    pub struct Toolbar {
        pub pack: Pack,
        text_listener: Rc<RefCell<Option<StringListener>>>,
    }

    impl Toolbar {
        pub fn new() -> Self {
            let mut pack = Pack::default().with_size(600, 40);
            pack.set_type(fltk::group::PackType::Horizontal);
            pack.set_frame(FrameType::UpFrame);

            let mut hello_button = Button::default().with_size(80, 30).with_label("Hello");
            hello_button.set_pos(5, 5);
            let mut goodbye_button = Button::default().with_size(80, 30).with_label("Goodbye");
            goodbye_button.set_pos(90, 5);
            
            pack.end();

            let text_listener: Rc<RefCell<Option<StringListener>>> = Rc::new(RefCell::new(None));

            let tl_clone1 = Rc::clone(&text_listener);
            hello_button.set_callback(move |_| {
                if let Some(cb) = tl_clone1.borrow_mut().as_mut() { cb("Hello\n".to_string()); }
            });

            let tl_clone2 = Rc::clone(&text_listener);
            goodbye_button.set_callback(move |_| {
                if let Some(cb) = tl_clone2.borrow_mut().as_mut() { cb("Goodbye\n".to_string()); }
            });

            Self { pack, text_listener }
        }

        pub fn set_string_listener<F: FnMut(String) + 'static>(&self, listener: F) {
            *self.text_listener.borrow_mut() = Some(Box::new(listener));
        }
    }
}

// Archivo 5: text_panel.rs
pub mod text_panel {
    use fltk::{text::{TextBuffer, TextDisplay}, group::Group, prelude::*};

    #[derive(Clone)]
    pub struct TextPanel {
        pub group: Group,
        buffer: TextBuffer,
    }

    impl TextPanel {
        pub fn new(w: i32, h: i32) -> Self {
            let group = Group::default().with_size(w, h);
            let buffer = TextBuffer::default();
            let mut display = TextDisplay::default_fill();
            display.set_buffer(buffer.clone());
            group.end();
            Self { group, buffer }
        }

        pub fn append_text(&mut self, text: &str) {
            self.buffer.append(text);
        }
    }
}

// Archivo 6: form_panel.rs
pub mod form_panel {
    use fltk::{button::{Button, CheckButton}, frame::Frame, input::Input, browser::SelectBrowser, menu::Choice, group::Group, prelude::*, enums::{Align, FrameType}};
    use super::{form_event::FormEvent, FormListener};
    use std::{cell::RefCell, rc::Rc};

    pub struct FormPanel {
        pub group: Group,
        form_listener: Rc<RefCell<Option<FormListener>>>,
    }

    impl FormPanel {
        pub fn new() -> Self {
            let group = Group::default().with_size(250, 460);
            let mut inner = Group::default().with_pos(10, 10).with_size(230, 440);
            inner.set_frame(FrameType::EngravedFrame);
            inner.set_label("Add Person");
            inner.set_align(Align::TopLeft | Align::Inside);

            let _name_label = Frame::default().with_pos(10, 30).with_size(80, 25).with_label("Name: ").with_align(Align::Right | Align::Inside);
            let name_field = Input::default().with_pos(90, 30).with_size(120, 25);

            let _occ_label = Frame::default().with_pos(10, 65).with_size(80, 25).with_label("Occupation: ").with_align(Align::Right | Align::Inside);
            let occ_field = Input::default().with_pos(90, 65).with_size(120, 25);

            let _age_label = Frame::default().with_pos(10, 100).with_size(80, 25).with_label("Age: ").with_align(Align::Right | Align::Inside);
            let mut age_list = SelectBrowser::default().with_pos(90, 100).with_size(120, 60);
            age_list.add("Under 18"); age_list.add("18 to 65"); age_list.add("65 or over");
            age_list.select(2); 

            let _emp_label = Frame::default().with_pos(10, 175).with_size(80, 25).with_label("Employment: ").with_align(Align::Right | Align::Inside);
            let mut emp_combo = Choice::default().with_pos(90, 175).with_size(120, 25);
            emp_combo.add_choice("Employed|Self-employed|Unemployed"); emp_combo.set_value(0);

            let _cit_label = Frame::default().with_pos(10, 210).with_size(80, 25).with_label("US Citizen: ").with_align(Align::Right | Align::Inside);
            let mut cit_check = CheckButton::default().with_pos(90, 210).with_size(25, 25);

            let mut tax_label = Frame::default().with_pos(10, 245).with_size(80, 25).with_label("Tax ID: ").with_align(Align::Right | Align::Inside);
            let mut tax_field = Input::default().with_pos(90, 245).with_size(120, 25);
            tax_label.deactivate(); tax_field.deactivate();

            let mut tax_l_clone = tax_label.clone();
            let mut tax_f_clone = tax_field.clone();
            cit_check.set_callback(move |c| {
                if c.is_checked() { tax_l_clone.activate(); tax_f_clone.activate(); } 
                else { tax_l_clone.deactivate(); tax_f_clone.deactivate(); }
            });

            let mut ok_btn = Button::default().with_pos(90, 290).with_size(60, 25).with_label("OK");
            
            inner.end();
            group.end();

            let form_listener: Rc<RefCell<Option<FormListener>>> = Rc::new(RefCell::new(None));
            let fl_clone = Rc::clone(&form_listener);

            ok_btn.set_callback(move |_| {
                let name = name_field.value();
                let occ = occ_field.value();
                let age_cat = if age_list.value() > 0 { (age_list.value() - 1) as usize } else { 0 };
                let emp_cat = emp_combo.text(emp_combo.value()).unwrap_or_default();
                let tax = tax_field.value();
                let cit = cit_check.is_checked();

                let ev = FormEvent::new(name, occ, age_cat, emp_cat, tax, cit);
                if let Some(cb) = fl_clone.borrow_mut().as_mut() { cb(ev); }
            });

            Self { group, form_listener }
        }

        pub fn set_form_listener<F: FnMut(FormEvent) + 'static>(&self, listener: F) {
            *self.form_listener.borrow_mut() = Some(Box::new(listener));
        }
    }
}

// Archivo 7: main_frame.rs
pub mod main_frame {
    use fltk::{window::Window, menu::{SysMenuBar, MenuFlag}, group::Flex, prelude::*, app};
    use super::{toolbar::Toolbar, text_panel::TextPanel, form_panel::FormPanel};

    pub struct MainFrame {
        pub wind: Window,
    }

    impl MainFrame {
        pub fn new() -> Self {
            let mut wind = Window::default().with_size(600, 500).with_label("Hello World");
            
            let mut flex_v = Flex::default_fill().column();
            
            let mut menu = SysMenuBar::default();
            menu.add("File/Exit", fltk::enums::Shortcut::None, MenuFlag::Normal, |_| app::quit());
            flex_v.fixed(&menu, 30);

            let toolbar = Toolbar::new();
            flex_v.fixed(&toolbar.pack, 40);

            let mut flex_h = Flex::default().row();
            let mut form_panel = FormPanel::new();
            flex_h.fixed(&form_panel.group, 250);
            
            let mut text_panel = TextPanel::new(350, 430);
            flex_h.end();
            flex_v.end();

            let mut fp_clone = form_panel.group.clone();
            menu.add("Window/Show/Person Form", fltk::enums::Shortcut::None, MenuFlag::Toggle | MenuFlag::Value, move |m| {
                if let Some(item) = m.find_item("Window/Show/Person Form") {
                    if item.value() { fp_clone.show(); } else { fp_clone.hide(); }
                }
            });

            let mut tp_clone1 = text_panel.clone();
            toolbar.set_string_listener(move |text| {
                tp_clone1.append_text(&text);
            });

            let mut tp_clone2 = text_panel.clone();
            form_panel.set_form_listener(move |e| {
                let s = format!("{}: {}: Age {}: {}, Citizen: {}, Tax ID: {}\n", 
                    e.get_name(), e.get_occupation(), e.get_age_category(), 
                    e.get_employment_category(), e.is_us_citizen(), e.get_tax_id());
                tp_clone2.append_text(&s);
            });

            wind.end();
            wind.show();
            Self { wind }
        }
    }
}

// Archivo 8: app.rs (main.rs entry point)
use fltk::app;
use main_frame::MainFrame;

fn main() {
    let application = app::App::default();
    let _main_frame = MainFrame::new();
    application.run().unwrap();
}