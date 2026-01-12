This is a resizable and rotatable rectangle control written using C# WPF MVVM.
The 'resize and rotate' operation is dependent on the Thumb control. You can behave it in other ways for example in adoner layer.
I had been finding such a control for long time, but many solutions perform not quite well.
The key idea of this solution comes from https://shihn.ca/posts/2020/resizing-rotated-elements/, many thanks.
There are still questions remain in this solution:

1. I use System.Window.Rect so if you strech one side of rectangle to the opposite side, the Retangle would move.So i set a fixed minimized value. I would consider a better way if was fired and decide not work any more(so sad)
2. I had no time to test the uploaded code, but i ensure the origin code of this copied one runs well
3. For convenience i use PRIM as the MVVM support, you can replace it to meet your requirements

Hope this can help u

![Preview](Images\Preview.png)

![Resize](Images\Resize.png)

![Rotate1](Images\Rotate1.png)

![Rotate2](Images\Rotate2.png)
