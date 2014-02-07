using UnityEngine;

public class UIText : UIWidget
{
    private TextMesh _textMesh;
    private MeshRenderer _textRenderer;
    private UIFont b_font;
    public UIFont Font
    {
        get
        {
            return b_font;
        }
        set
        {
            b_font = value;
            _textMesh.font = value.UnityFont;
            _textRenderer.material = value.DefaultMaterial;
        }
    }
    public new UIRect Bounds
    {
        get
        {
            return _textMesh ? new UIRect(_textMesh.renderer.bounds.Rect()) : new UIRect();
        }
    }

    public override void OnCreate()
    {
        _textMesh = gameObject.GetComponent<TextMesh>();
        
        if (!_textMesh)
        {
            _textMesh = gameObject.AddComponent<TextMesh>();
        }
        _textRenderer = gameObject.GetComponent<MeshRenderer>();
        if (!_textRenderer)
        {
            _textRenderer = gameObject.AddComponent<MeshRenderer>();
        }
        _textMesh.fontSize = 95;
    }

    public void SetText(string text)
    {
        _textMesh.text = text;
        SetDirty();
    }


    public override void OnDrawGizmos()
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
}
