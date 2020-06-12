using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMODUnity;

public class ReturnToMenu : MonoBehaviour
{

    public GameObject fade;

    private bool enable = false;

  
    IEnumerator Start()
    {
        yield return new WaitForSeconds(3.5f);
        enable = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && enable)
        {
            fade.GetComponent<Animation>().Play("FadeOut_02");
            StartCoroutine(Menu());
            enable = false;
        }
    }

    IEnumerator Menu()
    {
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene(0);
    }

}
