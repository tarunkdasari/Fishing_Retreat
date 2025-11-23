using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SitInKayak : MonoBehaviour
{

    public Transform seatPoint;
    public float sitSpeed = 2f;
    public KeyCode sitKey = KeyCode.E; // Press E to sit

    private bool sitting = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (seatPoint == null) return;

        if (Input.GetKeyDown(sitKey)) sitting = !sitting;

        if (sitting)
        {
            transform.position = Vector3.Lerp(transform.position, seatPoint.position, Time.deltaTime * sitSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation, seatPoint.rotation, Time.deltaTime * sitSpeed);
        }
    }
}
