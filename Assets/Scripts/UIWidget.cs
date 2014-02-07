#region Import Statements

using System;
using UnityEngine;

#pragma warning disable 0162

#endregion

public abstract class UIWidget : MonoBehaviour
{
    public delegate void OnDirtyHandler();
    
    private UIAnchorType _anchorType;
    private UICenterType _centerType;

    private UIRect _bounds;
    
    private bool _isDirty;

    private UIWidget _parent;

    public UIWidget Parent
    {
        get
        {
            return _parent;
        }
        set
        {
            _parent = value;
            _parent.OnDirty += OnDirty;
            _parent.SetDirty();
            SetDirty();
        }
    }

    public UIAnchorType AnchorType
    {
        get { return _anchorType; }
        set
        {
            _anchorType = value;
            SetDirty();
        }
    }

    public UICenterType CenterType
    {
        get { return _centerType; }
        set
        {
            _centerType = value;
            SetDirty();
        }
    }

    public Vector3 AnchorPoint
    {
        get
        {
            switch (CenterType)
            {
                case UICenterType.CENTER:
                    return Bounds.Center;

                case UICenterType.TOPLEFT:
                    return Bounds.TopLeft;

                case UICenterType.TOPCENTER:
                    return Bounds.TopCenter;

                case UICenterType.TOPRIGHT:
                    return Bounds.TopRight;

                case UICenterType.MIDDLELEFT:
                    return Bounds.MiddleLeft;

                case UICenterType.MIDDLERIGHT:
                    return Bounds.MiddleRight;

                case UICenterType.BOTTOMLEFT:
                    return Bounds.BottomLeft;

                case UICenterType.BOTTOMCENTER:
                    return Bounds.BottomCenter;

                case UICenterType.BOTTOMRIGHT:
                    return Bounds.BottomRight;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public Vector2 Size
    {
        get { return Bounds.Dimensions; }
        set
        {
            UIRect newBounds = Bounds;
            newBounds.SetDimensions(value);
            Bounds = newBounds;
        }
    }

    public Vector3 Position
    {
        get { return Bounds.Position; }
        set
        {
            UIRect newBounds = Bounds;

            switch (CenterType)
            {
                case UICenterType.CENTER:
                    newBounds.Center = value;
                    break;
                case UICenterType.TOPLEFT:
                    newBounds.TopLeft = value;
                    break;
                case UICenterType.TOPCENTER:
                    newBounds.TopCenter = value;
                    break;
                case UICenterType.TOPRIGHT:
                    newBounds.TopRight = value;
                    break;
                case UICenterType.MIDDLELEFT:
                    newBounds.MiddleLeft = value;
                    break;
                case UICenterType.MIDDLERIGHT:
                    newBounds.MiddleRight = value;
                    break;
                case UICenterType.BOTTOMLEFT:
                    newBounds.BottomLeft = value;
                    break;
                case UICenterType.BOTTOMCENTER:
                    newBounds.BottomCenter = value;
                    break;
                case UICenterType.BOTTOMRIGHT:
                    newBounds.BottomRight = value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            Bounds = newBounds;
        }
    }

    public UIRect Bounds
    {
        get { return _bounds; }
        set
        {
            _bounds = value;
            transform.position = AnchorPoint;
            SetDirty();
        }
    }

    public float Weight { get; set; }
    public event OnDirtyHandler OnDirty = delegate { };

   


    public virtual void UpdateAnchor()
    {
        if (!Parent) return;
        switch (AnchorType)
        {
            case UIAnchorType.NONE:
                return;
            case UIAnchorType.CENTER:
                Position = Parent.Bounds.Center;
                break;
            case UIAnchorType.TOPLEFT:
                Position = Parent.Bounds.TopLeft;
                break;
            case UIAnchorType.TOPCENTER:
                Position = Parent.Bounds.TopCenter;
                break;
            case UIAnchorType.TOPRIGHT:
                Position = Parent.Bounds.TopRight;
                break;
            case UIAnchorType.MIDDLELEFT:
                Position = Parent.Bounds.MiddleLeft;
                break;
            case UIAnchorType.MIDDLERIGHT:
                Position = Parent.Bounds.MiddleRight;
                break;
            case UIAnchorType.BOTTOMLEFT:
                Position = Parent.Bounds.BottomLeft;
                break;
            case UIAnchorType.BOTTOMCENTER:
                Position = Parent.Bounds.BottomCenter;
                break;
            case UIAnchorType.BOTTOMRIGHT:
                Position = Parent.Bounds.BottomRight;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void Awake()
    {
        OnDirty += () =>
        {
            UpdateAnchor();
            Debug.Log("Widget Recalced");
        };
        OnCreate();
    }

    public virtual void OnCreate(){}

    public void SetDirty()
    {
        _isDirty = true;
    }

    public void Update()
    {
        if (_isDirty)
        {
            OnDirty();
            _isDirty = false;
        }
        OnUpdate();
    }

    public virtual void OnUpdate(){}
    public Color DebugColor = Color.blue;
    public float DebugGizmoScale = .5f;
    public void DrawBounds()
    {
        Vector3 topLeft = new Vector3(Bounds.XMin, Bounds.YMax, Position.z);
        Vector3 topRight = new Vector3(Bounds.XMax, Bounds.YMax, Position.z);
        Vector3 bottomLeft = new Vector3(Bounds.XMin, Bounds.YMin, Position.z);
        Vector3 bottomRight = new Vector3(Bounds.XMax, Bounds.YMin, Position.z);

        Gizmos.color = DebugColor;

        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomRight, bottomLeft);
        Gizmos.DrawLine(bottomLeft, topLeft);
    }

    public virtual void OnDrawGizmos()
    {
        DrawBounds();
    }
    /// <summary>
    /// Signals UIWidget to fit itself into a given bounding rect
    /// It is up to each UIWidget to override this funtion if it requires a complex process to fit in the given rect
    /// </summary>
    /// <param name="boundLimits"> The UIRect for the UIWidget to fit in </param>
    public virtual void Squeeze(UIRect boundLimits)
    {
        Bounds = boundLimits;
    }
}