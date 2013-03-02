using UnityEngine;
using System.Collections;

public class RadialMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
		// Check for click
		if (OT.Clicked(gameObject)) OnTouch ();
		// Check for touch
		if (OT.Touched(gameObject)) OnTouch ();
		
	}
	
	
	public void OnTouch()
	{
		// Debug Message
		Debug.Log ("Click/touch detected.");
		
	}
}
