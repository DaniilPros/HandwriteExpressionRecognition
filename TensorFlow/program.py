from tkinter import *

trace = 0


class CanvasEvents:
    def __init__(self, parent=None):
        canvas = Canvas(parent, width=1000, height=1000, bg='white')
        canvas.pack()
        canvas.bind('<ButtonPress-1>', self.onStart)
        canvas.bind('<B1-Motion>', self.onDraw)
        canvas.bind('<Double-1>', self.onClear)
        canvas.bind('<ButtonPress-3>', self.onMove)
        self.canvas = canvas
        self.drawn = None
        self.kind = canvas.create_line

    def onStart(self, event):
        self.shape = self.kind
        self.start = event
        self.drawn = None

    def onDraw(self, event):
        canvas = event.widget
        #if self.drawn: canvas.delete(self.drawn)
        objectId = self.shape(self.start.x, self.start.y, event.x, event.y, width=3)
        self.start.x = event.x
        self.start.y = event.y
        if trace: print(objectId)
        self.drawn = objectId

    def onClear(self, event):
        event.widget.delete('all')

    def onMove(self, event):
        if self.drawn:
            if trace: print(self.drawn)
            canvas = event.widget
            diffX, diffY = (event.x - self.start.x), (event.y - self.start.y)
            canvas.move(self.drawn, diffX, diffY)
            self.start = event

if __name__ == "__main__":
    root = Tk()
    canvasEvents = CanvasEvents(root)

    root.mainloop()
