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

    public float leftLimit, rightLimit;
    public bool isClamped;

    private void Awake()
    {
        var levelManager = FindObjectOfType<LevelManager>();
        if (levelManager == null)
        {
            Debug.Log("LevelManager is missing - Create one and set the Level Borders");
            return;
        }
        leftLimit = levelManager.leftLevelLimit;
        rightLimit = levelManager.rightLevelLimit;
    }

    private void Start()
    {
        Camera.main.GetComponent<AudioListener>().enabled = false;
        if (!player.GetComponent<NetworkIdentity>().hasAuthority)
        {
            GetComponent<Camera>().enabled = false;
            GetComponent<AudioListener>().enabled = false;
            gameObject.SetActive(false); return;
        }
        var parallaxManager = FindObjectOfType<ParallaxManager>();

        if (parallaxManager == null) { Debug.Log("ParralaxManager: " + parallaxManager); return; }

        for (int i = 0; i < parallaxManager.parallaxTransforms.Count; i++)
        {
            var parallax = parallaxManager.parallaxTransforms[i].GetComponent<Parallax>();
            parallax.camObj = this.gameObject;
            parallax.cam = this;
        }
    }

    private void Update()
    {
        if (!player.GetComponent<NetworkIdentity>().hasAuthority) { return; }

        if (player == null) return;
        if (transform.localPosition.x != 0)
        {
            isClamped = true;
        }
        else
        {
            isClamped = false;
        }

        transform.position = new Vector3(Mathf.Clamp(player.transform.position.x, leftLimit, rightLimit), yAxisValue, zAxisValue);

    }

}
