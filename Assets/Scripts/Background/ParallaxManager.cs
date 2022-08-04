using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxManager : MonoBehaviour
{

    public List<Transform> parallaxTransforms;

    private void Start()
    {
        parallaxTransforms = new List<Transform>();
        for (int i = 0; i < transform.childCount; i++)
        {
            parallaxTransforms.Add(transform.GetChild(i));
        }
    }

}
