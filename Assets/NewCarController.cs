using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewCarController : MonoBehaviour, ICharacterController
{

    public float speed;
    public Sprite hitDogSpr;
    public bool vertical;
    public bool positiveDirection;
    private Vector2 normVel;
    private bool slowed = false;

    SpriteRenderer sr;
    Rigidbody2D rb;
    BoxCollider2D coll;

    private void Awake()
    {
        // register with level controller
        LevelController lvlctrl;
        if ((lvlctrl = FindObjectOfType<LevelController>()) != null)
        {
            lvlctrl.RegisterCharacter(this);
        }
        else
        {
            Debug.Log(this.GetType().Name + " could not find a LevelController!");
        }
    }

    // Use this for initialization
    void Start () {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        if(coll == null)
        {
            Debug.Log("We got problems");
        }
        if(!vertical)
        {
            if(positiveDirection)
            {
                normVel = new Vector2(speed * Time.deltaTime * 10, 0f);
            }
            else
            {
                normVel = new Vector2(-speed * Time.deltaTime * 10, 0f);
            }
        }
        else
        {
            if(positiveDirection)
            {
                normVel = new Vector2(0f, speed * Time.deltaTime * 10);
            }
            else
            {
                normVel = new Vector2(0f, -speed * Time.deltaTime * 10);
            }
        }
        rb.velocity = normVel;
    }

    private void FixedUpdate()
    {
        if (rb.velocity.magnitude < normVel.magnitude)
        {
            if (!slowed)
            {
                rb.velocity = normVel;
            }
            else
            {
                rb.velocity = new Vector2(normVel.x / 2, normVel.y / 2);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "BorderTree" || other.gameObject.tag == "Car")
        {
            Physics2D.IgnoreCollision(other.collider, coll);
        }
    }
    public void slowDown()
    {
        slowed = true;
        rb.velocity = new Vector2(normVel.x / 2, normVel.y / 2);
    }

    public void speedUp()
    {
        slowed = false;
        rb.velocity = normVel;
            
    }

    public void hitDog()
    {
        sr.sprite = hitDogSpr;
    }

    public void Begin() // called when the level becomes playable
    {

    }

    public void Pause()
    {

    }

    public void UnPause()
    {

    }
}
