using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlanetSystem;

public class RocketController : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] private PlanetGenerator GravityCapturedPlanet;
    [SerializeField] List<PlanetGenerator> GravityCapturedPlanets = new();
    
    
    [SerializeField] private float _rotateSpeed = 1f;
    [SerializeField] private float _engineSpeed = 1f;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out PlanetGenerator planet))
        {
            GravityCapturedPlanets.Add(planet);
        }
        transform.SetParent(planet.transform);
        GetSmallestDepthIndex();
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out PlanetGenerator planet))
        {
            GravityCapturedPlanets.Remove(planet);
        }
        GetSmallestDepthIndex();
    }
    void GetSmallestDepthIndex()
    {
        int maxDepth = -1;
        foreach(PlanetGenerator planet in GravityCapturedPlanets)
        {
            if (planet.DepthIndex > maxDepth)
            {
                GravityCapturedPlanet = planet;
                maxDepth = planet.DepthIndex;
            }
        }
        if (maxDepth<0)
            GravityCapturedPlanet = null;
    }

    private void Update()
    {
        AddForce();
    }

    void AddForce()
    {
        if (GravityCapturedPlanet == null)
            return;
        Vector2 R = transform.position - GravityCapturedPlanet.transform.position;
        Vector2 force = R.normalized * (MainController.GRAVITY * ((rb.mass * GravityCapturedPlanet.mass) / (R.sqrMagnitude)));//TODO: использовать одностороннюю и более дешёвую формулу для гравитации (формула гравитации к планетам)
        rb.AddForce(-force, ForceMode2D.Force);
    }

    public void Control(float speed, float rotation)
    {
        rb.angularVelocity -= _rotateSpeed * rotation;
        rb.AddForce(_engineSpeed * speed * transform.up);
    }
}
