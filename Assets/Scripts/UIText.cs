using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class UIText : UIWidget
{
    private TextMesh _textMesh;
    private MeshRenderer _textRenderer;
    private UIFont b_font;
    private const int FitShrinkRate = 2;

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

    private string _idealText;
    private UIRect _idealBounds;
    public string Text
    {
        get
        {
            return _idealText;
        }
        set
        {
            _idealText = value;
            SetDirty();
        }
    }

    public new UIRect Bounds
    {
        get
        {
            return _idealBounds;
        }
        set
        {

            _idealBounds = value;
            string moddedText = _idealText;
            var fitsBounds = new Func<bool>(() =>
            {
                _textMesh.text = moddedText;
                return _textRenderer.bounds.size.x < _idealBounds.Width;
            });
            var availibleLineBreaks = new Func<int>(() => _idealText.Count(char.IsWhiteSpace));
            transform.position = value.Position;

            while (!fitsBounds.Invoke())
            {

                while (!fitsBounds.Invoke() && (availibleLineBreaks.Invoke() > 0))
                {
                    int indexOfLastWhitespace = Array.FindLastIndex(moddedText.ToCharArray(),
                        char.IsWhiteSpace);
                    moddedText = moddedText.Insert(indexOfLastWhitespace, Environment.NewLine);
                }
                if (fitsBounds.Invoke())
                    break;
                moddedText = _idealText;
                _textMesh.characterSize -= FitShrinkRate;
            }
            _textMesh.text = moddedText;
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
        _textMesh.anchor = TextAnchor.UpperLeft;
        OnDirty += () => { Bounds = _idealBounds; };
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
