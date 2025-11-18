using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishCatching : MonoBehaviour
{
    public Transform fishTransform;
    public GameObject fishPrefab;
    private GameObject currFish;
    void Start()
    {
        
    }

    // Update is called once per frame
    public void Catch()
    {
        if (currFish)
        {
            return;
        }
        currFish = Instantiate(fishPrefab, fishTransform.position, Quaternion.identity);

        float scale = Random.Range(0.7f, 1.4f);
        currFish.transform.localScale = Vector3.one * scale;

        currFish.transform.SetParent(fishTransform);
        MeshRenderer mr = currFish.GetComponentInChildren<MeshRenderer>();
        // Get a copy of the material array
        Material[] mats = mr.materials;
        mats[2].color = new Color(
            Random.Range(0.0f, 1.0f),
            Random.Range(0.0f, 1.0f),
            Random.Range(0.0f, 1.0f)
        );
        mats[3].color = new Color(
            Random.Range(0.0f, 1.0f),
            Random.Range(0.0f, 1.0f),
            Random.Range(0.0f, 1.0f)
        );
        mr.materials = mats;


        Debug.Log("Fish hooked!");
    }
}
