use std::rc::Rc;
use std::cell::RefCell;
use fltk::{app, prelude::*};
use crate::Mstopwatch::Mstopwatch;
use crate::Vstopwatch::Vstopwatch;

pub struct Cstopwatch {
    model: Rc<RefCell<Mstopwatch>>,
    view: Rc<RefCell<Vstopwatch>>,
}

impl Cstopwatch {
    pub fn new(model: Rc<RefCell<Mstopwatch>>, view: Rc<RefCell<Vstopwatch>>) -> Self {
        let mut controller = Self { model, view };
        controller.init_listeners();
        controller
    }

    fn init_listeners(&mut self) {
        let m1 = Rc::clone(&self.model);
        let m2 = Rc::clone(&self.model);
        let v1 = Rc::clone(&self.view);
        let v2 = Rc::clone(&self.view);

        // Start 
        self.view.borrow_mut().getStartButton().set_callback(move |_| {
            m1.borrow_mut().start();
            let timer_m = Rc::clone(&m2);
            let timer_v = Rc::clone(&v1);
            
            // Loop equivalente a javax.swing.Timer
            app::add_timeout3(1.0, move |handle| {
                if timer_m.borrow().isRunning() {
                    timer_v.borrow_mut().setTime(&timer_m.borrow().getFormattedTime());
                    app::repeat_timeout3(1.0, handle);
                }
            });
        });

        // Stop
        let m3 = Rc::clone(&self.model);
        self.view.borrow_mut().getStopButton().set_callback(move |_| {
            m3.borrow_mut().stop();
        });

        // Exit
        self.view.borrow_mut().getExitButton().set_callback(|_| {
            app::quit();
        });
    }
}