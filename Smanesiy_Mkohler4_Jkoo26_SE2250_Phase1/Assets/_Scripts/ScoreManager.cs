using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    
    static public int HIGHSCORE = 0;
    //public static ScoreManager S = null;
    public Text _scoreBoard;
    public Text _highScore;
    //use key, setkey getkey, haskey

    //[Header("Set Dynamically")]
    public static int score = 0;
    // Start is called before the first frame update
        
    
    void Start()
    {
        _highScore.text = "High Score: " + PlayerPrefs.GetInt("High Score: ", 0).ToString();
    }
    void Update()
    {
        _scoreBoard.text = "Score: " + score;

        if(score > PlayerPrefs.GetInt("High Score: ", 0))
        {
            PlayerPrefs.SetInt("High Score: ", score);
            _scoreBoard.text = score.ToString();
        }

    }
   
}
