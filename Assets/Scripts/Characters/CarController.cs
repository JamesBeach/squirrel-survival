using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour, ICharacterController {
	public float mph = 15f;
	public Sprite hitDogSpr;
	public float distance;
	public float direction;

	private float start;
	private bool slowed;
	SpriteRenderer sr;

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
		start = transform.position.x;
		sr = GetComponent<SpriteRenderer>();
	}

	// Update is called once per frame
	void Update () {

		if (slowed) {
			transform.position = new Vector3 (transform.position.x+.02f*direction, transform.position.y, transform.position.z);
		} else {
			//modulo magic
			transform.position = new Vector3 (((start + (Time.time * direction * mph)) % distance) - (distance * direction) / 2, transform.position.y, transform.position.z);
		}

	}

    private void FixedUpdate()
    {
        // update sorting order
        sr.sortingOrder = (int)((transform.position.y + 0.05) * -10);
    }

    public void slowDown(){
		slowed = true;
	}
	public void speedUp(){
		slowed = false;
	}

	public void hitDog(){
		sr.sprite = hitDogSpr;
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
