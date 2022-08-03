using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetGenerator : MonoBehaviour
{
    [SerializeField] private int _meshCount = 360;
    [SerializeField] private int _colliderCount = 100;
    [SerializeField] private float _radius = 5;
    [SerializeField] private float _aAngle = 0f;
    [SerializeField] private float _bAngle = 6.283185f;
    [SerializeField] private float _pSize = 1f;
    Main_Controller main_Controller;
    Rocket_Controller r_controller;
    [SerializeField] private GameObject _rocket;
    private EdgeCollider2D ec2d;

    private void Start()
    {
        ec2d = GetComponent<EdgeCollider2D>();
        main_Controller = Camera.main.GetComponent<Main_Controller>();
    }


    void Update()
    {
        _rocket = main_Controller._rocket;
        r_controller = _rocket.GetComponent<Rocket_Controller>();


        Vector2 bl = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane)) - transform.position;
        Vector2 br = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, Camera.main.nearClipPlane)) - transform.position;
        Vector2 tl = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, Camera.main.nearClipPlane)) - transform.position;
        Vector2 tr = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.nearClipPlane)) - transform.position;
        float bl_angle = Mathf.Atan2(bl.y, bl.x);
        float br_angle = Mathf.Atan2(br.y, br.x);
        float tl_angle = Mathf.Atan2(tl.y, tl.x);
        float tr_angle = Mathf.Atan2(tr.y, tr.x);
        _aAngle = Mathf.Min(bl_angle, br_angle, tl_angle, tr_angle);
        _bAngle = Mathf.Max(bl_angle, br_angle, tl_angle, tr_angle);

        if (_bAngle < _aAngle)
        {
            var temp = _aAngle;
            _aAngle = _bAngle;
            _bAngle = temp;
        }

        if (_bAngle - _aAngle > Mathf.PI)
        {
            _aAngle = tr_angle;
            _bAngle = br_angle + Mathf.PI * 2;
        }

        if (transform.position.x > bl.x & transform.position.x < br.x & transform.position.y > bl.y & transform.position.y < tr.y)
        {
            _aAngle = 0;
            _bAngle = 6.283185f;
        }

        MeshGenerate(_aAngle, _bAngle);
    }

    public void MeshGenerate(float a, float b)
    {
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[_meshCount + 1];
        Vector2[] uv = new Vector2[_meshCount + 1];
        int[] triangles = new int[_meshCount * 3];

        for (int n = 0; n < _meshCount; n++)
        {
            float angle_p = (n / (_meshCount - 1f) * (b - a)) + (a);
            vertices[n] = new Vector3(Mathf.Cos(angle_p) * _radius, Mathf.Sin(angle_p) * _radius);
            uv[n] = vertices[n] / _radius / 2f + new Vector3(0.5f, 0.5f);
        }
        vertices[_meshCount] = new Vector3(0, 0);
        uv[_meshCount] = new Vector3(0.5f, 0.5f);

        for (int i = 0; i < _meshCount; i++)
        {
            if (i < _meshCount - 1)
            {
                triangles[i * 3] = _meshCount;
                triangles[i * 3 + 1] = i;
                triangles[i * 3 + 2] = i + 1;
            }
            else
            {
                triangles[i * 3] = _meshCount;
                triangles[i * 3 + 1] = i;
                triangles[i * 3 + 2] = 0;
            }
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

        GetComponent<MeshFilter>().mesh = mesh;
        CollideGenerate(a, b);
    }

    public void CollideGenerate(float a, float b)
    {
        float size = Mathf.Atan(_pSize / _radius) * 2f;
        float ang = Mathf.Atan2(_rocket.transform.position.y - transform.position.y, _rocket.transform.position.x - transform.position.x);
        
        Vector2[] c_points = new Vector2[_colliderCount];
        for (int n = 0; n < _colliderCount; n++)
        {
            float cAngle_p = n / (_colliderCount - 1f) * size + ang - (size/2);
            c_points[n] = new Vector2(Mathf.Cos(cAngle_p) * _radius, Mathf.Sin(cAngle_p) * _radius);
        }
        ec2d.points = c_points;
    }

    [ContextMenu("Generate")]
    public void MeshGenerateEditor()
    {
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[_meshCount + 1];
        Vector2[] uv = new Vector2[_meshCount + 1];
        int[] triangles = new int[_meshCount * 3];

        for (int n = 0; n < _meshCount; n++)
        {
            float angle_p = (n / (_meshCount - 1f)) * 6.283185f;
            vertices[n] = new Vector3(Mathf.Cos(angle_p) * _radius, Mathf.Sin(angle_p) * _radius);
            uv[n] = vertices[n] / _radius / 2f + new Vector3(0.5f, 0.5f);
        }
        vertices[_meshCount] = new Vector3(0, 0);
        uv[_meshCount] = new Vector3(0.5f, 0.5f);

        for (int i = 0; i < _meshCount; i++)
        {
            if (i < _meshCount - 1)
            {
                triangles[i * 3] = _meshCount;
                triangles[i * 3 + 1] = i;
                triangles[i * 3 + 2] = i + 1;
            }
            else
            {
                triangles[i * 3] = _meshCount;
                triangles[i * 3 + 1] = i;
                triangles[i * 3 + 2] = 0;
            }
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

        GetComponent<MeshFilter>().mesh = mesh;
    }
}
