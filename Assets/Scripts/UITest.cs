using System.Collections.Generic;
using UnityEngine;

public class UITest : MonoBehaviour
{
    private UIScreen _testScreen;
    private UIVerticalPanel _testVerticalPanel;
    private List<UIQuad> _testQuads = new List<UIQuad>();
    public Material TestMat;

    public Vector2 VerticalPanelSize;
    void Awake()
    {
        _testScreen = UIWidget.Create<UIScreen>("Test Screen");

        _testVerticalPanel = UIWidget.Create<UIVerticalPanel>("Test Panel", _testScreen);

        _testVerticalPanel.AnchorType = UIAnchorType.TOPRIGHT; //Anchor to top left side of parent (_testScreen)
        _testVerticalPanel.CenterType = UICenterType.TOPRIGHT; //Set position based on top left corner

        _testVerticalPanel.RelativeSize = VerticalPanelSize; 

        _testVerticalPanel.FixedSpacing = true; //Each element takes up the same amount of space
        _testVerticalPanel.RelativeMaxVerticalElementMargin = 10f; //10% of the panel is used as margins for the elements
        AddTestQuad(3);
    }

    void AddTestQuad(int num = 1)
    {
        for (int i = 0; i < num; i++)
        {
            UIQuad newQuad = UIWidget.Create<UIQuad>(string.Format("Test Quad {0}", _testQuads.Count),
                _testVerticalPanel);

            newQuad.Material = TestMat;
            _testQuads.Add(newQuad);
        }
    }
    void OnGUI()
    {
        Vector2 oldPanelSize = VerticalPanelSize;
        GUILayout.Label("Panel Width");
        VerticalPanelSize.x = GUILayout.HorizontalSlider(VerticalPanelSize.x, 0f, 100f, GUILayout.MinWidth(80));
        GUILayout.Label("Panel Height");
        VerticalPanelSize.y = GUILayout.HorizontalSlider(VerticalPanelSize.y, 0f, 100f, GUILayout.MinWidth(80));
        if (GUILayout.Button("Add Test Quad"))
        {
            AddTestQuad();
        }
        if (oldPanelSize != VerticalPanelSize)
        {
            _testVerticalPanel.RelativeSize = VerticalPanelSize;
        }
    }
}
