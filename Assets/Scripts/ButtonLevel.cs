using UnityEngine;
using System.Collections;

public class ButtonLevel : MonoBehaviour {
	
	// Exposed data members
	public string m_level = "01";

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
		Debug.Log ("ButtonLevel Click/touch detected - Loading Level_"+m_level);
		
		Application.LoadLevel("Level_"+m_level); 
	}
}
