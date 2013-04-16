using UnityEngine;
using System.Collections;

public class Pollen : MonoBehaviour {
	
	// Exposed variables
	public int m_maxPollen = 100;
	
	// Private variables
	private int m_pollen = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	// Increase pollen
	public void AddPollen(int quantity)
	{
		m_pollen += quantity;
		
		// clamp to max pollen
		if (m_pollen > m_maxPollen)
			m_pollen = m_maxPollen;
		
		// Update readout
		GameObject.Find ("pollen_count").GetComponent<OTTextSprite>().text = ""+m_pollen;
	}
}
