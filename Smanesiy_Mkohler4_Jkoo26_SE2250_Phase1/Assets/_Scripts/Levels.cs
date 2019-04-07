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
        LEVEL.text = "Level: " + 1;
    }

    // Update is called once per frame
    void Update()
    {
        if(ScoreManager.SCORE >= 250 && ScoreManager.SCORE < 500)
        {
            level = 2;
            LEVEL.text = "Level: " + level;
        }
        else if(ScoreManager.SCORE >= 500 && ScoreManager.SCORE < 750)
        {
            level = 3;
            LEVEL.text = "Level: " + level;
        }
        else if(ScoreManager.SCORE >= 750)
        {
            level = 4;
            LEVEL.text = "Level: " + level;
        }
    }
}
