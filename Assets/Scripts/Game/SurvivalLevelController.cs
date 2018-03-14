using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SurvivalLevelController : LevelController
{
    private Text gameOverText;
    private Text restartText;
    private Text tutorialText;
    private Text scoreText;

    public float time;

    public GameObject acorn;
	private AdsController ads;
	private int goldenNuts;
    private GameObject[] dogs;
    private GameObject[] cars;
    private bool gameOver;
    private bool atHome;
    private float atHomeMessageTimer;

    public int xMin;
    public int xMax;
    public int yMin;
    public int yMax;

    public override float LevelTime
    {
        get
        {
            return time;
        }
    }

    protected override void Awake()
    {
        base.Awake();

        LevelProperties props = FindObjectOfType<LevelProperties>();

        if (props != null)
        {
            SurvivalLevelParameters p = props.survivalParams;
            acorn = p.acornPrefab;
            xMin = p.xMin;
            xMax = p.xMax;
            yMin = p.yMin;
            yMax = p.yMax;
            time = p.timeOnTimer;
        }
    }

    // Use this for initialization
    protected override void Start () {
        base.Start();

		#if (UNITY_ANDROID || UNITY_IPHONE)

			ads = FindObjectOfType<AdsController> ();
			DontDestroyOnLoad(ads);
			ads.incrementTimesPlayed ();


			if (ads.getTimesPlayed() % 3 == 0) {
				ads.showAd();
			}
		#endif

        { // Big initialization/autoconfig phase
            GameObject temp;

            temp = GameObject.FindGameObjectWithTag("RestartText");
            if (temp == null)
            {
                Debug.Log("No RestartText found in scene!");
            }
            else
            {
                restartText = temp.GetComponent<Text>();
            }

            temp = GameObject.FindGameObjectWithTag("loseText");
            if (temp == null)
            {
                Debug.Log("No loseText/gameOverText found in scene!");
            }
            else
            {
                gameOverText = temp.GetComponent<Text>();
            }
            temp = GameObject.FindGameObjectWithTag("TutorialText");
            if (temp == null)
            {
                Debug.Log("No TutorialText found in scene!");
            }
            else
            {
                tutorialText = temp.GetComponent<Text>();
            }

            temp = GameObject.FindGameObjectWithTag("ScoreText");
            if (temp == null)
            {
                Debug.Log("No ScoreText found in scene!");
            }
            else
            {
                scoreText = temp.GetComponent<Text>();
            }
        }

        for(int i = 0; i < 100; i++)
        {
            nutSpawn();
        }

        scoreText.text = "";
        tutorialText.text = "";
        restartText.text = "";
        gameOverText.text = "";

        canvasRect = canvas.GetComponent<RectTransform>();

        dogs = GameObject.FindGameObjectsWithTag("Dog");
        cars = GameObject.FindGameObjectsWithTag("Car");


        SquirrelController.restart();

        gameOver = false;
        atHome = false;

        GameObject hometree = GameObject.FindGameObjectWithTag("HomeTree");
    }
	
	// Update is called once per frame
	private void Update () {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            Pause();
        }

        //if game is over listen for R to restart level
        if (gameOver)
        {
            if (Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
			#if (UNITY_ANDROID || UNITY_IPHONE)
				if(Input.touchCount==2){ 
					SceneManager.LoadScene(SceneManager.GetActiveScene().name);
				}
			#endif
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (timer.paused && Time.time >= delay.startTime && !gameOver)
        {
            timer.paused = false;
        }
    }

    override public void SlowTime()
    {
        if (dogs != null && dogs.Length > 0)
        {
            foreach (GameObject dog in dogs)
            {
                dog.SendMessage("slowDown");
            }
        }

        if (cars != null && cars.Length > 0)
        {
            foreach (GameObject car in cars)
            {
                car.SendMessage("slowDown");
            }
        }

        timer.SendMessage("slowDown");

    }

    override public void SpeedUp()
    {
        if (dogs != null && dogs.Length > 0)
        {
            foreach (GameObject dog in dogs)
            {
                dog.SendMessage("speedUp");
            }
        }

        if (cars != null && cars.Length > 0)
        {
            foreach (GameObject car in cars)
            {
                car.SendMessage("speedUp");
            }
        }

        timer.SendMessage("speedUp");

    }

    override public void GameOver(bool levelWon)
    {
        gameOver = true;
        PausePlayers();
        SquirrelController.setDead();
        timer.paused = true;

        if (levelWon)
        {
            gameOverText.text = "You Win!!";
			#if (UNITY_ANDROID || UNITY_IPHONE)
			restartText.text = "Two-finger touch to continue!";
			#else
			restartText.text = "Press 'C' to Continue";
			#endif
        }
        else
        {
			tallyScore ();
			scoreText.text = "Score: " + score;
            gameOverText.text = "Game Over!";
			#if (UNITY_ANDROID || UNITY_IPHONE)
			restartText.text = "Two-finger touch to retry!";
			#else
			restartText.text = "Press 'R' for Restart";
			#endif
        }
    }

	override public void tallyScore() {
		int tempNuts = nuts - goldenNuts * 20;
		score += tempNuts * 25;
		score += goldenNuts * 100;
	}

    override public void hitHome()
    {

    }

    override public void hometreeOffScreen(bool value)
    {

    }

    public void nutSpawn()
    {
        GameObject nut = Instantiate(acorn, new Vector3(Random.Range(xMin, xMax), Random.Range(yMin, yMax), 0f), Quaternion.identity);
        EvilSquirrels.AddNut(nut);
    }

    public override void OnNutCollect(NutProperties nut)
    {
        base.OnNutCollect(nut);
        nutSpawn();
        timer.SendMessage("addTime");
    }
}
