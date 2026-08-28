mod Mstopwatch;
mod Vstopwatch;
mod Cstopwatch;

use std::rc::Rc;
use std::cell::RefCell;
use fltk::app;

fn main() {
    let fltk_app = app::App::default();
    
    let model = Rc::new(RefCell::new(Mstopwatch::Mstopwatch::new()));
    let view = Rc::new(RefCell::new(Vstopwatch::Vstopwatch::new()));
    
    Cstopwatch::Cstopwatch::new(model, view);
    
    fltk_app.run().unwrap();
}