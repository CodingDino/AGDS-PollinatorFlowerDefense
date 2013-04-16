using UnityEngine;
using System.Collections;

public class HexButton : MonoBehaviour {
	
	public GameObject m_spawnTarget;
	public int m_cost = 10;
	
	
	private GameObject s_overlay = null;
	private bool m_active = false;

	// Use this for initialization
	void Start () {
		if(!s_overlay) s_overlay = GameObject.Find ("Overlay");
	}
	
	// Update is called once per frame
	void Update () {
	
		// Check for touch
		if (OT.Touched(gameObject)) OnTouch ();
	
	}
	
	public void Activate()
	{
		gameObject.GetComponent<OTSprite>().visible = true;
		m_active = true;
	}
	
	public void Deactivate()
	{
		gameObject.GetComponent<OTSprite>().visible = false;
		m_active = false;
	}
	
	public void OnTouch()
	{
		// Debug Message
		Debug.Log ("HexButton Click/touch detected.");
		
		// Make sure we're active
		if (!m_active) return;
		
		// Make sure we have the pollen to pay for this
		GameObject hive = GameObject.FindGameObjectWithTag("Hive");
		if(!hive.GetComponent<Pollen>().UsePollen(m_cost)) return;
		
		// Get the calling hex
		GameObject hex = s_overlay.GetComponent<Overlay>().GetCallingHex();
		
		// Create the new object
		GameObject new_object = (GameObject)Instantiate(m_spawnTarget,
			hex.transform.position, 
			Quaternion.identity);
		
		// Drop the new object to the flower or hex.
		if (new_object.tag == "Flower")
			new_object.transform.parent = hex.transform;
		if (new_object.tag == "Bee")
			new_object.transform.parent = hex.transform.GetChild(0);
		
	}
}
