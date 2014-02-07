using System.Linq;
using UnityEngine;

public class UIVerticalPanel : UIPanel
{
    public bool FixedSpacing { get; set; }
    public float HorizontalMargin { get; set; }
    public float VerticalMargin { get; set; }

    public float MinVerticalElementMargin { get; set; }

    public override void OnAddChild(UIWidget child)
    {
        child.AnchorType = UIAnchorType.NONE;
    }

    protected override void SpaceChildren()
    {
        //Total Vertical Space taken up by margins
        float totalVerticalMarginSpace = MinVerticalElementMargin * (Children.Count - 1);
        totalVerticalMarginSpace += VerticalMargin * 2f;

        //Height left after margins
        float usableHeight = Bounds.Height - totalVerticalMarginSpace;

        if (FixedSpacing)
        {
            //Height of each element
            float elementBoundsHeight = (usableHeight/(Children.Count));

            for (int i = 0; i < Children.Count; i++)
            {
                UIWidget child = Children[i];
                UIRect positionBounds = new UIRect();
                
                positionBounds.YMax = (Bounds.YMax - VerticalMargin);
                positionBounds.YMax -= ((elementBoundsHeight + MinVerticalElementMargin) * i);
                
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

                float elementHeight = usableHeight*(child.Weight/totalWeight);
                
                positionBounds.YMax = (Bounds.YMax - VerticalMargin);
                positionBounds.YMax -= usedHeight;
                usedHeight += (elementHeight + MinVerticalElementMargin);
                positionBounds.YMin = positionBounds.YMax - elementHeight;

                positionBounds.XMax = Bounds.XMax - HorizontalMargin;
                positionBounds.XMin = Bounds.XMin + HorizontalMargin;
                
                child.Squeeze(positionBounds);
            }
        }
    }
    
}
