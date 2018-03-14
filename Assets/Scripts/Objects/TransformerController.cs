using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformerController : MonoBehaviour {
    public Animator arcLeft, arcRight;
    public int arcProbabilityOneIn; // one in this many chance of an arc occurring on any particular FixedUpdate

    private System.Random rand;

	// Use this for initialization
	void Start () {
        rand = new System.Random((int)(transform.position.x + transform.position.y));
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate()
    {
        int fire = rand.Next(1, arcProbabilityOneIn + 1);
        if (fire == 1)
        {
            arcLeft.SetTrigger("fire");
        }

        fire = rand.Next(1, arcProbabilityOneIn + 1);
        if (fire == arcProbabilityOneIn)
        {
            arcRight.SetTrigger("fire");
        }
    }
}
