using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class HexButton : MonoBehaviour {
	
	public AudioClip m_badClick;
	public AudioClip m_goodClick;
	
	public GameObject m_spawnTarget;
	public int m_cost = 10;
	
	private GameObject s_overlay = null;
	private bool m_active = false;

	// Use this for initialization
	void Start () {
		if(!s_overlay) s_overlay = GameObject.Find ("Overlay");
	}
	
	// Update is called once per frame
	void Update () {
	
		// Check for touch
		if (OT.Touched(gameObject)) OnTouch ();
	
	}
	
	public void Activate()
	{
		gameObject.GetComponent<OTSprite>().visible = true;
		Debug.Log("Button input: allowed.");
		gameObject.GetComponent<OTSprite>().registerInput = true;
		m_active = true;
	}
	
	public void Deactivate()
	{
		gameObject.GetComponent<OTSprite>().visible = false;
		Debug.Log("Button input: forbidden.");
		gameObject.GetComponent<OTSprite>().registerInput = false;
		m_active = false;
	}
	
	public void OnTouch()
	{
		// Make sure we're active
		if (!m_active) 
		{
			Debug.Log ("HexButton Click/touch detected, but INACTIVE");
			return;
		}
		// Debug Message
		Debug.Log ("HexButton Click/touch detected.");
		
		// Make sure we have the pollen to pay for this
		if (tag == "ButtonBeeBase" || tag == "ButtonBeeUpgrade")
		{
			GameObject hive = GameObject.FindGameObjectWithTag("Hive");
			if(!hive.GetComponent<Hive>().UsePollen(m_cost)) 
			{
				Debug.Log("Not enough pollen.");
				if (m_badClick) audio.PlayOneShot(m_badClick);
				return;
			}
		}
		if (tag == "ButtonFlowerBase" || tag == "ButtonFlowerUpgrade")
		{
			GameObject hive = GameObject.FindGameObjectWithTag("Hive");
			if(!hive.GetComponent<Hive>().UseSeeds(m_cost)) 
			{
				Debug.Log("Not enough seeds.");
				if (m_badClick) audio.PlayOneShot(m_badClick);
				return;
			}
		}
		if (m_goodClick) audio.PlayOneShot(m_goodClick);
		
		// Get the calling hex
		GameObject hex = s_overlay.GetComponent<Overlay>().GetCallingHex();
		
		// Create the new object
		Debug.Log("Spawning entity: "+m_spawnTarget);
		GameObject new_object = (GameObject)Instantiate(m_spawnTarget,
			hex.transform.position, 
			Quaternion.identity);
		
		// Drop the new object to the flower or hex.
		Debug.Log("Dropping entity to hex");
		if (new_object.tag == "Flower")
		{
			new_object.transform.parent = hex.transform;
			new_object.GetComponent<OTSprite>().position = new Vector2(0,0);
			new_object.GetComponent<OTSprite>().size = new Vector2(0.85f,0.95f);
			//GameObject.Find("A*").GetComponent<AstarPath>().UpdateGraphs(new Bounds(hex.transform.position,new_object.transform.lossyScale));
			//GameObject.Find("A*").GetComponent<AstarPath>().FlushGraphUpdates();
			GameObject.Find ("AntSpawner").GetComponent<AntSpawner>().RecalculatePaths();
		}
		if (new_object.tag == "Bee")
		{
			new_object.transform.parent = hex.transform.GetChild(0);
			new_object.GetComponent<OTSprite>().position = new Vector2(0,0);
		}
		

	}
}
