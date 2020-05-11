using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderQueue : MonoBehaviour
{

    public Material[] material;

    void Start()
    {
        for (int i = 0; i < material.Length; i++)
        {
            material[i].renderQueue = 3001;
        }
    }

}
