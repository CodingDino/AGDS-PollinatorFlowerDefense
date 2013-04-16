using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Overlay : MonoBehaviour {
	
	private GameObject m_callingHex = null;
	private bool m_active = false;
	private DateTime m_nextTouch = System.DateTime.Now;
	
	public bool IsActive()
	{
		return m_active;
	}
	
	public GameObject GetCallingHex()
	{
		return m_callingHex;
	}

	// Use this for initialization
	void Start () {
		
		// Hide children
		OTSprite [] sprites = transform.GetComponentsInChildren<OTSprite>();
		for (int i=0; i < sprites.Length; ++i)
		{
			sprites[i].visible = false;
		}
		
		gameObject.GetComponent<OTSprite>().visible = false;
		
		// Overlay should be invisible to start
		gameObject.GetComponent<OTSprite>().visible = false;
		
	}
	
	// Update is called once per frame
	void Update () {
		
		// Check for touch
		if (OT.Touched(gameObject)) OnTouch ();
	
	}
	
	// Called when a hex is tapped.
	public void ActivateOverlay(GameObject hex)
	{
		// If already active, stop.
		DateTime now = System.DateTime.Now;
		if (m_active || now < m_nextTouch) return;
		
		// Set next touch
		m_nextTouch = now.AddMilliseconds(100);
		
		// Makes overlay visible
		gameObject.GetComponent<OTSprite>().visible = true;
		
		// Check if hex has a flower
		bool hasFlower = false, hasBee = false;
		if (hex.transform.childCount > 0) hasFlower = true;
		if (hasFlower && hex.transform.GetChild(0).transform.childCount > 0) hasBee = true;
		
		// Make children invisible if they should be
		for (int i=0; i < transform.childCount; ++i)
		{
			GameObject button = transform.GetChild(i).gameObject;
			// If there is a flower...
			if (hasFlower)
			{
				// If there is a bee...
				if (hasBee)
				{
					// Show upgrades for flowers and bees
					if (button.tag == "ButtonFlowerBase" || button.tag == "ButtonBeeBase")
						button.GetComponent<HexButton>().Deactivate();
					else
						button.GetComponent<HexButton>().Activate();
				}
				// If there is NOT a bee...
				else
				{
					// Show flower upgrades and base bees
					if (button.tag == "ButtonFlowerBase" || button.tag == "ButtonBeeUpgrade")
						button.GetComponent<HexButton>().Deactivate();
					else
						button.GetComponent<HexButton>().Activate();
				}
				
			}
			// If there is NOT a flower...
			else
			{
				// Show base flowers and no bees.
				if (button.tag != "ButtonFlowerBase")
					button.GetComponent<HexButton>().Deactivate();
				else
					button.GetComponent<HexButton>().Activate();
			}
		}
		
		// Sets calling hex
		m_callingHex = hex;
		
		m_active = true;
	}
	
	// Called on touch
	public void OnTouch()
	{
		DateTime now = System.DateTime.Now;
		if (!m_active || now < m_nextTouch) return;
		
		// Set next touch
		m_nextTouch = now.AddMilliseconds(100);
		
		// Debug Message
		Debug.Log ("Overlay Click/touch detected.");
		
		DeactivateOverlay();
	}
	
	private void DeactivateOverlay()
	{
		// Makes overlay invisible
		gameObject.GetComponent<OTSprite>().visible = false;
		
		// Resets calling hex
		m_callingHex = null;
		
		m_active = false;
		
		// Deactivate buttons
		for (int i=0; i < transform.childCount; ++i)
		{
			GameObject button = transform.GetChild(i).gameObject;
			button.GetComponent<HexButton>().Deactivate();
		}
		
	}
}
