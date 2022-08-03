using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_Controller : MonoBehaviour
{
    Rocket_Controller r_controller;
    [SerializeField] private int _rSelect = 0;
    public GameObject _rocket;
    private Camera _cam;

    public List<string> rockets = new List<string>();

    
    void Start()
    {
        rockets.Add("Rocket_1");
        rockets.Add("Rocket_2");
        _rocket = GameObject.Find(rockets[_rSelect]);
        r_controller = _rocket.GetComponent<Rocket_Controller>();
        _cam = GetComponent<Camera>();
        transform.position = new Vector3(_rocket.transform.position.x, _rocket.transform.position.y, transform.position.z);
        transform.SetParent(_rocket.transform);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (_rSelect == 0)
            {
                _rSelect = rockets.Count-1;
            }
            else
            {
                _rSelect -= 1;
            }
            _rocket = GameObject.Find(rockets[_rSelect]);
            r_controller = _rocket.GetComponent<Rocket_Controller>();
            transform.position = new Vector3(_rocket.transform.position.x, _rocket.transform.position.y, transform.position.z);
            transform.SetParent(_rocket.transform);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (_rSelect == rockets.Count-1)
            {
                _rSelect = 0;
            }
            else
            {
                _rSelect += 1;
            }
            _rocket = GameObject.Find(rockets[_rSelect]);
            r_controller = _rocket.GetComponent<Rocket_Controller>();
            transform.position = new Vector3(_rocket.transform.position.x, _rocket.transform.position.y, transform.position.z);
            transform.SetParent(_rocket.transform);
        }
        if (Input.GetKey(KeyCode.A))
        {
            r_controller.TurnLeft();
        }
        if (Input.GetKey(KeyCode.D))
        {
            r_controller.TurnRight();
        }
        if (Input.GetKey(KeyCode.W))
        {
            r_controller.TurnEngine();
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            _cam.orthographicSize *= 1.1f;
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            _cam.orthographicSize *= 0.9f;
        }

        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
