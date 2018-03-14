using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if (UNITY_ANDROID || UNITY_IPHONE)
	using UnityEngine.Advertisements;
#endif

public class AdsController : MonoBehaviour {

	private int timesPlayed;

	// Use this for initialization
	void Start () {
		timesPlayed = 0;
	}
	
	public int getTimesPlayed(){
		return timesPlayed;
	}

	public void incrementTimesPlayed(){
		timesPlayed++;
	}

	public void showAd(){
		#if (UNITY_ANDROID || UNITY_IPHONE)
			Advertisement.Show ();
		#endif

	}
}
