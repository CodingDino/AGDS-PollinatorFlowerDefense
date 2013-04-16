using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HexGrid : MonoBehaviour {
	
	// Exposed data members
	public int m_numRows = 17;
	public int m_numColumns = 5;
	
	// Private data members
	public GameObject [][] m_hexes;

	// Use this for initialization
	void Start () {
		
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	// Procedurally generate hexes
	public void CreateHexes() {
		
		// Load Prefab
		GameObject hex = (GameObject)Resources.Load("hex", typeof(GameObject));
		
		// Calculate spacing
		float spacingX = (-1)*hex.transform.localScale.x*(3.1f/2.1f);
		float spacingY = hex.transform.localScale.y/2;
		
		// Calculate starting offset
		float startingOffsetY = (-1)*((m_numRows-1)*spacingY/2);
		
		// Create hex objects
		// Create column
		m_hexes = new GameObject [m_numRows][];
		for (int y = 0; y < m_numRows; ++y)
		{
			// Set up number of columns for this row
			int columns = m_numColumns;
			// Determine if this is an odd or even row
			// True if even (0,2,4, etc)
			if (y%2 == 1)
			{
				--columns;
			}
			
			// Calculate starting offset
			float startingOffsetX = -1*((columns-1)*spacingX/2);
			
			// Create row
			m_hexes[y] = new GameObject [m_numColumns];
			
			for (int x = 0; x < columns; ++x)
			{
				// Create a hex
				m_hexes[y][x] = (GameObject)Instantiate(
					hex,
					//new Vector3(startingOffsetX,startingOffsetY,1), 
					new Vector3(x,y,1), 
					Quaternion.identity);

				// Set HexGrid as parent
				m_hexes[y][x].transform.parent = transform;
				
				// Attempting to set position
				//m_hexes[y][x].transform.localPosition = new Vector3(startingOffsetX+spacingX*x,startingOffsetY+spacingY*y,0);
				//m_hexes[y][x].transform.position = new Vector3(startingOffsetX+spacingX*x,startingOffsetY+spacingY*y,1);
				//m_hexes[y][x].GetComponent<OTSprite>().position = new Vector2(startingOffsetX+spacingX*x,startingOffsetY+spacingY*y);
				//m_hexes[y][x].transform.localPosition = new Vector3(x,y,0);
				//m_hexes[y][x].transform.position = new Vector3(x,y,1);
				m_hexes[y][x].GetComponent<OTSprite>().position = new Vector2(startingOffsetX+x*spacingX,startingOffsetY+y*spacingY);
			}
			
		}
	}
	
	// Drop flowers to hexes
	public void DropFlowers()
	{
		GameObject[] flowers = null;
		flowers = GameObject.FindGameObjectsWithTag("Flower");
		
		
		GameObject[] hexes = null;
		hexes = GameObject.FindGameObjectsWithTag("Hex");
		
		for (int i=0; i < flowers.Length ; ++i)
		{
			Vector3 flowerPosition = flowers[i].transform.position;
			float min_distance = 100.0f;
			GameObject new_parent = null;
			
			// Determine closest hex
			for (int j=0; j < hexes.Length ; ++j)
			{
				if (Vector3.Distance(flowerPosition,hexes[j].transform.position) < min_distance)
				{
					min_distance = Vector3.Distance(flowerPosition,hexes[j].transform.position);
					new_parent = hexes[j];
				}
			}
			
			flowers[i].transform.parent = new_parent.transform;
			flowers[i].transform.localPosition = new Vector3(0,0,0);
		}
	}
	
	// Drop Bees
	public void DropBees()
	{
		GameObject[] bees = null;
		bees = GameObject.FindGameObjectsWithTag("Bee");
		
		
		GameObject[] flowers = null;
		flowers = GameObject.FindGameObjectsWithTag("Flower");
		
		for (int i=0; i < bees.Length ; ++i)
		{
			Vector3 beePosition = bees[i].transform.position;
			float min_distance = 100.0f;
			GameObject new_parent = null;
			
			// Determine closest hex
			for (int j=0; j < flowers.Length ; ++j)
			{
				if (Vector3.Distance(beePosition,flowers[j].transform.position) < min_distance)
				{
					min_distance = Vector3.Distance(beePosition,flowers[j].transform.position);
					new_parent = flowers[j];
				}
			}
			
			bees[i].transform.parent = new_parent.transform;
			bees[i].transform.localPosition = new Vector3(0,0,0);
		}
	}
}
