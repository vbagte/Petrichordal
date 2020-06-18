using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class scoreupdater : MonoBehaviour
{
    Text TextComponent;
   // public Text TextComponent;
    // Start is called before the first frame update
    void Start()
    {
        TextComponent = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
       TextComponent.text = (playerstats.score + playerstats.levelscore).ToString();
   
    }
}
