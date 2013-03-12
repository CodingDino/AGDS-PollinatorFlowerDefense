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
	
	// Internal data members
	protected DateTime nextAttack = System.DateTime.Now;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		// TODO: Maintain a list of all enemies, the ones closest to the hive first, then each object
		// can walk through the list and select the first one within range.
		
		// Determine target
		// Get possible targets
		GameObject[] potentialTargets = null;
		potentialTargets = GameObject.FindGameObjectsWithTag("Ant");
		//if (potentialTargets != null) Debug.Log("Potential targets found: "+potentialTargets);
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
		if(target !=null)
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
}
