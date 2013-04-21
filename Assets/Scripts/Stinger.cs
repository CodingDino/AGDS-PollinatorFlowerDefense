using UnityEngine;
using System.Collections;

public class Stinger : MonoBehaviour {
	
	// Exposed Variables
	public float m_speed = 10.0f;
	public int m_damage = 10;
	public GameObject m_target = null;
    public float m_hitDistance = 0.5f;
	
	// Private variables
    //private CharacterController m_controller;
	
	// Use this for initialization
	void Start () {
        //m_controller = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
		// If our target doesn't exist, destroy self
		if (!m_target) 
		{
			Destroy(gameObject);
			return;
		}
		
		// Go in the direction of the target
        Vector3 dir = (m_target.transform.position-transform.position).normalized;
        dir *= m_speed * Time.fixedDeltaTime;
        //m_controller.Move (dir);
		gameObject.GetComponent<OTSprite>().position = new Vector2(transform.position.x+dir.x,transform.position.y+dir.y);
		gameObject.GetComponent<OTSprite>().rotation = Mathf.Atan2(dir.y, dir.x)*(180.0f/Mathf.PI);
		
		// If we've hit the target, deal damage and destroy self
        if (Vector2.Distance (transform.position,m_target.transform.position) < m_hitDistance) {
			m_target.GetComponent<Health>().SetHP(m_target.GetComponent<Health>().HP-m_damage);
			m_target = null;
			Destroy(gameObject);
            return;
        }
		
	}
}
