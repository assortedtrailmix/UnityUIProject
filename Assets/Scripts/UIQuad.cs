using System;
using System.Linq;
using UnityEngine;

public class UIQuad : UIMesh
{


    public UIScaleMode ScaleMode
    {
        get { return _scaleMode; }
        set
        {
            _scaleMode = value;
            SetDirty();
        }
    }

    public float AspectRatio
    {
        get;
        set;
    }
    private UIScaleMode _scaleMode;
    private UIRect _idealBounds;

    public override UIRect Bounds
    {
        get
        {
            return _idealBounds;
        }
        set
        {
            _idealBounds = value;
            UIRect scaledBounds;
            switch (ScaleMode)
            {
                case UIScaleMode.MAINTAIN_ASPECT_RATIO:
                    scaledBounds = value;

                    if (value.Width > value.Height)
                    {
                        scaledBounds.Width = value.Height * AspectRatio;
                        scaledBounds.Height = value.Height;
                    }
                    else if (value.Width < value.Height)
                    {
                        scaledBounds.Width = value.Width;
                        scaledBounds.Height = value.Width * (1 / AspectRatio);
                    }

                    break;
                case UIScaleMode.STRETCH:
                    scaledBounds = value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            if (RelativeSize.HasValue)
            {
                scaledBounds.Width *= RelativeSize.Value.x;
                scaledBounds.Height *= RelativeSize.Value.y;
            }
            Mesh.ResizeQuad(scaledBounds.Dimensions);
            SetDirty();
        }
    }
    public override void OnCreate()
    {
        base.OnCreate();
        Mesh = MeshExtenstions.CreateQuadMesh(Size);
    }
}

public static class MeshExtenstions
{
    public static Mesh ResizeQuad(this Mesh mesh, Vector2 size)
    {
        return mesh.ResizeQuad(size.x, size.y);
    }
    public static Mesh ResizeQuad(this Mesh mesh, float height, float width)
    {
        if ((mesh.vertices.Length != 4) && (mesh.triangles.Length != 6))
            return mesh;


        mesh.vertices = GenerateQuadMeshVerts(height, width);
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        mesh.Optimize();
        return mesh;
    }

    public static Mesh CreateQuadMesh(Vector2 size)
    {
        return CreateQuadMesh(size.x, size.y);
    }

    private static Vector3[] GenerateQuadMeshVerts(float height, float width)
    {
        Vector3 p0 = new Vector3(-(height / 2), -(width / 2), 0);
        Vector3 p1 = new Vector3((height / 2), -(width / 2), 0);
        Vector3 p2 = new Vector3(-(height / 2), (width / 2), 0);
        Vector3 p3 = new Vector3((height / 2), (width / 2), 0);
        return new[] { p0, p1, p2, p3 };
    }
    public static Mesh CreateQuadMesh(float height = 1f, float width = 1f)
    {
        Mesh mesh = new Mesh();
        mesh.Clear();
        mesh.vertices = GenerateQuadMeshVerts(height, width);
        mesh.triangles = new[]
        {
            0, 2, 3,
            0, 3, 1
        };
        Vector2 bottomLeftCoord = new Vector2(0, 0);
        Vector2 topLeftCoord = new Vector2(0, 1);
        Vector2 bottomRightCoord = new Vector2(1, 0);
        Vector2 topRightCoord = new Vector2(1, 1);
        mesh.uv = new[]
        {
            
            
            
            bottomLeftCoord,
            bottomRightCoord,
            topLeftCoord,
            topRightCoord,
        };
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        mesh.Optimize();
        return mesh;
    }
}
