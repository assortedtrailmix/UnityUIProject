using System.Linq;
using UnityEngine;

public class UIVerticalPanel : UIPanel
{
    public override bool CanSqueeze
    {
        get { return true; }
    }
    private float? _relativeMaxVerticalElementMargin;
    private float _maxVerticalElementMargin;
    private float _verticalMargin;
    private float _horizontalMargin;
    public bool FixedSpacing { get; set; }
    public override void OnCreate()
    {
        base.OnCreate();
        OnDirty += ValidateMargins;
    }

    public float HorizontalMargin
    {
        get { return _horizontalMargin; }
        set
        {
            _horizontalMargin = value;
            ValidateMargins();
        }
    }

    public float VerticalMargin
    {
        get { return _verticalMargin; }
        set
        {
            _verticalMargin = value;
            ValidateMargins();
        }
    }

    private float VerticalElementMargin { get; set; }
    public float MaxVerticalElementMargin
    {
        get
        {

            return _maxVerticalElementMargin;
        }
        set
        {

            _maxVerticalElementMargin = value;
            VerticalElementMargin = _maxVerticalElementMargin;

            ValidateMargins();
        }
    }

    private void ValidateMargins()
    {
        _horizontalMargin = Mathf.Clamp(_horizontalMargin, 0f, Bounds.Width);
        _verticalMargin = Mathf.Clamp(_verticalMargin, 0f, Bounds.Height);
        if (UseableHeight <= 0f)
            VerticalElementMargin = (Bounds.Height - (VerticalMargin * 2f)) / (Children.Count - 1);
        SetDirty();
    }
    public float? RelativeMaxVerticalElementMargin
    {
        get { return _relativeMaxVerticalElementMargin; }
        set
        {
            _relativeMaxVerticalElementMargin = value;
            UpdateRelativeSizes();
        }
    }

    protected override void UpdateRelativeSizes()
    {
        base.UpdateRelativeSizes();
        if (RelativeMaxVerticalElementMargin.HasValue)
            MaxVerticalElementMargin = (RelativeMaxVerticalElementMargin.Value / 100f) * Bounds.Height;
    }
    public override void OnAddChild(UIWidget child)
    {
        child.AnchorType = UIAnchorType.NONE;
    }

    //Total Vertical Space taken up by margins
    private float TotalVerticalMarginSpace
    {
        get
        {
            float totalVerticalMarginSpace = VerticalElementMargin * (Children.Count - 1);
            totalVerticalMarginSpace += VerticalMargin * 2f;
            return totalVerticalMarginSpace;
        }
    }
    //Height left after margins
    private float UseableHeight
    {
        get
        {
            float usableHeight = Bounds.Height - TotalVerticalMarginSpace;
            usableHeight = Mathf.Clamp(usableHeight, 0f, Bounds.Height);
            return usableHeight;
        }
    }
    protected override void SpaceChildren()
    {
        if (FixedSpacing)
        {
            //Height of each element
            float elementBoundsHeight = (UseableHeight / (Children.Count));

            for (int i = 0; i < Children.Count; i++)
            {
                UIWidget child = Children[i];
                UIRect positionBounds = new UIRect();

                positionBounds.YMax = (Bounds.YMax - VerticalMargin);
                positionBounds.YMax -= ((elementBoundsHeight + VerticalElementMargin) * i);

                positionBounds.YMin = positionBounds.YMax - elementBoundsHeight;

                positionBounds.XMax = Bounds.XMax - HorizontalMargin;
                positionBounds.XMin = Bounds.XMin + HorizontalMargin;

                child.Squeeze(positionBounds);
            }
        }
        else
        {
            float totalWeight = Children.Sum(t => t.Weight);
            float usedHeight = 0f;
            foreach (UIWidget child in Children)
            {
                UIRect positionBounds = new UIRect();

                float elementHeight = UseableHeight * (child.Weight / totalWeight);

                positionBounds.YMax = (Bounds.YMax - VerticalMargin);
                positionBounds.YMax -= usedHeight;
                usedHeight += (elementHeight + VerticalElementMargin);
                positionBounds.YMin = positionBounds.YMax - elementHeight;

                positionBounds.XMax = Bounds.XMax - HorizontalMargin;
                positionBounds.XMin = Bounds.XMin + HorizontalMargin;

                child.Squeeze(positionBounds);
            }
        }
    }

}
