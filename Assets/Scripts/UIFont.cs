using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIFont
{
    private readonly static List<UIFont> _loadedFonts = new List<UIFont>();
    public readonly string Name;

    private UIFont(string fontName, Font unityFont)
    {
        Name = fontName;
        UnityFont = unityFont;
    }

    public Material DefaultMaterial
    {
        get
        {
            return UnityFont.material;
        }
    }

    public Font UnityFont { get; private set; }
    public static UIFont GetFont(string fontName, string path = "")
    {
        UIFont returnFont = _loadedFonts.Where(font => font.Name == fontName).DefaultIfEmpty(null).FirstOrDefault();
        if (returnFont != null)
        {
            return returnFont;
        }

        if (string.IsNullOrEmpty(path))
        {
            throw new ArgumentException(string.Format("Font not found: {0}", fontName), "fontName");
        }
        else return LoadFont(fontName, path);
    }

    public static UIFont LoadFont(string fontName, string path)
    {
        UIFont newFont = new UIFont(fontName, Resources.Load<Font>(path));

        _loadedFonts.Add(newFont);

        return newFont;
    }
}
