using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class playtimeupdater : MonoBehaviour
{
    Text TextComponent;
    // Start is called before the first frame update
    void Start()
    {
        TextComponent = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        TextComponent.text = TimeSpan.FromSeconds(playerstats.playtime).ToString(@"mm\:ss\.ff");
    }
}
