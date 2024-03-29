using UnityEngine;
using System.Collections;
using Pathfinding;

[RequireComponent(typeof(AudioSource))]
public class AntAI : MonoBehaviour {
	
	public GameObject target;
    public Vector3 targetPosition;
	public int damage=10;
	public AudioClip m_attackSound = null;
    
    private Seeker seeker;
    //private CharacterController controller;
 
    //The calculated path
    public Path path;
    
    //The AI's speed per second
    public float speed = 50;
    
    //The max distance from the AI to a waypoint for it to continue to the next waypoint
    public float nextWaypointDistance = 0.1f;
 
    //The waypoint we are currently moving towards
    private int currentWaypoint = 0;
 
    public void Start () {
        seeker = GetComponent<Seeker>();
        //controller = GetComponent<CharacterController>();
		
		// If not target, get it
		if (!target)
			target = GameObject.FindGameObjectWithTag("Hive");
		
		targetPosition = target.transform.position;
        
		CalculatePath();
    }
    
	public void CalculatePath()
	{
        //Start a new path to the targetPosition, return the result to the OnPathComplete function
        Debug.Log ("Looking for a path between our position: "+transform.position+" and target position: "+targetPosition);
        seeker.StartPath (transform.position,targetPosition, OnPathComplete);
	}
	
    public void OnPathComplete (Path p) {
        Debug.Log ("Yey, we got a path back. Did it have an error? "+p.error);
        if (!p.error) {
            path = p;
			Debug.Log("Number of waypoints: "+path.vectorPath.Count);
            //Reset the waypoint counter
            currentWaypoint = 0;
        }
    }
	
	public int GetNodesToTarget()
	{
		if (path == null || path.vectorPath == null) return 100;
		return path.vectorPath.Count - currentWaypoint;
	}
 
    public void FixedUpdate () {
        if (path == null) {
            //We have no path to move after yet
            return;
        }
        
        if (currentWaypoint >= path.vectorPath.Count) {
            Debug.Log ("End Of Path Reached");
			target.GetComponent<Health>().SetHP(target.GetComponent<Health>().HP-damage);
			gameObject.GetComponent<Health>().Kill();
			audio.enabled = true;
			//if (m_attackSound) 
				audio.PlayOneShot(m_attackSound,1.0f);
            return;
        }
        
        //Direction to the next waypoint
        Vector3 dir = (path.vectorPath[currentWaypoint]-transform.position).normalized;
        dir *= speed * Time.fixedDeltaTime;
        //controller.Move (dir);
		gameObject.GetComponent<OTSprite>().position = new Vector2(transform.position.x+dir.x,transform.position.y+dir.y);
		gameObject.GetComponent<OTSprite>().rotation = Mathf.Atan2(dir.y, dir.x)*(180.0f/Mathf.PI);
		if (gameObject.GetComponent<OTAnimatingSprite>() != null) 
			gameObject.GetComponent<OTSprite>().rotation = Mathf.Atan2(dir.y, dir.x)*(180.0f/Mathf.PI)-90.0f;
        
        //Check if we are close enough to the next waypoint
        //If we are, proceed to follow the next waypoint
        if (Vector2.Distance (transform.position,path.vectorPath[currentWaypoint]) < nextWaypointDistance) {
            currentWaypoint++;
            return;
        }
    }
} 