using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float length, startpos;
    public GameObject camObj;
    public CameraController cam;
    public float parallaxEffect;

    private void Start()
    {
        startpos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        if (camObj == null) return;

        if (cam.isClamped) return;

        float temp = (camObj.transform.position.x * (1 - parallaxEffect));
        float dist = (camObj.transform.position.x * parallaxEffect);

        transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);

        if (temp > startpos + length) startpos += length;
        else if (temp < startpos - length) startpos -= length;


    }
}
