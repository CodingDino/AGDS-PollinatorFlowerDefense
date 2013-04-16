using UnityEngine;
using UnityEditor;
using System.Collections;
 
public class HexGridEditor : MonoBehaviour {

	[MenuItem ("Edit/Create Hex Grid")]
	public static void CreateHexGrid(){
		Debug.Log("Creating HexGrid...");
		GameObject.Find ("HexGrid").GetComponent<HexGrid>().CreateHexes();	
	}

	[MenuItem ("Edit/Drop Bees to Flowers")]
	public static void DropBeesToFlowers(){
		Debug.Log("Dropping Bees to Flowers...");
		GameObject.Find ("HexGrid").GetComponent<HexGrid>().DropBees();	
	}

	[MenuItem ("Edit/Drop Flowers to Hexes")]
	public static void DropFlowersToHexes(){
		Debug.Log("Dropping Flowers to Hexes...");
		GameObject.Find ("HexGrid").GetComponent<HexGrid>().DropFlowers();	
	}
	
	
	
}
