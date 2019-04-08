using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Levels : MonoBehaviour
{
    public Text LEVEL;
    public int level = 1;

    // Start is called before the first frame update
    void Start()
    {
        //Start at level 1
        LEVEL.text = "Level: " + 1;
    }

    // Update is called once per frame
    void Update()
    {
        //Display level 2 if conditions met
        if(ScoreManager.SCORE >= 250 && ScoreManager.SCORE < 500)
        {
            level = 2;
            LEVEL.text = "Level: " + level;
        }
        //Display level 3 if conditions met
        else if(ScoreManager.SCORE >= 500 && ScoreManager.SCORE < 750)
        {
            level = 3;
            LEVEL.text = "Level: " + level;
        }
        //Display level 4 if conditions met
        else if(ScoreManager.SCORE >= 750)
        {
            LEVEL.text = "Level: Max Level";
        }
        //If conditions met display max level
        else if(ScoreManager.SCORE > 1000)
        {
            LEVEL.text = "Level: Max Level";
        }
    }
}
