using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class playerstats
{
    //Static variables are shared across all instances
    //of a class.p
    private static int score_ = 0;
    private static float playtime_ = 0f;
   
    public static float playtime
    {
        get
        {
            return playtime_;
        }
        set
        {
            playtime_ = value;
        }
    }
    public static int score
    {
        get
        {
            return score_;
        }
        set
        {
            score_ = value;
        }

    } 
}
