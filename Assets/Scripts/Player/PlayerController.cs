using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    public PlayerDirection direction;

    myUI uiManager;


    public float step_Length = 0.2f;


    public float movement_Frequency = 0.1f;

    private float counter;
    private bool move;

    
    public GameObject tail;

    private List<Vector3> delta_Position;

    private List<Rigidbody> nodes;

    private Rigidbody main_Body;
    private Rigidbody head_Body;
    private Transform tr;

	public bool gameOver=false;
	private int count = 0;
	private int Highscore;
    public Text countText;
    public Text overText;

    private bool New_Tail;
    

    void Awake () 
    {
        tr = transform;
        main_Body = GetComponent <Rigidbody>();

        InitSnakeNodes();
        InitPlayer();

        delta_Position = new List<Vector3>() {
            new Vector3(-step_Length, 0f), //-x left
            new Vector3(0f, step_Length),  //+y up
            new Vector3(step_Length, 0f),  //+x right
            new Vector3(0f, -step_Length)  //-y down
        };
        Highscore = PlayerPrefs.GetInt("High_Score");
    }

    // Update is called once per frame
    void Update()
    {
        CheckMovementFrequency();
    }

    void FixedUpdate ()
    {
        if (move)
        {
            move = false;

            Move();
        }
    }

    void InitSnakeNodes() 
    {
        nodes = new List<Rigidbody>();

        nodes.Add(tr.GetChild(0).GetComponent<Rigidbody>());
        nodes.Add(tr.GetChild(1).GetComponent<Rigidbody>());
        nodes.Add(tr.GetChild(2).GetComponent<Rigidbody>());

        head_Body = nodes[0];
    }

    void SetDirectionRandom ()
    {
        int dirRandom = Random.Range(0, (int)PlayerDirection.COUNT);
        direction = (PlayerDirection)dirRandom;
    }

    void InitPlayer() 
    {
        SetDirectionRandom ();
        switch(direction)
        {
            case PlayerDirection.RIGHT:
                nodes[1].position = nodes[0].position - new Vector3(Metrics.NODE, 0f, 0f);
                nodes[2].position = nodes[0].position - new Vector3(Metrics.NODE * 2f, 0f, 0f);
                break;

            case PlayerDirection.LEFT:
                nodes[1].position = nodes[0].position + new Vector3(Metrics.NODE, 0f, 0f);
                nodes[2].position = nodes[0].position + new Vector3(Metrics.NODE * 2f, 0f, 0f);
                break;

            case PlayerDirection.UP:
                nodes[1].position = nodes[0].position - new Vector3(Metrics.NODE, 0f, 0f);
                nodes[2].position = nodes[0].position - new Vector3(Metrics.NODE * 2f, 0f, 0f);
                break;
            
            case PlayerDirection.DOWN:
                nodes[1].position = nodes[0].position + new Vector3(Metrics.NODE, 0f, 0f);
                nodes[2].position = nodes[0].position + new Vector3(Metrics.NODE * 2f, 0f, 0f);
                break;
        }
    }

    void Move () 
    {
        Vector3 dPosition = delta_Position[(int)direction];
        Vector3 parentPos = head_Body.position;
        Vector3 prevPosition;
        main_Body.position = main_Body.position + dPosition;
        head_Body.position = head_Body.position + dPosition;

        for(int i = 1; i < nodes.Count; i++)
        {
            prevPosition = nodes[i].position;

            nodes[i].position = parentPos;
            parentPos = prevPosition;
        }


        if (New_Tail)
        {
            New_Tail = false;
            GameObject newNode = Instantiate(tail, nodes[nodes.Count - 1].position, Quaternion.identity);
            newNode.transform.SetParent(transform, true);
            nodes.Add(newNode.GetComponent<Rigidbody>());
        }
    }

    void CheckMovementFrequency ()
    {
        counter += Time.deltaTime;
        if (counter >= movement_Frequency)
        {
            counter = 0f;
            move = true;
        }
    }

    public void SetInputDirection(PlayerDirection dir)
    {
        if (dir == PlayerDirection.UP && direction == PlayerDirection.DOWN ||
            dir == PlayerDirection.DOWN && direction == PlayerDirection.UP ||
            dir == PlayerDirection.RIGHT && direction == PlayerDirection.LEFT ||
            dir == PlayerDirection.LEFT && direction == PlayerDirection.RIGHT
        )
        {
            return;
        }   

         direction = dir;

         ForceMove();
    }

    void ForceMove() 
    {
        counter = 0;
        move = false;
        Move();
    }

    private void OnTriggerEnter(Collider target)
    {
        if (target.tag == Tags.BLUE)
        {
            target.gameObject.SetActive(false);
            New_Tail = true;
            count = count + GameObject.FindGameObjectWithTag ("Respawn").GetComponent<GameplayController> ().scoreCount;
        
            SetCountText ();
            GameplayController.instance.IncreaseScore ();
        }
        if (target.tag == Tags.WALL || target.tag == Tags.RED || target.tag == Tags.TAIL)
        {
            Time.timeScale = 0f;    
            gameOver=true;
            overText.text = "Game Over\nScore : "+count.ToString();

            if (count > Highscore)
            {
                PlayerPrefs.SetInt("High_Score", count);
                overText.text +=" New HighScore!!";
            }                
        }
    }

    void SetCountText()
	{
		countText.text = "Score : " + count.ToString ();
	}

}
