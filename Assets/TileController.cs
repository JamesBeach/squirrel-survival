using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour {

    public bool up;
    public bool down;
    public bool left;
    public bool right;
    public bool random;

    private int numDir = 0;
    private string approach;
    private string[] validDir;

    public void Start()
    {
        bool[] directions = { up, down, left, right };
        foreach (bool dir in directions)
        {
            if(dir)
            {
                numDir++;
            }
        }
        
        if (numDir >= 3 && !random)
        {
            Debug.LogError("Too many directions for non-random tile, disabling tile");
            gameObject.SetActive(false);
        }
        else
        {
            validDir = new string[numDir];
            if(directions[3])
            {
                validDir[--numDir] = "right";
            }
            if(directions[2])
            {
                validDir[--numDir] = "left";
            }
            if(directions[1])
            {
                validDir[--numDir] = "down";
            }
            if(directions[0])
            {
                validDir[--numDir] = "up";
            }
        }
    }
	
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Mower" || other.gameObject.tag == "Tractor")
        {
            if (validDir.Length == 1)
            {
                other.SendMessage("Turn", validDir[0]);
                return;
            }

            if (other.transform.position.y > transform.position.y)
            {
                approach = "up";
            }
            else if (other.transform.position.y < transform.position.y)
            {
                approach = "down";
            }
            else if (other.transform.position.x < transform.position.x)
            {
                approach = "left";
            }
            else if (other.transform.position.x > transform.position.x)
            {
                approach = "right";
            }

            if (!random)
            {
                if (validDir[0] == approach)
                {
                    other.SendMessage("Turn", validDir[1]);
                }
                else if (validDir[1] == approach)
                {
                    other.SendMessage("Turn", validDir[0]);
                }
                else
                {
                    Debug.LogError("Other approached from invalid direction");
                }
            }
            else
            {
                while (true)
                {
                    string temp = validDir[Random.Range(0, validDir.Length - 1)];
                    if (temp != approach)
                    {
                        other.SendMessage("Turn", temp);
                        return;
                    }
                }
            }
        }
    }
}
