using UnityEngine;
using System.Collections;

public class Hive : MonoBehaviour {
	
	// Exposed variables
	public int m_maxPollen = 100;
	public int m_maxSeeds = 100;
	
	// Private variables
	private int m_pollen = 0;
	private int m_seeds = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	// Take pollen if possible
	public bool UsePollen(int quantity)
	{
		if (quantity > m_pollen)
			return false;
		
		m_pollen -= quantity;
		
		// Update readout
		GameObject.Find ("pollen_count").GetComponent<OTTextSprite>().text = ""+m_pollen;
		
		return true;
	}
	
	// Take seeds if possible
	public bool UseSeeds(int quantity)
	{
		if (quantity > m_seeds)
			return false;
		
		m_seeds -= quantity;
		
		// Update readout
		GameObject.Find ("seed_count").GetComponent<OTTextSprite>().text = ""+m_seeds;
		
		return true;
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
	
	// Increase pollen
	public void AddSeeds(int quantity)
	{
		m_seeds += quantity;
		
		// clamp to max pollen
		if (m_seeds > m_maxSeeds)
			m_seeds = m_maxSeeds;
		
		// Update readout
		GameObject.Find ("seed_count").GetComponent<OTTextSprite>().text = ""+m_seeds;
	}
}
