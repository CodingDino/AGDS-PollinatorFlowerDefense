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
	public float m_seedSpeed = 1.0f;
	
	// Internal data members
	protected DateTime m_nextPollen = System.DateTime.Now;
	protected DateTime m_nextSeed = System.DateTime.Now;
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
		GenerateSeeds ();
		
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
			// Turn to look at target
		    Vector3 dir = (target.transform.position-transform.position);
			gameObject.GetComponent<OTSprite>().rotation = Mathf.Atan2(dir.y, dir.x)*(180.0f/Mathf.PI);
			
			DateTime now = System.DateTime.Now;
			if (now >= nextAttack)
			{
				nextAttack = (now).AddMilliseconds(speed*1000);
				Debug.Log("Attacking target: "+target);
				
				// REMOVED: Directly damage target
				//Health targetHealth = target.GetComponent<Health>();
				//targetHealth.SetHP(targetHealth.HP-damage);
				
				
				// Spawn a stinger
				GameObject stinger = (GameObject)Instantiate(
					(GameObject)Resources.Load("stinger", typeof(GameObject)),
					transform.position, 
					Quaternion.identity);
				
				// Set the stinger's parameters
				stinger.GetComponent<OTSprite>().position = new Vector2(transform.position.x,transform.position.y);
				stinger.GetComponent<Stinger>().m_target = target;
				stinger.GetComponent<Stinger>().m_damage = damage;
				
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
			hive.GetComponent<Hive>().AddPollen(1);
		}
	}
	
	// Generate Seeds
	private void GenerateSeeds()
	{
		// Make sure bee is on the flower
		if (m_dragging) return;
		
		// Generate pollen if it is time
		DateTime now = System.DateTime.Now;
		if (now >= m_nextSeed)
		{
			// Determine when next pollen will be generated
			m_nextSeed = (now).AddMilliseconds(m_seedSpeed*1000);
			// Get hive and generate pollen
			GameObject hive = GameObject.FindGameObjectWithTag("Hive");
			hive.GetComponent<Hive>().AddSeeds(1);
		}
	}
	
	// Called when the bee is dropped on a new flower
	public void DroppedOnFlower(GameObject flower)
	{
		Debug.Log("Dropped on a flower.");
		// Set the bee's parent to the new flower
		transform.parent = flower.transform;
		// Record this flower for future dragging
		m_flower = flower.transform;	
		// Set the bee's local position to 0 (snap to flower)
		transform.localPosition = new Vector3(0,0,transform.localPosition.z);	
	}
	
	void DragStart(OTObject owner)
	{
		Debug.Log("Dragging started.");
		m_dragging = true;
		transform.parent = null; // remove parent
		m_dragStart = transform.position; // record starting position
	}
	
	
	void DragEnd(OTObject owner)
	{
		Debug.Log("Dragging ended.");
		
		GameObject[] flowers = null;
		flowers = GameObject.FindGameObjectsWithTag("Flower");
		if (flowers != null && flowers.Length > 0)
		{
			Debug.Log("Flower list found, length = "+flowers.Length);
		}
		else
		{
			Debug.Log("No flowers found.");
		}
		Vector3 beePosition = transform.position;
		float min_distance = 10000.0f;
		GameObject new_parent = null;
		
		// Determine closest flower
		for (int j=0; j < flowers.Length ; ++j)
		{
			if (Vector2.Distance(beePosition,flowers[j].transform.position) < min_distance)
			{
				min_distance = Vector2.Distance(beePosition,flowers[j].transform.position);
				new_parent = flowers[j];
			}
		}
		if (new_parent != null)
		{
			Debug.Log ("Potential parent found at distance = " + min_distance);
		}
		else
		{
			Debug.Log ("No potential parents found.");
		}
		
		// Is the bee actually on that flower?
		if (min_distance < 100)
		{
			// Does the flower already have a bee on it?
			if (new_parent.transform.childCount == 0)
			{
				Debug.Log("New flower found! Dropping to it.");
				// Record this flower as our new home
				m_flower = new_parent.transform;
			}
			else
			{
				Debug.Log ("Flower already has a bee, snapping back to original position.");
			}
		}
		else
		{
			Debug.Log ("No nearby flower found, snapping back to original position.");
		}
		
		m_dragging = false;
		transform.parent = m_flower; // Set parent to the flower - if no new flower was set, this will be the previous flower parent.
		transform.localPosition = new Vector3(0,0,transform.localPosition.z); // snap to center of flower.
		
	}
	
	
	void Dragging(OTObject owner)
	{
	}
}
