using UnityEngine;

public class UIScreen : UIPanel
{
    private GameObject _cameraGameObject;
    private Camera _uiCamera = new Camera();
    public int DrawDepth = 100;

    public LayerMask UILayer
    {
        get
        {
            return _uiCamera.cullingMask;
        }
        set
        {
            _uiCamera.cullingMask = value;
        }
    }

    public void InitCamera()
    {
        _cameraGameObject = new GameObject(string.Format("UICamera: {0}", transform.name), typeof(Camera));

        _uiCamera = _cameraGameObject.GetComponent<Camera>();
        float aspectRatio = (float)Screen.width / Screen.height;

        _uiCamera.aspect = aspectRatio;
        _uiCamera.isOrthoGraphic = true;
        _uiCamera.orthographicSize = Screen.height / 2f;
        _uiCamera.clearFlags = CameraClearFlags.Depth;

        _uiCamera.nearClipPlane = 0.3f;
        _uiCamera.farClipPlane = 50.0f;
        _uiCamera.depth = DrawDepth;
        _uiCamera.rect = new Rect(0.0f, 0.0f, 1.0f, 1.0f);
        _uiCamera.orthographic = true;
        _uiCamera.orthographicSize = Screen.height / 2f;

        // Set the camera position based on the screenResolution/orientation
        _uiCamera.transform.position = new Vector3(Screen.width / 2f, Screen.height / 2f, -10.0f);
        _uiCamera.cullingMask = UILayer;
    }

    void InitBounds()
    {
        Vector3 bottomLeft = _uiCamera.ViewportToWorldPoint(new Vector3(0, 0, DrawDepth));
        Vector3 topRight = _uiCamera.ViewportToWorldPoint(new Vector3(1, 1, DrawDepth));

        UIRect cameraBounds = UIRect.RectFromMinMax(topRight.x, bottomLeft.y, bottomLeft.x, topRight.y);
        Bounds = cameraBounds;
    }

   
    public override void OnCreate()
    {
        base.OnCreate();
        AnchorType = UIAnchorType.NONE;
        CenterType = UICenterType.CENTER;
        
        InitCamera();
        InitBounds();
    }
    
}
