using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Overlay : MonoBehaviour {
	
	private GameObject m_callingHex = null;
	private bool m_activating = false;
	private bool m_deactivating = false;
	private bool m_active = false;
	private DateTime m_nextTouch = System.DateTime.Now;
	private DateTime m_nextActive = System.DateTime.Now;
	
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
		
		// Activate or deactivate as needed
		DateTime now = System.DateTime.Now;
		if (m_activating && now >= m_nextActive) ActivateAfterDelay();
		if (m_deactivating && now >= m_nextActive) DeactivateAfterDelay();
		
		// Check for touch
		if (OT.Touched(gameObject)) OnTouch ();
	
	}
	
	// Called when a hex is tapped.
	public void ActivateOverlay(GameObject hex)
	{
		// If already active, stop.
		DateTime now = System.DateTime.Now;
		if (m_active || GameObject.Find("Dialog-Loss").GetComponent<DialogLoss>().IsActive() || now < m_nextTouch) return;
		
		// Check if hex has a flower
		bool hasFlower = false, hasBee = false;
		if (hex.transform.childCount > 0) hasFlower = true;
		if (hasFlower && hex.transform.GetChild(0).transform.childCount > 0) hasBee = true;
		// For now, exit if there is a bee
		if (hasBee) return;
		
		// Set next touch
		m_nextTouch = now.AddMilliseconds(100);
		
		// Set activating for next frame
		m_activating = true;
		m_nextActive = now.AddMilliseconds(50);
		
		// Sets calling hex
		m_callingHex = hex;
	}
	private void ActivateAfterDelay()
	{
		m_activating = false;
		
		// Check if hex has a flower
		bool hasFlower = false, hasBee = false;
		if (m_callingHex.transform.childCount > 0) hasFlower = true;
		if (hasFlower && m_callingHex.transform.GetChild(0).transform.childCount > 0) hasBee = true;
		
		// Makes overlay visible
		gameObject.GetComponent<OTSprite>().visible = true;
		Debug.Log("Overlay input: allowed.");
		gameObject.GetComponent<OTSprite>().registerInput = true;
		
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
	
	public void DeactivateOverlay()
	{
		m_deactivating = true;
		DateTime now = System.DateTime.Now;
		m_nextActive = now.AddMilliseconds(50);
	}
	private void DeactivateAfterDelay()
	{
		m_deactivating = false;
		
		// Makes overlay invisible
		gameObject.GetComponent<OTSprite>().visible = false;
		Debug.Log("Overlay input: forbidden.");
		gameObject.GetComponent<OTSprite>().registerInput = false;
			
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
