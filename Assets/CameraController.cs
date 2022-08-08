using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Mirror;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public float xAxisValue = 0f;
    public float yAxisValue = 0f;
    public float zAxisValue = -10f;

    public float smoothFactor = 2;

    public Vector3 minValues, maxValues;

    private void Start()
    {
        if (!player.GetComponent<NetworkIdentity>().hasAuthority) { GetComponent<Camera>().enabled = false; return; }
        var parallaxManager = FindObjectOfType<ParallaxManager>();


        if (parallaxManager == null) { Debug.Log("ParralaxManager: " + parallaxManager); return; }

        for (int i = 0; i < parallaxManager.parallaxTransforms.Count; i++)
        {
            parallaxManager.parallaxTransforms[i].GetComponent<Parallax>().cam = this.gameObject;
        }
    }

    private void Update()
    {
        if (!player.GetComponent<NetworkIdentity>().hasAuthority) { return; }

        if (player == null) return;

        //Vector3 targetPosition = player.transform.position; /*new Vector3(player.transform.position.x, yAxisValue, zAxisValue);*/

        //Vector3 boundPosition = new Vector3(
        //    Mathf.Clamp(targetPosition.x, minValues.x, maxValues.x),
        //    Mathf.Clamp(targetPosition.y, minValues.y, maxValues.y),
        //    Mathf.Clamp(targetPosition.z, minValues.z, maxValues.z));

        //Vector3 smoothPosition = Vector3.Lerp(transform.position, boundPosition, smoothFactor * Time.deltaTime); 

        transform.position = new Vector3(player.transform.position.x, yAxisValue, zAxisValue);

    }

}
