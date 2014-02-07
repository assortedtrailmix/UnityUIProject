using UnityEngine;

public class UITest : MonoBehaviour
{
    private UIScreen testScreen;
    private GameObject testGO;
    private UIText text;
    private UIVerticalPanel verticalPanel;
    void Awake()
    {
        testGO = new GameObject("UI Test");

        testScreen = testGO.AddComponent<UIScreen>();
        
        verticalPanel = testGO.AddComponent<UIVerticalPanel>();

        verticalPanel.Parent = testScreen;
        verticalPanel.AnchorType = UIAnchorType.TOPLEFT;
        verticalPanel.CenterType = UICenterType.CENTER;
        verticalPanel.Size = new Vector2(30,30);
        verticalPanel.DebugColor = Color.red;
        verticalPanel.FixedSpacing = true;
        UIPlaceholder testWidgetA = testGO.AddComponent<UIPlaceholder>();
        testWidgetA.Weight = .3f;
        UIPlaceholder testWidgetB = testGO.AddComponent<UIPlaceholder>();
        testWidgetB.Weight = .8f;
        UIPlaceholder testWidgetC = testGO.AddComponent<UIPlaceholder>();
        testWidgetC.Weight = .3f;

        text = testGO.AddComponent<UIText>();
        text.Parent = testScreen;
        text.AnchorType = UIAnchorType.TOPLEFT;
        text.CenterType = UICenterType.CENTER;
        text.SetText("Hello World");
        text.Font = UIFont.LoadFont("default", "AGENCYR");
        verticalPanel.VerticalMargin = 1f;
        verticalPanel.HorizontalMargin = 1f;
        verticalPanel.MinVerticalElementMargin = 3f;
        verticalPanel.AddChild(testWidgetA);
        verticalPanel.AddChild(testWidgetB);
        verticalPanel.AddChild(testWidgetC);
    }

}
