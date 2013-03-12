// ************************************************************************ 
// File Name:   HealthBarScript.cs 
// Purpose:     Controls health bar display for mobs
// Project:		#PROJECTNAME#
// Author:      Sarah Herzog  
// Copyright: 	2013 Bound-Dare Studios
// ************************************************************************ 

// ************************************************************************ 
// Imports 
// ************************************************************************ 
using UnityEngine;
using System.Collections;

// ************************************************************************ 
// Class: HealthBarScript
// ************************************************************************ 
public class HealthBar : MonoBehaviour {

    // ********************************************************************
    // Private Data Members 
    // ********************************************************************
	public GameObject m_healthbar;
	public GameObject m_healthbarBackground;
	public float m_maxHP = 100f;
	public float m_width = 1f;
	public float m_height = 0.15f;
	public float m_positionX = 0f;
	public float m_positionY = 0.6f;
	private float m_currentHP;
	public float HP {
		get {
			return m_currentHP;
		}
		set {
			SetHP(value);
		}
	}
	private GameObject m_healthbarInstance;
	private GameObject m_healthbarBackgroundInstance;
	private OTSprite m_sprite;
	
    // ********************************************************************
    // Exposed Data Members 
    // ********************************************************************
	
    // ********************************************************************
    // Function:	Start()
	// Purpose:		Run when new instance of the object is created.
    // ********************************************************************
	void Start () {
		Debug.Log("Instantiating healthbar prefab...");
		
		// Background bar
		m_healthbarBackgroundInstance = (GameObject)Instantiate(m_healthbarBackground,transform.position,transform.rotation);
		m_healthbarBackgroundInstance.transform.parent = transform;
		m_sprite = m_healthbarBackgroundInstance.GetComponent<OTSprite>();
		m_sprite.size= new Vector2(m_width,m_height);
		m_sprite.position = new Vector2(m_positionY,m_positionX);
		
		m_healthbarInstance = (GameObject)Instantiate(m_healthbar,transform.position,transform.rotation);
		m_healthbarInstance.transform.parent = transform;
		m_sprite = m_healthbarInstance.GetComponent<OTSprite>();
		SetHP(m_maxHP);
	}
	
    // ********************************************************************
    // Function:	Start()
	// Purpose:		Called once per frame.
    // ********************************************************************
	void Update () {
	}
	public void SetHP(float HP) {
		if (HP > m_maxHP) HP = m_maxHP;
		if (HP < 0) HP = 0;
		m_currentHP = HP;
		m_sprite.size= new Vector2(m_width*(HP/m_maxHP),m_height);
		m_sprite.position = new Vector2(m_positionY,(m_width-m_sprite.size.x)/2+m_positionX);
		
	}
}
