using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HometreeController : MonoBehaviour {
    public GameObject playerCharacter;

    public float emergeTime;        // Time squirrel walks downward for
    private bool emerging = true;
    private float startTime;
    private float endTime;

    private LevelController lvlctrl;
    private GameObject player;
    private SquirrelController squirrel;

    private void Awake()
    {
        player = Instantiate(playerCharacter);
        if (player == null)
        {
            Debug.Log("HometreeController did not successfully instantiate a player squirrel.");
		}
        player.transform.position = this.transform.position;
        squirrel = player.GetComponent<SquirrelController>();
        squirrel.isDisabled = true;


    }

    // Use this for initialization
    void Start () {
        startTime = Time.time;
        endTime = startTime + emergeTime;

        lvlctrl = FindObjectOfType<LevelController>();
	}

    private void FixedUpdate()
    {
        if (emerging)
        {
            if (Time.time > endTime)
            {
                squirrel.move(false, false, false, false);
                squirrel.isDisabled = false;
                emerging = false;
            }
            else
            {
                squirrel.move(false, false, false, true, 0.3f);
            }
        }
    }

    private void OnBecameInvisible()
    {
        lvlctrl.SendMessage("hometreeOffScreen", true);
    }

    private void OnBecameVisible()
    {
        lvlctrl.SendMessage("hometreeOffScreen", false);
    }
}
