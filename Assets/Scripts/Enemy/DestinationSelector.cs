using UnityEngine;
using System.Collections;

public class DestinationSelector : MonoBehaviour {

	public GameObject[] patrolPoints;
//	public Vector3 nextDestination;

	private int patrolPointIdx = 0;

	// Use this for initialization
	void Start () {


	}
	
	// Update is called once per frame
	void Update () {

	}

	public Vector3 NextPatrolPoint () {
		if (patrolPoints.Length == 0) {
			Debug.Log ("!No points to patrol!");
			return Vector3.zero;
		}

		int currentPatrolPointIdx = patrolPointIdx;
		patrolPointIdx = ++patrolPointIdx % patrolPoints.Length;
		return patrolPoints[currentPatrolPointIdx].transform.position;
	}	
}
