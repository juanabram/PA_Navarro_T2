use std::time::{SystemTime, UNIX_EPOCH};

#[allow(non_snake_case)]
pub struct Mstopwatch {
    startTime: i64,
    elapsedTime: i64,
    running: bool,
}

impl Mstopwatch {
    pub fn new() -> Self {
        Self { startTime: 0, elapsedTime: 0, running: false }
    }

    #[allow(non_snake_case)]
    pub fn start(&mut self) {
        self.startTime = Self::current_time_millis();
        self.running = true;
    }

    #[allow(non_snake_case)]
    pub fn stop(&mut self) {
        self.elapsedTime = Self::current_time_millis() - self.startTime;
        self.running = false;
    }

    #[allow(non_snake_case)]
    pub fn getElapsedTime(&self) -> i64 {
        if self.running { Self::current_time_millis() - self.startTime } else { self.elapsedTime }
    }

    #[allow(non_snake_case)]
    pub fn getFormattedTime(&self) -> String {
        let totalSeconds = self.getElapsedTime() / 1000;
        let hours = totalSeconds / 3600;
        let minutes = (totalSeconds % 3600) / 60;
        let seconds = totalSeconds % 60;
        format!("{:02}:{:02}:{:02}", hours, minutes, seconds)
    }

    #[allow(non_snake_case)]
    pub fn isRunning(&self) -> bool { self.running }

    fn current_time_millis() -> i64 {
        SystemTime::now().duration_since(UNIX_EPOCH).unwrap().as_millis() as i64
    }
}