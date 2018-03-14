using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class climbTree : MonoBehaviour {
	public Sprite occupied;
	private Sprite vacant;
	SpriteRenderer sr;

	// Use this for initialization
	void Start () {
		sr = GetComponent<SpriteRenderer>();
		vacant = sr.sprite;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void Climbed(){
		sr.sprite = occupied;
		//transform.localScale = transform.localScale / 7f;
	}

	public void Unclimbed(){
		sr.sprite = vacant;
		//transform.localScale = transform.localScale * 7f;
	}
}
