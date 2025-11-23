using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SeatSocketHandler : MonoBehaviour
{

    public XRSocketInteractor socket;
    public GameObject xrOrigin;

    // Start is called before the first frame update
    void Start()
    {
        if (socket == null)
            socket = GetComponent<XRSocketInteractor>();

        socket.selectEntered.AddListener(OnSeatEntered);
        socket.selectExited.AddListener(OnSeatExited);
    }

    private void OnSeatEntered(SelectEnterEventArgs args)
    {
        // Move XR Rig to socket position
        xrOrigin.transform.position = transform.position;
        xrOrigin.transform.rotation = transform.rotation;
    }

    private void OnSeatExited(SelectExitEventArgs args)
    {
        // Optional: do something when leaving seat
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
