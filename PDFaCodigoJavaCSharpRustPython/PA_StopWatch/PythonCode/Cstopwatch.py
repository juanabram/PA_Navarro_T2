class SwingTimerEmulator:
    def __init__(self, interval, action, root):
        self.interval = interval
        self.action = action
        self.root = root
        self.timer_id = None
        self.is_running = False

    def start(self):
        if not self.is_running:
            self.is_running = True
            self._tick()

    def stop(self):
        self.is_running = False
        if self.timer_id:
            self.root.after_cancel(self.timer_id)

    def _tick(self):
        if self.is_running:
            self.action()
            self.timer_id = self.root.after(self.interval, self._tick)

class Cstopwatch:
    def __init__(self, model, view):
        self.model = model
        self.view = view
        
        def timer_action():
            self.view.setTime(self.model.getFormattedTime())
            
        self.timer = SwingTimerEmulator(1000, timer_action, self.view)

        def start_action():
            self.model.start()
            self.timer.start()
        self.view.getStartButton().config(command=start_action)

        def stop_action():
            self.model.stop()
            self.timer.stop()
        self.view.getStopButton().config(command=stop_action)

        def exit_action():
            self.view.destroy()
        self.view.getExitButton().config(command=exit_action)