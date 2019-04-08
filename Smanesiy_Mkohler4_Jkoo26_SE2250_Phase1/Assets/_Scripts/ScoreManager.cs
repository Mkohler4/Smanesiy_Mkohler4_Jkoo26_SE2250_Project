using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text scoreBoard;
    public Text highScore;
    //[Header("Set Dynamically")]
    public static int SCORE = 0;

    // Start is called before the first frame update
    void Start()
    {
        //Get the most recent high score
        highScore.text = "High Score: " + PlayerPrefs.GetInt("High Score: ", 0).ToString();
    }
    void Update()
    {
        scoreBoard.text = "Score: " + SCORE;

        if(SCORE > PlayerPrefs.GetInt("High Score: ", 0))
        {
            //Save a new highscore if the current score is greater than the old highscore
            PlayerPrefs.SetInt("High Score: ", SCORE);
            scoreBoard.text = SCORE.ToString();
            //Update the highscore if the score is greater than the previous highscore
            highScore.text = "High Score: " + PlayerPrefs.GetInt("High Score: ", 0).ToString();
        }

    }
   
}
