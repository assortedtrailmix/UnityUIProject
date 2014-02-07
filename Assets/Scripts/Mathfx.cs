using UnityEngine;

public static class Mathfx {
    public static Vector2 TopLeft(this Rect self)
    {
        return new Vector2(self.xMin,self.yMin);
    }
    public static Vector2 TopCenter(this Rect self)
    {
        return new Vector2(self.xMin + (self.width / 2), self.yMin);
    }
    public static Vector2 TopRight(this Rect self)
    {
        return new Vector2(self.xMax, self.yMin);
    }

    public static Vector2 MiddleLeft(this Rect self)
    {
        return new Vector2(self.xMin, self.yMin + (self.height / 2));
    }
    public static Vector2 MiddleCenter(this Rect self)
    {
        return new Vector2(self.xMin + (self.width / 2), self.yMin + (self.height / 2));
    }
    public static Vector2 MiddleRight(this Rect self)
    {
        return new Vector2(self.xMax, self.yMin + (self.height / 2));
    }
    public static Vector2 BottomLeft(this Rect self)
    {
        return new Vector2(self.xMin, self.yMax);
    }
    public static Vector2 BottomCenter(this Rect self)
    {
        return new Vector2(self.xMin + (self.width / 2), self.yMax);
    }
    public static Vector2 BottomRight(this Rect self)
    {
        return new Vector2(self.xMax, self.yMax);
    }
    public static void GrowToFit(this Rect self,Rect otherRect)
    {
        if(otherRect.xMin < self.xMin)
        {
            self.xMin = otherRect.xMin;
        }
        if (otherRect.yMin < self.yMin)
        {
            self.yMin = otherRect.yMin;
        }
        if (otherRect.xMax > self.xMax)
        {
            self.xMax = otherRect.xMax;
        }
        if (otherRect.yMax > self.yMax)
        {
            self.yMax = otherRect.yMax;
        }
    }
}
