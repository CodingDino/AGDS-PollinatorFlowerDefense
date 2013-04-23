using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class DialogLoss : MonoBehaviour {

	private bool m_active = false;
	private Health m_hive = null;
	private DateTime m_nextTouch = System.DateTime.Now;
	
	public bool IsActive()
	{
		return m_active;
	}

	// Use this for initialization
	void Start () {
		
		// Get hive
		m_hive = GameObject.Find("hive").GetComponent<Health>();
		
		// Overlay should be invisible to start
		gameObject.GetComponent<OTSprite>().visible = false;
		gameObject.GetComponent<OTSprite>().registerInput = false;
		
	}
	
	// Update is called once per frame
	void Update () {
		if (m_hive.HP <= 0) Activate ();
		
		// Check for touch
		if (OT.Touched(gameObject)) OnTouch ();
	}
	
	public void Activate()
	{
		// If already active, stop.
		DateTime now = System.DateTime.Now;
		if (m_active || now < m_nextTouch) return;
		
		// Set active
		m_active = true;
		
		// Set next touch
		m_nextTouch = now.AddMilliseconds(500);
		
		// Overlay should visible
		gameObject.GetComponent<OTSprite>().visible = true;
		gameObject.GetComponent<OTSprite>().registerInput = true;
	}
	
	
	public void OnTouch()
	{
		
		DateTime now = System.DateTime.Now;
		if (!m_active || now < m_nextTouch) return;
		
		// Set next touch
		m_nextTouch = now.AddMilliseconds(1000);
		
		// Debug Message
		Debug.Log ("DialogLoss Click/touch detected - Loading LevelSelect");
		
		Application.LoadLevel("LevelSelect"); 
	}
}
