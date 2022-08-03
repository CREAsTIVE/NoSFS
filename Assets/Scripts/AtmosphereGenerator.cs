using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MeshFilter))]
public class AtmosphereGenerator : MonoBehaviour
{
    public float AtmosphereSize = 0f;

    [ContextMenu("gen")]
    void Start()
    {
        var radius = gameObject.transform.parent.GetComponent<PlanetGenerator>().getRadius();
        var atmosRadius = AtmosphereSize + radius;
        var meshFilter = GetComponent<MeshFilter>();
        var meshRenderer = GetComponent<MeshRenderer>();
        meshFilter.mesh = new Mesh();
        meshFilter.mesh.vertices = new Vector3[]
                {
                    new Vector3(-atmosRadius, -atmosRadius, 1), new Vector3(-atmosRadius, atmosRadius, 1),
                    new Vector3(atmosRadius, -atmosRadius, 1), new Vector3(atmosRadius, atmosRadius, 1)
                };
        meshFilter.mesh.triangles = new int[] { 0, 1, 2, 1, 3, 2 };
        meshFilter.mesh.SetUVs(0,new Vector2[] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 0), new Vector2(1, 1) });
        float startFrom = radius/atmosRadius;
        meshRenderer.material.SetFloat("_StartFrom", startFrom);
    }
    //float getAtmosRadiusStart(float value) => 1f / Mathf.Abs(1f - value);
}
