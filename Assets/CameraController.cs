using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Mirror;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public float yAxisValue = 0f;
    public float zAxisValue = -10f;


    private void Start()
    {
        if (!player.GetComponent<NetworkIdentity>().hasAuthority) { GetComponent<Camera>().enabled = false; return; }
        var parallaxManager = FindObjectOfType<ParallaxManager>();

        for (int i = 0; i < parallaxManager.parallaxTransforms.Count; i++)
        {
            parallaxManager.parallaxTransforms[i].GetComponent<Parallax>().cam = this.gameObject;
        }
    }

    private void Update()
    {
        if (!player.GetComponent<NetworkIdentity>().hasAuthority) { return; }

        if (player == null) return;
        transform.position = new Vector3(player.transform.position.x, yAxisValue, zAxisValue);
    }

}
