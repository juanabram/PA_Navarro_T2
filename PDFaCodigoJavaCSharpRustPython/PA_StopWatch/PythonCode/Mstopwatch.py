import time

class Mstopwatch:
    def __init__(self):
        self.startTime = 0.0
        self.elapsedTime = 0.0
        self.running = False

    def start(self):
        self.startTime = time.time() * 1000  # Convertir a milisegundos
        self.running = True

    def stop(self):
        self.elapsedTime = (time.time() * 1000) - self.startTime
        self.running = False

    def getElapsedTime(self):
        if self.running:
            return (time.time() * 1000) - self.startTime
        return self.elapsedTime

    def getFormattedTime(self):
        totalSeconds = int(self.getElapsedTime() / 1000)
        hours = totalSeconds // 3600
        minutes = (totalSeconds % 3600) // 60
        seconds = totalSeconds % 60
        return f"{hours:02d}:{minutes:02d}:{seconds:02d}"

    def isRunning(self):
        return self.running