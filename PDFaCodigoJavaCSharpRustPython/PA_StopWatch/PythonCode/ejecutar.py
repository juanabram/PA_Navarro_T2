from Mstopwatch import Mstopwatch
from Vstopwatch import Vstopwatch
from Cstopwatch import Cstopwatch

if __name__ == "__main__":
    model = Mstopwatch()
    view = Vstopwatch()
    Cstopwatch(model, view)
    view.mainloop()