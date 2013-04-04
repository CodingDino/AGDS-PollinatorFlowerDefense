using UnityEngine;
using System.Collections;
using System;

public class BeeBasic : MonoBehaviour {
	
	// Design data members
	public int health = 25;
	public int damage = 20;
	public float speed = 0.5f;
	public int defense = 1;
	public int range = 1;
	public float m_pollenSpeed = 1.0f;
	
	// Internal data members
	protected DateTime m_nextPollen = System.DateTime.Now;
	protected DateTime nextAttack = System.DateTime.Now;
	protected OTSprite m_sprite;
	protected Vector3 m_dragStart = new Vector3(0,0,0);
	protected Transform m_flower;
	protected bool m_dragging = false;

	// Use this for initialization
	void Start () {
		
		m_sprite = GetComponent<OTSprite>();
		
		// hookup our drag events
		m_sprite.onDragStart = DragStart;
		m_sprite.onDragEnd = DragEnd;
		m_sprite.onDragging = Dragging;
		
		// Set flower pointer
		m_flower = transform.parent;
	
	}
	
	// Update is called once per frame
	void Update () {
		
		// Attack if an enemy is near
		Attack ();
		
		// Generate pollen for the hive
		GeneratePollen ();
		
	}
	
	// Check for nearby enemies and attack
	private void Attack()
	{
	
		// TODO: Maintain a list of all enemies, the ones closest to the hive first, then each object
		// can walk through the list and select the first one within range.
		
		// Determine target
		// Get possible targets
		GameObject[] potentialTargets = null;
		potentialTargets = GameObject.FindGameObjectsWithTag("Ant");
		// if (potentialTargets != null) Debug.Log("Potential targets found: "+potentialTargets);
		GameObject target = null;
		//float minDistance = 1000;
		// For each potential target
        foreach (GameObject potentialTarget in potentialTargets) {
			float distance = (potentialTarget.transform.position-transform.position).sqrMagnitude;
			if (distance < range) 
			{
				// Choose first target in range
				target = potentialTarget;
				break;
			}
		}
		
		// Attack target if it is time.
		if(target !=null && !m_dragging)
		{
			DateTime now = System.DateTime.Now;
			if (now >= nextAttack)
			{
				nextAttack = (now).AddMilliseconds(speed*1000);
				Debug.Log("Attacking target: "+target);
				Health targetHealth = target.GetComponent<Health>();
				targetHealth.SetHP(targetHealth.HP-damage);
				//Debug.Log("current time: "+(now).TimeOfDay);
				//Debug.Log("Next attack available: "+nextAttack.TimeOfDay);
			}
		}
	}
	
	// Generate Pollen
	private void GeneratePollen()
	{
		// Make sure bee is on the flower
		if (m_dragging) return;
		
		// Generate pollen if it is time
		DateTime now = System.DateTime.Now;
		if (now >= m_nextPollen)
		{
			// Determine when next pollen will be generated
			m_nextPollen = (now).AddMilliseconds(m_pollenSpeed*1000);
			// Get hive and generate pollen
			GameObject hive = GameObject.FindGameObjectWithTag("Hive");
			hive.GetComponent<Pollen>().AddPollen(1);
		}
	}
	
	// Called when the bee is dropped on a new flower
	public void DroppedOnFlower(GameObject flower)
	{
		// Set the bee's parent to the new flower
		transform.parent = flower.transform;
		// Record this flower for future dragging
		m_flower = flower.transform;	
		// Set the bee's local position to 0
		transform.localPosition = new Vector3(0,0,transform.localPosition.z);	
	}
	
	void DragStart(OTObject owner)
	{
		Debug.Log("Dragging started.");
		m_dragging = true;
		transform.parent = null;
		m_dragStart = transform.position;
		
		
	}
	
	
	void DragEnd(OTObject owner)
	{
		Debug.Log("Dragging ended.");
		m_dragging = false;
		transform.parent = m_flower;
		transform.localPosition = new Vector3(0,0,transform.localPosition.z);
	}
	
	
	void Dragging(OTObject owner)
	{
	}
}
