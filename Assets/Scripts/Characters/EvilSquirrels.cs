using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameManagement;

public class EvilSquirrels : MonoBehaviour, ICharacterController {
    private const float V_ANGLE = 20; // angle, top and bottom, under which squirrels will assume the vertical animation
    private const float DIV_1 = (180 - V_ANGLE) / 2;
    private const float DIV_2 = DIV_1 + V_ANGLE;

    public float speed;

    private static List<GameObject> _nuts;

    private Vector3 startLoc;
    private Vector3 endLoc;
    private int num;
    private float step;
    private string acornName;
    private GameObject acorn;
    private bool arrived;
	private Animator animator;
    private SpriteRenderer sr;

    private static List<GameObject> nuts
    {
        get
        {
            if (_nuts == null)
            {
                _nuts = new List<GameObject>();
                _nuts.AddRange(GameObject.FindGameObjectsWithTag("Nut"));
            }
            return _nuts;
        }
    }

    public static void AddNut(GameObject nut)
    {
        nuts.Add(nut);
    }

    private void Awake()
    {
        // register with LevelController
        LevelController lvlctrl;
        if ((lvlctrl = FindObjectOfType<LevelController>()) != null)
        {
            lvlctrl.RegisterCharacter(this);
        }
        else
        {
            Debug.Log(this.GetType().Name + " could not find a LevelController!");
        }

        while (acorn == null)
        {
            if (nuts.Count == 0)
            {
                Destroy(this.gameObject);
                return;
            }
            acorn = nuts[Random.Range(0, nuts.Count - 1)];
            nuts.Remove(acorn);
        }
    }

    // Use this for initialization
    void Start()
    {
		animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        startLoc = transform.position;
        endLoc = acorn.transform.position;

        arrived = false;

        UpdateAnimation(transform.position, acorn.transform.position);

        step = speed * Time.deltaTime;
    }
        // Update is called once per frame
    void Update () {
        
        if (!arrived)
        {
            transform.position = Vector3.MoveTowards(transform.position, endLoc, step);
            if(transform.position == endLoc)
            {
                Destroy(acorn);
                UpdateAnimation(transform.position, startLoc);
                arrived = true;
            }
        } else
        {
            transform.position = Vector3.MoveTowards(transform.position, startLoc, step);
            if(transform.position == startLoc)
            {
                Destroy(gameObject);
            }
        }
    }

    private void FixedUpdate()
    {
        // update sorting order
        sr.sortingOrder = (int)((transform.position.y + 0.05) * -10);
    }

    private void UpdateAnimation(Vector2 from, Vector2 to)
    {
        Cardinals dir = Cardinals.NONE;
        Vector2 difference = to - from;
        int sign = (from.y < to.y) ? 1 : -1;
        float angle = Vector2.Angle(Vector2.right, difference) * sign;

        if (angle < DIV_1 && angle > DIV_1 * -1)
        {
            dir = Cardinals.EAST;
        }
        else if (angle > DIV_2 || angle < DIV_2 * -1)
        {
            dir = Cardinals.WEST;
        }
        else
        {
            dir = (angle > 0) ? Cardinals.NORTH : Cardinals.SOUTH;
        }

        animator.ResetTrigger("SquirrelUp");
        animator.ResetTrigger("SquirrelDown");
        animator.ResetTrigger("SquirrelLeft");
        animator.ResetTrigger("SquirrelRight");
        switch (dir)
        {
            case Cardinals.NORTH:
                animator.SetTrigger("SquirrelUp");
                break;
            case Cardinals.SOUTH:
                animator.SetTrigger("SquirrelDown");
                break;
            case Cardinals.EAST:
                animator.SetTrigger("SquirrelRight");
                break;
            case Cardinals.WEST:
                animator.SetTrigger("SquirrelLeft");
                break;
        }
    }

    public void Begin()
    {

    }

    public void Pause()
    {

    }

    public void UnPause()
    {

    }
}
