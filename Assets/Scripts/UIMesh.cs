using System.Linq;
using UnityEngine;
using System.Collections;

public class UIMesh : UIWidget
{
    public GameObject _meshGO;
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
            return _meshRenderer.material;
        }
        set
        {
            _meshRenderer.material = value;
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

    public override void OnCreate()
    {
        _meshGO = new GameObject(string.Format("UIMesh{0}", gameObject.name), typeof(MeshRenderer), typeof(MeshFilter));
        _meshFilter = _meshGO.GetComponent<MeshFilter>();
        _meshRenderer = _meshGO.GetComponent<MeshRenderer>();
        OnDirty += () => PlaceMesh(Bounds);
    }
    protected virtual void PlaceMesh(UIRect bounds)
    {

        _meshGO.transform.position = AnchorPoint;
    }
    public void Start()
    {

    }
}
