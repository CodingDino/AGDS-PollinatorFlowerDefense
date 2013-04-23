using UnityEngine;
using System.Collections;

public class ButtonExit : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
		// Check for touch
		if (OT.Touched(gameObject)) OnTouch ();
		
	}
	
	public void OnTouch()
	{
		// Debug Message
		Debug.Log ("ButtonExit Click/touch detected.");
		
		Application.Quit();
	}
}
