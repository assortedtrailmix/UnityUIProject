using System.Collections.Generic;
/// <summary>
/// Class from which all panels derive from.
/// </summary>
public abstract class UIPanel : UIWidget
{
    /// <summary>
    /// List containing all children added to this UIPanel with AddChild.
    /// UIWidgets in this list should have this UIPanel as their parents, but having this UIPanel as a parent is not a 
    /// guarantee the UIWidget will be in the list
    /// </summary>
    protected readonly List<UIWidget> Children = new List<UIWidget>();
    
    /// <summary>
    /// Suscribes the UIPanel to the OnDirty event with the SpaceChildren.
    /// Derived classes overriding this method should call the base method
    /// </summary>
    public override void OnAwake()
    {
        OnDirty += SpaceChildren;
    }
    /// <summary>
    /// Allows the adding of UIWidgets to a panel.
    /// The Parent property of the child will be set to this UIPanel, OnAddChild will be called, and finally SetDirty will be called
    /// </summary>
    /// <param name="child">The UIWidget who's parent will be set to this panel</param>
    public void AddChild(UIWidget child)
    {
        child.Parent = this;
        Children.Add(child);
        OnAddChild(child);
        SetDirty();
    }
    /// <summary>
    /// Called when a new child is added to the panel.
    /// Derived classes should override this method
    /// </summary>
    /// <param name="child"></param>
    public virtual void OnAddChild(UIWidget child) { }
    /// <summary>
    /// Allows the the removing of UIWidgets to a panel.
    /// Optionally, the child's Parent will be set to null, and the child will be removed from the Children list, before SetDirty will be called.
    /// </summary>
    /// <param name="child">The UIWidget to remove this panel</param>
    /// <param name="nullParent">If this parameter is true, the Parent property of the child will be set to null</param>
    public void RemoveChild(UIWidget child, bool nullParent = true)
    {
        if (nullParent)
            child.Parent = null;
        Children.Remove(child);
        SetDirty();
    }
    /// <summary>
    /// Called any time a panel's children should be arranged.
    /// Derived classes should override this to allow for special arrangements of children
    /// </summary>
    protected virtual void SpaceChildren() { }

}
