using UnityEngine;
using UnityEditor;
using System.Collections;
 
public class HexGridEditor : MonoBehaviour {

	[MenuItem ("Edit/Create Hex Grid")]
	public static void CreateHexGrid(){
		Debug.Log("Creating HexGrid...");
		GameObject.Find ("HexGrid").GetComponent<HexGrid>().CreateHexes();	
	}
	
	
	
}
