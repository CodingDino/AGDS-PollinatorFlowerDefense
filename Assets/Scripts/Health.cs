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
public class Health : MonoBehaviour {

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
	private OTSprite m_spriteBackground;
	
    // ********************************************************************
    // Exposed Data Members 
    // ********************************************************************
	
    // ********************************************************************
    // Function:	Awake()
	// Purpose:		Run when new instance of the object is created.
    // ********************************************************************
	void Awake () {
		if (!m_healthbarBackground) m_healthbarBackground =  (GameObject)Resources.Load("healthbar-background", typeof(GameObject));
		if (!m_healthbar) m_healthbar = (GameObject)Resources.Load("healthbar", typeof(GameObject));
	}
	
    // ********************************************************************
    // Function:	Start()
	// Purpose:		Run when new instance of the object is created.
    // ********************************************************************
	void Start () {
		Debug.Log("Instantiating healthbar prefab for: "+gameObject);
		
		// Background bar		
		m_healthbarBackgroundInstance = (GameObject)Instantiate(m_healthbarBackground,transform.position, Quaternion.identity);
		Debug.Log ("Successfully created instance: "+m_healthbarBackgroundInstance);
		//m_healthbarBackgroundInstance.transform.parent = transform;
		m_spriteBackground = m_healthbarBackgroundInstance.GetComponent<OTSprite>();
		m_spriteBackground.size= new Vector2(m_width,m_height);
		
		// Health bar		
		m_healthbarInstance = (GameObject)Instantiate(m_healthbar,transform.position, Quaternion.identity);
		Debug.Log ("Successfully created instance: "+m_healthbarInstance);
		//m_healthbarInstance.transform.parent = transform;
		m_sprite = m_healthbarInstance.GetComponent<OTSprite>();
		SetHP(m_maxHP);
	}
	
    // ********************************************************************
    // Function:	LateUpdate()
	// Purpose:		Called once per frame, after Update() operations.
    // ********************************************************************
	void LateUpdate () {
		m_sprite.position = new Vector2(transform.position.x+m_positionX-(m_width-m_sprite.size.x)/2,transform.position.y+m_positionY);
		m_spriteBackground.position = new Vector2(transform.position.x+m_positionX,transform.position.y+m_positionY);
	}
	
    // ********************************************************************
    // Function:	SetHP()
	// Purpose:		Updates the HP to the supplied value.
    // ********************************************************************
	public void SetHP(float HP) {
		if (HP > m_maxHP) HP = m_maxHP;
		if (HP < 0) HP = 0;
		m_currentHP = HP;
		m_sprite.size= new Vector2(m_width*(HP/m_maxHP),m_height);
		
		if(HP <= 0)
		{
			Kill();
		}
	}
	
    // ********************************************************************
    // Function:	Kill()
	// Purpose:		Kills the ant and it's health bars
    // ********************************************************************
	public void Kill() {
		Destroy (m_healthbarInstance);
		Destroy (m_healthbarBackgroundInstance);
		Destroy(gameObject);
	}
}
