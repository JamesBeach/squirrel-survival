using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class EditController : MonoBehaviour {
    public bool snapOn = true;
    public bool autoZOrderOn = true;
    public bool forcePrefabScale = false;

    public bool snapOnXAxis = true;
    public bool snapOnYAxis = true;

    public float snapValue = 0.5f;
    public float snapOffsetY = 0;
    public float snapOffsetX = 0;
    public float sortByOffsetY;

#if UNITY_EDITOR
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame!
	void Update () {
        if (forcePrefabScale)
        {
            transform.localScale = new Vector3(1,1,1);
        }

        if (snapOn)
        {
            float snapInverse = 1 / snapValue;

            float x, y;

            // if snapValue = .5, x = 1.45 -> snapInverse = 2 -> x*2 => 2.90 -> round 2.90 => 3 -> 3/2 => 1.5
            // so 1.45 to nearest .5 is 1.5
            if (snapOnXAxis)
                x = Mathf.Round((transform.position.x - snapOffsetX) * snapInverse) / snapInverse;
            else
                x = transform.position.x;

            if (snapOnYAxis)
                y = Mathf.Round((transform.position.y - snapOffsetY) * snapInverse) / snapInverse;
            else
                y = transform.position.y;
            // z = depth;  // depth from camera


            transform.position = new Vector2(x + snapOffsetX, y + snapOffsetY);
        }

        if (autoZOrderOn)
        {
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            sr.sortingOrder = (int)((transform.position.y * -10) + sortByOffsetY);
        }
    }
#endif
}
