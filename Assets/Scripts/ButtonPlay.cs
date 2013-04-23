using UnityEngine;
using System.Collections;

public class ButtonPlay : MonoBehaviour {

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
		Debug.Log ("ButtonPlay Click/touch detected.");
		
		Application.LoadLevel("LevelSelect"); 
	}
}
