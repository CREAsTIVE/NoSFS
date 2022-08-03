using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket_Controller : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] private float _rotateSpeed = 1f;
    [SerializeField] private float _engineSpeed = 1f;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        
    }
    public void Control(float speed, float rotation)
    {
        rb.angularVelocity -= _rotateSpeed * rotation;
        rb.AddForce(transform.up * _engineSpeed * speed);
    }
}
