using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameManagement;

abstract public class LevelController : MonoBehaviour {
    protected new Camera camera;
    protected Canvas canvas;
    protected RectTransform canvasRect;
    protected LevelTimer timer;
    protected int score;
    protected GameManager manager;

    [SerializeField]
    protected int goldenAcornValue = 20;

    private Transform pauseMenu;
    private Text getReadyText;
    protected class DelayData // collect data related to level start delay in one place
    {
        public readonly int delay = 3;
        public int dPart;
        public float startTime;

        public DelayData()
        {
            dPart = delay / 3;
        }
    }
    protected DelayData delay = new DelayData();

    protected List<ICharacterController> characters = new List<ICharacterController>();
    protected int nuts;
    protected UIAcornController acornCtrl;

    public virtual int MinimumNuts
    {
        get
        {
            return 0;
        }
    }

    public virtual float LevelTime
    {
        get
        {
            return 0;
        }
    }

    /// <summary>
    /// Register as a character with the level controller to be signalled for pausing, unpausing, and other events.
    /// </summary>
    /// <param name="character">The character that will be registered.</param>
    public void RegisterCharacter(ICharacterController character)
    {
        characters.Add(character);
    }

    /// <summary>
    /// Call this method to tally a nut and trigger the appropriate animation.
    /// </summary>
    /// <param name="nut">The type of nut collected.</param>
    /// <param name="worldPosition">The world position of the nut that was collected.</param>
    virtual public void OnNutCollect(NutProperties nut)
    {
        nuts += (int)nut.type;
        Vector2 pos = camera.WorldToViewportPoint(nut.worldPosition);
        pos = new Vector2(
            ((pos.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f)),
            ((pos.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f)));
        acornCtrl.OnNutCollect(nut, pos);
    }

    public virtual void Pause()
    {
        pauseMenu.gameObject.SetActive(true);
        PausePlayers();
        Time.timeScale = 0;
    }

    public virtual void UnPause()
    {
        pauseMenu.gameObject.SetActive(false);
        UnPausePlayers();
        Time.timeScale = 1;
    }

    protected virtual void Awake()
    {
        manager = GameManager.instance;

        GameObject temp;

        temp = GameObject.FindGameObjectWithTag("UI");
        if (temp == null)
        {
            Debug.Log(this.GetType().Name + " could not find a UI in the scene!");
        }
        else
        {
            canvas = temp.GetComponent<Canvas>();
            canvasRect = canvas.GetComponent<RectTransform>();
            if ((timer = canvas.GetComponent<LevelTimer>()) == null)
            {
                Debug.Log(this.GetType().Name + " could not find a LevelTimer in UI canvas!");
            }
            // initialize timer's time property before the timer runs Start()
            if (timer != null)
            {
                timer.time = LevelTime;
            }
        }
    }

    protected virtual void Start()
    {
        GameObject temp;    // temporary storing place for possibly null objects

        if ((acornCtrl = FindObjectOfType<UIAcornController>()) == null)
        {
            Debug.Log(this.GetType().Name + " could not find a UIAcornController in the scene!");
        }

        temp = GameObject.FindGameObjectWithTag("MainCamera");
        if (temp == null)
        {
            Debug.Log(this.GetType().Name + " could not find a MainCamera in the scene!");
        }
        else
        {
            camera = temp.GetComponent<Camera>();
        }

        temp = GameObject.FindGameObjectWithTag("PauseMenu");
        if (temp == null)
        {
            Debug.Log(this.GetType().Name + " could not find a PauseMenu in the scene!");
        }
        else
        {
            pauseMenu = temp.transform;
            pauseMenu.gameObject.SetActive(false);
        }

        temp = GameObject.FindGameObjectWithTag("ReadyText");
        if (temp == null)
        {
            Debug.Log(this.GetType().Name + " could not find a ReadyText found in the scene!");
        }
        else
        {
            getReadyText = temp.GetComponent<Text>();
        }

        delay = new DelayData();
        delay.startTime = Time.time + delay.delay;
    }

    protected virtual void FixedUpdate()
    {
        if (Time.time < delay.startTime + 1)
        {
            float dTime = Time.time - delay.startTime + delay.delay;
            if (dTime < delay.dPart)
            {
                getReadyTextField = "3";
            }
            else if (dTime < 2 * delay.dPart)
            {
                getReadyTextField = "2";
            }
            else if (dTime < delay.delay)
            {
                getReadyTextField = "1";
            }
            else
            {
                getReadyTextField = "Go!!";

                SquirrelController.setGo();

                foreach (ICharacterController npc in characters)
                {
                    npc.Begin();
                }
            }
        }
        else
        {
            Destroy(getReadyText);
        }
    }
    
    protected virtual void OnDestroy()
    {
        GameObject[] objs = FindObjectsOfType<GameObject>() as GameObject[];

        foreach (GameObject obj in objs)
        {
            Destroy(obj);
        }
    }

    protected virtual string getReadyTextField
    {
        set
        {
            if (getReadyText != null)
            {
                getReadyText.text = value;
            }
        }
        get
        {
            if (getReadyText != null)
            {
                return getReadyText.text;
            }
            else
            {
                return "";
            }
        }
    }

    protected virtual void UnPausePlayers()
    {
        SquirrelController.setGo();

        foreach (ICharacterController NPC in characters)
        {
            NPC.UnPause();
        }
    }

    protected virtual void PausePlayers()
    {
        SquirrelController.setStop();

        foreach (ICharacterController NPC in characters)
        {
            NPC.Pause();
        }
    }

    protected void EndLevel()
    {
        manager.OnLevelComplete(score);
    }

    abstract public void SlowTime();
    abstract public void SpeedUp();
    abstract public void GameOver(bool levelWon);
	abstract public void tallyScore();
    abstract public void hitHome();
    abstract public void hometreeOffScreen(bool value);
}
