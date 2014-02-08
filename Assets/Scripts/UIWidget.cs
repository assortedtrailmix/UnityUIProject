
#region Import Statements

using System;
using UnityEngine;

#pragma warning disable 0162

#endregion

public abstract class UIWidget : MonoBehaviour
{
    #region Events
    /// <summary>
    /// This event is called in the Update method if the isDirty flag has been set.
    /// Use SetDirty to signal this event needs to be called
    /// Suscribe to this event to allow code to run when the UIWidget is modified
    /// </summary>
    //TODO: Future optimisation: Remove empty delegate and use null check on event invocation
    public event OnDirtyHandler OnDirty = delegate { };
    public delegate void OnDirtyHandler();
    #endregion

    #region Private Fields
    
    /// <summary>
    /// This is a flag that denotes a UIWidget needs to be updated
    /// Every update it's value is checked. If it is true, the OnDirty event is invoked, and
    /// it's value is set to false
    /// It can be set to true with the SetDirty method.
    /// </summary>
    private bool _isDirty;
    #region Backing Fields

    private UIWidget b_parent;
    private Vector2? b_relativeSize;
    private UIAnchorType b_anchorType;
    private UIRect b_bounds;
    private UICenterType b_centerType;

    #endregion
    #endregion

    #region Public Properties
    /// <summary>
    /// Checks if this UIWidget implements the ISqueezer interface
    /// Classes implementing use the Squeeze method to modify their children's 
    /// position and dimensions.
    /// This property allows special handling of resizing in those cases.
    /// </summary>
    public bool CanSqueeze
    {
        get {
            return this is ISqueezer;
        }
    }
    /// <summary>
    /// This property denotes the parent UIWidget to this UIWidget.
    /// Setting it suscribes this UIWidget to the parent's OnDirty event
    /// and calls the parent UIWidget's SetDirty method, casuing it to be flagged as changed
    /// </summary>
    public UIWidget Parent
    {
        get { return b_parent; }
        set
        {
            b_parent = value;
            b_parent.OnDirty += OnDirty;
            b_parent.SetDirty();
            SetDirty();
        }
    }
    /// <summary>
    /// This property is used by other methods and properties to denote the part of a parent
    /// this object should be anchored to
    /// </summary>
    public UIAnchorType AnchorType
    {
        get { return b_anchorType; }
        set
        {
            b_anchorType = value;
            SetDirty();
        }
    }
    /// <summary>
    /// This property is used by other methods and properties to denote the location of the center of the
    /// UIWidget relative to it's bounds
    /// </summary>
    public UICenterType CenterType
    {
        get { return b_centerType; }
        set
        {
            b_centerType = value;
            SetDirty();
        }
    }
    /// <summary>
    /// Accessing this property provides point at which other objects should position themselves to be anchored to this UIWidget's
    /// center according to the CenterType property
    /// </summary>
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
    /// <summary>
    /// Set and retrieve the absolute size of the UIWidget via the Bounds property
    /// </summary>
    public Vector2 Size
    {
        get { return Bounds.Dimensions; }
        set
        {
            UIRect newBounds = Bounds;
            newBounds.Dimensions = value;
            Bounds = newBounds;
        }
    }
    /// <summary>
    /// Set and retrieve the size of the UIWidget relative to it's parent
    /// Derived classes may also assign side-effects to setting the value of 
    /// this property by overriding the UpdateRelativeSizes method
    /// It is a percentage value (0-100), but is not clamped to that range
    /// </summary>
    public Vector2? RelativeSize
    {
        get { return b_relativeSize; }
        set
        {
            b_relativeSize = value / 100f;
            UpdateRelativeSizes();
        }
    }
    /// <summary>
    /// Set and retrieve the position of the UIWidget via the Bounds property.
    /// When this property is set UIWidget's bounds will be shifted so that part 
    /// of the UIWdget denoted by CenterType's value will reside at the point assigned
    /// </summary>
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
    /// <summary>
    /// Defines the UIRect this UIWidget's postion and size are based on
    /// Can be overriden to allow custom handling of a UIWidget's postion and size
    /// The default accessor also sets the gameObject's position to the AnchorPoint 
    /// and calls the SetDirty method to signal the UIWidget needs to be updated.
    /// If this property is overridden, the derived class should also call SetDirty
    /// </summary>
    public virtual UIRect Bounds
    {
        get { return b_bounds; }
        set
        {
            b_bounds = value;
            transform.position = AnchorPoint;
            SetDirty();
        }
    }
    
