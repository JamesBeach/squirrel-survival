using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Dog : MonoBehaviour {

//	private GameObject squirrel;
	public float speed;
	public CampaignLevelController lvlctrl;
    public AudioSource dog_sound;

	private bool squirrelAlive;
	private bool gameStarted; 
	private bool firstChase;

	private Animator animator;
    private SpriteRenderer sr;
	//private Vector3 previousPosition;

	public float repathRate = .5f;
	private float lastRepath = -9999;

	//a* stuff
	private Transform target;
	private Seeker seeker; 
	public Path path;
	public float nextWaypointDistance = 3;
	private int currentWaypoint = 0;
	private Vector3 starting;

	// Use this for initialization
	void Start () {
        target = GameObject.FindGameObjectWithTag("Squirrel").transform;

		squirrelAlive = true;
		gameStarted = false;

		//Get a component reference to the Player's animator component
		animator = GetComponent<Animator>();

        sr = GetComponent<SpriteRenderer>();

		//a* stuff
		seeker = GetComponent<Seeker>();
		firstChase = true;
		starting = transform.position;
		//previousPosition = starting;
	}
	
	// Update is called once per frame
	void Update () {
		if (seeker.isActiveAndEnabled && SquirrelController.inTree) {
			currentWaypoint++;
			seeker.StartPath (transform.position, starting, onPathComplete);
		}
		if (squirrelAlive && gameStarted) {

			if (Vector2.Distance (target.transform.position, transform.position) < 8f) {
				if (firstChase) {
                    GetComponent<AudioSource>().Play();
					seeker.StartPath (transform.position, target.position, onPathComplete);
					firstChase = false;
				} else {
					if (Time.time - lastRepath > repathRate && seeker.IsDone ()) {
						lastRepath = Time.time + Random.value * repathRate * .5f;
						seeker.StartPath (transform.position, target.position, onPathComplete);
					}
				}

				if (path == null) {
					//We have no path to move after yet
					animator.SetTrigger ("dogIdle");
					return;
				}

				if (currentWaypoint >= path.vectorPath.Count) {
					currentWaypoint++;
					seeker.StartPath (transform.position, target.position, onPathComplete);
					return;
				}

				//Direction to the next waypoint
				Vector3 dir = (path.vectorPath [currentWaypoint] - transform.position).normalized;
				dir *= speed * Time.fixedDeltaTime;


				if (dir.x > 0) {
					animator.SetTrigger ("dogRight");
				} else if (dir.x < 0) {
					animator.SetTrigger ("dogLeft");
				}

				this.gameObject.transform.Translate (dir);

				//Check if we are close enough to the next waypoint
				//If we are, proceed to follow the next waypoint
				if (Vector3.Distance (transform.position, path.vectorPath [currentWaypoint]) < nextWaypointDistance) {
					currentWaypoint++;
					return;
				}

			} else {
				animator.SetTrigger ("dogIdle");
			}
				
		} 
	}

    private void FixedUpdate()
    {
        // update sorting order
        sr.sortingOrder = (int)((transform.position.y + 0.05) * -10);
    }

    public void onPathComplete(Path p){
		if (!p.error) {
			path = p;
			currentWaypoint = 0;
		}
	}

	void setGo(){
		gameStarted = true;
	}

	void setStop(){
		gameStarted = false;
	}

	void killSquirrel(){
		squirrelAlive = false;
		animator.SetTrigger ("dogKill");
	}

	public void slowDown(){
		speed = speed / 3f;
		animator.speed = animator.speed / 3f;
	}
	public void speedUp(){
		speed = speed * 3f;
		animator.speed = animator.speed * 3f;
	}
		
}
