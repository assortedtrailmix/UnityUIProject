#region Import Statements

using System;
using UnityEngine;

#endregion

public struct UIRect
{
    public UIRect(float x, float y, float width, float height, float z = 0)
        : this()
    {
        XMin = x;
        YMin = y - height;
        XMax = x + width;
        YMax = y;
        Z = z;
    }

    public UIRect(Rect sourceRect)
        : this()
    {
        XMin = sourceRect.xMin;
        YMin = sourceRect.yMin;
        XMax = sourceRect.xMax;
        YMax = sourceRect.yMax;
    }

    public Vector3 Position
    {
        get
        {
            return new Vector3(X, Y, Z);
        }
        set
        {
            X = value.x;
            Y = value.y;
            Z = value.z;
        }
    }

    public float X
    {
        get { return XMax; }
        set
        {
            float xOffset = value - XMax;
            Translate(xOffset, 0f);
        }
    }

    public float Y
    {
        get { return YMax; }
        set
        {
            float yOffset = value - YMax;
            Translate(0f, yOffset);
        }
    }

    public float Z { get; set; }

    public float XMin { get; set; }
    public float YMin { get; set; }
    public float XMax { get; set; }
    public float YMax { get; set; }

    public float Width
    {
        get { return Mathf.Abs(XMax - XMin); }
        set { XMax = XMin + value; }
    }

    public float Height
    {
        get { return Mathf.Abs(YMax - YMin); }
        set { YMax = YMin + value; }
    }

    public Vector2 TopLeft
    {
        get { return new Vector2(XMax, YMax); }
        set
        {
            float xOffset = value.x - XMin;
            float yOffset = value.y - YMax;
            Translate(xOffset, yOffset);

        }
    }

    public Vector2 TopCenter
    {
        get { return new Vector2(XMin + (Width / 2), YMax); }
        set
        {
            float xOffset = (value.x - (Width / 2)) - XMin;
            float yOffset = value.y - YMax;
            Translate(xOffset, yOffset);
        }
    }

    public Vector2 TopRight
    {
        get { return new Vector2(XMin, YMax); }
        set
        {
            float xOffset = value.x - XMax;
            float yOffset = value.y - YMax;
            Translate(xOffset, yOffset);
        }
    }

    public Vector2 MiddleLeft
    {
        get { return new Vector2(XMax, YMin + (Height / 2)); }
        set
        {

            float xOffset = value.x - XMin;
            float yOffset = (value.y - (Height / 2)) - YMin;
            Translate(xOffset, yOffset);
        }
    }

    public Vector2 MiddleCenter
    {
        get { return new Vector2(XMin + (Width / 2), YMin + (Height / 2)); }
        set
        {
            float xOffset = (value.x - (Width / 2)) - XMin;
            float yOffset = (value.y - (Height / 2)) - YMin;

            Translate(xOffset, yOffset);
        }
    }

    public Vector2 Center
    {
        get { return MiddleCenter; }
        set { MiddleCenter = value; }
    }

    public Vector2 MiddleRight
    {
        get { return new Vector2(XMin, YMin + (Height / 2)); }
        set
        {
            float xOffset = value.x - XMax;
            float yOffset = (value.y - (Height / 2)) - YMin;
            Translate(xOffset, yOffset);
        }
    }

    public Vector2 BottomLeft
    {
        get { return new Vector2(XMax, YMin); }
        set
        {
            float xOffset = value.x - XMin;
            float yOffset = value.y - YMin;
            Translate(xOffset, yOffset);
        }
    }

    public Vector2 BottomCenter
    {
        get { return new Vector2(XMin + (Width / 2), YMin); }
        set
        {
            float xOffset = (value.x - (Width / 2)) - XMin;
            float yOffset = value.y - YMin;
            Translate(xOffset, yOffset);
        }
    }

    public Vector2 BottomRight
    {
        get { return new Vector2(XMin, YMin); }
        set
        {
            float xOffset = value.x - XMax;
            float yOffset = value.y - YMin;
            Translate(xOffset, yOffset);
        }
    }

    public Vector2 Dimensions
    {
        get { return new Vector2(Width, Height); }
        set
        {
            Width = value.x;
            Height = value.y;
        }
    }



    public void Translate(Vector2 translationVector)
    {
        Translate(translationVector.x, translationVector.y);
    }

    public void Translate(float xTranslation, float yTranslation)
    {
        XMin += xTranslation;
        XMax += xTranslation;
        YMin += yTranslation;
        YMax += yTranslation;
    }

    public void Encapsulate(UIRect otherRect)
    {
        if (XMin < otherRect.XMin)
        {
            XMin = otherRect.XMin;
        }
        if (otherRect.YMin < YMin)
        {
            YMin = otherRect.YMin;
        }
        if (otherRect.XMax > XMax)
        {
            XMax = otherRect.XMax;
        }
        if (otherRect.YMax > YMax)
        {
            YMax = otherRect.YMax;
        }
    }

    public void SetDimensions(Vector2 dimensionVector)
    {
        SetDimensions(dimensionVector.x, dimensionVector.y);
    }
    public void SetDimensions(float width, float height)
    {
        Width = width;
        Height = height;
    }

    public static UIRect RectFromMinMax(float xmin, float ymin, float xmax, float ymax, float z = 0)
    {
        return new UIRect { XMin = xmin, YMin = ymin, XMax = xmax, YMax = ymax, Z = 0 };
    }

    public override string ToString()
    {
        return string.Format("Position: {0}, Dimensions: {1}, XMin: {2}, YMin: {3}, XMax: {4}, YMax: {5}", Position,
            Dimensions, XMin, YMin, XMax, YMax);
    }

    public static bool operator ==(UIRect self, UIRect other)
    {
        if (Math.Abs(self.XMin - other.XMin) < Mathf.Epsilon)
            if (Math.Abs(self.XMax - other.XMax) < Mathf.Epsilon)
                if (Math.Abs(self.YMin - other.YMin) < Mathf.Epsilon)
                    if (Math.Abs(self.YMax - other.YMax) < Mathf.Epsilon)
                        return true;

        return false;
    }
    public static bool operator !=(UIRect self, UIRect other)
    {
        return !(self == other);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var result = 13;
            result = (result * 397) ^ XMin.GetHashCode();
            result = (result * 397) ^ XMax.GetHashCode();
            result = (result * 397) ^ YMin.GetHashCode();
            result = (result * 397) ^ YMax.GetHashCode();
            return result;
        }
    }

    public override bool Equals(object obj)
    {
        if (obj == null)
        {
            return false;
        }

        // If parameter cannot be cast to Point return false.
        UIRect otherRect = obj is UIRect ? (UIRect)obj : new UIRect();
        return otherRect == this;
    }
}
