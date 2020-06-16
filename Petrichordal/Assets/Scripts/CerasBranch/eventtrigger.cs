//==============================================================================
//Project:       Petrichordal
//File Name:     eventtrigger.cs
//Author:        Cera Baltzley
//Class:         CS 185 2020 Spring Quarter
//Description:   This script is used to provide a timing/location based trigger 
//                  with two independent types of effects
//                  A) stop the level autoscroll until either specified duration elapses
//                        or all enemies of specified tag are killed.
//                        then resume scrolling at a new custom rate, and
//                  B) set player movement speed
//==============================================================================
//Known Bugs: 
//==============================================================================
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
[CanEditMultipleObjects]
[CustomEditor(typeof(eventtrigger))]
public class eventtriggerEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        eventtrigger etrig = (eventtrigger)target;
  
        EditorGUILayout.LabelField("[Event Duration]", EditorStyles.boldLabel);
        etrig.show_MustKill = EditorGUILayout.Toggle("Destroy to Continue Mode:", etrig.show_MustKill);
        if (etrig.show_MustKill==true)
        {
            etrig.TagsThatMustDie = EditorGUILayout.TagField("All Objects with Tag:",etrig.TagsThatMustDie);
        }
        else etrig.seconds = EditorGUILayout.IntField("Seconds:",etrig.seconds);
        EditorGUILayout.LabelField("[Event Effects]",EditorStyles.boldLabel);
        etrig.new_speed_multiplier = EditorGUILayout.FloatField("Scroll Multiplier(Event Start):", etrig.new_speed_multiplier);
        etrig.speed_on_exit_multiplier = EditorGUILayout.FloatField("Scroll Multiplier(Event End:):", etrig.speed_on_exit_multiplier);
        etrig.slow_player_divider = EditorGUILayout.IntField("Set Player Speed:", etrig.slow_player_divider);
        EditorGUILayout.LabelField("[Level Layers]", EditorStyles.boldLabel);
        etrig.foreground = (GameObject)EditorGUILayout.ObjectField("Foreground Object", etrig.foreground, typeof(GameObject), true);
        etrig.midground = (GameObject)EditorGUILayout.ObjectField("Midground Object", etrig.midground, typeof(GameObject), true);
        etrig.background = (GameObject)EditorGUILayout.ObjectField("Background Object", etrig.background, typeof(GameObject), true);
        //allow changes to instances
        if (PrefabUtility.GetPrefabInstanceStatus(target)!=PrefabInstanceStatus.NotAPrefab)  EditorUtility.SetDirty(target);
    }
}
#endif

public class eventtrigger : MonoBehaviour
{
    [HideInInspector]
    public bool show_MustKill = false;
    [HideInInspector]
    public int seconds;
    [HideInInspector]
    public string TagsThatMustDie;
    [HideInInspector]
    public float speed_on_exit_multiplier;
    [HideInInspector]
    public float new_speed_multiplier;
    [HideInInspector]
    public GameObject foreground;
    [HideInInspector]
    public GameObject midground;
    [HideInInspector]
    public GameObject background;
    [HideInInspector]
    public int slow_player_divider = 4;


    //  public GameObject enemylayer;
    private long counter;
    private float bgOS, mgOS, fgOS;
    //public SoundManager soundManager;

    // Start is called before the first frame update
    void Start()
    {
        counter = 0;
        bgOS = background.GetComponent<Scroll>().speed;
        mgOS = midground.GetComponent<Scroll>().speed;
        fgOS = foreground.GetComponent<Scroll>().speed;
        //soundManager = GameObject.Find("Main Camera").GetComponent<SoundManager>();
        //elOS = enemylayer.GetComponent<Scroll>().layerspeed;
    }

    // Update is called once per frame
    void Update()
    {
 
        if (transform.position.x <= 10 && transform.position.x >= -10 && transform.position.y >= -5.5 && transform.position.y < 3.5)    
        {
            bool condition = true;
            // plays level 04 music
            //if (counter == 0 && gameObject.name == "eventsquare 1 (music)") 
            //{
            //    soundManager.PlayMusic();
            //}
            if (show_MustKill==true)
            {
                GameObject[] objects;
                objects= GameObject.FindGameObjectsWithTag( TagsThatMustDie);
                if (objects.Length == 0) condition = false;
            }
            else
            {
                counter++;
                if (Mathf.Floor(counter / 60) >= seconds) condition = false;
            }
            if (condition)
            {
               
                background.GetComponent<Scroll>().speed = bgOS * new_speed_multiplier;
                midground.GetComponent<Scroll>().speed = mgOS * new_speed_multiplier;
                foreground.GetComponent<Scroll>().speed = fgOS * new_speed_multiplier;

            }
            else
            {
               background.GetComponent<Scroll>().speed = bgOS*speed_on_exit_multiplier;
                midground.GetComponent<Scroll>().speed = mgOS * speed_on_exit_multiplier;
                foreground.GetComponent<Scroll>().speed = fgOS * speed_on_exit_multiplier;

             Destroy(gameObject);
            }
            GameObject.FindWithTag("Player").gameObject.GetComponent<PlayerController>().speed = slow_player_divider;

        }
    }
}
