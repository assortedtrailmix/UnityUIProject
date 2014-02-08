using UnityEngine;

public class UIMesh : UIWidget
{
    private Mesh b_mesh;
    public Mesh Mesh
    {
        get
        {
            return b_mesh;
        }
        set
        {
            b_mesh = value;
            MeshFilter.mesh = value;
        }
    }

    public Material Material
    {
        get
        {
            return _meshRenderer.sharedMaterial;
        }
        set
        {
            _meshRenderer.sharedMaterial = value;
        }
    }

    private MeshFilter _meshFilter;
    private MeshRenderer _meshRenderer;
    public MeshFilter MeshFilter
    {
        get
        {

            return _meshFilter;
        }
        set
        {
            _meshFilter = value;
        }
    }

    public override void OnAwake()
    {
        _meshFilter = gameObject.AddComponent<MeshFilter>();
        _meshRenderer = gameObject.AddComponent<MeshRenderer>();
        OnDirty += () => PlaceMesh(Bounds);
    }
    protected virtual void PlaceMesh(UIRect bounds)
    {
        gameObject.transform.position = AnchorPoint;
    }
    public void Start()
    {

    }
}
