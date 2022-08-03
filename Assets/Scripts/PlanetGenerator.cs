using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetGenerator : MonoBehaviour
{
    [SerializeField] private int _meshVertsCount = 360;
    [SerializeField] private int _colliderVertsCount = 100;
    [SerializeField] private float _radius = 5;
    [SerializeField] private float _physicSize = 1f; // in units

    private MeshFilter mesh;
    private EdgeCollider2D ec2d;
    public float getRadius() => _radius;
    [ContextMenu("upd")]
    private void Start()
    {
        ec2d = GetComponent<EdgeCollider2D>();
        mesh = GetComponent<MeshFilter>();
    }

    void Update()
    {
        //перенести в Start
        {
            _physicSize = Mathf.Atan(0.5f * MainController.CALCULATE_PHYSIC_SIZE / _radius)*2f;
        }
        //-

        Vector2 bl = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane)) - transform.position;
        Vector2 br = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, Camera.main.nearClipPlane)) - transform.position;
        Vector2 tl = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, Camera.main.nearClipPlane)) - transform.position;
        Vector2 tr = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.nearClipPlane)) - transform.position;
        float bl_angle = Mathf.Atan2(bl.y, bl.x);
        float br_angle = Mathf.Atan2(br.y, br.x);
        float tl_angle = Mathf.Atan2(tl.y, tl.x);
        float tr_angle = Mathf.Atan2(tr.y, tr.x);
        float _minAngle = Mathf.Min(bl_angle, br_angle, tl_angle, tr_angle);
        float _maxAngle = Mathf.Max(bl_angle, br_angle, tl_angle, tr_angle);

        if (_maxAngle < _minAngle)
            (_minAngle, _maxAngle) = (_maxAngle, _minAngle);

        if (_maxAngle - _minAngle > Mathf.PI)
        {
            _minAngle = tr_angle;
            _maxAngle = br_angle + Mathf.PI * 2;
        }

        if (transform.position.x > bl.x & transform.position.x < br.x & transform.position.y > bl.y & transform.position.y < tr.y)
            GenerateMesh();
        else 
            GenerateMesh(_minAngle, _maxAngle);

        GenerateCollider();
    }

    [ContextMenu("Generate")]
    public void GenerateMesh() => GenerateMesh(0, Mathf.PI * 2);
    public void GenerateMesh(float _minAngle, float _maxAngle)
    {
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[_meshVertsCount + 1];
        Vector2[] uv = new Vector2[_meshVertsCount + 1];
        int[] triangles = new int[_meshVertsCount * 3];

        for (int n = 0; n < _meshVertsCount; n++)
        {
            float angle_p = (n / (_meshVertsCount - 1f) * (_maxAngle - _minAngle)) + (_minAngle);
            vertices[n] = new Vector3(Mathf.Cos(angle_p) * _radius, Mathf.Sin(angle_p) * _radius);
            uv[n] = vertices[n] / _radius / 2f + new Vector3(0.5f, 0.5f);
        }
        vertices[_meshVertsCount] = new Vector3(0, 0);
        uv[_meshVertsCount] = new Vector3(0.5f, 0.5f);

        for (int i = 0; i < _meshVertsCount; i++)
        {
            if (i < _meshVertsCount - 1)
            {
                triangles[i * 3] = _meshVertsCount;
                triangles[i * 3 + 1] = i;
                triangles[i * 3 + 2] = i + 1;
            }
            else
            {
                triangles[i * 3] = _meshVertsCount;
                triangles[i * 3 + 1] = i;
                triangles[i * 3 + 2] = 0;
            }
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

        this.mesh.mesh = mesh;
    }

    public void GenerateCollider()
    {
        float ang = Mathf.Atan2(MainController.main.transform.position.y - transform.position.y, MainController.main.transform.position.x - transform.position.x) - (_physicSize / 2);
        
        Vector2[] collider_points = new Vector2[_colliderVertsCount];
        for (int n = 0; n < _colliderVertsCount; n++)
        {
            float cAngle_p = n / (_colliderVertsCount - 1f) * _physicSize + ang;
            collider_points[n] = new Vector2(Mathf.Cos(cAngle_p) * _radius, Mathf.Sin(cAngle_p) * _radius);
        }
        ec2d.points = collider_points;
    }
}
