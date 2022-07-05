using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField] Transform PlayerT;
    [SerializeField] float lerpSpeed = 100;
    void Start()
    {
        this.transform.position = new Vector3(PlayerT.position.x, PlayerT.position.y, this.transform.position.z);
    }

    void Update()
    {
        Vector3 newCamPos = new Vector3(PlayerT.position.x, PlayerT.position.y, this.transform.position.z);

        this.transform.position = Vector3.Lerp(this.transform.position, newCamPos, Time.deltaTime * lerpSpeed);
    }
}
