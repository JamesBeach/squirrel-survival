using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
	private GameObject player;		//Public variable to store a reference to the player game object
	public static bool dead;
    public static bool upTree;

	private Camera camera;
	private Vector3 offset;			//Private variable to store the offset distance between the player and camera

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Squirrel");
        if (player == null)
        {
            Debug.Log("CameraController could not find a player squirrel in the scene!");
        }

        // center camera
        {
            Vector3 pos = this.transform.position;
            pos.x = player.transform.position.x;
            pos.y = player.transform.position.y;
            this.transform.position = pos;
        }

		dead = false;
        upTree = false;
        /* AR code below lifted from: http://gamedesigntheory.blogspot.com/2010/09/controlling-aspect-ratio-in-unity.html */
        // set the desired aspect ratio (the values in this example are
        // hard-coded for 16:9, but you could make them into public
        // variables instead so you can set them at design time)
        float targetaspect = 16.0f / 9.0f;

        // determine the game window's current aspect ratio
        float windowaspect = (float)Screen.width / (float)Screen.height;

        // current viewport height should be scaled by this amount
        float scaleheight = windowaspect / targetaspect;

        // obtain camera component so we can modify its viewport
        camera = GetComponent<Camera>();

        // if scaled height is less than current height, add letterbox
        if (scaleheight < 1.0f)
        {
            Rect rect = camera.rect;

            rect.width = 1.0f;
            rect.height = scaleheight;
            rect.x = 0;
            rect.y = (1.0f - scaleheight) / 2.0f;

            camera.rect = rect;
        }
        else // add pillarbox
        {
            float scalewidth = 1.0f / scaleheight;

            Rect rect = camera.rect;

            rect.width = scalewidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scalewidth) / 2.0f;
            rect.y = 0;

            camera.rect = rect;
        }

        //Calculate and store the offset value by getting the distance between the player's position and camera's position.
        offset = transform.position - player.transform.position;

	}
	
	// LateUpdate is called after Update each frame
	void LateUpdate () 
	{
        // Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
        if (!dead)
        {
            transform.position = player.transform.position + offset;
            if(upTree)
            {
                camera.orthographicSize = 5.5f;
            }
            else
            {
                camera.orthographicSize = 3.5f;
            }
        }
        else
        {
            if (camera.orthographicSize > .7)
            {
                camera.orthographicSize -= .005f;
            }
        }

        float step = 100f;
        float div = 1f / step;
        Vector3 pos = transform.position;
        pos.x = (int)(pos.x / div) * div;
        pos.y = (int)(pos.y / div) * div;
        transform.position = pos;
	}

	public static void deadSquirrel(){
		
		dead = true;
	}

    public static void climbTree()
    {
        upTree = true;
    }

    public static void descend()
    {
        upTree = false;
    }
}
