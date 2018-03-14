using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalCarController : MonoBehaviour, ICharacterController {

	public float mph;
	private float start;
	private SpriteRenderer sr;
	private bool slowed;

	public float distance;
	public int direction;
	public Sprite upCar;

    private void Awake()
    {
        // register with level controller
        LevelController lvlctrl;
        if ((lvlctrl = FindObjectOfType<LevelController>()) != null)
        {
            lvlctrl.RegisterCharacter(this);
        }
        else
        {
            Debug.Log(this.GetType().Name + " could not find a LevelController!");
        }
    }

    // Use this for initialization
    void Start () {
		sr = GetComponent<SpriteRenderer>();
		if (direction == 1) {
			sr.sprite = upCar;
		}

		start = transform.position.y;
	}

	// Update is called once per frame
	void Update () {
		if (slowed) {
			transform.position = new Vector3 (transform.position.x, transform.position.y+.02f*direction, transform.position.z);
		} else {
			//modulo magic
			transform.position = new Vector3 ( transform.position.x, (((start + (Time.time*direction*mph))%distance)) - (distance/2)*direction, transform.position.z);

		}
	}

	public void slowDown(){
		slowed = true;
	}
	public void speedUp(){
		slowed = false;
	}

	public void hitDog(){
		//sr.sprite = hitDogSpr;
	}

    public void Begin() // called when the level becomes playable
    {

    }

    public void Pause()
    {

    }

    public void UnPause()
    {

    }
}
