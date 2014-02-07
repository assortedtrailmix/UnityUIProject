using System;
using UnityEngine;

public static class EventHandlerExtensions
{
    public static void Raise(this EventHandler handler, object sender, EventArgs args)
    {
        if (handler != null) handler(sender, args);
    }
}
public static class BoundsExtentions{

    public static Rect Rect(this Bounds self)
    {
        float left = self.center.x - self.extents.x;
        float top = self.center.y - self.extents.y;
        return new Rect(left, top, self.size.x, self.size.y);


    }
}
