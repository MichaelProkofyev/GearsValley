using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {
	
	private EnemySight enemySight;
	private DestinationSelector destinationSelector;
	private NavMeshAgent navAgent;



	void Awake () {
		enemySight = GetComponent<EnemySight>();
		destinationSelector = GetComponent<DestinationSelector>();
		navAgent = GetComponent<NavMeshAgent>();
	}

	// Use this for initialization
	void Start () {
		navAgent.destination = destinationSelector.NextPatrolPoint();  //Start patrolling
	}
	
	// Update is called once per frame
	void Update () {
		if (enemySight.playerInSight) {
			transform.LookAt(enemySight.personalLastSighting);	
		}

		if (navAgent.remainingDistance < 0.5f)
			navAgent.destination = destinationSelector.NextPatrolPoint();
	}
}
