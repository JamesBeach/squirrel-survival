using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Dog behaviors:
///   1. Dogs chase the squirrel.
///   2. Dogs only give chase when all of these conditions are met:
///     a. The squirrel and the dog are both on screen. (Consider requiring even closer proximity.)
///     b. The dog has direct line of sight with the squirrel (the Euclidean line between them is unobstructed by environmental colliders)
///   3. Dogs first move vertically if possible, then horizontally. They do not move diagonally.
///   4. The position of a dog is constrained to the grid, same as the squirrel.
///   5. When line of sight with the squirrel is lost, the dog continues to the last place it saw the squirrel and resumes chasing
///         if line if sight is reestablished.
///   6. Dogs return (at a walking pace) to their original position when they fail to find the squirrel.
///   7. Dogs fail to find the squirrel when:
///     a. Dog and squirrel are no longer on the screen together.
///     b. Line of sight is lost and is not reestablished.
///   8. Line of sight is not reestablished if the squirrel hides (elevated, underground, cubby, etc.)
/// </summary>
public class DogController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
