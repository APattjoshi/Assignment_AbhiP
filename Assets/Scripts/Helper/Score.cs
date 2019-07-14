using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    int myScore = 0;
    public Text highscore;

    // Start is called before the first frame update
    void Start()
    {
        myScore = PlayerPrefs.GetInt("High_Score",0);
        
    }

    // Update is called once per frame
    void Update () 
    {
        print(myScore);
		myScore=PlayerPrefs.GetInt("High_Score");
        highscore.text = myScore.ToString();
	}
}
