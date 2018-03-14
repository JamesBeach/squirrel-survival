using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogAnimationController : MonoBehaviour {

	private Animator animator;

	// Use this for initialization
	void Start () {
		//Get a component reference to the Player's animator component
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Car")
		{
			this.transform.parent.gameObject.SendMessage ("setStop");
			other.gameObject.SendMessage ("hitDog");
			this.gameObject.SetActive (false);
		}
	}


	void killSquirrel(){
		animator.SetTrigger ("dogKill");
	}

//	public void slowDown(){
//		animator.speed = animator.speed / 3f;
//	}
//	public void speedUp(){
//		animator.speed = animator.speed * 3f;
//	}
}
