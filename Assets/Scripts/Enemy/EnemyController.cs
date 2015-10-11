using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {
	
	private EnemySight enemySight;
		


	void Awake () {
		enemySight = GetComponent<EnemySight>();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (enemySight.playerInSight) {
			transform.LookAt(enemySight.personalLastSighting);	
		}
	}
}
