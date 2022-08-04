using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour
{
    public RocketController CurrentRocket;
    [SerializeField] List<RocketController> _rockets = new();

    private Camera _cam;

    public static MainController main;

    //Constants:
    public const float CALCULATE_PHYSIC_SIZE = 10f;
    public const float GRAVITY = 9.8f;


    public float GlobalTime = 0f;

    void Start()
    {
        CurrentRocket = _rockets[0];
        _cam = Camera.main;
        _cam.transform.position = new Vector3(CurrentRocket.transform.position.x, CurrentRocket.transform.position.y, transform.position.z);
        transform.SetParent(CurrentRocket.transform);
    }

    private void Awake()
    {
        main = this;
        CurrentRocket = _rockets[0];
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            CurrentRocket = _rockets[0];
            _cam.transform.position = new Vector3(CurrentRocket.transform.position.x, CurrentRocket.transform.position.y, transform.position.z);
            transform.SetParent(CurrentRocket.transform);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            CurrentRocket = _rockets[1];
            _cam.transform.position = new Vector3(CurrentRocket.transform.position.x, CurrentRocket.transform.position.y, transform.position.z);
            transform.SetParent(CurrentRocket.transform);
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
        //_cam.transform.position = new Vector3(CurrentRocket.transform.position.x, CurrentRocket.transform.position.y, transform.position.z);
        CurrentRocket.Control(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"));

        GlobalTime += Time.deltaTime;
    }
}
