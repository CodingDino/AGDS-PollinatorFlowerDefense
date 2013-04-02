using UnityEngine;
using System.Collections;

public class Flower : MonoBehaviour {
	
	// Internal data members
	protected OTSprite m_sprite;
	
	// Use this for initialization
	void Start () {
		m_sprite = GetComponent<OTSprite>();
		// hookup our drag events
		m_sprite.onReceiveDrop = ReceiveDrop;
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	// Receive Drop
	void ReceiveDrop(OTObject owner)
	{
		Debug.Log("Dropped on flower.");
		owner.dropTarget.gameObject.GetComponent<BeeBasic>().DroppedOnFlower(gameObject);
	}
}
