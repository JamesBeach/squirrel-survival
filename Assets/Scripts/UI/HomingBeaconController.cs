using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingBeaconController : MonoBehaviour {
    private bool enabled = false;    // should the beacon be displayed right now

    // these parameters determine how far from the edge of the screen the beacon should appear
    public float topOffset;
    public float bottomOffset;
    public float leftOffset;
    public float rightOffset;

    public GameObject pointer;
    public GameObject house;
    public float width;

    private Transform hometreeLocation;
    private RectTransform rect;

    private Vector2 quadUp,
                    quadDown,
                    quadRight;
    private Vector2 pos;
    private Vector3 dir;

	// Use this for initialization
	void Start () {
        isEnabled = enabled;

        pointer.GetComponent<RectTransform>().sizeDelta = new Vector2(width, width);
        house.GetComponent<RectTransform>().sizeDelta = new Vector2(width, width);

        rect = this.GetComponent<RectTransform>();

        rect.sizeDelta = new Vector2(rect.sizeDelta.x - leftOffset - rightOffset, rect.sizeDelta.y - topOffset - bottomOffset);
        rect.anchoredPosition = new Vector2(rect.anchoredPosition.x + (leftOffset / 2) - (rightOffset / 2),
                                            rect.anchoredPosition.y - (topOffset / 2) + (bottomOffset / 2));

        // calculate our quadrants
        float a = Mathf.Atan(rect.rect.height / rect.rect.width);

        quadUp.x = a;
        quadUp.y = Mathf.PI - a;

        quadDown.x = (0 - Mathf.PI) + a;
        quadDown.y = 0 - a;

        quadRight.x = quadDown.y;
        quadRight.y = a;
	}
	
	void FixedUpdate () {
		if (enabled)
        {
            dir = hometreeLocation.position - this.transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x);

            // rotate the pointer
            pointer.transform.rotation = Quaternion.AngleAxis((angle * Mathf.Rad2Deg) - 90, Vector3.forward);

            // move the icons
            //float a = Mathf.Atan(rect.rect.height / rect.rect.width);
            //Debug.Log("height = " + rect.rect.height + "; width = " + rect.rect.width);

            if (angle >= quadUp.x && angle <= quadUp.y) // TOP QUAD
            {
                pos.x = rect.rect.height / (2 * Mathf.Tan(angle));
                pos.y = rect.rect.height / 2;
            }
            else if (angle <= quadDown.y && angle >= quadDown.x) // BOTTOM QUAD
            {
                pos.x = -1 * ((rect.rect.height / (2 * Mathf.Tan(angle))));
                pos.y = -1 * (rect.rect.height / 2);
            }
            else if (angle > quadRight.x && angle < quadRight.y) // RIGHT QUAD
            {
                pos.x = rect.rect.width / 2;
                pos.y = (rect.rect.width / 2) * Mathf.Tan(angle);
            }
            else // LEFT QUAD
            {
                pos.x = -1 * (rect.rect.width / 2);
                pos.y = -1 * ((rect.rect.width / 2) * Mathf.Tan(angle));
            }

            pointer.transform.localPosition = house.transform.localPosition = pos;
        }
	}

    public void setHometreeLocation(Transform loc)
    {
        hometreeLocation = loc;
    }

    public bool isEnabled
    {
        get
        {
            return enabled;
        }

        set
        {
            pointer.SetActive(value);
            house.SetActive(value);
            enabled = value;
        }
    }
}
