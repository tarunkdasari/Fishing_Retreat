using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class InWater : MonoBehaviour
{
    public float waterLevel;
    public float reliefTime;

    private bool inWater = false;
    private bool nearWater = false;
    private Rigidbody rb;
    private float timeOut = 0;
    private float timeIn = 0;

    private float randomFishTime = 27.75f;
    private FishCatching fc;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        fc = GetComponent<FishCatching>();
    }

    // Update is called once per frame
    void Update()
    {
        float currLevel = transform.position.y;
        if (currLevel > waterLevel)
        {
            inWater = false;
            timeOut += Time.deltaTime;
            if (timeOut > reliefTime)
            {
                nearWater = false;
                timeIn = 0;
            }
        }
        else
        {
            inWater = true;
            nearWater = true;
            timeOut = 0;
            timeIn += Time.deltaTime;
            if(timeIn > randomFishTime)
            {
                fc.Catch();
            }
        }

    }

    public bool LureInWater()
    {
        return nearWater;
    }
    
    public bool LureInWaterLiterally()
    {
        return inWater;
    }
}
