using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbTreeController : MonoBehaviour {
    public SpriteRenderer squirrel;

	// Use this for initialization
	void Start () {
        squirrel.enabled = false;
	}

    public void Climbed()
    {
        squirrel.enabled = true;
    }

    public void Unclimbed()
    {
        squirrel.enabled = false;
    }
}
