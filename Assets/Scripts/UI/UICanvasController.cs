using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICanvasController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Canvas canvas = GetComponent<Canvas>();
        Camera mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        if (mainCamera == null)
        {
            Debug.Log("UICanvasController could not find a MainCamera in the scene! Some components, like the homing beacon, may not work as intended.");
        }
        else
        {
            canvas.worldCamera = mainCamera;
        }
	}
}