    /// <summary>
    /// Used by flexible layouts
    /// Represents the proportion of space an element should use in a flexible layout
    /// There is no fixed unit for this property. This value
    /// is divided by the total of a layout's children's weights to calculate what percentahe of
    /// the layout's availible space should be assigned to it
    /// </summary>
    public float Weight { get; set; }
    #endregion

    #region Public Fields
    public Color DebugColor = Color.yellow;
    public float DebugGizmoScale = .5f;
    #endregion

    #region Public Functions

    /// <summary>
    /// Signals a control has changed and this UIWidget needs to be updated.
    /// Actual update does not occur until next Update call
    /// </summary>
    public void SetDirty()
    {
        _isDirty = true;
    }

    #region MonoBehavior Callbacks
    /// <summary>
    /// Initalises OnDirty event to call UpdateAnchor and UpdateRelativeSizes events 
    /// then calls OnAwake
    /// Instead of using new with this method, classes should override the OnAwake method
    /// </summary>
    public void Awake()
    {
        OnDirty += () =>
        {
            UpdateAnchor();
            UpdateRelativeSizes();
            Debug.Log("Widget Recalced");
        };
        OnAwake();
    }

    /// <summary>
    /// Calls OnUpdate then processes _isDirty flag (Calls OnDirty event if it is true, then sets it to false
    /// </summary>
    public void Update()
    {
        OnUpdate();
        if (_isDirty)
        {
            OnDirty(); //OnDirty must be called before clearing the flag, because it may be set by the OnDirty event's invocation
            _isDirty = false;
        }
    }
    #endregion
    
    #region Virtual Functions
    /// <summary>
    /// Called in the Awake method.
    /// Derived classes should override this method, instead of hiding the Awake method
    /// </summary>
    public virtual void OnAwake() { }

    /// <summary>
    /// Called on each update. 
    /// Derived classes should override this method instead of hiding the Update method
    /// </summary>
    //TODO: Convert to event
    public virtual void OnUpdate(){}


    /// <summary>
    /// Calls DrawBounds to draw a visual representation of the Bounds property in the editor  
    /// Derived classes can override this to allow for custom visualisation of the UIWidget's position and orientation
    /// </summary>
    public virtual void OnDrawGizmos()
    {
        DrawBounds();
    }

    /// <summary>
    ///     Signals UIWidget to fit itself into a given bounding rect
    ///     Usually bounds should be overriden instead of this method
    /// </summary>
    /// <param name="boundLimits"> The UIRect for the UIWidget to fit in </param>
    public virtual void Squeeze(UIRect boundLimits)
    {
        if(!Parent.CanSqueeze)
            Debug.LogError(string.Format("Class not implementing ISqueezer interface calling Squeeze method: {0}", GetType().Name));
        Bounds = boundLimits;
    }
    #endregion
    #endregion

    #region Protected Functions
    /// <summary>
    /// Debug function, draws box based on Bounds property with the DebugColor property's color
    /// </summary>
    protected void DrawBounds()
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

    /// <summary>
    /// Called by OnDirty event
    /// Updates Postion based on AnchorType property and Parent's psotion
    /// If Parent has not been set the method will not modify the UIWidget's bounds
    /// </summary>
    protected virtual void UpdateAnchor()
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
    

    /// <summary>
    /// Updates the Size property based on the RelativeSize property
    /// Called by the OnDirty event
    /// Most classes overriding this method should call the base constructor
    /// </summary>
    protected virtual void UpdateRelativeSizes()
    {
        if (RelativeSize.HasValue && Parent && !Parent.CanSqueeze)
        {

            Size = new Vector2
            {
                x = RelativeSize.Value.x*Parent.Size.x,
                y = RelativeSize.Value.y*Parent.Size.y
            };
        }
    }
    #endregion

    public static T Create<T>(string name, UIWidget parent = null) where T : UIWidget
    {
        GameObject widgetGO = new GameObject(name);
        T widget = widgetGO.AddComponent<T>();
        if (parent)
        {
            UIPanel panel = parent as UIPanel;
            if (panel != null)
            {
                panel.AddChild(widget);
            }
            else
            {
                widget.Parent = parent;
            }
        }
        return widget;
    }
}