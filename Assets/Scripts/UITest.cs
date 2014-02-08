using UnityEngine;

public class UITest : MonoBehaviour
{
    private UIScreen testScreen;
    private GameObject testGO;
    private UIVerticalPanel verticalPanel;
    public GameObject testGOA;
    public Material testMat;
    public Vector2 panelSize;
    void Awake()
    {
        testGO = new GameObject("UI Test");

        testScreen = testGO.AddComponent<UIScreen>();

        verticalPanel = testGO.AddComponent<UIVerticalPanel>();

        verticalPanel.Parent = testScreen;
        verticalPanel.AnchorType = UIAnchorType.TOPLEFT;
        verticalPanel.CenterType = UICenterType.TOPLEFT;
        verticalPanel.RelativeSize = new Vector2(50, 50);
        verticalPanel.RelativeMaxVerticalElementMargin = 20f;


        verticalPanel.DebugColor = Color.red;
        verticalPanel.FixedSpacing = true;

        UIQuad testWidgetA = testGO.AddComponent<UIQuad>();
        testWidgetA.Material = testMat;
        testWidgetA.CenterType = UICenterType.CENTER;
        testWidgetA.ScaleMode = UIScaleMode.STRETCH;
        testWidgetA.AspectRatio = 1f;

        UIQuad testWidgetB = testGO.AddComponent<UIQuad>();
        testWidgetB.Material = testMat;
        testWidgetB.CenterType = UICenterType.CENTER;
        testWidgetB.ScaleMode = UIScaleMode.STRETCH;
        testWidgetB.AspectRatio = 1f;
        testWidgetB.Weight = .8f;
        UIQuad testWidgetC = testGO.AddComponent<UIQuad>();
        testWidgetC.Material = testMat;
        testWidgetC.CenterType = UICenterType.CENTER;
        testWidgetC.ScaleMode = UIScaleMode.MAINTAIN_ASPECT_RATIO;
        testWidgetC.AspectRatio = 1f;
        testWidgetC.Weight = .3f;

        UIPlaceholder testWidgetD = testGO.AddComponent<UIPlaceholder>();
        verticalPanel.VerticalMargin = 1f;
        verticalPanel.HorizontalMargin = 1f;
        verticalPanel.AddChild(testWidgetA);
        verticalPanel.AddChild(testWidgetB);
        verticalPanel.AddChild(testWidgetC);
        verticalPanel.AddChild(testWidgetD);
        testWidgetC.RelativeSize = new Vector2(10f, 10f);

        verticalPanel.RelativeSize = new Vector2(20, 20);

        testGOA.transform.position = testScreen.Bounds.TopLeft;
        oldPanelSize = panelSize;
    }

    private Vector2 oldPanelSize;
    void Update()
    {
        if (oldPanelSize == panelSize)
            return;
        verticalPanel.RelativeSize = panelSize;
        oldPanelSize = panelSize;
    }
}
