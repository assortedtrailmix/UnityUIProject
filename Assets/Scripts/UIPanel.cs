using System.Collections.Generic;

public abstract class UIPanel : UIWidget
{
    protected readonly List<UIWidget> Children = new List<UIWidget>();



    public override void OnCreate()
    {
        OnDirty += SpaceChildren;
    }
    public void AddChild(UIWidget child)
    {
        child.Parent = this;
        Children.Add(child);
        OnAddChild(child);
        SetDirty();
    }

    public virtual void OnAddChild(UIWidget child) { }
    public void RemoveChild(UIWidget child, bool nullParent = true)
    {
        if (nullParent)
            child.Parent = null;
        Children.Remove(child);
        SetDirty();
    }

    protected virtual void SpaceChildren() { }

}
