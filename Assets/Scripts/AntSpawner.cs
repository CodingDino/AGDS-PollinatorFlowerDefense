// ************************************************************************ 
// File Name:   AntSpawner.cs 
// Purpose:     Spawns ants based on string of spawns
// Project:		Pollinator - Flower Defense
// Author:      Sarah Herzog  
// Copyright: 	2013 Bound-Dare Studios
// ************************************************************************ 


// ************************************************************************ 
// Imports 
// ************************************************************************ 
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class AntSpawner : MonoBehaviour {
    // ********************************************************************
    // Constants
    // ********************************************************************
	
	
    // ********************************************************************
    // Struct: SpawnInstruction
    // ********************************************************************
	private struct SpawnInstruction {
		public float m_time;
		public GameObject m_enemy;
		public string m_name;
		public int m_level;
		
	    // ****************************************************************
	    // ToString
	    // ****************************************************************
		public override string ToString() {
			return "Spawn " + m_enemy + " at level " +m_level+ " after " + m_time + "s ";
		}
	}
	
	
    // ********************************************************************
    // Exposed Data Members 
    // ********************************************************************
	public string m_instructionString;
	
	
    // ********************************************************************
    // Private Data Members 
    // ********************************************************************
	private List<SpawnInstruction> m_instructions = new List<SpawnInstruction>();
	private DateTime m_nextSpawn = System.DateTime.Now;
	private int m_instructionIndex = 0;
	private List<GameObject> m_enemies = new List<GameObject>();
	
	
    // ********************************************************************
    // Function:	Start()
	// Purpose:		Run when new instance of the object is created.
    // ********************************************************************
	void Start () {
		
		// Process level string
		ProcessInstructions();
		
		// Get initial spawn time
		DateTime now = System.DateTime.Now;
		m_nextSpawn = (now).AddMilliseconds(m_instructions[m_instructionIndex].m_time*1000);
		
	}
	
    // ********************************************************************
    // Function:	Update()
	// Purpose:		Called once per frame.
    // ********************************************************************
	void Update () {
		
		// Spawn enemy if it is time
		if(m_instructionIndex < m_instructions.Count)
		{
			DateTime now = System.DateTime.Now;
			if (now >= m_nextSpawn)
			{
				if (m_instructions[m_instructionIndex].m_enemy)
				{
					// Spawn enemy
					GameObject new_enemy = (GameObject)Instantiate(
						m_instructions[m_instructionIndex].m_enemy,
						transform.position, 
						Quaternion.identity);
					new_enemy.GetComponent<OTSprite>().position = new Vector2(transform.position.x,transform.position.y);
	
					// TODO: Set enemy level
					
					// Set up animation
					OTAnimatingSprite sprite = new_enemy.GetComponent<OTAnimatingSprite>();
					if (sprite != null)
					{
						sprite.animation = GameObject.Find("OT").
							transform.FindChild("Animations").transform.
								FindChild(m_instructions[m_instructionIndex].m_name).
								GetComponent<OTAnimation>();
					}
						
					// Add the enemy to the list
					m_enemies.Add (new_enemy);
				}
				
				// Increment instruction
				++m_instructionIndex;
				
				// Calculate time to next spawn
				if (m_instructionIndex < m_instructions.Count)
					m_nextSpawn = (now).AddMilliseconds(m_instructions[m_instructionIndex].m_time*1000);
				
			}
		}
		else // We're done spawning enemies, check if any enemies are still alive
		{
			foreach (GameObject enemy in m_enemies)
			{
				if (enemy)
					return;
			}
			// If we got here, there are no enemies left
			GameObject.Find("Dialog-Win").GetComponent<DialogWin>().Activate();
		}
	
	}
	
	
    // ********************************************************************
    // Function:	ProcessInstructions()
	// Purpose:		Processes the instructionString and creates a list
	//				of SpawnInstruction structs.
    // ********************************************************************
	public void ProcessInstructions()
	{
		// If the string is empty, leave requirements as an empty list
		if (m_instructionString == "") return;
		
		// Split string into separate requirements to process individually.
		string[] instructionStrings = m_instructionString.Split(';');
		
		// For each requirement...
		foreach(string instructionString in instructionStrings)
		{
			SpawnInstruction instruction = new SpawnInstruction();
			
			// Break the instruction into three fields
			string[] instructionFields = instructionString.Split(',');
			// Deal with possible leading white space
			if (instructionFields.Length > 0 && instructionFields[0] == " ")
			{
				string newinstructionString = instructionString.Remove(0,1);
				instructionFields = newinstructionString.Split(',');
			}
			
			// If there are not exactly three fields, there was a formatting error.
			if (instructionFields.Length < 1 || instructionFields.Length > 3) 
			{
				Debug.LogError("Instruction parsing problem. Obtained "+instructionFields.Length+" fields when parsing "+instructionString);
				for (int i=0; i<instructionFields.Length; ++i)
				{
					Debug.LogError("   Field "+i+" = "+instructionFields[i]);
				}
				return;
			}
			
			// Parse and record the type of creature to be spawned
			instruction.m_enemy =  (GameObject)Resources.Load(instructionFields[0], typeof(GameObject));
			instruction.m_name = instructionFields[0];
			
			// Parse and record the creature level
			if (instructionFields.Length >= 2)
			{
				int level;
				bool isInteger = int.TryParse(instructionFields[1], out level);
				// If it wasn't numeric, there was an error
				if (!isInteger) 
				{
					Debug.LogError("Second argument must be an integer, failed parsing ("+instructionFields[1]+") from instruction string "+instructionString);
					return;
				}
				instruction.m_level = level;
			} else
				instruction.m_level = 1;
			
			// Parse and record the time between spawns
			if (instructionFields.Length >= 3)
			{
				float time;
				bool isFloat = float.TryParse(instructionFields[2], out time);
				// If it wasn't numeric, there was an error
				if (!isFloat) 
				{
					Debug.LogError("Third argument must be a float, failed parsing ("+instructionFields[2]+") from instruction string "+instructionString);
					return;
				}
				instruction.m_time = time;
			} else
				instruction.m_time = 1;
			
			// Log results
			Debug.Log("Parsed instruction: "+instruction);
			
			// Add our new instruction to the list
			m_instructions.Add (instruction);
			
		}
			
	}
	
	
    // ********************************************************************
    // Function:	RecalculatePaths()
	// Purpose:		Recalculates path for all enemies
    // ********************************************************************
	public void RecalculatePaths()
	{
		Debug.Log("Recalculating path...");
		
		// Recalculate grid
		GameObject.Find("A*").GetComponent<AstarPath>().Scan();
		//GameObject.Find("A*").GetComponent<AstarPath>().FlushGraphUpdates();
		
		// Recalculate paths
		foreach(GameObject enemy in m_enemies)
		{
			if (enemy) enemy.GetComponent<AntAI>().CalculatePath();
		}
	}
}
