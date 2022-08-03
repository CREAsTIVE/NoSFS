using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketController : MonoBehaviour
{
    Rigidbody2D rb;
    PlanetGenerator planetGenerator;
    [SerializeField] private GameObject CurrentPlanet;
    
    [SerializeField] private float _rotateSpeed = 1f;
    [SerializeField] private float _engineSpeed = 1f;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        planetGenerator = CurrentPlanet.GetComponent<PlanetGenerator>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        CurrentPlanet = GameObject.Find(other.name);
        planetGenerator = CurrentPlanet.GetComponent<PlanetGenerator>();
    }

    private void Update()
    {
        AddForce();
    }

    void AddForce()
    {
        Vector2 R = transform.position - CurrentPlanet.transform.position;
        Vector2 force = R.normalized * (MainController.GRAVITY* ((rb.mass * planetGenerator.mass) / (R.sqrMagnitude)));
        rb.AddForce(-force, ForceMode2D.Force);
    }

    public void Control(float speed, float rotation)
    {
        rb.angularVelocity -= _rotateSpeed * rotation;
        rb.AddForce(transform.up * _engineSpeed * speed);
    }
}
