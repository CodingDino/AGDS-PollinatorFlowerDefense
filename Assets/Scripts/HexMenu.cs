using UnityEngine;
using System.Collections;

public class HexMenu : MonoBehaviour {
	
	private static GameObject s_overlay = null;

	// Use this for initialization
	void Start () {
		if (!s_overlay) s_overlay = GameObject.Find ("Overlay");
	}
	
	// Update is called once per frame
	void Update () {
	
		// Check for touch
		if (OT.Touched(gameObject)) OnTouch ();
		
	}
	
	public void OnTouch()
	{
		// Debug Message
		Debug.Log ("Hex Click/touch detected.");
		
		// Activate overlay
		if (s_overlay != null && s_overlay.GetComponent<Overlay>() != null ) s_overlay.GetComponent<Overlay>().ActivateOverlay(gameObject);
		
	}
}
