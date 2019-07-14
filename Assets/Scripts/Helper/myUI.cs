using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class myUI : MonoBehaviour
{

    GameObject[] finishGame;
    int myScore = 0;
    public Text highscore;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        finishGame = GameObject.FindGameObjectsWithTag("Finish");
        hideOver();        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0 && GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().gameOver == true)
        {
            showOver();
        }
    }

    public void Reload()
    {
        SceneManager.LoadScene( SceneManager.GetActiveScene().name);
    }

    public void showOver()
    {
        foreach (GameObject myPlayer in finishGame)
        {
            myPlayer.SetActive(true);
        }
    }

    public void LoadMainScene()
    {
         SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void hideOver()
    {
        foreach (GameObject myPlayer in finishGame)
        {
            myPlayer.SetActive(false);
        }
    }

    

	void SetHighScore()
	{
		print(myScore);
		highscore.text=myScore.ToString();
	}

}
