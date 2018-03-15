using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class CampaignLevelController : LevelController
{
    private Text gameOverText;
    private Text restartText;
	private Text tutorialText;
	private Text scoreText;
    
    [SerializeField]
    private float time;
    [SerializeField]
	private int nutsNeeded;

    private HomingBeaconController beacon;
	private AdsController ads;

	private int goldenNuts;
	private GameObject[] dogs;
	private GameObject[] cars;
	private bool gameOver;
	private bool localLevelWon;
	private bool atHome;
	private float atHomeMessageTimer;
	private float tutorialMessageTimer;

	int timesTried = 0;

    public override int MinimumNuts
    {
        get { return this.nutsNeeded; }
    }

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
            CampaignLevelParameters p = props.campaignParams;
            time = p.timeLimit;
            nutsNeeded = p.minimumNuts;
        }
    }

    // Use this for initialization
    protected override void Start()
	{
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
            GameObject temp;    // temporary storing place for possibly null objects

            if ((beacon = FindObjectOfType<HomingBeaconController>()) == null)
            {
                Debug.Log("No HomingBeaconController found in scene!");
            }

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

		scoreTextField = "";
		tutorialTextField = "";
		restartTextField = "";
		gameOverTextField = "";

		dogs = GameObject.FindGameObjectsWithTag("Dog");
		cars = GameObject.FindGameObjectsWithTag("Car");


		SquirrelController.restart();

		gameOver = false;
		atHome = false;

        // set up homing beacon
        GameObject hometree = GameObject.FindGameObjectWithTag("HomeTree");

		if (beacon != null) beacon.setHometreeLocation(hometree.transform);




	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
		{
			Pause ();
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
					if(localLevelWon){
						goToNextLevel ();
					}else{
						SceneManager.LoadScene(SceneManager.GetActiveScene().name);
					}
				}
			#endif
        }
    }

	protected override void FixedUpdate()
	{
        base.FixedUpdate();

		if (timer != null
			&& timer.paused
			&& Time.time >= delay.startTime
			&& !gameOver)
		{
			timer.paused = false;
		}

		//if colliding with home tree
		if (atHome) {

			//if you have enough nuts
			if (nuts >= nutsNeeded) {
				//listen for C key to load next level
				if (Input.GetKeyDown (KeyCode.C)) {
					goToNextLevel ();
				}
			} else {
				//display temporary message that you need more nuts
				if (atHomeMessageTimer > 0) {
					atHomeMessageTimer -= Time.deltaTime;
				} else {
					gameOverTextField = "";
					atHome = false;
				}
			}
		}
	}


	override public void SlowTime(){
		if (dogs != null && dogs.Length > 0) {
			foreach (GameObject dog in dogs) {
				dog.SendMessage ("slowDown");
			}
		}

		if (cars != null && cars.Length > 0) {
			foreach (GameObject car in cars) {
				car.SendMessage ("slowDown");
			}
		}

		timer.SendMessage ("slowDown");

	}

	override public void SpeedUp(){
		if (dogs != null && dogs.Length > 0) {
			foreach (GameObject dog in dogs) {
				dog.SendMessage ("speedUp");
			}
		}

		if (cars != null && cars.Length > 0) {
			foreach (GameObject car in cars) {
				car.SendMessage ("speedUp");
			}
		}

		timer.SendMessage ("speedUp");

	}


	override public void GameOver(bool levelWon)
	{
		gameOver = true;
		localLevelWon = levelWon;
		PausePlayers ();
		SquirrelController.setDead ();
		timer.paused = true;

		if (levelWon) {
			tallyScore ();
			clearTutorialText ();
			scoreTextField = "Score: " + score;
			gameOverTextField = "You Win!!";
			#if (UNITY_ANDROID || UNITY_IPHONE)
				restartTextField = "Two-finger touch to continue!";
			#else
				restartTextField = "Press 'C' to Continue";
			#endif
		} else {
			gameOverTextField = "Game Over!";
			#if (UNITY_ANDROID || UNITY_IPHONE)
				restartTextField = "Two-finger touch to retry!";
			#else
				restartTextField = "Press 'R' for Restart";
			#endif
		}
	}

	override public void tallyScore() {
		int tempNuts = nuts - goldenNuts * 20;
		score += tempNuts * 25;
		score += goldenNuts * 100;
		score += Mathf.RoundToInt(timer.timeLeft) * 10;
	}

	override public void hitHome(){
		atHome = true;
		if (nuts >= nutsNeeded) {
			if (!gameOver) {
				GameOver (true);
			}
		} else {
			clearTutorialText ();
			gameOverTextField = "Not enough nuts!!";
			atHomeMessageTimer = 4;
		}
	}

	private string tutorialTextField {
		set {
			if (tutorialText != null) {
				tutorialText.text = value;
			}
		}
		get {
			if (tutorialText != null) {
				return tutorialText.text;
			} else {
				return "";
			}
		}
	}

	private string scoreTextField {
		set {
			if (scoreText != null) {
				scoreText.text = value;
			}
		}
		get {
			if (scoreText != null) {
				return scoreText.text;
			} else {
				return "";
			}
		}
	}

	private string gameOverTextField {
		set {
			if (gameOverText != null) {
				gameOverText.text = value;
			}
		}
		get {
			if (gameOverText != null) {
				return gameOverText.text;
			} else {
				return "";
			}
		}
	}

	private string restartTextField {
		set {
			if (restartText != null) {
				restartText.text = value;
			}
		}
		get {
			if (restartText != null) {
				return restartText.text;
			} else {
				return "";
			}
		}
	}

	private void tutorial(int x) {
		gameOverText.text = "";
		if (x == 1) {
			tutorialText.text = "Eat red time berries to temporarily slow enemies!";
		}
		else if (x == 2) {
			tutorialText.text = "Hide from dogs by running up trees!";
		}
		else if (x == 3) {
			tutorialText.text = "Find the golden acorns on each level for extra nuts!";
		}
		else if (x == 4) {
			tutorialText.text = "Collect enough acorns before time runs out!";
		}
		else if (x == 5) {
			tutorialText.text = "Boost your score by ending the game with extra time left!";
		}
		else if (x == 6) {
#if (UNITY_ANDROID || UNITY_IPHONE)
				tutorialText.text = "Use virtual joystick for movement";
#else
				tutorialText.text = "Use WASD or arrow keys for movement";
#endif

		}
        CancelInvoke();
		Invoke ("clearTutorialText", 3);
	}

	private void clearTutorialText(){
		tutorialText.text = "";
	}

	private void goToNextLevel(){
        // TODO: Checking if a callback has been registered is temporary while old
        // scene change mechanism still needs to be preserved.
        EndLevel();
        /*
        else
        {
            Scene scene = SceneManager.GetActiveScene();
            if (scene.name == "Park level")
            {
                SceneManager.LoadScene("sf1");
            }
            if (scene.name == "Suburb level")
            {
                SceneManager.LoadScene("sf2");
            }
            if (scene.name == "Highway level")
            {
                SceneManager.LoadScene("sf3");
            }
            if (scene.name == "Forest level")
            {
                SceneManager.LoadScene("sf4");
            }
            if (scene.name == "Tree Farm level")
            {
                SceneManager.LoadScene("Win scene");
            }
        }
        */
	}

    override public void hometreeOffScreen(bool value)
    {
        beacon.isEnabled = value;
    }
}
