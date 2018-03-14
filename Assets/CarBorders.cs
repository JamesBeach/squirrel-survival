using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBorders : MonoBehaviour {

    public bool verticalBound;
    public float lowValue;
    public float highValue;

    public void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Car")
        {
            if(verticalBound)
            {
                if(Math.Abs(highValue - other.transform.position.y) < 2f)
                {
                    other.transform.position = new Vector2(other.transform.position.x, lowValue + 2f);
                }
                else if(Math.Abs(lowValue - other.transform.position.y) < 2f)
                {
                    other.transform.position = new Vector2(other.transform.position.x, highValue - 2f);
                }
                else
                {
                    Debug.Log("Something screwed up with the border");
                }
            }
            else
            {
                if (Math.Abs(lowValue - other.transform.position.x) < 2f)
                {
                    other.transform.position = new Vector2(highValue - 2f, other.transform.position.y);
                }
                else if (Math.Abs(highValue - other.transform.position.x) < 2f)
                {
                    other.transform.position = new Vector2(lowValue + 2f, other.transform.position.y);
                }
                else
                {
                    Debug.Log("Something screwed up with the border");
                }
            }
        }
    }
	
}
